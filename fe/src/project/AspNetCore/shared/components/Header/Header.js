import React, { useState } from 'react';
import PropTypes from 'prop-types';

import logo from '../../layouts/images/logo.png';

function Header(props) {

    const [isActive, setIsActive] = useState(false);

    return (
        <header>
            <nav className="navbar is-spaced">
                <div className="navbar-brand">
                    <a href={props.logo.targetUrl}>
                        <img src={logo} alt={props.logo.logoAltLabel} />
                    </a>
                    <a role="button" onClick={() => setIsActive(!isActive)} className={isActive ? 'navbar-burger is-active' : 'navbar-burger'} aria-label="menu" aria-expanded="false">
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                        <span aria-hidden="true"></span>
                    </a>
                </div>
                <div className={isActive ? 'navbar-menu is-active' : 'navbar-menu'}>
                    <div className="navbar-start">
                    </div>
                    <div className="navbar-end">
                        <div className="navbar-item">
                            <div className="buttons m-b-0">
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
        </header>
    );
}

Header.propTypes = {
    logo: PropTypes.object.isRequired
}

export default Header;