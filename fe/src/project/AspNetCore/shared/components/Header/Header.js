import React, { useState, useContext } from "react";
import { toast } from "react-toastify";
import Autosuggest from "react-autosuggest";
import PropTypes from "prop-types";
import { Button } from "@material-ui/core";
import LanguageSwitcher from "../../../../../shared/components/LanguageSwitcher/LanguageSwitcher";
import HeaderConstants from "./HeaderConstants";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import NavigationHelper from "../../../../../shared/helpers/globals/NavigationHelper";
import { Context } from "../../../../../shared/stores/Store";

function Header(props) {

    const [searchTerm, setSearchTerm] = useState(props.searchTerm ? props.searchTerm : "");
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
                headers: { "Content-Type": "application/json" }
            };

            const url = props.getSuggestionsUrl + "?" + QueryStringSerializer.serialize(searchParameters);

            return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

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
                        <form action={props.searchUrl} method="get" role="search" onSubmit={onSearchSubmit}>
                            <div className="field is-flex is-flex-centered search">
                                <Autosuggest
                                    suggestions={suggestions}
                                    onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                                    onSuggestionsClearRequested={() => setSuggestions([])}
                                    getSuggestionValue={getSuggestionValue}
                                    onSuggestionSelected={onSuggestionSelected}
                                    renderSuggestion={renderSuggestion}
                                    inputProps={searchInputProps} />
                                <Button style={{ maxHeight: "40px", marginLeft: "0.5rem"  }} type="submit" variant="contained" color="primary">
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
