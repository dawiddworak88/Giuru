import React from "react";
import PropTypes from "prop-types";

function Catalog(props) {

    return (

        <section className="catalog section">
            <h1 className="title is-3">{props.title}</h1>
            {props.resultsCount &&
                <p className="subtitle is-6">{props.resultsCount} {props.resultsLabel}</p>
            }
            {props.items ?
                (
                    <div>
                        <div className="columns is-tablet is-multiline">
                            {props.items.map((item, index) =>
                                <div key={item.id} className="column is-3">
                                    <div className="catalog-item card">
                                        <a href={item.url}>
                                            <div className="card-image">
                                                <figure className="image is-4by3">
                                                    <img src={item.imageUrl} alt={item.imageAlt} />
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
                                            {item.inStock &&
                                                <div className="catalog-item__in-stock">
                                                    {props.inStockLabel}
                                                </div>
                                            }
                                            <div className="catalog-item__price">
                                                {!props.isAuthenticated &&
                                                    <a href={props.signInUrl}>
                                                        {props.signInToSeePricesLabel}
                                                    </a>
                                                }
                                            </div>
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
                                rowsPerPageOptions={[PaginationConstants.defaultRowsPerPage()]}
                                component="div"
                                count={total}
                                page={page}
                                onChangePage={handleChangePage}
                                rowsPerPage={PaginationConstants.defaultRowsPerPage()}
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
    items: PropTypes.array
};

export default Catalog;