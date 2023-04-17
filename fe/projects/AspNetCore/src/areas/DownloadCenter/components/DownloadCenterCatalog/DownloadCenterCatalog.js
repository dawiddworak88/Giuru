import React from "react";
import PropTypes from "prop-types";

const DownloadCenterCatalog = (props) => {
    return (
        <div className="section dc-catalog">
            <div className="container">
                <div className="columns">
                    <div className="column">
                        {props.pagedResults && props.pagedResults.data ? (
                            props.pagedResults.data.map((category, index) => {
                                return (
                                    <div className="dc-catalog__category" key={index}>
                                        <h3 className="title is-3">{category.name}</h3>
                                        <div className="is-flex is-flex-wrap-wrap mt-2">
                                            {category.subcategories && category.subcategories.length > 0 && category.subcategories.map((subcategory, index) => {
                                                return (
                                                    <a href={subcategory.url} className="dc-catalog__subcategory-link is-flex is-justify-content-center is-align-items-center m-2 p-1" key={index}>
                                                        <div className="subtitle is-6">{subcategory.name}</div>
                                                    </a>
                                                )
                                            })}
                                        </div>
                                    </div>
                                )
                            })
                        ) : (
                            <h2 className="title">{props.noCategoriesLabel}</h2>
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}

DownloadCenterCatalog.propTypes = {
    noCategoriesLabel: PropTypes.string.isRequired,
    pagedResults: PropTypes.object
}

export default DownloadCenterCatalog;
