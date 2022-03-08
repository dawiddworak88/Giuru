import React from "react";
import LazyLoad from "react-lazyload";
import { CalendarToday, Apps, PictureAsPdf, Folder, Attachment } from "@material-ui/icons";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
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
                            <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                <ResponsiveImage sources={props.heroImages} imageSrc={props.heroImageUrl} />
                            </LazyLoad>
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
                        {props.files && props.files.length > 0 &&
                            <div className="news-details__files">
                                <h2 className="headline-2">Pliki</h2>
                                <div className="columns">
                                    {props.files.map((file, index) => {
                                        if (file.mimeType.includes("pdf")){
                                            return (
                                                <div className="column is-3" key={index}>
                                                    <div className="card">
                                                        <div className="card-image">
                                                            <PictureAsPdf />
                                                        </div>
                                                        <div className="media-content">
                                                            <span className="file">{file.filename}</span>
                                                            <a href={file.url}>Pobierz plik</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            )
                                        } else if (file.mimeType.includes("zip")) {
                                                return (
                                                    <div className="column is-3" key={index}>
                                                        <div className="card">
                                                            <div className="card-image">
                                                                <Folder />
                                                            </div>
                                                            <div className="media-content">
                                                                <span className="file">{file.filename}</span>
                                                                <a href={file.url}>Pobierz plik</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                )
                                        } else {
                                            return (
                                                <div className="column is-3" key={index}>
                                                    <div className="card">
                                                        <div className="card-image">
                                                            <Attachment />
                                                        </div>
                                                        <div className="media-content">
                                                            <span className="file">{file.filename}</span>
                                                            <a href={file.url}>Pobierz plik</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            )
                                        }
                                    })}
                                </div>
                            </div>
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}

export default NewsItemDetails;