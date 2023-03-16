import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import { Carousel } from "react-responsive-carousel";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../../../shared/components/Picture/ResponsiveImage";

function HeroSlider(props) {

    if (props.items) {

        return (
            <div className="hero-slider">
                <Carousel autoPlay={true} showArrows={true} showThumbs={false} useKeyboardArrows={false} dynamicHeight={false} showStatus={false} swipeable={true}>
                    {props.items.map((item, index) =>
                        <LazyLoad offset={LazyLoadConstants.defaultOffset()} key={index}>
                            <div className="hero-slider__item" >
                                    <ResponsiveImage {...item.image} />
                                    {item.teaserTitle &&
                                    <div className="hero-slider__teaser">
                                        <div className="hero-slider__teaser-title title is-5 has-text-white">
                                            {item.teaserTitle}
                                        </div>
                                        <div className="hero-slider__teaser-text">
                                            {item.teaserText}
                                        </div>
                                        {item.ctaUrl &&
                                            <div className="field">
                                                <p className="control is-flex">
                                                    <a href={item.ctaUrl} className="button is-primary hero-slider__teaser-link">{item.ctaText}</a>
                                                </p>
                                            </div>
                                        }
                                    </div>
                                }
                            </div>
                        </LazyLoad>
                    )}
                </Carousel>
            </div>
        );
    }

    return null;
}

HeroSlider.propTypes = {
    items: PropTypes.array
};

export default HeroSlider;