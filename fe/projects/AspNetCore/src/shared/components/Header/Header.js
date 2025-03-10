import React, { useState, useContext, useEffect } from "react";
import { toast } from "react-toastify";
import Autosuggest from "react-autosuggest";
import PropTypes from "prop-types";
import { Button } from "@mui/material";
import LanguageSwitcher from "../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import HeaderConstants from "./HeaderConstants";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../stores/Store";
import { ShoppingCart } from '@mui/icons-material';
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";

function Header(props) {
    const [state, dispatch] = useContext(Context);
    const [searchTerm, setSearchTerm] = useState(props.searchTerm ? props.searchTerm : "");
    const [suggestions, setSuggestions] = useState([]);
    const [totalBasketItems, setTotalBasketItems] = useState(props.totalBasketItems ? props.totalBasketItems : 0);

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
        className: "search__field",
        value: searchTerm,
        onChange: (_, { newValue, method }) => {
            setSearchTerm(newValue);
        }
    };

    useEffect(() => {
        const totalItems = state.totalBasketItems;
        if (totalItems != null && totalItems < props.totalBasketItems) {
            state.totalBasketItems = props.totalBasketItems;
        }

        setTotalBasketItems(state.totalBasketItems);

    }, [state])

    return (
        <header>
            <nav className="is-flex is-justify-content-space-between p-3 px-4 is-align-items-center header">
                <div className="navbar-brand is-align-items-center">
                    <a className="navbar-logo" href={props.logo.targetUrl}>
                        <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} />
                    </a>
                    {props.isLoggedIn &&
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
                                    <Button type="submit" variant="contained" color="secondary" className="search-button ml-2">
                                        {props.searchLabel}
                                    </Button>
                                </div>
                            </form>
                        </div>
                    }
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
