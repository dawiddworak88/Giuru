import React from "react";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../constants/LazyLoadConstants";
import ContentConstants from "../../constants/ContentConstants";

export const Video = (props) => {
  return (
    <div className="video video__container">
        {props.type == ContentConstants.externalMediaServiceTypeName() ? (
            <iframe
                src={props.videoUrl}
                className="video__player"
                frameBorder="0"
            />
        ) : (
            <LazyLoad offset={LazyLoadConstants.videoOffset()}>
                <video className="video__player" muted controls>
                    <source
                        src={props.videoUrl}
                        type={props.videoType ?? ContentConstants.defaultMediaType()}
                    />
                </video>
            </LazyLoad>
        )}
    </div>
  );
}
