import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import ResponsiveImage from "../../../shared/components/Picture/ResponsiveImage";
import LazyLoadConstants from "../../../shared/constants/LazyLoadConstants";
import { Context } from "../../../shared/stores/Store";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import { TablePagination, Button } from "@mui/material";
import CatalogConstants from "./CatalogConstants";
import { ShoppingCart } from "@mui/icons-material";
import Sidebar from "../Sidebar/Sidebar";
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import moment from "moment";
import Modal from "../Modal/Modal";
import Price from "../Price/Price";
import { useOrderManagement } from "../../../shared/hooks/useOrderManagement";
import QuantityCalculatorService from "../../services/QuantityCalculatorService";

function Catalog(props) {
    const [state, dispatch] = useContext(Context);
    const [page, setPage] = useState(0);
    const [itemsPerPage,] = useState(props.itemsPerPage ? props.itemsPerPage : CatalogConstants.defaultCatalogItemsPerPage());
    const [items, setItems] = useState(props.pagedItems.data);
    const [total, setTotal] = useState(props.pagedItems.total);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null);

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

        let searchParameters = {
            categoryId: props.categoryId,
            brandId: props.brandId,
            pageIndex: newPage + 1,
            itemsPerPage,
            orderBy: props.orderBy
        };

        if (props.searchTerm != null) {
            searchParameters = {
                ...searchParameters,
                searchTerm: props.searchTerm
            }
        }

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

    const {
        orderItems,
        addOrderItemToBasket
    } = useOrderManagement({
        initialBasketId: props.basketId ? props.basketId : null,
        initialOrderItems: props.basketItems ? props.basketItems : [],
        maxAllowedOrderQuantity: props.maxAllowedOrderQuantity,
        maxAllowedOrderQuantityErrorMessage: props.maxAllowedOrderQuantityErrorMessage,
        minOrderQuantityErrorMessage: props.minOrderQuantityErrorMessage,
        generalErrorMessage: props.generalErrorMessage,
        updateBasketUrl: props.updateBasketUrl,
        getPriceUrl: props.getProductPriceUrl
    });

    const handleAddOrderItemClick = (item) => {
        const quantity = parseInt(item.quantity);

        const getImages = () => {
            if (Array.isArray(productVariant.images) && productVariant.images.length > 0) {
                return productVariant.images.map(img => (typeof img === "object" && img.id ? img.id : img));
            }

            return [];
        }

        const product = {
            id: productVariant.id,
            sku: productVariant.subtitle ? productVariant.subtitle : productVariant.sku,
            name: productVariant.title,
            images: getImages(),
            stockQuantity: productVariant.availableQuantity,
            outletQuantity: productVariant.availableOutletQuantity,
            price: productVariant.price ? parseFloat(productVariant.price.current * quantity).toFixed(2) : null,
            currency: productVariant.price ? productVariant.price.currency : null
        }

        addOrderItemToBasket({
            product,
            quantity,
            isOutletOrder: item.isOutletOrder,
            externalReference: item.externalReference,
            moreInfo: item.moreInfo,
            resetData:() => {
                setIsModalOpen(false);
            }
        })
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
                                return (
                                    <div key={index} className="column is-3">
                                        <div className="catalog-item card">
                                            <a href={item.url}>
                                                <div className="card-image" aria-label={item.outletDescription} title={item.outletDescription}>
                                                    {item.inOutlet && item.outletTitle &&
                                                        <div className="catalog-item__discount p-1">
                                                            <span className="p-1">{item.outletTitle}</span>
                                                        </div>
                                                    }
                                                    <figure className="image is-4by3">
                                                        <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                            <ResponsiveImage imageSrc={item.imageUrl} imageAlt={item.imageAlt} sources={item.sources} imageClassName="card-image-scale-down" />
                                                        </LazyLoad>
                                                    </figure>
                                                </div>
                                            </a>
                                            <div className="media-content">
                                                <p className="catalog-item__sku">{props.skuLabel} {item.sku}</p>
                                                <h2 className="catalog-item__title"><a href={item.url}>{item.title}</a></h2>
                                                {item.productAttributes &&
                                                    <div className="catalog-item__productAttributes">
                                                        <h3>{item.productAttributes}</h3>
                                                    </div>
                                                }
                                                {item.inStock &&
                                                    <div className="catalog-item__in-stock-details">
                                                        {item.availableQuantity > 0 && item.availableQuantity &&
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
                                                        {item.availableOutletQuantity > 0 && item.availableOutletQuantity &&
                                                            <div className="stock">
                                                                {props.inOutletLabel} {item.availableOutletQuantity}
                                                            </div>
                                                        }
                                                    </div>
                                                }
                                                {item.price && 
                                                    <Price {...item.price} />
                                                }
                                            </div>
                                            {props.isLoggedIn &&
                                                <div className="catalog-item__add-to-cart-button-container">
                                                    {props.showAddToCartButton ? (
                                                        item.canOrder && (
                                                            <div className="row is-flex is-flex-centered">
                                                                <Button variant="contained" startIcon={<ShoppingCart />} onClick={() => toggleModal(item)} color="primary">
                                                                    {props.basketLabel}
                                                                </Button>
                                                            </div>
                                                        )
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
                                rowsPerPageOptions={[itemsPerPage]}
                                component="div"
                                count={total}
                                page={page}
                                onPageChange={handleChangePage}
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
                    maxOutletValue={productVariant ? QuantityCalculatorService.calculateMaxQuantity(orderItems, 'outletQuantity', productVariant.availableOutletQuantity, productVariant.subtitle ? productVariant.subtitle : productVariant.sku) : null}
                    outletQuantityInBasket={productVariant ? QuantityCalculatorService.getCurrentQuantity(orderItems, 'outletQuantity', productVariant.subtitle ? productVariant.subtitle : productVariant.sku) : 0}
                    handleClose={handleCloseModal}
                    handleOrder={handleAddOrderItemClick}
                    product={productVariant}
                    labels={props.modal}
                />
            }
        </section>
    );
}

Catalog.propTypes = {
    title: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    resultsLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string.isRequired,
    signInUrl: PropTypes.string.isRequired,
    signInToSeePricesLabel: PropTypes.string.isRequired,
    displayedRowsLabel: PropTypes.string.isRequired,
    rowsPerPageLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    productsApiUrl: PropTypes.string.isRequired,
    orderBy: PropTypes.string,
    successfullyAddedProduct: PropTypes.string,
    inStock: PropTypes.bool,
    inOutlet: PropTypes.bool,
    availableQuantity: PropTypes.number,
    showAddToCartButton: PropTypes.bool,
    items: PropTypes.array,
    sidebar: PropTypes.object,
    maxAllowedOrderQuantity: PropTypes.number,
    maxAllowedOrderQuantityErrorMessage: PropTypes.string,
    getProductPriceUrl: PropTypes.string
};

export default Catalog;
