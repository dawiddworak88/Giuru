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
                                    <div className="dc-catalog__item" key={index}>
                                        <h3 className="is-size-5 has-text-weight-bold is-uppercase">{item.categoryName}</h3>
                                        <div className="dc-catalog__list">
                                            {item.categories.length > 0 && item.categories.map((category, index) => {
                                                return (
                                                    <a href={category.url} className="dc-catalog__list-item" key={index}>
                                                        <span className="dc-catalog__list-title">{category.name}</span>
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
    pagedResults: PropTypes.array
}

export default DownloadCenterCatalog;