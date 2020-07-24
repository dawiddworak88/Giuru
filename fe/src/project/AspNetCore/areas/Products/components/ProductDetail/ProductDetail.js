import React from 'react';
import PropTypes from 'prop-types';

function ProductDetail(props) {

    return (

        <section className="product-detail section">
            <div className="columns is-tablet">
                <div className="column">

                </div>
                <div className="column">
                    <h1 className="title is-5">{props.title}</h1>
                </div>
            </div>            
        </section>
    );
}

ProductDetail.propTypes = {
    title: PropTypes.string.isRequired
}

export default ProductDetail;