import React, { Fragment } from "react";
import PropTypes from "prop-types";

function ResponsiveImage(props) {

    return (
        <Fragment>
            {props.sources &&
                <picture>
                    {props.sources.map((item) =>
                        <source media={item.media} srcset={item.srcset} />
                    )}
                    <img src={props.imageSrc} alt={props.imageAlt} title={props.imageTitle} srcset={props.imageSrcset} />
                </picture>
            }

            {(!props.sources || props.sources.length === 0) &&
                <img src={props.imageSrc} alt={props.imageAlt} title={props.imageTitle} srcset={props.imageSrcset} />
            }
        </Fragment>       
    );
}

ResponsiveImage.propTypes = {
    sources: PropTypes.array,
    imageSrc: PropTypes.string.isRequired,
    imageAlt: PropTypes.string.isRequired,
    imageTitle: PropTypes.string
};

export default ResponsiveImage;
