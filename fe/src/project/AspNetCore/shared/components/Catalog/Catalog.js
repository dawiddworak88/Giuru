import React, { useContext, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import ResponsiveImage from "../../../../../shared/components/Picture/ResponsiveImage";
import LazyLoadConstants from "../../../../../shared/constants/LazyLoadConstants";
import { Context } from "../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import { TablePagination, Button, TextField } from "@material-ui/core";
import CatalogConstants from "./CatalogConstants";
import { ShoppingCart } from "@material-ui/icons";
import Sidebar from "../Sidebar/Sidebar";

function Catalog(props) {
    const [, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = React.useState(props.orderItems ? props.orderItems : []);
    const [page, setPage] = React.useState(0);
    const [basketId, setBasketId] = React.useState(props.basketId ? props.basketId : null);
    const [itemsPerPage,] = React.useState(props.itemsPerPage ? props.itemsPerPage : CatalogConstants.defaultCatalogItemsPerPage());
    const [items, setItems] = React.useState(props.pagedItems.data);
    const [total, setTotal] = React.useState(props.pagedItems.total);
    const [quantities, setQuantities] = React.useState([]);
    const [isSidebarOpen, setIsSidebarOpen] = React.useState(false);
    const [product, setProduct] = React.useState(null)

    const toggleSidebar = (item) => {
        setProduct(item.id);
        setIsSidebarOpen(true)
    }

    const handleChangePage = (event, newPage) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        setPage(() => newPage);

        const searchParameters = {
            categoryId: props.categoryId,
            brandId: props.brandId,
            pageIndex: newPage + 1,
            itemsPerPage,
            orderBy: props.orderBy
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        };

        const url = props.productsApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);
        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        
                        setItems(() => []);
                        setItems(() => jsonResponse.data);
                        setTotal(() => jsonResponse.total);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const handleOrder = (item) => {
        const orderItem = {
            quantity: quantities.find(x => x.id === item.id).quantity,
            ...item
        }

        handleAddOrderItemClick(orderItem);
    }

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const orderItem = {
            productId: item.id, 
            sku: item.subtitle ? item.subtitle : item.sku, 
            name: item.title, 
            imageId: item.images ? item.images[0].id ? item.images[0].id : item.images[0] : null,
            quantity: parseInt(item.quantity), 
            externalReference: null, 
            deliveryFrom: null, 
            deliveryTo: null, 
            moreInfo: null
        };

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                        }
                        else {
                            setOrderItems([]);
                        }
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const onQuantityChange = (id) => (e) => {
        const itemQuantityIndex = quantities.findIndex(x => x.id === id);
        let prevQuantities = [...quantities];

        let item = prevQuantities.find(x => x.id === id);
        item.quantity = parseInt(e.target.value);

        prevQuantities[itemQuantityIndex] = item;

        setQuantities(prevQuantities)
    }

    useEffect(() => {
        if (items){
            let quantities = []
            items.forEach((item) => {
                const itemQuantity = {
                    id: item.id,
                    quantity: 1
                }

                quantities.push(itemQuantity);
            })

            setQuantities(quantities);
        }
    }, [items])

    return (
        <section className="catalog section">
            <h1 className="title is-3">{props.title}</h1>
            {items && items.length > 0 ?
                (
                    <div>
                        {total &&
                            <p className="subtitle is-6">{total} {props.resultsLabel}</p>
                        }
                        <div className="columns is-tablet is-multiline">
                            {items.map((item, index) => {
                                let quantity = 1;
                                if (quantities.length !== 0){
                                    quantity = quantities.find(x => x.id === item.id).quantity;
                                }

                                let fabrics = null;
                                if (item.productAttributes.length > 0) {
                                    fabrics = item.productAttributes.find(x => x.key === "primaryFabrics") ? item.productAttributes.find(x => x.key === "primaryFabrics").value : "";
                                    fabrics += item.productAttributes.find(x => x.key === "secondaryFabrics") ? item.productAttributes.find(x => x.key === "secondaryFabrics").value : "";
                                }

                                return (
                                    <div key={item.id} className="column is-3">
                                        <div className="catalog-item card">
                                            <a href={item.url}>
                                                <div className="card-image">
                                                    <figure className="image is-4by3">
                                                        <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                            <ResponsiveImage imageSrc={item.imageUrl} imageAlt={item.imageAlt} sources={item.sources} />
                                                        </LazyLoad>
                                                    </figure>
                                                </div>
                                            </a>
                                            <div className="media-content">
                                                <p className="catalog-item__sku">{props.skuLabel} {item.sku}</p>
                                                <h2 className="catalog-item__title"><a href={item.url}>{item.title}</a></h2>
                                                {props.showBrand && item.brandName &&
                                                    <div className="catalog-item__brand">
                                                        <h2 className="catalog-item__brand-text">{props.byLabel} <a href={item.brandUrl}>{item.brandName}</a></h2>
                                                    </div>
                                                }
                                                {item.productAttributes && fabrics &&
                                                    <div className="catalog-item__fabric">
                                                        <h3>{props.primaryFabricLabel} {fabrics}</h3>
                                                    </div>
                                                }
                                                {item.inStock && item.availableQuantity && item.availableQuantity >  0 &&
                                                    <div className="catalog-item__in-stock">
                                                        {props.inStockLabel} {item.availableQuantity}
                                                    </div>
                                                }
                                            </div>
                                            {props.isLoggedIn &&
                                                <div className="catalog-item__add-to-cart-button-container">
                                                    {props.showAddToCartButton ? (
                                                        <div className="row">
                                                            <TextField 
                                                                id={item.id} 
                                                                name="quantity" 
                                                                type="number" 
                                                                inputProps={{ 
                                                                    min: 1, 
                                                                    step: 1 
                                                                }}
                                                                value={quantity} 
                                                                onChange={onQuantityChange(item.id)}
                                                                className="quantity-input"
                                                            />
                                                            <Button variant="contained" startIcon={<ShoppingCart />} onClick={() => handleOrder(item)} color="primary">
                                                                {props.basketLabel}
                                                            </Button>
                                                        </div>
                                                    ) : (
                                                        <Button variant="contained" onClick={() => toggleSidebar(item)} color="primary">
                                                            {props.basketLabel}
                                                        </Button>
                                                    )}
                                                </div>
                                            }
                                        </div>
                                        
                                    </div>
                                )
                            })}
                        </div>
                        <div className="catalog__pagination is-flex-centered">
                            <TablePagination
                                labelDisplayedRows={({ from, to, count }) => `${from} - ${to} ${props.displayedRowsLabel} ${count}`}
                                labelRowsPerPage={props.rowsPerPageLabel}
                                backIconButtonText={props.backIconButtonText}
                                nextIconButtonText={props.nextIconButtonText}
                                rowsPerPageOptions={[itemsPerPage]}
                                component="div"
                                count={total}
                                page={page}
                                onChangePage={handleChangePage}
                                rowsPerPage={itemsPerPage}
                            />
                        </div>
                    </div>
                ) :
                (
                <section className="section is-flex-centered">
                    <span className="is-title is-5">{props.noResultsLabel}</span>
                </section>
            )}
            {props.sidebar &&  
                <Sidebar
                    productId={product}
                    isOpen={isSidebarOpen}
                    manyUses={true}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleAddOrderItemClick}
                    labels={props.sidebar}
                />
            }
        </section>
    );
}

Catalog.propTypes = {
    title: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    resultsCount: PropTypes.number.isRequired,
    resultsLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string.isRequired,
    isAuthenticated: PropTypes.bool.isRequired,
    signInUrl: PropTypes.string.isRequired,
    signInToSeePricesLabel: PropTypes.string.isRequired,
    displayedRowsLabel: PropTypes.string.isRequired,
    rowsPerPageLabel: PropTypes.string.isRequired,
    backIconButtonText: PropTypes.string.isRequired,
    nextIconButtonText: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    productsApiUrl: PropTypes.string.isRequired,
    categoryId: PropTypes.string.isRequired,
    orderBy: PropTypes.string,
    successfullyAddedProduct: PropTypes.string,
    inStock: PropTypes.bool.isRequired,
    availableQuantity: PropTypes.number,
    showAddToCartButton: PropTypes.bool,
    items: PropTypes.array,
    sidebar: PropTypes.object
};

export default Catalog;
