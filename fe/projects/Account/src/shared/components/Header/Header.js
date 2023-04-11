import React, { Fragment, useState } from "react";
import PropTypes from "prop-types";
import clsx from "clsx";
import { makeStyles } from "@mui/styles";
import { Drawer, List, Divider, IconButton, ListItem, ListItemIcon, ListItemText } from "@mui/material";
import { Menu } from "@mui/icons-material"
import LanguageSwitcher from "../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import ColorConstants from "../../constants/ColorConstants";
import * as Icon from "react-feather";

const useStyles = makeStyles({
    fullList: {
        width: "auto"
    },
    darkMenuColor: {
        color: ColorConstants.swampColor()
    }
});

const drawerStyles = {
    width: "63px",
    borderRadius: 0
};

function Header(props) {

    const [isActive, setIsActive] = useState(false);
    const classes = useStyles();
    const [open, setOpen] = React.useState(false);

    const handleDrawerOpen = () => {
        setOpen(true);
    };

    const handleDrawerClose = (e) => {
        e.stopPropagation();
        setOpen(false);
    };

    const ListIcon = (name) => {

        const IconTag = Icon[name];

        return (
            <IconTag color={ColorConstants.swampColor()} />
        );
    };

    return (
        <header>
            <nav className="navbar is-spaced">
                <div className="navbar-brand">
                    {props.drawerMenuCategories &&
                        <Fragment>
                            <IconButton
                                color="inherit"
                                aria-label="open drawer"
                                onClick={handleDrawerOpen}
                                edge="start"
                                style={drawerStyles}
                                className={clsx(classes.menuButton, open && classes.hide)}>
                                <Menu />
                            </IconButton>
                            <Drawer anchor="top" open={open} onClose={handleDrawerClose}>
                                <div
                                    className={clsx(classes.fullList, classes.darkMenuColor)}
                                    role="presentation"
                                    onClick={handleDrawerOpen}
                                    onKeyDown={handleDrawerOpen}>
                                        <List>
                                            <ListItem button onClick={handleDrawerClose}>
                                                <ListItemIcon>{ListIcon(props.drawerBackIcon)}</ListItemIcon>
                                                <ListItemText primary={props.drawerBackLabel} />
                                            </ListItem>
                                        </List>
                                        <Divider /><Divider />
                                    {props.drawerMenuCategories.map((category, index) => (
                                        <Fragment key={index}>
                                            <List>
                                                {category.items.map((item, index) => (
                                                    <a href={item.url} key={index}>
                                                        <ListItem selected={item.isActive} button>
                                                            <ListItemIcon>{ListIcon(item.icon)}</ListItemIcon>
                                                            <ListItemText primary={item.title} />
                                                        </ListItem>
                                                    </a>
                                                ))}
                                            </List>
                                            <Divider />
                                        </Fragment>
                                    ))}
                                </div>
                            </Drawer>
                        </Fragment>
                    }
                    {props.logo &&
                        <a href={props.logo.targetUrl}>
                            <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} />
                        </a>
                    }
                    <div role="button" onClick={() => setIsActive(!isActive)} className={isActive ? "navbar-burger is-active" : "navbar-burger"} aria-label="menu" aria-expanded="false">
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                    </div>
                </div>
                <div className={isActive ? "navbar-menu is-active" : "navbar-menu"}>
                    <div className="navbar-start">
                        {props.links && props.links.length > 0 && props.links.map((link, index) => 
                                <a key={index} className="navbar-item" href={link.url}>{link.text}</a>
                            )
                        }
                    </div>
                    <div className="navbar-end">
                        {props.isLoggedIn && props.signOutLink &&
                            <div className="navbar-item">
                                <span className="welcome-text">{props.welcomeText} {props.name}, </span>
                                <a className="button is-text" href={props.signOutLink.url}>
                                    {props.signOutLink.text}
                                </a>
                            </div>
                        }
                        <div className="navbar-item">
                            <LanguageSwitcher {...props.languageSwitcher} />
                        </div>
                    </div>
                </div>
            </nav>
        </header>
    );
}

Header.propTypes = {
    drawerBackLabel: PropTypes.string,
    drawerBackIcon: PropTypes.string,
    logo: PropTypes.object.isRequired,
    links: PropTypes.array.isRequired,
    drawerMenuCategories: PropTypes.array,
    isLoggedIn: PropTypes.bool,
    signOutLink: PropTypes.object,
    welcomeText: PropTypes.string,
    name: PropTypes.string.isRequired
};

export default Header;