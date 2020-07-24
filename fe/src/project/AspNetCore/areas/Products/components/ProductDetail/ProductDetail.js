import React from 'react';
import PropTypes from 'prop-types';
import ImageGallery from 'react-image-gallery';

function ProductDetail(props) {

    return (

        <section className="product-detail section">
            <div className="columns is-tablet">
                <div className="column is-6">
                    <ImageGallery items={props.images} />
                </div>
                <div className="column is-4">
                    <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
                    <h1 className="title is-5">{props.title}</h1>
                    <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                    {props.inStock &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel}
                        </div>
                    }
                    <div className="product-detail__price">
                        <h3 className="product-detail__price-information">{props.pricesLabel}</h3>
                        {!props.isAuthenticated &&
                            <a href={props.signInUrl}>
                                {props.signInToSeePricesLabel}
                            </a>
                        }
                    </div>
                    <h3 className="product-detail__product-information">{props.productInformationLabel}</h3>
                </div>
            </div>            
        </section>
    );
}

ProductDetail.propTypes = {
    title: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    sku: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    brand: PropTypes.string.isRequired,
    pricesLabel: PropTypes.string.isRequired,
    productInformationLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string.isRequired
}

export default ProductDetail;