import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import ResponsiveImage from "../../../../../shared/components/Picture/ResponsiveImage";
import LazyLoadConstants from "../../../../../shared/constants/LazyLoadConstants";
import { Context } from "../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import { TablePagination, Button } from "@material-ui/core";
import CatalogConstants from "./CatalogConstants";
import { ShoppingCart } from "@material-ui/icons";
import Sidebar from "../Sidebar/Sidebar";
import AuthenticationHelper from "../../../../../shared/helpers/globals/AuthenticationHelper";
import moment from "moment";
import Modal from "../Modal/Modal";

function Catalog(props) {
    const [state, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = useState(props.basketItems ? props.basketItems : []);
    const [page, setPage] = useState(0);
    const [basketId, setBasketId] = useState(props.basketId ? props.basketId : null);
    const [itemsPerPage,] = useState(props.itemsPerPage ? props.itemsPerPage : CatalogConstants.defaultCatalogItemsPerPage());
    const [items, setItems] = useState(props.pagedItems.data);
    const [total, setTotal] = useState(props.pagedItems.total);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null)

    const toggleSidebar = (item) => {
        setProductVariant(item);
        setIsSidebarOpen(true)
    }

    const toggleModal = (item) => {
        setProductVariant(item);
        setIsModalOpen(true)
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
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const url = props.productsApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);
        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

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

    const handleModal = (item) => {
        setIsModalOpen(true)
        setProductVariant(item);
    }

    const handleCloseModal = () => {
        setIsModalOpen(false);
    }

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const quantity = parseInt(item.quantity);
        const stockQuantity = parseInt(item.stockQuantity);
        const outletQuantity = parseInt(item.outletQuantity);

        const totalQuantity = quantity + stockQuantity + outletQuantity;
        const orderItem = {
            productId: productVariant.id, 
            sku: productVariant.subtitle ? productVariant.subtitle : productVariant.sku, 
            name: productVariant.title, 
            imageId: productVariant.images ? productVariant.images[0].id ? productVariant.images[0].id : productVariant.images[0] : null,
            totalQuantity: totalQuantity,
            quantity: quantity,
            stockQuantity: stockQuantity,
            outletQuantity: outletQuantity,
            externalReference: item.externalReference,
            deliveryFrom: moment(item.deliveryFrom).startOf("day"), 
            deliveryTo: moment(item.deliveryTo).startOf("day"), 
            moreInfo: item.moreInfo
        };

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                dispatch({ type: "SET_TOTAL_BASKET", payload: parseInt(totalQuantity + state.totalBasketItems) })

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                            setIsModalOpen(false);
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
                                let fabrics = null;
                                if (item.productAttributes && item.productAttributes.length > 0) {
                                    fabrics = item.productAttributes.find(x => x.key === "primaryFabrics") ? item.productAttributes.find(x => x.key === "primaryFabrics").value : "";
                                    var secondaryFabrics = item.productAttributes.find(x => x.key === "secondaryFabrics") ? item.productAttributes.find(x => x.key === "secondaryFabrics").value : "";

                                    if (secondaryFabrics) {
                                        fabrics += ", " + secondaryFabrics;
                                    }
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
                                                {item.productAttributes && fabrics &&
                                                    <div className="catalog-item__fabric">
                                                        <h3>{props.primaryFabricLabel} {fabrics}</h3>
                                                    </div>
                                                }
                                                {item.inStock &&
                                                    <div className="catalog-item__in-stock-details">
                                                        {item.availableQuantity && item.availableQuantity > 0 && 
                                                            <div className="stock">
                                                                {props.inStockLabel} {item.availableQuantity}
                                                            </div>
                                                        }
                                                        {item.expectedDelivery &&
                                                            <div className="expected-delivery">
                                                                {props.expectedDeliveryLabel} {moment.utc(item.expectedDelivery).local().format("L")}
                                                            </div>
                                                        }
                                                    </div>
                                                }
                                                {item.inOutlet &&
                                                    <div className="catalog-item__in-stock-details">
                                                        {item.availableQuantity && item.availableQuantity > 0 && 
                                                            <div className="stock">
                                                                {props.inOutletLabel} {item.availableQuantity}
                                                            </div>
                                                        }
                                                    </div>
                                                }
                                            </div>
                                            {props.isLoggedIn &&
                                                <div className="catalog-item__add-to-cart-button-container">
                                                    {props.showAddToCartButton ? (
                                                        <div className="row is-flex is-flex-centered">
                                                            <Button variant="contained" startIcon={<ShoppingCart />} onClick={() => toggleModal(item)} color="primary">
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
                    productId={productVariant ? productVariant.id : null}
                    isOpen={isSidebarOpen}
                    manyUses={true}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleModal}
                    labels={props.sidebar}
                />
            }
            {props.modal && 
                <Modal 
                    isOpen={isModalOpen}
                    setIsOpen={setIsModalOpen}
                    handleClose={handleCloseModal}
                    maxOutletValue={productVariant ? productVariant.availableOutletQuantity : null}
                    maxStockValue={productVariant ? productVariant.availableQuantity : null}
                    handleOrder={handleAddOrderItemClick}
                    labels={props.modal}
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
