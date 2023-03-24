import React, { useContext, useState, useEffect, Fragment } from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";
import QueryStringSerializer from "../../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../../../shared/stores/Store";
import { CalendarToday } from "@mui/icons-material";
import { Hash } from "react-feather"
import { Button } from "@mui/material";
import moment from "moment";

const NewsCatalog = (props) => {
    const [state, dispatch] = useContext(Context);
    const [items, setItems] = useState(props.pagedResults.data ? props.pagedResults.data : null);
    const [hasMore, setHasMore] = useState(props.hasMore ? props.hasMore : false);

    let pageIndex = 2;
    let loadMore = hasMore;

    const handleLoadNews = () => {
        if (!loadMore) return;

        dispatch({ type: "SET_IS_LOADING", payload: true });
        
        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        }

        const queryStrings = {
            itemsPerPage: props.topUpContentSize, 
            pageIndex: pageIndex
        }  

        const getNewsUrl = props.newsApiUrl + "?" + QueryStringSerializer.serialize(queryStrings);

        fetch(getNewsUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok){
                        jsonResponse.data.forEach(item => {
                            setItems(element => [...element, item]);
                        })

                        pageIndex += 1;

                        if (pageIndex > jsonResponse.pageCount){
                            setHasMore(false)
                        }
                    }
                });
            });
    }

    const handleDynamicScroll = (e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = Math.ceil(
            e.target.documentElement.scrollTop + window.innerHeight
        );

        if (currentHeight + 1 >= scrollHeight) {
            handleLoadNews();
        }
    }

    useEffect(() => {
        if (typeof window !== "undefined") {
            window.addEventListener("scroll", handleDynamicScroll)

            return () => window.removeEventListener("scroll", handleDynamicScroll)
        }
    }, [hasMore])

    const handleCategory = (category) => {
        if (!category){
            return setItems(props.pagedResults.data);
        }

        const filtered = props.pagedResults.data.filter((item) => item.categoryName === category.name);

        return setItems(filtered);
    }

    return (
        <section className="section news-catalog">
            {items && items.length > 0 ? (
                <Fragment>
                    <div className="container">
                        {props.categories && props.categories.length > 0 &&
                            <div className="news-catalog__categories">
                                <Button type="text" variant="contained" className="category" color="primary" onClick={() => handleCategory(null)}>{props.allCategoryLabel}</Button>
                                {props.categories.map((category, index) => {
                                    return (
                                        <Button type="text" variant="contained" className="category" color="primary" onClick={() => handleCategory(category)} key={index}>{category.name}</Button>
                                    )
                                })}
                            </div>
                        }
                        <div className="news-catalog__news columns is-tablet is-multiline">
                            {items.map((news, index) => {
                                return (
                                    <div className="column is-4" onClick={() => NavigationHelper.redirect(news.url)} key={index}>
                                        <div className="card">
                                            {news.thumbImages && 
                                                <div className="card-image">
                                                    <figure className="image is-16by9">
                                                        <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                            <ResponsiveImage imageSrc={news.thumbImageUrl} sources={news.thumbImages} />
                                                        </LazyLoad>
                                                    </figure>
                                                </div>
                                            }
                                            <div className="media-content">
                                                {(news.categoryName || news.createdDate) &&
                                                    <div className="news-data">
                                                        {news.categoryName &&
                                                            <div className="data">
                                                                <Hash /> 
                                                                <span className="data-text">{news.categoryName}</span>
                                                            </div>
                                                        }
                                                        {news.createdDate &&
                                                            <div className="data">
                                                                <CalendarToday />
                                                                <span className="data-text">{moment.utc(news.createdDate).local().format("L")}</span>
                                                            </div>
                                                        }
                                                    </div>   
                                                }
                                                {news.title &&
                                                    <h4 className="title is-4 mb-2">{news.title}</h4>
                                                }
                                                {news.description &&
                                                    <p className="subtitle is-6 mt-0 news-description">{news.description}</p>
                                                }
                                            </div>
                                        </div>
                                    </div>
                                )
                            })}
                        </div>
                    </div>
                </Fragment>
            ) : (
                <section className="section is-flex-centered">
                    <span className="is-title is-5">{props.noResultsLabel}</span>
                </section>
            )}
        </section>
    )
}

NewsCatalog.propTypes = {
    noResultsLabel: PropTypes.string,
    newsApiUrl: PropTypes.string,
    categories: PropTypes.array,
    newsApiUrl: PropTypes.string
};

export default NewsCatalog;
