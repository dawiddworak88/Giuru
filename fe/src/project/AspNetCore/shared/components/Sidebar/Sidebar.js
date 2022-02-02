import React, {useState, useContext, useEffect} from "react";
import { 
    Button, SwipeableDrawer, List, ListItem 
} from "@material-ui/core";
import { Close, AddShoppingCart, ArrowRight } from "@material-ui/icons";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import { CircularProgress, TextField } from "@material-ui/core";
import PropTypes from "prop-types";
import { Context } from "../../../../../shared/stores/Store";

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

        if (!isOpen && manyUses){
            setProductVariants([])
        }

        setIsOpen(open)
    };

    const variantDetails = (item) => (e) => {
        e.preventDefault()
        NavigationHelper.redirect(item.url)
    }

    const fetchProductVariants = () => {
        if (productVariants.length === 0){
            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            };

            const requestQuery = {
                id: productId
            }

            const url = labels.productsApiUrl + "?" + QueryStringSerializer.serialize(requestQuery);
            return fetch(url, requestOptions)
                .then(function (response) {
                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    return response.json().then(jsonResponse => {
                        if (response.ok) {
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
                    });
                }).catch(() => {
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
        const itemQuantityIndex = quantities.findIndex(x => x.id === id);
        let prevQuantities = [...quantities];

        let item = prevQuantities.find(x => x.id === id);
        item.quantity = parseInt(e.target.value);

        prevQuantities[itemQuantityIndex] = item;

        setQuantities(prevQuantities)
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
                {productVariants.length === 0 ? (
                    <div className="not-found">{labels.notFound}</div>
                ) : (
                    productVariants.map((item) => 
                        item.carouselItems.map((carouselItem) => {
                                let fabrics = labels.lackInformation;
                                if (carouselItem.attributes.length > 0) {
                                    fabrics = item.productAttributes.find(x => x.key === "primaryFabrics") ? item.productAttributes.find(x => x.key === "primaryFabrics").value : "";
                                    fabrics += item.productAttributes.find(x => x.key === "secondaryFabrics") ? item.productAttributes.find(x => x.key === "secondaryFabrics").value : "";
                                }

                                let quantity = 1;
                                if (quantities.length !== 0){
                                    quantity = quantities.find(x => x.id === carouselItem.id).quantity;
                                }

                                return (
                                    <ListItem className="sidebar-item">
                                        <div className="sidebar-item__row">
                                            <div className="sidebar-item__image">
                                                <img src={carouselItem.imageUrl} alt={carouselItem.imageAlt}/>
                                            </div>
                                            <div className="sidebar-item__details">
                                                <h1 className="title">{carouselItem.title}</h1>
                                                <span className="sku">{labels.skuLabel} {carouselItem.subtitle}</span>
                                                <div className="fabrics">
                                                    <span>{labels.fabricsLabel}</span>
                                                    <p>{fabrics}</p>
                                                </div>
                                            </div>
                                            <div className="sidebar-item__buttons">
                                                <TextField 
                                                    id={carouselItem.id} 
                                                    name="quantity" 
                                                    type="number" 
                                                    inputProps={{ 
                                                        min: 1, 
                                                        step: 1 
                                                    }}
                                                    value={quantity} 
                                                    onChange={onQuantityChange(carouselItem.id)}
                                                    className="quantity-input"
                                                />
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={() => handleAddOrderItemClick(carouselItem)}><AddShoppingCart /></Button>
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={variantDetails(carouselItem)}><ArrowRight /></Button>
                                            </div>
                                        </div>
                                        <hr className="divider"></hr>
                                    </ListItem>
                                )
                            }
                        ),
                        state.isLoading && <CircularProgress className="progressBar" />
                    )
                )}
            </List>
        </SwipeableDrawer>
    )
}

Sidebar.propTypes = {
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
    handleOrder: PropTypes.func.isRequired
}

export default Sidebar;