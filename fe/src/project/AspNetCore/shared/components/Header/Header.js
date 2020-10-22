import React, { useState, useContext } from "react";
import { toast } from "react-toastify";
import Autosuggest from 'react-autosuggest';
import PropTypes from "prop-types";
import { Button } from "@material-ui/core";
import LanguageSwitcher from "../../../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import HeaderConstants from "./HeaderConstants";
import FetchErrorHandler from "../../../../../shared/helpers/errorHandlers/FetchErrorHandler";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../../../../shared/stores/Store";
import DebounceHelper from "../../../../../shared/helpers/globals/DebounceHelper";

function Header(props) {

    const [searchTerm, setSearchTerm] = useState(props.searchTerm);
    const [suggestions, setSuggestions] = useState([]);
    const [, dispatch] = useContext(Context);

    const getSuggestionValue = (suggestion) => {

        return suggestion;
    };

    const renderSuggestion = (suggestion) => {
        return (
            <div className="suggestion">
                {suggestion}
            </div>
        )
    };

    const onSuggestionsFetchRequested = (args) => {

        if (args.value && args.value.length >= HeaderConstants.MinSearchTermLength()) {
        
            dispatch({ type: "SET_IS_LOADING", payload: true });

            const searchParameters = {

                searchTerm,
                size: HeaderConstants.SearchSuggenstionsSize()
            };

            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json" }
            };

            const url = props.getSuggestionsUrl + "?" + QueryStringSerializer.serialize(searchParameters);

            return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setSuggestions(() => []);
                        setSuggestions(() => jsonResponse);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(searchParameters, response, jsonResponse);
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
        }
    }

    const onSuggestionsClearRequested = () => {

        setSuggestions(() => []);
    }

    const onSuggestionSelected = (event, { suggestion }) => {

        NavigationHelper.redirect(props.searchUrl + '?' + 'searchTerm=' + encodeURI(suggestion));
    }

    const handleSuggestionChange = (event, args) => {

        setSearchTerm(args.newValue);
    }

    const searchInputProps = {
        placeholder: props.searchPlaceholderLabel,
        className: "search__field",
        value: searchTerm,
        onChange: handleSuggestionChange
    };

    return (
        <header>
            <nav className="navbar is-spaced">
                <div className="navbar-brand">
                    <a href={props.logo.targetUrl}>
                        <img src={props.logo.logoUrl} alt={props.logo.logoAltLabel} />
                    </a>
                </div>
                <div className="navbar-menu is-flex is-flex-wrap">
                    <div className="navbar-start">
                        <form action={props.searchUrl} method="get" role="search">
                            <div className="field is-flex is-flex-centered has-text-centered search">
                                <Autosuggest
                                    suggestions={suggestions}
                                    onSuggestionsFetchRequested={DebounceHelper.debounce(onSuggestionsFetchRequested)}
                                    onSuggestionsClearRequested={onSuggestionsClearRequested}
                                    getSuggestionValue={getSuggestionValue}
                                    onSuggestionSelected={onSuggestionSelected}
                                    renderSuggestion={renderSuggestion}
                                    inputProps={searchInputProps} />
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
    searchUrl: PropTypes.string.isRequired,
    searchTerm: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default Header;
