import React, {useState} from "react";
import { CalendarToday } from "@material-ui/icons";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import PropTypes from "prop-types";
import { Hash } from "react-feather"
import moment from "moment";

const NewsCatalog = (props) => {
    const [items] = useState(props.pagedResults.data ? props.pagedResults.data : null);

    const navigateToNews = (item) => {
        NavigationHelper.redirect(item.url)
    }

    return (
        items.length > 0 && items &&
            <section className="section news-catalog">
                <div className="news-catalog__container">
                    <p className="title is-4">{props.title}</p>
                    <div className="columns is-tablet is-multiline">
                        {items.map((item, index) => {
                            return (
                                <div className="column is-4 news-catalog__item" onClick={() => navigateToNews(item)} key={index}>
                                    <div className="card">
                                        {item.thumbImages && 
                                            <div className="card-image">
                                                <figure className="image is-16by9">
                                                    <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                                        <ResponsiveImage imageSrc={item.thumbImageUrl} sources={item.thumbImages} />
                                                    </LazyLoad>
                                                </figure>
                                            </div>
                                        }
                                        <div className="media-content">
                                            <h2 className="title is-5">{item.title}</h2>
                                            <div className="media-data">
                                                <div className="data">
                                                    <Hash />
                                                    <span className="data-text">{item.categoryName}</span>
                                                </div>
                                                <div className="data">
                                                    <CalendarToday /> 
                                                    <span className="data-text">{moment.utc(item.createdDate).local().format("L")}</span>
                                                </div>
                                            </div>
                                            <p className="is-6 media-description">{item.description}</p>
                                        </div>
                                    </div>
                                </div>
                            )
                        })}
                    </div>
                </div>
            </section>
    )
}

NewsCatalog.propTypes = {
    title: PropTypes.string.isRequired
}

export default NewsCatalog;