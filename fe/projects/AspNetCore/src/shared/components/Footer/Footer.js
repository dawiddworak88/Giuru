import React from "react";
import PropTypes from "prop-types";

const Footer = (props) => {

    return (
        <footer className="footer">
            <div className="footer_wrapper">
                <div className="footer_copyright">{props.copyright}</div>
                {props.links && props.links.length > 0 &&
                    <div className="footer_navigation">
                        <div className="footer_navigation_list">
                            {props.links.map((navigationItem, index) => (
                                <a key={index} href={navigationItem.url} className="footer_navigation_link">
                                    {navigationItem.text}
                                </a>
                            ))}
                        </div>
                    </div>
                }
            </div>
        </footer>
    );
}

Footer.propTypes = {
    links: PropTypes.array.isRequired,
    copyright: PropTypes.string.isRequired
};

export default Footer;