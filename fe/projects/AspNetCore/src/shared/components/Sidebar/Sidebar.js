import React, {useState, useContext, useEffect} from "react";
import { 
    Button, SwipeableDrawer, List, ListItem 
} from "@mui/material";
import { Close, AddShoppingCart, ArrowRight } from "@mui/icons-material";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import PropTypes from "prop-types";
import { Context } from "../../../shared/stores/Store";
import ResponsiveImage from "../../../shared/components/Picture/ResponsiveImage";
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import Price from "../Price/Price";
import Availability from "../Availability/Availability";

const Sidebar = (props) => {
    const [state, dispatch] = useContext(Context);
    const [productVariants, setProductVariants] = useState([]);
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
        if (productVariants.length === 0 || manyUses) {
            dispatch({ type: "SET_IS_LOADING", payload: true });

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

                    AuthenticationHelper.HandleResponse(response);

                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            dispatch({ type: "SET_IS_LOADING", payload: false });
                            
                            if (jsonResponse && jsonResponse.length > 0 ){
                                setProductVariants(() => jsonResponse)
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
        setIsOpen(false)

        const { carouselItems } = productVariants[0];

        const productIndex = carouselItems.findIndex(x => x.id === item.id);
        const productVariant = carouselItems[productIndex];

        const payload = {
            ...item,
            availableQuantity: productVariant.availableQuantity,
            availableOutletQuantity: productVariant.availableOutletQuantity
        }

        handleOrder(payload);
    }

    useEffect(() => {
        if (isOpen){
            fetchProductVariants();
        }
    }, [isOpen, productId])

    return (
        <SwipeableDrawer
            anchor="right"
            open={isOpen}
            onOpen={toggleDrawer(true)}
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
                            item.carouselItems.map((carouselItem, index) => {
                                    return (
                                        <ListItem className="sidebar-item" key={index}>
                                            <div className="sidebar-item__row">
                                                <a href={carouselItem.url}>
                                                    <figure className="sidebar-item__image">
                                                        <ResponsiveImage sources={carouselItem.sources} imageSrc={carouselItem.imageUrl} imageAlt={carouselItem.imageAlt} />
                                                    </figure>
                                                </a>
                                                <div className="sidebar-item__details">
                                                    <a href={carouselItem.url}>
                                                        <h1 className="title">{carouselItem.title}</h1>
                                                        <span className="attribute">{labels.skuLabel} {carouselItem.subtitle}</span>
                                                        {carouselItem.ean &&
                                                            <span className="attribute">{labels.eanLabel} {carouselItem.ean}</span>
                                                        }
                                                    </a>
                                                    <div className="sidebar-item__availability mt-2">
                                                        <Availability
                                                            label={labels.inStockLabel}
                                                            availableQuantity={carouselItem.availableQuantity}
                                                        />
                                                        <Availability
                                                            label={labels.inOutletLabel}
                                                            availableQuantity={carouselItem.availableOutletQuantity}
                                                        />
                                                    </div>
                                                    <div className="productAttributes">
                                                        <p>{carouselItem.productAttributes}</p>
                                                    </div>
                                                    {carouselItem.price &&
                                                        <Price {...carouselItem.price} />
                                                    }
                                                </div>
                                            </div>
                                            <div className="sidebar-item__buttons">
                                                <Button title={props.labels.addToCartLabel} aria-label={props.labels.addToCartLabel} type="text" color="primary" variant="contained" className="cart-button" onClick={() => handleAddOrderItemClick(carouselItem)}><AddShoppingCart /></Button>
                                                <Button title={props.labels.goToDetailsLabel} aria-label={props.labels.goToDetailsLabel} type="text" color="primary" variant="contained" className="cart-button" onClick={variantDetails(carouselItem)}><ArrowRight /></Button>
                                            </div>
                                            <hr className="divider"></hr>
                                        </ListItem>
                                    )
                                }
                            )
                        )
                    ) : (
                        <div className="not-found">{labels.notFound}</div> 
                    )
                )}
            </List>
        </SwipeableDrawer>
    )
}

Sidebar.propTypes = {
    productId: PropTypes.string,
    labels: PropTypes.object.isRequired,
    setIsOpen: PropTypes.func.isRequired,
    isOpen: PropTypes.bool.isRequired,
    handleOrder: PropTypes.func.isRequired
}

export default Sidebar;
