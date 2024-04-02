import React, { useState } from "react";
import { Button, Drawer } from "@mui/material";
import MenuIcon from "../../Icons/Menu"
import RemoveIcon from "../../Icons/Remove";

const SidebarMobile = (props) => {
    const [open, setOpen] = useState(false);

    return (
        <div className="sidebar">
            <Button
                className="sidebar__button"
                disableRipple
                sx={{ p: '0.3rem', minWidth: 24 }}
                onClick={() => setOpen(!open)}
            >
                <MenuIcon />
            </Button>
            <Drawer PaperProps={{ sx: { width: '80%' } }} open={open} onClose={() => setOpen(!open)}>
                <div className="sidebar__menu">
                    <div className="sidebar__menu__header is-flex is-justify-content-space-between is-align-items-center">
                        <div>
                            <a href={props.logo.targetUrl}>
                                <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} className="sidebar__menu__header__logo" />
                            </a>
                        </div>
                        <div className="sidebar__menu__header__close">
                            <Button
                                onClick={() => setOpen(!open)}
                                sx={{
                                    p: 0,
                                    minWidth: '24px',
                                }}
                            >
                                <RemoveIcon />
                            </Button>
                        </div>
                    </div>
                    <div className="sidebar__menu__links">
                        {props.links && props.links.map((link, index) =>
                            <div key={index}>
                                <a href={link.url} className="sidebar__menu__links__button has-text-weight-bold">{link.text}</a>
                            </div>
                        )}
                        {props.isLoggedIn ?
                            <a href={props.signOutLink.url} className="sidebar__menu__links__button">
                                {props.signOutLink.text}
                            </a> :

                            <a href={props.signInLink.url} className="sidebar__menu__links__button">
                                {props.signInLink.text}
                            </a>
                        }
                    </div>
                    <div className="sidebar__language is-flex">
                        {props.languageSwitcher.availableLanguages && props.languageSwitcher.availableLanguages.map((language, index) =>
                            <a key={index} href={language.url} className="sidebar__language__button">{language.text}</a>
                        )}
                    </div>
                </div>
            </Drawer>
        </div>
    )
}

export default SidebarMobile;