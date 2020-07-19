import React from 'react';
import PropTypes from 'prop-types';
import Carousel from 'react-multi-carousel';

function ContentGrid(props) {

    const responsive = {
        superLargeDesktop: {
          breakpoint: { max: 4000, min: 3000 },
          items: 5
        },
        desktop: {
          breakpoint: { max: 3000, min: 1024 },
          items: 3
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

            <Carousel responsive={responsive}>
                <div>1</div>
                <div>2</div>
                <div>3</div>
            </Carousel>

        </section>
    );
}

ContentGrid.propTypes = {
    links: PropTypes.array
}

export default ContentGrid;