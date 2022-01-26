import React, {useEffect, useState} from "react";
import { Close, AddShoppingCart, ExpandMore } from "@material-ui/icons";
import PropTypes from "prop-types";
import { 
    Button, SwipeableDrawer, List, ListItem, 
} from "@material-ui/core";

const VariantSidebar = (props) => {
    console.log(props)
    const [sideBar, setSideBar] = useState(props.value);
    const toggleDrawer = (open) => (e) => {
        if (e && e.type === 'keydown' && (e.key === 'Tab' || e.key === 'Shift')) {
          return;
        }
    
        props.setValue(open)
    };

    useEffect(() => {
        setSideBar(props.value)
    }, [props])

    return (
        <SwipeableDrawer
            anchor="right"
            open={sideBar}
            onClose={toggleDrawer(false)}
        >
         <div className="sidebar-content">
                <div className="sidebar-content__close">
                    <div className="icon">
                        <Close/>
                    </div>
                </div>
            </div>
            <List className="sidebar-list">
                <div className="sidebar-list__info">
                    <h2 className="title">Dodaj wybrany produkt do koszyka</h2>
                    <a href="#" className="link">Zobacz koszyk</a>
                </div>
                {!props.items ? (
                    <div>brak</div>
                ) : (
                    props.items.map((item) => 
                        item.carouselItems.map((carouselItem) => 
                            <ListItem className="sidebar-item">
                                <div className="sidebar-item__row">
                                    <div className="sidebar-item__image">
                                        <img src={carouselItem.imageUrl} alt={carouselItem.imageAlt}/>
                                    </div>
                                    <div className="sidebar-item__details">
                                        <h1 className="title">{carouselItem.title}</h1>
                                        <span className="sku">Sku: {carouselItem.subtitle}</span>
                                        <div className="fabrics">
                                            <span>Tkaniny</span>
                                            {carouselItem.attributes.find(x => x.Key === "primaryFabrics") ? (
                                                <p>Kronos 19, Kronos 20, Kronos 21</p>
                                            ) : (
                                                <div>Brak informacji</div>
                                            )}
                                            
                                        </div>
                                    </div>
                                    <div className="sidebar-item__buttons">
                                        <Button type="text" color="primary" variant="contained" className="cart-button"><AddShoppingCart /></Button>
                                        <Button type="text" color="primary" variant="contained" className="cart-button"><ExpandMore /></Button>
                                    </div>
                                </div>
                                <div className="divider"></div>
                            </ListItem>
                        )
                    )
                )}
            </List>
        </SwipeableDrawer>
    )
}
VariantSidebar.propTypes = {
    items: PropTypes.array,
    setValue: PropTypes.func.isRequired
}

export default VariantSidebar;