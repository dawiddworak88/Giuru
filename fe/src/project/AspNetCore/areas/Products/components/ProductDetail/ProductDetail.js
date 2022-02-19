import React, { Fragment, useContext } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button, TextField } from "@material-ui/core";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import { ShoppingCart, Done } from "@material-ui/icons";
import { Context } from "../../../../../../shared/stores/Store";
import Sidebar from "../../../../shared/components/Sidebar/Sidebar";
import CarouselGrid from "../../../../shared/components/CarouselGrid/CarouselGrid";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";

function ProductDetail(props) {
    const [state, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = React.useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = React.useState(props.basketId ? props.basketId : null);
    const [isSidebarOpen, setIsSidebarOpen] = React.useState(false);
    const [quantity, setQuantity] = React.useState(1);
    const [isProductOrdered, setIsProductOrdered] = React.useState(false);

    const toggleSidebar = () => {
        setIsSidebarOpen(true)
    }

    const handleAddOrderItemClick = (item) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        let product = props;
        if (!props.isProductVariant){
            product = {
                productId: item.id,
                sku: item.subtitle,
                title: item.title,
                images: item.images,
                quantity: item.quantity,
                externalReference: null, 
                deliveryFrom: null, 
                deliveryTo: null, 
                moreInfo: null
            };
        }

        const orderItem = {
            productId: product.productId, 
            sku: product.sku, 
            name: product.title, 
            imageId: product.images ? product.images[0].id : null, 
            quantity: product.quantity ? product.quantity : quantity, 
            externalReference: null, 
            deliveryFrom: null, 
            deliveryTo: null, 
            moreInfo: null
        };

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    dispatch({ type: "SET_TOTAL_BASKET", payload: parseInt(orderItem.quantity + state.totalBasketItems) })
                    
                    if (response.ok) {
                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {
                            toast.success(props.successfullyAddedProduct)
                            setOrderItems(jsonResponse.items);
                            setIsProductOrdered(true)
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
                        </div>
                    }
                    {props.isAuthenticated && 
                        <div className="product-detail__add-to-cart-button">
                            {props.isProductVariant ? (
                                <div className="row">
                                    <TextField 
                                        id={props.productId} 
                                        name="quantity" 
                                        type="number" 
                                        inputProps={{ 
                                            min: 1, 
                                            step: 1,
                                            style: { textAlign: 'center' }
                                        }}
                                        value={quantity} 
                                        onChange={(e) => {
                                            if (e.target.value > 0) {
                                                setQuantity(e.target.value);
                                            }
                                        }}
                                        className="quantity-input"
                                    />
                                    <Button type="submit" startIcon={isProductOrdered ? <Done/> : <ShoppingCart />} variant="contained" color="primary" onClick={() => handleAddOrderItemClick()}>
                                        {isProductOrdered ? props.addedProduct : props.basketLabel}
                                    </Button>
                                </div>
                            ) : (
                                <div className="product-detail__add-to-cart-button">
                                <Button type="text" variant="contained" color="primary" onClick={toggleSidebar}>
                                    {props.basketLabel}
                                </Button>
                            </div>
                            )}
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
                <Sidebar 
                    productId={props.productId}
                    isOpen={isSidebarOpen}
                    manyUses={false}
                    setIsOpen={setIsSidebarOpen}
                    handleOrder={handleAddOrderItemClick}
                    labels={props.sidebar}
                />
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
    files: PropTypes.object,
    sidebar: PropTypes.object,
    addedProduct: PropTypes.string
};

export default ProductDetail;
