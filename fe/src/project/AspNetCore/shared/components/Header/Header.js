import React, { useState } from 'react';
import PropTypes from 'prop-types';
import LanguageSwitcher from '../../../../../shared/components/LanguageSwitcher';

import logo from '../../../../../shared/layouts/images/logo.png';

function Header(props) {

    const isActiveHook = useState(false); const isActive = isActiveHook[0]; const setIsActive = isActiveHook[1];

    const links = props.links.map(link => <a key={link.uniqueId} className="navbar-item" href={link.url}>{link.text}</a>);

    return (
        <header>
            <nav className="navbar is-spaced">
                <div className="navbar-brand">
                    <a href={props.logo.targetUrl}>
                        <img src={logo} alt={props.logo.logoAltLabel} />
                    </a>
                    <div role="button" onClick={() => setIsActive(!isActive)} className={isActive ? 'navbar-burger is-active' : 'navbar-burger'} aria-label="menu" aria-expanded="false">
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                    </div>
                </div>
                <div className={isActive ? 'navbar-menu is-active' : 'navbar-menu'}>
                    <div className="navbar-start">
                        {links}
                    </div>
                    <div className="navbar-end">
                        <a href={props.loginLink.url}>{props.loginLink.text}</a>
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
    logo: PropTypes.object.isRequired,
    links: PropTypes.array.isRequired,
    loginLink: PropTypes.object.isRequired
}

export default Header;