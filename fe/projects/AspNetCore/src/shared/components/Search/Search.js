import React, { useState, useContext, useEffect } from "react";
import Autosuggest from "react-autosuggest";
import { toast } from "react-toastify";
import { Button, FormControl, MenuItem, Select, Drawer, Tabs, Tab } from "@mui/material";
import CategoryIcon from "../../Icons/Category";
import RemoveIcon from "../../Icons/Remove";
import SearchIcon from "../../Icons/Search";
import BackIcon from "../../Icons/Back";
import ArrowShowMoreCategoryIcon from "../../Icons/ArrowShowMoreCategory";
import HeaderConstants from "../Header/HeaderConstants";
import AuthenticationHelper from "../../helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../helpers/globals/NavigationHelper";
import QueryStringSerializer from "../../helpers/serializers/QueryStringSerializer";
import { Context } from "../../stores/Store";

const Search = (props) => {
    const [suggestions, setSuggestions] = useState([]);
    const [searchArea, setSearchArea] = useState(HeaderConstants.searchAreaAllValue);
    const [searchTerm, setSearchTerm] = useState(props.searchTerm ? props.searchTerm : "");
    const [state, dispatch] = useContext(Context);
    const [open, setOpen] = useState(false);
    const [isFocused, setIsFocused] = useState(false);

    const onSuggestionsFetchRequested = (args) => {
        setSearchTerm(args.value);

        if (args.value && args.value.length >= HeaderConstants.minSearchTermLength()) {

            dispatch({ type: "SET_IS_LOADING", payload: true });

            const searchParameters = {

                searchTerm: args.value,
                size: HeaderConstants.searchSuggenstionsSize(),
                searchArea: searchArea
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

    const onSuggestionsClearRequest = () => {
        if (searchTerm.length < 1) {
            setSuggestions([]);
        }
    }

    const onSuggestionSelected = (event, { suggestion }) => {
        NavigationHelper.redirect(props.searchUrl + "?searchTerm=" + encodeURI(suggestion) + "&searchArea=" + encodeURI(searchArea));
        setSearchTerm('');
    };

    const onSidebarSuggestionSelected = (name) => {
        NavigationHelper.redirect(props.searchUrl + "?searchTerm=" + encodeURI(name));
    };

    useEffect(() => {
        const keyDownHandler = event => {
            if (event.key === 'Enter') {
                onSearchSubmit(event);
            }
        };

        document.addEventListener('keydown', keyDownHandler);

        return () => {
            document.removeEventListener('keydown', keyDownHandler);
        }
    }, []);

    const onSearchSubmit = (e) => {
        e.preventDefault();

        if (searchTerm && searchTerm.length >= HeaderConstants.minSearchTermLength()) {
            NavigationHelper.redirect(props.searchUrl + "?searchTerm=" + encodeURI(searchTerm));
        }
    };

    const getSuggestionValue = (suggestion) => {
        return suggestion;
    };

    const renderSuggestion = (suggestion) => {
        return (
            <div className="suggestion">
                <div className="is-flex">
                    <div className="suggestion__icon">
                        <SearchIcon />
                    </div>
                    <div className="suggestion__text">
                        {suggestion}
                    </div>
                </div>
            </div>
        );
    };

    const searchInputProps = {
        placeholder: props.searchPlaceholderLabel,
        value: searchTerm,
        className: "search__field",
        onChange: (_, { newValue, method }) => {
            setSearchTerm(newValue)
        },
        onFocus: () => setIsFocused(true),
        onBlur: () => setIsFocused(false)
    };

    const noResultInformation = (query) => {
        return (
            <div>
                <div className="has-text-weight-bold p-1">{props.noResultText} "{query}"</div>
                <div className="pl-1">{props.changeSearchTermText}</div>
            </div>
        )
    }

    function renderSuggestionsContainer({ containerProps, children, query }) {
        return (
            <div>
                {!open &&
                    <div>
                        {isFocused && searchTerm && searchTerm.length > 0 &&
                            <div {...containerProps}>
                                {children ? children :
                                    <div>
                                        {noResultInformation(query)}
                                    </div>
                                }
                            </div>
                        }
                    </div>
                }
            </div>
        );
    }

    const searchInput = () => {
        return (
            <div className={`search__text is-flex ${open ? "search__text__no-border" : ""}`}>
                <div className="search__text__icon">
                    <SearchIcon />
                </div>
                <div className="search__text__input">
                    <Autosuggest
                        suggestions={suggestions}
                        onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                        onSuggestionsClearRequested={onSuggestionsClearRequest}
                        getSuggestionValue={getSuggestionValue}
                        onSuggestionSelected={onSuggestionSelected}
                        renderSuggestion={renderSuggestion}
                        inputProps={searchInputProps}
                        renderSuggestionsContainer={renderSuggestionsContainer}
                        shouldRenderSuggestions={() => !open}
                    />
                </div>
                <div className="search__text__remove">
                    {searchTerm.length > 0 &&
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
        )
    }

    return (
        <div className="search">
            <div className="is-flex">
                <form onSubmit={onSearchSubmit} className="search is-flex">
                    {searchInput()}
                    <div className="search__area">
                        <div className="search__area__desktop">
                            <FormControl fullWidth>
                                <Select
                                    className="search__area__select"
                                    value={searchArea}
                                    onChange={(e) => { setSearchArea(e.target.value) }}
                                    displayEmpty
                                    IconComponent={props => <ArrowShowMoreCategoryIcon {...props} />}
                                    sx={{
                                        boxShadow: 'none',
                                        borderRadius: 0,
                                        '.MuiOutlinedInput-notchedOutline': { border: 0 },
                                        '.MuiSelect-select': { p: 1, textAlign: 'center' },
                                        '.MuiSelect-iconOpen': { right: '16px' },
                                    }}
                                >
                                    {props.searchAreas && props.searchAreas.length > 0 && props.searchAreas.map((area, index) =>
                                        <MenuItem key={index} value={area.value}>
                                            <div className="is-flex pt-2 is-justify-content-center">
                                                <div className="pr-2">
                                                    <CategoryIcon />
                                                </div>
                                                <div className="search__area__select__text">
                                                    {area.name}
                                                </div>
                                            </div>
                                        </MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="search__area__mobile">
                            <Button
                                disableRipple
                                onClick={() => setOpen(true)}
                                sx={{ pt: '0.9rem', pl: '0.8rem' }}
                            >
                                <div className="pr-2">
                                    <CategoryIcon />
                                </div>
                                <div>
                                    <ArrowShowMoreCategoryIcon />
                                </div>
                            </Button>
                        </div>
                    </div>
                </form>
            </div>
            <Drawer open={open} PaperProps={{ sx: { width: '100%' } }} className="lol">
                <div className="sidebar">
                    <div className="sidebar__header is-flex">
                        <div className="sidebar__header__back">
                            <Button
                                onClick={() => {
                                    setOpen(false)
                                    setSearchTerm('')
                                }}
                                disableRipple
                                sx={{
                                    p: 0,
                                    minWidth: '24px',
                                }}
                            >
                                <BackIcon />
                            </Button>
                        </div>
                        <div className="search is-flex">
                            {searchInput()}
                        </div>
                    </div>
                    <div className="sidebar__header__areas is-flex">
                        <Tabs value={searchArea} onChange={(e, newValue) => setSearchArea(newValue)}>
                            {props.searchAreas && props.searchAreas.length > 0 && props.searchAreas.map((area, index) => 
                                <Tab key={index} label={area.name} value={area.value} className="sidebar__header__areas__button" disableRipple />
                            )}
                        </Tabs>
                    </div>
                    <div className="sidebar__suggestions">
                        {suggestions && suggestions.length > 0 ? suggestions.map((name, index) =>
                            <Button
                                key={index}
                                className="sidebar__suggestions"
                                sx={{ width: '100%', justifyContent: 'left', color: '#171717' }}
                                disableRipple
                                onClick={() => onSidebarSuggestionSelected(name)}
                            >
                                {renderSuggestion(name)}
                            </Button>
                        ) :
                            <div>
                                {isFocused && searchTerm &&
                                    <div>
                                        {noResultInformation(searchTerm)}
                                    </div>
                                }
                            </div>
                        }
                    </div>
                </div>
            </Drawer>
        </div>
    )
}

export default Search;