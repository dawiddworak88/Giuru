import React, { useState, useContext, useEffect, useRef } from "react";
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
    const [isUserPopupOpen, setIsUserPopupOpen] = useState(false);
    const [isLanguageSwitechOpen, setIsLanguageSwitechOpen] = useState(false);
    const overlayRef = useRef(null);

    const updateTotalBasketItems = (newTotal) => {
        
        if (newTotal === 0) {
            setTotalBasketItems(newTotal);
        }
        else if (newTotal != null && newTotal !== props.totalBasketItems) {
            setTotalBasketItems(newTotal);
        }
    }

    if (state.totalBasketItems !== totalBasketItems) {
        updateTotalBasketItems(state.totalBasketItems);
    }

    const overlayDisplaying = (isDisplay) => {
        if (overlayRef.current) {
            if (isDisplay) {
                overlayRef.current.classList.add('show');
                document.body.classList.add("no-scroll");
            }
            else {
                overlayRef.current.classList.remove('show');
                document.body.classList.remove("no-scroll");
            }
        }
    };

    const userPopupOnClick = (state) => {
        setIsUserPopupOpen(!state)
        setIsLanguageSwitechOpen(false)
    };

    const languageSwitcherOnClick = (state) => {
        setIsLanguageSwitechOpen(!state)
        setIsUserPopupOpen(false)
    };

    return (
        <header>
            <nav className="navbar p-4 px-4 is-align-items-center header">
                <div className="navbar__start is-flex">
                    <a href={props.logo.targetUrl}>
                        <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} className="navbar__start__logo" />
                    </a>
                </div>
                <div className="navbar__search">
                    <Search {...props.search} overlayDisplaying={overlayDisplaying} />
                </div>
                <div className="navbar__actions is-flex">
                    <div className="navbar__actions__language">
                        <LanguageSwitcher {...props.languageSwitcher} isOpen={isLanguageSwitechOpen} toggle={languageSwitcherOnClick}/>
                    </div>
                    <div className="navbar__actions__userpopup">
                        <UserPopup {...props.userPopup} isOpen={isUserPopupOpen} toggle={userPopupOnClick} />
                    </div>
                    <div className="navbar__actions__cart">
                        <a href={props.basketUrl} title={props.goToCartLabel} aria-label={props.goToCartLabel}>
                            <ShoppingCartIcon />
                            <span className="navbar__actions__cart__count">{totalBasketItems == null ? 0 : totalBasketItems}</span>
                        </a>
                    </div>
                    <div className="navbar__actions__sidebar">
                        <SidebarMobile {...props.sidebarMobile} />
                    </div>
                </div>
                <div ref={overlayRef} className="overlay"></div>
            </nav>
        </header>
    );
}

Header.propTypes = {
    logo: PropTypes.object.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
};

export default Header;