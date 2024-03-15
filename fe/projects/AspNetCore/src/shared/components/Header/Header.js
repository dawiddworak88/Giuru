import React, { useState, useContext, useEffect } from "react";
import { toast } from "react-toastify";
import Autosuggest from "react-autosuggest";
import PropTypes from "prop-types";
import { Button, FormControl, Icon, InputLabel, MenuItem, Select } from "@mui/material";
import LanguageSwitcher from "../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import HeaderConstants from "./HeaderConstants";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../stores/Store";
import CategoryIcon from "../../Icons/Category";
import RemoveIcon from "../../Icons/Remove";
import SearchIcon from "../../Icons/Search";
import { ShoppingCart, ExpandMoreOutlined } from '@mui/icons-material';
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import UserPopup from "../UserPopup/UserPopup";

function Header(props) {
    const [state, dispatch] = useContext(Context);
    const [searchTerm, setSearchTerm] = useState(props.searchTerm ? props.searchTerm : "");
    const [suggestions, setSuggestions] = useState([]);
    const [totalBasketItems, setTotalBasketItems] = useState(props.totalBasketItems ? props.totalBasketItems : 0);
    const [searchArea, setSearchArea] = useState(1);

    const getSuggestionValue = (suggestion) => {
        return suggestion;
    };

    const renderSuggestion = (suggestion) => {
        return (
            <div className="suggestion">
                {suggestion}
            </div>
        );
    };

    const onSuggestionsFetchRequested = (args) => {
        setSearchTerm(args.value);

        if (args.value && args.value.length >= HeaderConstants.minSearchTermLength()) {
        
            dispatch({ type: "SET_IS_LOADING", payload: true });

            const searchParameters = {

                searchTerm: args.value,
                size: HeaderConstants.searchSuggenstionsSize()
            };

            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
            };

            const url = props.getSuggestionsUrl + "?" + QueryStringSerializer.serialize(searchParameters);

            return fetch(url, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setSuggestions(() => []);
                        setSuggestions(() => jsonResponse);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
        }
    };

    const onSuggestionSelected = (event, { suggestion }) => {
        NavigationHelper.redirect(props.searchUrl + "?searchTerm=" + encodeURI(suggestion));
    };

    const onSearchSubmit = (e) => {
        e.preventDefault();

        if (searchTerm && searchTerm.length >= HeaderConstants.minSearchTermLength()) {
            NavigationHelper.redirect(props.searchUrl + "?searchTerm=" + encodeURI(searchTerm));
        }
    };

    const searchInputProps = {
        placeholder: props.searchPlaceholderLabel,
        value: searchTerm,
        className: "search__field",
        onChange: (_, { newValue, method }) => {
            setSearchTerm(newValue);
        }
    };

    useEffect(() => {
        const totalItems = state.totalBasketItems;
        if (totalItems != null && totalItems < props.totalBasketItems){
            state.totalBasketItems = props.totalBasketItems;
        }

        setTotalBasketItems(state.totalBasketItems);

    }, [state])

    return (
        <header>
            <nav className="is-flex p-4 px-4 is-align-items-center header">
                <div className="navbar__start is-flex is-justify-content-center">
                    <a href={props.logo.targetUrl}>
                        <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} className="navbar__start__logo"/>
                    </a>
                </div>
                <div className="navbar__search is-flex">
                    <div className="navbar__search__text is-flex">
                        <div className="navbar__search__text__icon">
                            <SearchIcon />
                        </div>
                        <div className="navbar__search__text__input">
                            <Autosuggest
                                suggestions={suggestions}
                                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                                onSuggestionsClearRequested={() => setSuggestions([])}
                                getSuggestionValue={getSuggestionValue}
                                onSuggestionSelected={onSuggestionSelected}
                                renderSuggestion={renderSuggestion}
                                inputProps={searchInputProps}
                            />
                            </div>
                        <div className="navbar__search__text__remove">
                            { searchTerm.length > 0 &&
                                <Button 
                                    onClick={() => setSearchTerm("")}
                                    sx={{ 
                                        p: 0,
                                        minWidth: '24px',
                                    }}
                                >
                                    <RemoveIcon />
                                </Button>
                            }
                        </div>
                    </div>
                    <div className="navbar__search__area">
                        <FormControl fullWidth>
                            <Select
                                className="navbar__search__area__select"
                                value={searchArea}
                                onChange={(e) => {setSearchArea(e.target.value)}}
                                displayEmpty
                                IconComponent={ExpandMoreOutlined}
                                sx={{ 
                                    boxShadow: 'none',
                                    borderRadius: 0,
                                    '.MuiOutlinedInput-notchedOutline': { border: 0 },
                                    '.MuiSelect-select': { p: 1, textAlign: 'center' }
                                    }}
                                >
                                <MenuItem value={1}>
                                    <div className="is-flex pt-2 is-justify-content-center">
                                        <div className="pr-2">
                                            <CategoryIcon />
                                        </div>
                                        <div>
                                            Wszytkie
                                        </div>
                                    </div>
                                </MenuItem>
                                <MenuItem value={2}>
                                <div className="is-flex pt-2">
                                        <div className="pr-2">
                                            <CategoryIcon />
                                        </div>
                                        <div>
                                            Stany magazynowe
                                        </div>
                                    </div>
                                </MenuItem>
                            </Select>
                        </FormControl>
                    </div>
                </div>
                <div className="navbar__actions is-flex is-justify-content-center">
                    <div className="navbar__actions__language">
                        <LanguageSwitcher {...props.languageSwitcher} />
                    </div>
                    <UserPopup {...props.userPopup} />
                </div>
            </nav>
        </header>
    );
}

Header.propTypes = {
    logo: PropTypes.object.isRequired,
    searchPlaceholderLabel: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired,
    searchUrl: PropTypes.string.isRequired,
    searchTerm: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    isLoggedIn: PropTypes.bool,
    signOutLink: PropTypes.object
};

export default Header;

{/* <div className="navbar-brand is-align-items-center">
<a className="navbar-logo" href={props.logo.targetUrl}>
    <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} />
</a>
<div className="navbar-start">
    <form action={props.searchUrl} method="get" role="search" onSubmit={onSearchSubmit}>
        <div className="field is-flex is-flex-centered search">
            <Autosuggest
                suggestions={suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={() => setSuggestions([])}
                getSuggestionValue={getSuggestionValue}
                onSuggestionSelected={onSuggestionSelected}
                renderSuggestion={renderSuggestion}
                inputProps={searchInputProps} 
            />
            <Button type="submit" variant="contained" color="secondary" className="search-button">
                {props.searchLabel}
            </Button>
        </div>
    </form>
</div>
</div>
<div className="navbar-container">
<div className="navbar-end is-flex is-align-items-center">
    {props.isLoggedIn ? (
        props.signOutLink &&
            <div className="navbar-item">
                <span className="welcome-text">{props.welcomeText} {props.name}, </span>
                <a href={props.signOutLink.url} className="button is-text">{props.signOutLink.text}</a>
            </div>
    ) : (
        props.signInLink &&
            <div className="navbar-item">
                <a className="button is-text" href={props.signInLink.url}>
                    {props.signInLink.text}
                </a>
            </div>
    )}
    <div className="navbar-item">
        <LanguageSwitcher {...props.languageSwitcher} />
    </div>
    <div className="navbar-item">
        <a href={props.basketUrl} className="button is-text" title={props.goToCartLabel} aria-label={props.goToCartLabel}>
            <ShoppingCart />
            {totalBasketItems > 0 &&
                <span className="count">{totalBasketItems}</span>
            }
        </a>
    </div>
</div>
</div> */}