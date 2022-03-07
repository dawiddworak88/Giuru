import React from "react";
import LazyLoad from "react-lazyload";
import { CalendarToday, Apps, PictureAsPdf } from "@material-ui/icons";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import moment from "moment";

const NewsItemDetails = (props) => {
    return (
        <div className="section news-details">
            <div className="container">
                <div className="columns is-centered">
                    <div className="column is-8">
                        <div className="news-details__head">
                            <h1 className="">{props.title}</h1>
                            <p>{props.description}</p>
                        </div>
                        <figure className="image is-16by9 news-details__image">
                            <ResponsiveImage sources={props.heroImages} imageSrc={props.heroImageUrl} />
                        </figure>
                    </div>
                </div>
                <div className="columns is-centered">
                    <div className="column is-7">
                        <div className="news-details__meta-data">
                            <div className="meta">
                                <Apps />
                                <span className="text-data">{props.categoryName}</span>
                            </div>
                            <div className="meta">
                                <CalendarToday />
                                <span className="text-data">{moment.utc(props.createdDate).local().format("L")}</span>
                            </div>
                        </div>
                        <div className="news-details__content" dangerouslySetInnerHTML={{__html: props.content}}></div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default NewsItemDetails;