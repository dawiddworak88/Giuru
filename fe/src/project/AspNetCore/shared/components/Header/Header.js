import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { TextField, Button } from '@material-ui/core';
import LanguageSwitcher from '../../../../../shared/components/LanguageSwitcher/LanguageSwitcher';

import logo from '../../../../../shared/layouts/images/logo.png';

function Header(props) {

    const [search, setSearch] = useState('');

    return (
        <header>
            <nav className="navbar is-spaced">
                <div className="navbar-brand">
                    <a href={props.logo.targetUrl}>
                        <img src={logo} alt={props.logo.logoAltLabel} />
                    </a>
                </div>
                <div className="navbar-menu is-flex is-flex-wrap">
                    <div className="navbar-start">
                        <form action={props.searchUrl} method="get" role="search">
                            <div className="field is-flex is-flex-centered has-text-centered search">
                                <TextField className="search__field" id="search" name="search" label={props.searchPlaceholderLabel} fullWidth={true} value={search} onChange={(event) => setSearch(event.target.value)} />
                                <Button className="search__button" type="submit" variant="contained" color="primary">
                                    {props.searchLabel}
                                </Button>
                            </div>
                        </form>
                    </div>
                    <div className="navbar-end">
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
    searchPlaceholderLabel: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired,
    searchUrl: PropTypes.string.isRequired
}

export default Header;