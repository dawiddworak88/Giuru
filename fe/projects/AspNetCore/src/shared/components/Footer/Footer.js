import React from "react";
import PropTypes from "prop-types";

function Footer(props) {
    const links = props.links.map((link, index) => <li key={index}><a href={link.url} target={link.target}>{link.text}</a></li>);

    return (
        <footer className="footer">
            <div className="content">
                <ul>
                    {links}
                </ul>
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