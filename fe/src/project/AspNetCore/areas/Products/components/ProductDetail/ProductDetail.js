import React, { Fragment, useContext } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { toast } from "react-toastify";
import { Button, SwipeableDrawer, List, ListItem, ListItemIcon, ListItemText, Box } from "@material-ui/core";
import ImageGallery from "react-image-gallery";
import Files from "../../../../shared/components/Files/Files";
import { ShoppingCart, Close, AddShoppingCart, ExpandMore, Done } from "@material-ui/icons";
import { Context } from "../../../../../../shared/stores/Store";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

function ProductDetail(props) {
    const [, dispatch] = useContext(Context);
    const [orderItems, setOrderItems] = React.useState(props.orderItems ? props.orderItems : []);
    const [basketId, setBasketId] = React.useState(props.basketId ? props.basketId : null);
    const [sideBar, setSideBar] = React.useState(false);
    const [orderedProduct, setOrderedProduct] = React.useState(false);
    const toggleDrawer = (open) => (e) => {
        if (e && e.type === 'keydown' && (e.key === 'Tab' || e.key === 'Shift')) {
          return;
        }
    
        setSideBar(open)
    };

    const handleAddOrderItemClick = (item) => {
        let product = props;
        if (!props.isProductVariant){
            product = {
                productId: item.id,
                sku: item.sku,
                title: item.title,
                images: item.images,
                quantity: parseInt(1),
                externalReference: "", 
                deliveryFrom: null, 
                deliveryTo: null, 
                moreInfo: ""

            };
        }

        dispatch({ type: "SET_IS_LOADING", payload: true });
        const orderItem = {
            productId: product.productId, 
            sku: product.sku, 
            name: product.title, 
            imageId: product.images ? product.images[0].id : null, 
            quantity: parseInt(1), 
            externalReference: "", 
            deliveryFrom: null, 
            deliveryTo: null, 
            moreInfo: ""
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
                            setOrderedProduct(true)
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

    const variantDetails = (item) => (e) => {
        e.preventDefault()
        NavigationHelper.redirect(item.url)
    }

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
                    {props.isAuthenticated && 
                        <div className="product-detail__add-to-cart-button">
                            {props.isProductVariant ? (
                                <Button type="submit" startIcon={orderedProduct ? <Done/> : <ShoppingCart />} variant="contained" color="primary" onClick={() => handleAddOrderItemClick()}>
                                    {orderedProduct ? props.addedProduct : props.basketLabel}
                                </Button>
                            ) : (
                                <div className="product-detail__add-to-cart-button">
                                <Button type="text" variant="contained" color="primary" onClick={toggleDrawer(true)}>
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
                <SwipeableDrawer
                    anchor="right"
                    open={sideBar}
                    onClose={toggleDrawer(false)}
                >
                <div className="sidebar-content">
                        <div className="sidebar-content__close">
                            <div className="icon" onClick={toggleDrawer(false)}>
                                <Close/>
                            </div>
                        </div>
                    </div>
                    <List className="sidebar-list">
                        <div className="sidebar-list__info">
                            <h2 className="title">{props.sidebarTitle}</h2>
                            <a href={props.basketUrl} className="link">{props.toBasketLabel}</a>
                        </div>
                        {!props.productVariants ? (
                            <div className="not-found">{props.notFound}</div>
                        ) : (
                            props.productVariants.map((item) => 
                                item.carouselItems.map((carouselItem) => 
                                    <ListItem className="sidebar-item">
                                        <div className="sidebar-item__row">
                                            <div className="sidebar-item__image">
                                                <img src={carouselItem.imageUrl} alt={carouselItem.imageAlt}/>
                                            </div>
                                            <div className="sidebar-item__details">
                                                <h1 className="title">{carouselItem.title}</h1>
                                                <span className="sku">{props.skuLabel} {carouselItem.sku}</span>
                                                <div className="fabrics">
                                                    <span>{props.fabricsLabel}</span>
                                                    {carouselItem.attributes.find(x => x.key === "primaryFabrics") ? (
                                                        <p>{carouselItem.attributes.find(x => x.key === "primaryFabrics").value}</p>
                                                    ) : (
                                                        <div>{props.lackInformation}</div>
                                                    )}
                                                    
                                                </div>
                                            </div>
                                            <div className="sidebar-item__buttons">
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={() => handleAddOrderItemClick(carouselItem)}><AddShoppingCart /></Button>
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={variantDetails(carouselItem)}><ExpandMore /></Button>
                                            </div>
                                        </div>
                                        <div className="divider"></div>
                                    </ListItem>
                                )
                            )
                        )}
                    </List>
                </SwipeableDrawer>
            </div>
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
    sidebarTitle: PropTypes.string,
    basketUrl: PropTypes.string,
    basketLabel: PropTypes.string,
    toBasketLabel: PropTypes.string,
    notFound: PropTypes.string,
    fabricsLabel: PropTypes.string,
    lackInformation: PropTypes.string,
    variantLabel: PropTypes.string,
    addedProduct: PropTypes.string
};

export default ProductDetail;
