import React, { Fragment } from "react";
import PropTypes from "prop-types";
import Carousel from "react-multi-carousel";
import CarouselConstants from "../Carousel/CarouselConstants";

function ContentGrid(props) {

    return (

        <Fragment>
            {props.items && props.items.map((item) =>
                <section className="section content-grid">
                    {item.carouselItems && item.carouselItems.length > 0 && 
                        <div key={item.id} className="content-grid__item">
                            <p className="title is-4">{item.title}</p>
                            <Carousel responsive={CarouselConstants.defaultCarouselResponsive()}>
                                {item.carouselItems.map((carouselItem) =>
                                    <div key={carouselItem.id} className="card">
                                        <a href={carouselItem.url}>
                                            <div className="card-image">
                                                <figure className="image is-4by3">
                                                    <img src={carouselItem.imageUrl} alt={carouselItem.imageAlt} />
                                                </figure>
                                            </div>
                                        </a>
                                        <div className="media-content">
                                            <a href={carouselItem.url}>
                                                <p className="content-grid-card__title title is-5 has-text-centered">{carouselItem.title}</p>
                                            </a>
                                        </div>
                                    </div>
                                    )}
                            </Carousel>
                        </div> 
                    }
                </section>
            )}
        </Fragment>
    );
}

ContentGrid.propTypes = {
    items: PropTypes.array
};

export default ContentGrid;
