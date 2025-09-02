import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import { Carousel } from "react-responsive-carousel";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";

function HeroSlider(props) {

    const TeaserContent = (title, content, url, text) => (
        <div>
            <div className="hero-slider__teaser__desktop">
                <div className="hero-slider__teaser-title title is-5 has-text-white">
                    {title}
                </div>
                <div className="hero-slider__teaser-text">
                    {content}
                </div>
                {url &&
                    <div className="hero-slider__teaser-button">
                        <p className="control is-flex">
                            <a href={url} className="button is-primary hero-slider__teaser-link">{text}</a>
                        </p>
                    </div>
                }
            </div>
            <div className="hero-slider__teaser__mobile">
                <div className="hero-slider__teaser-title title is-5">
                    {title}
                </div>
                <div className="hero-slider__teaser-text">
                    {content}
                </div>
                {url &&
                    <div className="hero-slider__teaser-button">
                        <p className="control">
                            <a href={url} className="button is-primary hero-slider__teaser-link">{text}</a>
                        </p>
                    </div>
                }
            </div>
        </div>
    )

    if (props.items) {
        return (
            <div className="hero-slider">
                <Carousel autoPlay={true} showArrows={true} showThumbs={false} useKeyboardArrows={false} dynamicHeight={false} showStatus={false} swipeable={true}>
                    {props.items.map((item, index) =>
                        <LazyLoad offset={LazyLoadConstants.defaultOffset()} key={index}>
                            <div className="hero-slider__item" >
                                {item.ctaUrl ? (
                                    <a
                                        href={item.ctaUrl}
                                        className="hero-slider__item__link"
                                        aria-label={item.ctaText || item.teaserTitle || item.image?.imageAlt || "Open slide"}
                                        title={item.ctaText || item.teaserTitle}
                                    >
                                        <ResponsiveImage {...item.image} />
                                        {item.teaserTitle &&
                                            TeaserContent(item.teaserTitle, item.teaserText, item.ctaUrl, item.ctaText)
                                        }
                                    </a>
                                ) : (
                                    <>
                                        <ResponsiveImage {...item.image} />
                                        {item.teaserTitle &&
                                            TeaserContent(item.teaserTitle, item.teaserText, item.ctaUrl, item.ctaText)
                                        }
                                    </>
                                )}
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