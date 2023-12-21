import React from "react";
import PropTypes from "prop-types";

function Footer(props) {

    const links = props.links.map((link, index) => <div className="navbar-item" key={index}><a href={link.url}>{link.text}</a></div>);

    return (
        <footer className="footer">
            <div className="navbar">
                {links}
            </div>
            <div className="content has-text-centered has-text-white">
                {props.copyright}
            </div>
        </footer>
    );
}

Footer.propTypes = {
    links: PropTypes.array.isRequired,
    copyright: PropTypes.string.isRequired
};

export default Footer;