import React, { useState, Fragment, useContext } from "react";
import { Context } from "../../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";
import PropTypes from "prop-types";

const DownloadCenterCatalog = (props) => {
    const [state, dispatch] = useContext(Context);
    const [items, setItems] = useState(props.pagedResults.data ? props.pagedResults.data : []);

    return (
        <div className="section download-catalog">
            <div className="container">
                <div className="columns">
                    <div className="column">
                        {items ? (
                            items.map((item, index) => {
                                return (
                                    <Fragment key={index}>
                                        <div className="list">
                                            <h3>{item.categoryName}</h3>
                                        </div>
                                        <div className="list-box">
                                            {item.categories.length > 0 && item.categories.map((category, index) => {
                                                return (
                                                    <a href={category.url} className="list-box__item" key={index}>
                                                        <span className="list-box__title">{category.name}</span>
                                                    </a>
                                                )
                                            })}
                                        </div>
                                    </Fragment>
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

export default DownloadCenterCatalog;