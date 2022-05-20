import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import { CalendarToday, PictureAsPdf, Folder, Attachment } from "@mui/icons-material";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";
import { Hash } from "react-feather"
import {marked} from "marked";
import moment from "moment";

const NewsItemDetails = (props) => {
    return (
        <div className="section news-details">
            <div className="container">
                <div className="columns is-centered">
                    <div className="column is-8">
                        <div className="news-details__head">
                            {props.title &&
                                <h1 className="title is-2 mb-4">{props.title}</h1>
                            }
                            {props.description &&
                                <p className="subtitle is-5 mt-0">{props.description}</p>
                            }
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
                        <div className="news-details__content subtitle is-5" dangerouslySetInnerHTML={{__html: marked.parse(props.content)}}></div>
                        {props.files && props.files.length > 0 &&
                            <div className="news-details__files">
                                <h2 className="subtitle">{props.filesLabel}</h2>
                                <div className="columns">
                                    {props.files.map((file, index) => {
                                        const iconType = {
                                            "application/zip": <Folder />,
                                            "application/pdf": <PictureAsPdf />
                                        }

                                        return (
                                            <div className="column is-3" key={index}>
                                                <a href={file.url}>
                                                    <div className="card">
                                                        <div className="card-image">
                                                            {iconType[file.mimeType] ? iconType[file.mimeType] : <Attachment />}
                                                        </div>
                                                        <div className="media-content">
                                                            <span className="file">{file.filename}</span>
                                                        </div>
                                                    </div>
                                                </a>
                                            </div>
                                        )
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
