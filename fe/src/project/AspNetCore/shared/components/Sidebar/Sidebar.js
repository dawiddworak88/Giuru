import React, {useState, useContext, useEffect, useRef} from "react";
import { 
    Button, SwipeableDrawer, List, ListItem 
} from "@material-ui/core";
import { Close, AddShoppingCart, ExpandMore } from "@material-ui/icons";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import {CircularProgress } from "@material-ui/core";
import PropTypes from "prop-types";
import { Context } from "../../../../../shared/stores/Store";
import usePrevious from "../../../../../shared/hooks/usePrevious";

const Sidebar = (props) => {
    const [state, dispatch] = useContext(Context);
    const [productVariants, setProductVariants] = useState([])
    const [productId2, setProductId] = useState(props.productId)
    const {productId, open, setOpen, handleOrder, labels} = props;
    const prevProductId = usePrevious({productId2, productId})

    const toggleDrawer = (open) => (e) => {
        if (e && e.type === 'keydown' && (e.key === 'Tab' || e.key === 'Shift')) {
          return;
        }

        setOpen(open)
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
                        }
                    });
                }).catch(() => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                });
        }
    }

    const useHasChanged= (val) => {
        const prevVal = usePrevious(val)
        return prevVal !== val
    }
    const hasVal1Changed = useHasChanged(productId)

    useEffect(() => {
        if (open){
            fetchProductVariants();
        }
        if (hasVal1Changed ) {
            setProductId(productId)
            setProductVariants([])
        }
    }, [open, productId])

    return (
        <SwipeableDrawer
            anchor="right"
            open={open}
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
                {!productVariants ? (
                    <div className="not-found">{labels.notFound}</div>
                ) : (
                    productVariants.map((item) => 
                        item.carouselItems.map((carouselItem) => {
                                const statement = carouselItem.attributes;
                                let fabrics = labels.lackInformation;
                                if (statement.length > 0) {
                                    fabrics = carouselItem.attributes.find(x => x.key === "primaryFabrics").value;
                                }
                                return (
                                    <ListItem className="sidebar-item">
                                        <div className="sidebar-item__row">
                                            <div className="sidebar-item__image">
                                                <img src={carouselItem.imageUrl} alt={carouselItem.imageAlt}/>
                                            </div>
                                            <div className="sidebar-item__details">
                                                <h1 className="title">{carouselItem.title}</h1>
                                                <span className="sku">{labels.skuLabel} {carouselItem.sku}</span>
                                                <div className="fabrics">
                                                    <span>{labels.fabricsLabel}</span>
                                                    <p>{fabrics}</p>
                                                </div>
                                            </div>
                                            <div className="sidebar-item__buttons">
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={() => handleOrder(carouselItem)}><AddShoppingCart /></Button>
                                                <Button type="text" color="primary" variant="contained" className="cart-button" onClick={variantDetails(carouselItem)}><ExpandMore /></Button>
                                            </div>
                                        </div>
                                        <div className="divider"></div>
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
    setOpen: PropTypes.func.isRequired,
    open: PropTypes.bool.isRequired,
    handleOrder: PropTypes.func.isRequired
}


export default Sidebar;