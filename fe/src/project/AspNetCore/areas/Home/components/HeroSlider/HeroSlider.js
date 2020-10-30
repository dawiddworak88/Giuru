import React from "react";
import PropTypes from "prop-types";
import { Carousel } from "react-responsive-carousel";

function HeroSlider(props) {

    if (props.items) {

        return (
            <section className="section pb-0 is-flex is-flex-centered is-desktop is-grey-background is-hidden-touch">
                <Carousel className="hero-slider" autoPlay={true} showArrows={true} showThumbs={false} useKeyboardArrows={false} dynamicHeight={false} showStatus={false} swipeable={true}>
                    {props.items.map((item, index) =>
                        <div className="hero-slider__item" key={index}>
                            <img src={item.imageSrc} alt={item.imageAlt} title={item.imageTitle} />
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
                                            <p className="control">
                                                <a href={item.ctaUrl} className="button is-primary hero-slider__teaser-link">{item.ctaText}</a>
                                            </p>
                                        </div>
                                    }
                                </div>
                            }
                        </div>
                    )}
                </Carousel>
            </section>
        );
    }

    return null;
}

HeroSlider.propTypes = {
    items: PropTypes.array
};

export default HeroSlider;