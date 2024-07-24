import React from "react";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../constants/LazyLoadConstants";
import ContentConstants from "../../constants/ContentConstants";

export const Video = (props) => {
  return (
    <div className="video video__container">
        {props.type == ContentConstants.externalMediaServiceTypeName() ? (
            <iframe
                src="https://www.youtube.com/embed/z3LBIhf8GWY?si=kjEjd9QXRAzmuOxD"
                className="video__player"
                frameBorder="0"
            />
        ) : (
            <LazyLoad offset={LazyLoadConstants.videoOffset()}>
                <video className="video__player" muted controls>
                    <source
                        src="https://videos.pexels.com/video-files/3685374/3685374-hd_1920_1080_30fps.mp4"
                        type={"video/mp4"}
                    />
                </video>
            </LazyLoad>
        )}
    </div>
  );
}
