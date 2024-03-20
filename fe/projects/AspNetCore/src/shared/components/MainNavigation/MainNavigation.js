import React from "react";
import PropTypes from "prop-types";

function MainNavigation(props) {

    return (
        <nav className="main-nav">
            <div className='main-nav__links-container'>
                {props.links && props.links.map((link, index) => 
                    <a key={index} href={link.url} target={link.target}>{link.text}</a>
                )}
            </div>
        </nav>
    );
}

MainNavigation.propTypes = {
    links: PropTypes.array
};

export default MainNavigation;