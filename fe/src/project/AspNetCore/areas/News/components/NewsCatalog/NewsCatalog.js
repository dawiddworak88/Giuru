import React, { useRef, useContext, useState, useCallback } from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import useDynamicSearch from "../hooks/useDynamicSearch";
import { Context } from "../../../../../../shared/stores/Store";
import moment from "moment";

const NewsCatalog = (props) => {
    const [state] = useContext(Context);
    const [items, setItems] = useState(props.pagedItems.data);
    const [pageIndex, setPageIndex] = useState(1)

    const {
        news, hasMore, setNews
    } = useDynamicSearch(props.newsApiUrl, items, 10, 1)

    const observer = useRef()
    const lastElement = useCallback(node => {
        if (loading) return
        if (observer.current) observer.current.disconnect()
            observer.current = new IntersectionObserver(entries => {
                if (entries[0].isIntersecting && hasMore) {
                    setPageIndex(prevPageIndex => prevPageIndex + 1)
                }
            })

        if (node) observer.current.observe(node)
    }, [state, hasMore]);

    const handleCategory = (category) => {
        if (category == null){
            return setNews(items);
        } 

        const filtered = items.filter((item) => item.categoryName === category.name);

        setNews(filtered);
    }

    const navigateToNews = (item) => {
        NavigationHelper.redirect(item.url)
    }

    return (
        <section className="section news-catalog">
            {news && news.length > 0 ? (
                <div>
                    <div className="columns is-centered">
                        {news.slice(0, 1).map(newsItem => {
                            return (
                                <>
                                    <div className="column is-6">
                                        <figure class="image is-16by9">
                                            <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                <ResponsiveImage imageSrc={newsItem.thumbImageUrl} sources={newsItem.thumbImages} />
                                            </LazyLoad>
                                        </figure>
                                    </div>

                                    <div className="column is-4">
                                        <div className="news-data">{newsItem.categoryName} | {moment.utc(newsItem.createdDate).local().format("L")}</div>
                                        <h1 className="news-title">{newsItem.title}</h1>
                                    </div>
                                </>
                            )
                        })}
                    </div>

                    <div class="container">
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
    
};

export default NewsCatalog;