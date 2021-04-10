import React, { Fragment } from "react";
import PropTypes from "prop-types";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import ContentGrid from "../../../../shared/components/ContentGrid/ContentGrid";

function ProductDetail(props) {

    return (

        <section className="product-detail section">
            <div className="product-detail__head columns is-tablet">
                <div className="column is-6">
                    {props.images && props.images.length &&
                        <div className="product-detail__image-gallery">
                            <ImageGallery items={props.images} />
                        </div>
                    }
                </div>
                <div className="column is-4">
                    <p className="product-detail__sku">{props.skuLabel} {props.sku}</p>
                    <h1 className="title is-4">{props.title}</h1>
                    <h2 className="product-detail__brand subtitle is-6">{props.byLabel} <a href={props.brandUrl}>{props.brandName}</a></h2>
                    {props.inStock &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel}
                        </div>
                    }
                    {props.description &&
                        <div className="product-detail__product-description">
                            <h3 className="product-detail__feature-title">{props.descriptionLabel}</h3>
                            <p>{props.description}</p>
                        </div>
                    }
                    {props.features &&
                        <div className="product-detail__product-information">
                            <h3 className="product-detail__feature-title">{props.productInformationLabel}</h3>
                            <div className="product-detail__product-information-list">
                                <dl>
                                    {props.features.map((item, index) =>
                                        <Fragment key={item.key}>
                                            <dt>{item.key}</dt>
                                            <dd>{item.value}</dd>
                                        </Fragment>
                                    )}
                                </dl>
                            </div>
                        </div>
                    }
                </div>
            </div>
            <ContentGrid items={props.productVariants} />
            <Files {...props.files} />
        </section>
    );
}

ProductDetail.propTypes = {
    title: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    sku: PropTypes.string.isRequired,
    byLabel: PropTypes.string.isRequired,
    brandUrl: PropTypes.string.isRequired,
    brandName: PropTypes.string.isRequired,
    pricesLabel: PropTypes.string.isRequired,
    productInformationLabel: PropTypes.string.isRequired,
    inStockLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    productDescription: PropTypes.string,
    productVariants: PropTypes.array,
    images: PropTypes.array,
    files: PropTypes.object
};

export default ProductDetail;