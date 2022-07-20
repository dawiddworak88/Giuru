import React, { useState } from "react";
import PropTypes from "prop-types";

const DownloadCenterCatalog = (props) => {
    const [items, setItems] = useState(props.pagedResults.data ? props.pagedResults.data : []);

    return (
        <div className="section dc-catalog">
            <div className="container">
                <div className="columns">
                    <div className="column">
                        {items ? (
                            items.map((item, index) => {
                                return (
                                    <div className="dc-catalog__category" key={index}>
                                        <h3 className="is-size-5 has-text-weight-bold is-uppercase">{item.name}</h3>
                                        <div className="is-flex is-flex-wrap-wrap mt-2">
                                            {item.categories && item.categories.length > 0 && item.categories.map((subcategory, index) => {
                                                return (
                                                    <a href={subcategory.url} className="dc-catalog__subcategory-link is-flex is-justify-content-center is-align-items-center m-2" key={index}>
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
