import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../../shared/constants/LazyLoadConstants";
import { Context } from "../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import { TablePagination } from "@material-ui/core";
import CatalogConstants from "./CatalogConstants";

function Catalog(props) {

    const [, dispatch] = useContext(Context);
    const [page, setPage] = React.useState(0);
    const [itemsPerPage,] = React.useState(CatalogConstants.defaultCatalogItemsPerPage());
    const [items, setItems] = React.useState(props.pagedItems.data);
    const [total, setTotal] = React.useState(props.pagedItems.total);

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
                            {items.map((item, index) =>
                                <div key={item.id} className="column is-3">
                                    <div className="catalog-item card">
                                        <a href={item.url}>
                                            <div className="card-image">
                                                <figure className="image is-4by3">
                                                    <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                        <img src={item.imageUrl} alt={item.imageAlt} />
                                                    </LazyLoad>
                                                </figure>
                                            </div>
                                        </a>
                                        <div className="media-content">
                                            <p className="catalog-item__sku">{props.skuLabel} {item.sku}</p>
                                            <h2 className="catalog-item__title"><a href={item.url}>{item.title}</a></h2>
                                            {item.brandName &&
                                                <div className="catalog-item__brand">
                                                    <h3>{props.byLabel} <a href={item.brandUrl}>{item.brandName}</a></h3>
                                                </div>
                                            }
                                        </div>
                                    </div>
                                </div>
                            )}
                        </div>
                        <div className="catalog__pagination is-flex-centered">
                            <TablePagination
                                labelDisplayedRows={({ from, to, count }) => `${from} - ${to} ${props.displayedRowsLabel} ${count}`}
                                labelRowsPerPage={props.rowsPerPageLabel}
                                backIconButtonText={props.backIconButtonText}
                                nextIconButtonText={props.nextIconButtonText}
                                rowsPerPageOptions={[CatalogConstants.defaultCatalogItemsPerPage()]}
                                component="div"
                                count={total}
                                page={page}
                                onChangePage={handleChangePage}
                                rowsPerPage={CatalogConstants.defaultCatalogItemsPerPage()}
                            />
                        </div>
                    </div>
                ) :
                (
                    <section className="section is-flex-centered">
                        <span className="is-title is-5">{props.noResultsLabel}</span>
                    </section>
                )}
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
    items: PropTypes.array
};

export default Catalog;
