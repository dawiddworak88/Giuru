import React from "react";
import PropTypes from "prop-types";

function MainNavigation(props) {

    return (

        <nav className="main-nav">
            <div className="main-nav__links-container is-flex is-flex-centered has-text-centered">
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