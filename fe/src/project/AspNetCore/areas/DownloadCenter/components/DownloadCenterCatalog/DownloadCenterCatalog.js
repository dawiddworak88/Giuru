import React, { useState } from "react";
import PropTypes from "prop-types";

const DownloadCenterCatalog = (props) => {
    const [items] = useState(props.pagedResults.data ? props.pagedResults.data : []);

    return (
        <div className="section dc-catalog">
            <div className="container">
                <div className="columns">
                    <div className="column">
                        {items ? (
                            items.map((item, index) => {
                                return (
                                    <div className="dc-catalog__container" key={index}>
                                        <h3 className="is-size-5 has-text-weight-bold is-uppercase">{item.categoryName}</h3>
                                        <div className="is-flex is-flex-wrap-wrap mt-2">
                                            {item.categories.length > 0 && item.categories.map((category, index) => {
                                                return (
                                                    <a href={category.url} className="dc-catalog__list-item is-flex is-justify-content-center is-align-items-center m-2" key={index}>
                                                        <span className="subtitle is-6">{category.name}</span>
                                                    </a>
                                                )
                                            })}
                                        </div>
                                    </div>
                                )
                            })
                        ) : (
                            <div>brak</div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}

DownloadCenterCatalog.propTypes = {
    pagedResults: PropTypes.object
}

export default DownloadCenterCatalog;