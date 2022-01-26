import React, {useEffect, useState} from "react";
import { 
    Button, SwipeableDrawer, List, ListItem, 
} from "@material-ui/core";
import { Close, AddShoppingCart, ExpandMore } from "@material-ui/icons";

const VariantSidebar = (props) => {
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
                                        <img src="https://eltap-media-cdn.azureedge.net/api/v1/files/432f201b-893c-4bfc-3102-08d907684408?w=1024&h=1024&o=true&extension=webp" />
                                    </div>
                                    <div className="sidebar-item__details">
                                        <h1 className="title">anton</h1>
                                        <span className="sku">Sku: An01</span>
                                        <div className="fabrics">
                                            <span>Tkaniny</span>
                                            <p>Kronos 19, Kronos 20, Kronos 21</p>
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

export default VariantSidebar;