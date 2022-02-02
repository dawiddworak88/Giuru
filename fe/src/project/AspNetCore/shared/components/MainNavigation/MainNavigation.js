import React, {useState} from "react";
import PropTypes from "prop-types";
import { Close, Menu } from "@material-ui/icons";

function MainNavigation(props) {
    const [isOpen, setIsOpen] = useState(false);

    const openNavigation = () => {
        setIsOpen(!isOpen)
    }

    return (
        <nav className="main-nav">
            <div className="main-nav__mobile">
                <div className="icon" onClick={openNavigation}>
                    {isOpen ? <Close /> : <Menu />}
                </div>
            </div>
            <div className={`main-nav__links-container ${isOpen ? "active" : ""}`}>
                {props.links && props.links.map((link, index) => 
                    <a key={index} href={link.url}>{link.text}</a>
                )}
            </div>
        </nav>
    );
}

MainNavigation.propTypes = {
    links: PropTypes.array
};

export default MainNavigation;