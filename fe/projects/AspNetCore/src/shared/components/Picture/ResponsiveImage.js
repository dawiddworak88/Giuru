import React, { Fragment } from "react";
import PropTypes from "prop-types";

function ResponsiveImage(props) {

    return (
        <Fragment>
            {props.sources &&
                <picture>
                    {props.sources.map((item, index) =>
                        <source media={item.media} srcSet={item.srcset} key={index}/>
                    )}
                    <img src={props.imageSrc} alt={props.imageAlt} title={props.imageTitle} srcSet={props.imageSrcset} />
                </picture>
            }

            {(!props.sources || props.sources.length === 0) &&
                <img src={props.imageSrc} alt={props.imageAlt} title={props.imageTitle} srcSet={props.imageSrcset} />
            }
        </Fragment>       
    );
}

ResponsiveImage.propTypes = {
    sources: PropTypes.array,
    imageSrc: PropTypes.string.isRequired,
    imageAlt: PropTypes.string,
    imageTitle: PropTypes.string
};

export default ResponsiveImage;
