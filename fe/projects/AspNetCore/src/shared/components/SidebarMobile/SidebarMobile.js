import React, { useState } from "react";
import { Button, Drawer } from "@mui/material";
import MenuIcon from "../../Icons/Menu"
import CancelIcon from "../../Icons/Cancel";
import PropTypes from "prop-types";

const SidebarMobile = (props) => {
    const [open, setOpen] = useState(false);

    return (
        <div className="sidebar-mobile">
            <Button
                className="sidebar-mobile__button"
                disableRipple
                sx={{ p: "1rem 0 1rem 1rem", minWidth: 24 }}
                onClick={() => setOpen(!open)}
            >
                <MenuIcon />
            </Button>
            <Drawer PaperProps={{ sx: { width: '80%' } }} open={open} onClose={() => setOpen(!open)}>
                <div className="sidebar-mobile__menu">
                    <div className="sidebar-mobile__menu__header is-flex is-justify-content-space-between is-align-items-center">
                        <div>
                            <a href={props.logo.targetUrl}>
                                <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} className="sidebar-mobile__menu__header__logo pt-1" />
                            </a>
                        </div>
                        <div className="sidebar-mobile__menu__header__close">
                            <Button
                                onClick={() => setOpen(!open)}
                                sx={{
                                    p: 0,
                                    minWidth: '24px',
                                    fontSize: '16px'
                                }}
                            >
                                <CancelIcon />
                            </Button>
                        </div>
                    </div>
                    <div className="sidebar-mobile__menu__links">
                        {props.links && props.links.map((link, index) =>
                            <div key={index}>
                                <a href={link.url} className="sidebar-mobile__menu__links__button has-text-weight-bold">{link.text}</a>
                            </div>
                        )}
                        {props.isLoggedIn ?
                            <a href={props.signOutLink.url} className="sidebar-mobile__menu__links__button">
                                {props.signOutLink.text}
                            </a> :

                            <a href={props.signInLink.url} className="sidebar-mobile__menu__links__button">
                                {props.signInLink.text}
                            </a>
                        }
                    </div>
                    <div className="sidebar-mobile__language is-flex">
                        {props.languageSwitcher.availableLanguages && props.languageSwitcher.availableLanguages.map((language, index) =>
                            <div key={index}>
                                <a
                                    href={language.url}
                                    className={`sidebar-mobile__language__button ${language.text == props.languageSwitcher.selectedLanguageText ? "selected" : ""}`}>
                                    {language.text}
                                </a>
                            </div>
                        )}
                    </div>
                </div>
            </Drawer>
        </div>
    )
}

SidebarMobile.propTypes = {
    logo: PropTypes.object.isRequired,
    links: PropTypes.array,
    sLoggedIn: PropTypes.bool,
    signOutLink: PropTypes.object,
    signInLink: PropTypes.object
}

export default SidebarMobile;