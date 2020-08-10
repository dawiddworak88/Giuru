import React from 'react';
import PropTypes from 'prop-types';
import Carousel from 'react-multi-carousel';

function ContentGrid(props) {

    const responsive = {
        superLargeDesktop: {
          breakpoint: { max: 4000, min: 3000 },
          items: 7
        },
        desktop: {
          breakpoint: { max: 3000, min: 1024 },
          items: 4
        },
        tablet: {
          breakpoint: { max: 1024, min: 464 },
          items: 2
        },
        mobile: {
          breakpoint: { max: 464, min: 0 },
          items: 1
        }
      };

    return (

        <section className="section content-grid">
            {props.items && props.items.map((item) => 
                <div key={item.id} className="content-grid__item">
                    <p className="title is-4">{item.title}</p>
                    <Carousel responsive={responsive}>
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
            )}
        </section>
    );
}

ContentGrid.propTypes = {
    items: PropTypes.array
}

export default ContentGrid;