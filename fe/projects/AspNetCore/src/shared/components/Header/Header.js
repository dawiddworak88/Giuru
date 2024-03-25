import React, { useState, useContext, useEffect } from "react";
import PropTypes from "prop-types";
import LanguageSwitcher from "../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import { Context } from "../../stores/Store";
import ShoppingCartIcon from "../../Icons/ShoppingCart";
import UserPopup from "../UserPopup/UserPopup";
import SidebarMobile from "../SidebarMobile/SidebarMobile"
import Search from "../Search/Search";

function Header(props) {
    const [state, dispatch] = useContext(Context);
    
    const [totalBasketItems, setTotalBasketItems] = useState(props.totalBasketItems ? props.totalBasketItems : 0);

    useEffect(() => {
        const totalItems = state.totalBasketItems;
        if (totalItems != null && totalItems < props.totalBasketItems) {
            state.totalBasketItems = props.totalBasketItems;
        }

        setTotalBasketItems(state.totalBasketItems);

    }, [state])

    return (
        <header>
            <nav className="navbar p-4 px-4 is-align-items-center header">
                <div className="navbar__start is-flex">
                    <a href={props.logo.targetUrl}>
                        <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} className="navbar__start__logo" />
                    </a>
                </div>
                <div className="navbar__search">
                    <Search {...props}/>
                </div>
                <div className="navbar__actions is-flex">
                    <div className="navbar__actions__language">
                        <LanguageSwitcher {...props.languageSwitcher} />
                    </div>
                    <div className="navbar__actions__userpopup">
                        <UserPopup {...props.userPopup} />
                    </div>
                    <div className="navbar__actions__cart">
                        <a href={props.basketUrl} title={props.goToCartLabel} aria-label={props.goToCartLabel}>
                            <ShoppingCartIcon />
                            <span className="navbar__actions__cart__count">{totalBasketItems}</span>
                        </a>
                    </div>
                    <div className="navbar__actions__sidebar">
                        <SidebarMobile {...props.sidebarMobile} />
                    </div>
                </div>
            </nav>
        </header>
    );
}

Header.propTypes = {
    logo: PropTypes.object.isRequired,
    searchPlaceholderLabel: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired,
    searchUrl: PropTypes.string.isRequired,
    searchTerm: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    isLoggedIn: PropTypes.bool,
    signOutLink: PropTypes.object
};

export default Header;