import React from "react";

export function Video(props) {
  return (
    <div className="video video__container">
        {props.type == "EXTERNAL" ? (
            <iframe
                src="https://www.youtube.com/embed/z3LBIhf8GWY?si=kjEjd9QXRAzmuOxD"
                className="video__player"
                // style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%' }}
                frameBorder="0"
                // allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            // allowFullScreen
            // title="YouTube video"
            />
        ) : (
            <video className="video__player" muted controls>
                <source
                    src="https://videos.pexels.com/video-files/3685374/3685374-hd_1920_1080_30fps.mp4"
                    type={"video/mp4"}
                />
            </video>
        )}
    </div>
    // props.type == "EXTERNAL" ? (
    //     <div style={{ position: 'relative', paddingBottom: '56.25%', height: 0, overflow: 'hidden', maxWidth: '100%', background: '#000' }}>

    //     </div>    
    // ) : (
    //     <video width={720} {...videoElementProps} muted={muted}>
    //   Your browser does not support the video tag.
    // </video>
    // )
  );
}
