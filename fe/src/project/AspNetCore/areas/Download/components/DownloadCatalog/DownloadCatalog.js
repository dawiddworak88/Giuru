import React, { useState, Fragment, useContext } from "react";
import { Context } from "../../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";
import PropTypes from "prop-types";

const DownloadCatalog = (props) => {
    const [state, dispatch] = useContext(Context);
    const [items, setItems] = useState(props.pagedResults.data ? props.pagedResults.data : []);

    const handleSubCategory = (id) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "GET",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            }
        }

        const queryStrings = {
            id
        }  

        const test = props.testUrl + "?" + QueryStringSerializer.serialize(queryStrings);

        fetch(test, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok){
                        if (jsonResponse.categories.length > 0){
                            setItems(jsonResponse)
                        }
                    }
                });
            });
    }

    return (
        <div className="section download-catalog">
            <div className="container">
                <div className="columns">
                    <div className="column">
                        {items ? (
                            items.length > 0 ? (
                                Array.isArray(items) && items.map((item, index) => {
                                    return (
                                        <Fragment key={index}>
                                            <div className="list">
                                                <h3>{item.categoryName}</h3>
                                            </div>
                                            <div className="list-box">
                                                {item.categories.length > 0 && item.categories.map((category, index) => {
                                                    return (
                                                        <div className="list-box__item" key={index} onClick={() => handleSubCategory(category.id)}>
                                                            <span className="list-box__title">{category.name}</span>
                                                        </div>
                                                    )
                                                })}
                                            </div>
                                        </Fragment>
                                    )
                                })
                            ) : (
                                <Fragment>
                                    <div className="list">
                                        <h3>{items.categoryName}</h3>
                                    </div>
                                    <div className="list-box">
                                        {items.categories.length > 0 && items.categories.map((category, index) => {
                                            return (
                                                <div className="list-box__item" key={index} onClick={() => handleSubCategory(category.id)}>
                                                    <span className="list-box__title">{category.name}</span>
                                                </div>
                                            )
                                        })}
                                    </div>
                                </Fragment>
                            )
                        ) : (
                            <div>Brak</div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default DownloadCatalog;