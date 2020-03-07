import React from 'react';
import PropTypes from 'prop-types';

function Footer(props) {

    const links = props.links.map(link => <li key={link.uniqueId}><a href={link.url}>{link.text}</a></li>);

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
}

export default Footer;