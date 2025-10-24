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
import Modal from "../Modal/Modal";
import FilterCollector from "../Filters/FilterCollector";
import SortingConstants from "../../constants/SortingConstants";
import Price from "../Price/Price";
import { useOrderManagement } from "../../../shared/hooks/useOrderManagement";
import QuantityCalculatorService from "../../services/QuantityCalculatorService";
import Availability from "../Availability/Availability";

function Catalog(props) {
    const [state, dispatch] = useContext(Context);
    const [page, setPage] = useState(0);
    const [itemsPerPage,] = useState(props.itemsPerPage ? props.itemsPerPage : CatalogConstants.defaultCatalogItemsPerPage());
    const [items, setItems] = useState(props.pagedItems.data);
    const [total, setTotal] = useState(props.pagedItems.total);
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [productVariant, setProductVariant] = useState(null);
    const [sorting, setSorting] = useState(props.filterCollector && props.filterCollector.sortItems.length > 0 ? SortingConstants.defaultKey() : "");
    const [filters, setFilters] = useState(props.filters 
        ? Object.entries(props.filters)
            .flatMap(([key, arr]) => (arr || []).map(v => ({ key, value: v, label: v }))) 
        : []);

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

        console.log("New page:", newPage);
        console.log("Page: ", page);
        

        setPage(() => newPage);

        const params = buildSearchParams({
            selectedFilters: filters,
            categoryId: props.categoryId,
            searchTerm: props.searchTerm,
            pageIndex: newPage + 1,
            itemsPerPage,
            orderBy: sorting
        })
        
        fetchData(params);
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
        addProductToBasketMessage: props.toastSuccessAddProductToBasket,
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

    const handleFilters = (value) => {
        setFilters(value);

        const params = buildSearchParams({
            selectedFilters: value,
            categoryId: props.categoryId,
            searchTerm: props.searchTerm,
            pageIndex: page + 1,
            itemsPerPage,
            orderBy: sorting
        })
        
        applyUrlFromParams(value, props.searchTerm);

        fetchData(params);
    }

    const handleSetSorting = (value) => {
        setSorting(value)

        const params = buildSearchParams({
            selectedFilters: filters,
            categoryId: props.categoryId,
            searchTerm: props.searchTerm,
            pageIndex: page + 1,
            itemsPerPage,
            orderBy: value
        })
        
        fetchData(params);
    }

    const fetchData = (params) => {
        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const url = `${props.productsApiUrl}?${params.toString()}`;

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
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
    }

    const buildSearchParams = ({
        searchTerm,
        selectedFilters = [],
        categoryId,
        pageIndex,
        itemsPerPage,
        orderBy
    }) => {
        const params = new URLSearchParams();

        if (searchTerm?.trim()) params.set("searchTerm", searchTerm.trim());
        if (categoryId != null) params.set("categoryId", categoryId);
        if (pageIndex != null) params.set("pageIndex", pageIndex);
        if (itemsPerPage != null) params.set("itemsPerPage", itemsPerPage);
        if (orderBy != null) params.set("orderBy", orderBy);

        appendFilterParams(params, selectedFilters);

        return params;
    }

    const appendFilterParams = (params, selectedFilters) => {
        if (!Array.isArray(selectedFilters) || selectedFilters.length === 0) return;

        const grouped = selectedFilters.reduce((acc, f) => {
            if (!f?.key || !f?.value) return acc;
            (acc[f.key] ||= []).push(f.value);
            return acc;
        }, {});

        Object.entries(grouped).forEach(([key, values]) => {
            values.forEach(v => params.append(`search[${key}]`, v));
        });
    };

    const buildFilterUrlParams = (selectedFilters, searchTerm) => {
        const params = new URLSearchParams();

        if (searchTerm?.trim()) {
            params.set("searchTerm", searchTerm.trim());
        }

        appendFilterParams(params, selectedFilters);

        return params;
    };

    const applyUrlFromParams = (filters, searchTerm, push = true) => {
        const basePath = window.location.pathname;
        const params = buildFilterUrlParams(filters, searchTerm);
        const query = params.toString();
        const newUrl = query ? `${basePath}?${query}` : basePath;

        const fn = push ? window.history.pushState : window.history.replaceState;
        fn.call(window.history, null, "", newUrl);

        return newUrl;
    };

    return (
        <section className="catalog section">
            <h1 className="title is-3">{props.title}</h1>
            <div>
                <div>
                    <FilterCollector
                        {...props.filterCollector}
                        total={total}
                        resultsLabel={props.resultsLabel}
                        filters={filters}
                        sorting={sorting}
                        setSorting={handleSetSorting}
                        setFilters={handleFilters}
                    />
                </div>
                {items && items.length > 0 ? (
                    <div>
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
                                                <div className="catalog-item__availability mt-3">
                                                    {item.inStock &&
                                                        <Availability 
                                                            label={props.inStockLabel}
                                                            availableQuantity={item.availableQuantity}
                                                        />
                                                    }
                                                    {item.inOutlet &&
                                                        <Availability 
                                                            label={props.inOutletLabel}
                                                            availableQuantity={item.availableOutletQuantity}
                                                        />
                                                    }
                                                </div>
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
                ) : (
                    <section className="section is-flex-centered">
                        <span className="is-title is-5">{props.noResultsLabel}</span>
                    </section>
                )}
            </div>
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
                    outletOrder={props.isDefaultOutletOrder ?? false}
                    setIsOpen={setIsModalOpen}
                    maxOutletValue={
                        productVariant 
                            ? QuantityCalculatorService.calculateMaxQuantity(
                                orderItems, 'outletQuantity', productVariant.availableOutletQuantity, productVariant.subtitle ? productVariant.subtitle : productVariant.sku
                              ) 
                            : null
                        }
                    outletQuantityInBasket={
                        productVariant 
                            ? QuantityCalculatorService.getCurrentQuantity(
                                orderItems, 'outletQuantity', productVariant.subtitle ? productVariant.subtitle : productVariant.sku
                              ) 
                            : 0
                        }
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
    noFilteredResultsLabel: PropTypes.string.isRequired,
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
    inStock: PropTypes.bool,
    inOutlet: PropTypes.bool,
    availableQuantity: PropTypes.number,
    showAddToCartButton: PropTypes.bool,
    items: PropTypes.array,
    sidebar: PropTypes.object,
    maxAllowedOrderQuantity: PropTypes.number,
    maxAllowedOrderQuantityErrorMessage: PropTypes.string,
    getProductPriceUrl: PropTypes.string,
    isDefaultOutletOrder: PropTypes.bool
};

export default Catalog;
