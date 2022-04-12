import React, {useState, useContext, useEffect, Suspense} from "react";
import { 
    Button, SwipeableDrawer, List, ListItem 
} from "@material-ui/core";
import { Close, AddShoppingCart, ArrowRight } from "@material-ui/icons";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import { TextField } from "@material-ui/core";
import PropTypes from "prop-types";
import { Context } from "../../../../../shared/stores/Store";
import ResponsiveImage from "../../../../../shared/components/Picture/ResponsiveImage";
import AuthenticationHelper from "../../../../../shared/helpers/globals/AuthenticationHelper";
import moment from "moment";

const Sidebar = (props) => {
    const [state, dispatch] = useContext(Context);
    const [productVariants, setProductVariants] = useState([]);
    const [quantities, setQuantities] = useState([]);
    const {productId, isOpen, manyUses, setIsOpen, handleOrder, labels} = props;

    const toggleDrawer = (open) => (e) => {
        if (e && e.type === 'keydown' && 
           (e.key === 'Tab' || e.key === 'Shift')) {
                return;
        }

        setIsOpen(open)
    };

    const variantDetails = (item) => (e) => {
        e.preventDefault()
        NavigationHelper.redirect(item.url)
    }

    const fetchProductVariants = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        if (productVariants.length === 0 || manyUses) {
            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
            };

            const requestQuery = {
                id: productId
            }

            const url = labels.productsApiUrl + "?" + QueryStringSerializer.serialize(requestQuery);
            return fetch(url, requestOptions)
                .then(function (response) {
                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    AuthenticationHelper.HandleResponse(response);

                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            if (jsonResponse && jsonResponse.length > 0 ){
                                setProductVariants(() => jsonResponse)
                            
                                let quantities = [];
                                jsonResponse[0].carouselItems.forEach((item, i) => {
                                    const itemQuantity = {
                                        id: item.id,
                                        quantity: 1
                                    }

                                    quantities.push(itemQuantity);
                                });

                                setQuantities(quantities)
                            }
                        } 
                    });
                }).catch(() => {
                    setProductVariants([]);
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                });
        }
    }

    const handleAddOrderItemClick = (item) => {
        const orderItem = {
            quantity: quantities.find(x => x.id === item.id).quantity,
            ...item
        }

        handleOrder(orderItem);
    }

    const onQuantityChange = (id) => (e) => {
        if (e.target.value > 0) {
            const itemQuantityIndex = quantities.findIndex(x => x.id === id);
            let prevQuantities = [...quantities];

            let item = prevQuantities.find(x => x.id === id);
            item.quantity = e.target.value;

            prevQuantities[itemQuantityIndex] = item;

            setQuantities(prevQuantities)
        }
    }

    useEffect(() => {
        if (isOpen){
            fetchProductVariants();
        }
        else {
            setQuantities(() => []);
        }
    }, [isOpen, productId])

    return (
        <SwipeableDrawer
            anchor="right"
            open={isOpen}
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
                    <h2 className="title">{labels.sidebarTitle}</h2>
                    <a href={labels.basketUrl} className="link">{labels.toBasketLabel}</a>
                </div>
                {state.isLoading ? (
                    <div className="not-found">{labels.loadingLabel}</div>
                ) : (
                    productVariants && productVariants.length > 0 ? (
                        productVariants.map((item) => 
                            item.carouselItems.map((carouselItem) => {
                                let fabrics = labels.lackInformation;
                                if (carouselItem.attributes.length > 0) {
                                    fabrics = carouselItem.attributes.find(x => x.key === "primaryFabrics") ? carouselItem.attributes.find(x => x.key === "primaryFabrics").value : "";
                                    var secondaryfabrics = carouselItem.attributes.find(x => x.key === "secondaryFabrics") ? carouselItem.attributes.find(x => x.key === "secondaryFabrics").value : "";
                                    if (secondaryfabrics) {
                                        fabrics += ", " + secondaryfabrics;
                                    }
                                }
                                let quantity = 1;
                                if (quantities.length !== 0){
                                    quantity = quantities.find(x => x.id === carouselItem.id).quantity;
                                }
                                return (
                                    <ListItem className="sidebar-item">
                                        <div className="sidebar-item__row">
                                            <figure className="sidebar-item__image">
                                                <ResponsiveImage sources={carouselItem.sources} imageSrc={carouselItem.imageUrl} imageAlt={carouselItem.imageAlt} />
                                            </figure>
                                            <div className="sidebar-item__details">
                                                <h1 className="title">{carouselItem.title}</h1>
                                                <span className="sku">{labels.skuLabel} {carouselItem.subtitle}</span>
                                                <div className="stock-details">
                                                    {carouselItem.availableQuantity && carouselItem.availableQuantity > 0 &&
                                                        <div className="stock">
                                                            {labels.inStockLabel} {carouselItem.availableQuantity}
                                                        </div>
                                                    }
                                                    {carouselItem.expectedDelivery &&
                                                        <div className="expected-delivery">
                                                            {labels.expectedDeliveryLabel} {moment(carouselItem.expectedDelivery).format("DD/MM/YYYY")}
                                                        </div>
                                                    }
                                                </div>
                                                <div className="fabrics">
                                                    <span>{labels.fabricsLabel}</span>
                                                    <p>{fabrics}</p>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="sidebar-item__buttons">
                                            <TextField 
                                                id={carouselItem.id} 
                                                name="quantity" 
                                                type="number" 
                                                inputProps={{ 
                                                    min: 1, 
                                                    step: 1,
                                                    style: { textAlign: 'center' }
                                                }}
                                                value={quantity} 
                                                onChange={onQuantityChange(carouselItem.id)}
                                                className="quantity-input" />
                                            <Button title={props.labels.addToCartLabel} aria-label={props.labels.addToCartLabel} type="text" color="primary" variant="contained" className="cart-button" onClick={() => handleAddOrderItemClick(carouselItem)}><AddShoppingCart /></Button>
                                            <Button title={props.labels.goToDetailsLabel} aria-label={props.labels.goToDetailsLabel} type="text" color="primary" variant="contained" className="cart-button" onClick={variantDetails(carouselItem)}><ArrowRight /></Button>
                                        </div>
                                        <hr className="divider"></hr>
                                    </ListItem>
                                )
                            }
                        ))
                    ) : (
                        <div className="not-found">{labels.notFound}</div>
                    )
                )}
            </List>
        </SwipeableDrawer>
    )
}

Sidebar.propTypes = {
    sources: PropTypes.array,
    sidebarTitle: PropTypes.string,
    basketUrl: PropTypes.string,
    basketLabel: PropTypes.string,
    toBasketLabel: PropTypes.string,
    notFound: PropTypes.string,
    fabricsLabel: PropTypes.string,
    lackInformation: PropTypes.string,
    productsApiUrl: PropTypes.string.isRequired,
    productId: PropTypes.string.isRequired,
    labels: PropTypes.object,
    setIsOpen: PropTypes.func.isRequired,
    isOpen: PropTypes.bool.isRequired,
    handleOrder: PropTypes.func.isRequired,
    loadingLabel: PropTypes.string.isRequired
}

export default Sidebar;
