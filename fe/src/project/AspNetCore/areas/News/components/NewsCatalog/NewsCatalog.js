import React, { useContext, useState, useEffect } from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../../../../../shared/stores/Store";
import moment from "moment";

const NewsCatalog = (props) => {
    const [state, dispatch] = useContext(Context);
    const [items, setItems] = useState(props.pagedItems.data ? props.pagedItems.data : null);
    const [news, setNews] = useState(props.pagedItems.data ? props.pagedItems.data : null);
    const [hasMore, setHasMore] = useState(true);

    let pageIndex = 2;
    const handleLoadNews = async () => {
        if (!hasMore) return;
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        }

        const queryStrings = {
            itemsPerPage: 10, 
            pageIndex: pageIndex
        }

        const getNewsUrl = props.newsApiUrl + "?" + QueryStringSerializer.serialize(queryStrings);
        await fetch(getNewsUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok){
                        jsonResponse.data.forEach(item => {
                            setNews(element => [...element, item]);
                        })

                        pageIndex += 1;

                        if (pageIndex > jsonResponse.pageCount){
                            setHasMore(false)
                        }
                    }
                });
            }).catch(() => {
                setHasMore(false)
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
        window.addEventListener("scroll", handleDynamicScroll)

        return () => window.removeEventListener("scroll", handleDynamicScroll)
    }, [hasMore])

    const handleCategory = (category) => {
        const filtered = items.filter((item) => item.categoryName === category.name);
        if (filtered){
            return setNews(filtered);
        }

        return setNews(items);
    }

    const navigateToNews = (item) => {
        NavigationHelper.redirect(item.url)
    }

    return (
        <section className="section news-catalog">
            {items && items.length > 0 ? (
                <div>
                    {items.slice(0, 1).map(newsItem => {
                            return (
                                <div className="columns is-centered hero-news" onClick={() => navigateToNews(newsItem)} key={newsItem.id}>
                                    <div className="column is-6">
                                        <figure className="image is-16by9">
                                            <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                <ResponsiveImage imageSrc={newsItem.thumbImageUrl} sources={newsItem.thumbImages} />
                                            </LazyLoad>
                                        </figure>
                                    </div>

                                    <div className="column is-4">
                                        <div className="news-data">{newsItem.categoryName} | {moment.utc(newsItem.createdDate).local().format("L")}</div>
                                        <h1 className="news-title">{newsItem.title}</h1>
                                        <p className="news-description">{newsItem.description}</p>
                                    </div>
                                </div>
                            )
                    })}

                    <div className="container">
                        {props.categories && props.categories.length > 0 &&
                            <div className="news-catalog__categories">
                                <div className="category-tag" onClick={() => handleCategory(null)}>{props.allCategoryLabel}</div>
                                {props.categories.map(category => {
                                    return (
                                        <div className="category-tag" onClick={() => handleCategory(category)}>{category.name}</div>
                                    )
                                })}
                            </div>
                        }

                        <div className="news-catalog__news columns is-tablet is-multiline">
                            {news.slice(1).map((news, index) => {
                                return (
                                    <div className="column is-4" onClick={() => navigateToNews(news)} key={index}>
                                        <div className="card">
                                            <div className="card-image">
                                                <figure className="image is-16by9">
                                                    <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                        <ResponsiveImage imageSrc={news.thumbImageUrl} sources={news.thumbImages} />
                                                    </LazyLoad>
                                                </figure>
                                            </div>
                                            <div className="media-content">
                                                <div className="news-data">{news.categoryName} | {moment.utc(news.createdDate).local().format("L")}</div>
                                                <h4 className="news-title">{news.title}</h4>
                                            </div>
                                        </div>
                                    </div>
                                )
                            })}
                        </div>
                    </div>
                </div>
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