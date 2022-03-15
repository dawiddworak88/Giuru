import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import { CalendarToday, PictureAsPdf, Folder, Attachment } from "@material-ui/icons";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import { Hash } from "react-feather"
import {marked} from "marked";
import moment from "moment";

const NewsItemDetails = (props) => {
    const handleDownloadFile = (file) => {
        NavigationHelper.redirect(file.url)
    }

    return (
        <div className="section news-details">
            <div className="container">
                <div className="columns is-centered">
                    <div className="column is-8">
                        <div className="news-details__head">
                            <h1 className="headline-1">{props.title}</h1>
                            <p>{props.description}</p>
                        </div>
                        {props.previewImages && 
                            <figure className="image is-16by9 news-details__image">
                                <LazyLoad offset={LazyLoadConstants.catalogOffset()}>
                                    <ResponsiveImage sources={props.previewImages} imageSrc={props.previewImageUrl} />
                                </LazyLoad>
                            </figure>
                        }
                    </div>
                </div>
                <div className="columns is-centered">
                    <div className="column is-7">
                        <div className="news-details__meta-data">
                            <div className="meta">
                                 <Hash />
                                 <span className="text-data">{props.categoryName}</span>
                            </div>
                            <div className="meta">
                                <CalendarToday />
                                <span className="text-data">{moment.utc(props.createdDate).local().format("L")}</span>
                            </div>
                        </div>
                        <div className="news-details__content" dangerouslySetInnerHTML={{__html: marked.parse(props.content)}}></div>
                        {props.files && props.files.length > 0 &&
                            <div className="news-details__files">
                                <h2 className="headline-2">{props.filesLabel}</h2>
                                <div className="columns">
                                    {props.files.map((file, index) => {
                                        if (file.mimeType.includes("pdf")){
                                            return (
                                                <div className="column is-3" key={index} onClick={() => handleDownloadFile(file)}>
                                                    <div className="card">
                                                        <div className="card-image">
                                                            <PictureAsPdf />
                                                        </div>
                                                        <div className="media-content">
                                                            <span className="file">{file.filename}</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            )
                                        } else if (file.mimeType.includes("zip")) {
                                                return (
                                                    <div className="column is-3" key={index} onClick={() => handleDownloadFile(file)}>
                                                        <div className="card">
                                                            <div className="card-image">
                                                                <Folder />
                                                            </div>
                                                            <div className="media-content">
                                                                <span className="file">{file.filename}</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                )
                                        } else {
                                            return (
                                                <div className="column is-3" key={index} onClick={() => handleDownloadFile(file)}>
                                                    <div className="card">
                                                        <div className="card-image">
                                                            <Attachment />
                                                        </div>
                                                        <div className="media-content">
                                                            <span className="file">{file.filename}</span>
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

NewsItemDetails.propTypes = {
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    heroImages: PropTypes.array,
    heroImageUrl: PropTypes.string,
    categoryName: PropTypes.string,
    files: PropTypes.array,
    content: PropTypes.string,
    filesLabel: PropTypes.string
}

export default NewsItemDetails;