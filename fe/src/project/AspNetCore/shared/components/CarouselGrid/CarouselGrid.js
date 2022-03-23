import React, { Fragment } from "react";
import PropTypes from "prop-types";
import Carousel from "react-multi-carousel";
import ResponsiveImage from "../../../../../shared/components/Picture/ResponsiveImage";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../../shared/constants/LazyLoadConstants";
import CarouselConstants from "../Carousel/CarouselConstants";
import { CalendarToday } from "@material-ui/icons";
import { Hash } from "react-feather";
import moment from "moment";

function CarouselGrid(props) {
    return (
        <Fragment>
            {props.items && props.items.map((item) =>
                <section className="section carousel-grid">
                    {item.carouselItems && item.carouselItems.length > 0 && 
                        <div key={item.id} className="carousel-grid__item">
                            <p className="title is-4">{item.title}</p>
                            <Carousel responsive={CarouselConstants.defaultCarouselResponsive()}>
                                {item.carouselItems.map((carouselItem) =>
                                    <div key={carouselItem.id} className="card">
                                        {carouselItem.sources && 
                                            <a href={carouselItem.url}>
                                                <div className="card-image">
                                                    <figure className="image is-4by3">
                                                        <LazyLoad offset={LazyLoadConstants.defaultOffset()}>
                                                            <ResponsiveImage sources={carouselItem.sources} imageSrc={carouselItem.imageUrl} imageAlt={carouselItem.imageAlt} />
                                                        </LazyLoad>
                                                    </figure>
                                                </div>
                                            </a>
                                        }
                                        <div className="card-content">
                                            <a href={carouselItem.url}>
                                                {carouselItem.title &&
                                                    <p className="title is-5 has-text-centered">{carouselItem.title}</p>
                                                }

                                                {props.isNews &&
                                                    <div className="carousel-grid__data">
                                                        <div className="data">
                                                            <Hash />
                                                            <span className="text">{carouselItem.categoryName}</span>
                                                        </div>
                                                        <div className="data">
                                                            <CalendarToday /> 
                                                            <span className="text">{moment.utc(carouselItem.createdDate).local().format("L")}</span>
                                                        </div>
                                                    </div>
                                                }

                                                {carouselItem.subtitle &&
                                                    <p className={props.isNews ? "subtitle is-6 has-text-centered grid-description" : "subtitle is-6 has-text-centered"}>{carouselItem.subtitle}</p>
                                                }
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

CarouselGrid.propTypes = {
    items: PropTypes.array
};

export default CarouselGrid;
