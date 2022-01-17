import React, { Fragment, useContext } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button } from "@material-ui/core";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import { ShoppingCart } from "@material-ui/icons";
import { Context } from "../../../../../../shared/stores/Store";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";

function ProductDetail(props) {
    const [, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = React.useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = React.useState(props.id ? props.id : null);

    const handleAddOrderItemClick = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const orderItem = {
            productId: props.productId, sku: props.sku, name: props.title, 
            imageId: props.images ? props.images[0].id : null, quantity: parseInt(1), 
            externalReference: "", deliveryFrom: null, deliveryTo: null, moreInfo: ""
        };

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                        }
                        else {
                            setOrderItems([]);
                        }
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

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
                    {props.inStock && props.availableQuantity && props.availableQuantity > 0 &&
                        <div className="product-detail__in-stock">
                            {props.inStockLabel} {props.availableQuantity}
                            {props.expectedDelivery && 
                                <div className="product-detail__expected-delivery">{props.expectedDeliveryLabel} {moment.utc(props.expectedDelivery).local().format("L")}</div>
                            }
                            {props.restockableInDays && 
                                <div className="product-detail__restockable-in-days">{props.restockableInDaysLabel} {props.restockableInDays}</div>
                            }
                        </div>
                    }
                    {props.isAuthenticated && props.isProductVariant &&
                        <div className="product-detail__add-to-cart-button">
                            <Button type="submit" startIcon={<ShoppingCart />} variant="contained" color="primary" onClick={() => handleAddOrderItemClick()}>
                                {props.basketLabel}
                            </Button>
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
            <CarouselGrid items={props.productVariants} />
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
    inStockLabel: PropTypes.string,
    inStock: PropTypes.bool.isRequired,
    availableQuantity: PropTypes.number,
    restockableInDaysLabel: PropTypes.string,
    restockableInDays: PropTypes.number,
    descriptionLabel: PropTypes.string.isRequired,
    productDescription: PropTypes.string,
    productVariants: PropTypes.array,
    isProductVariant: PropTypes.bool,
    isAuthenticated: PropTypes.bool,
    images: PropTypes.array,
    files: PropTypes.object
};

export default ProductDetail;
