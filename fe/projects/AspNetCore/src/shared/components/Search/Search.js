import React, { useState, useContext } from "react";
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
import PropTypes from "prop-types";

const Search = (props) => {
    const [suggestions, setSuggestions] = useState([]);
    const [searchHistory, setSearchHistory] = useState([]);
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
        NavigationHelper.redirect(suggestion.url);
        updateSearchHistory(suggestion);
        setSearchTerm('');
    };

    const onSidebarSuggestionSelected = (suggestion) => {
        NavigationHelper.redirect(suggestion.url);
        updateSearchHistory(suggestion);
        setOpen(false);
    };

    const getSuggestionValue = (suggestion) => {
        return `${suggestion.name ? suggestion.name : ""}${suggestion.attributes ? ` ${suggestion.attributes}` : ""}${suggestion.sku ? ` (${suggestion.sku})` : ""}`;
    };

    const updateSearchHistory = (suggestion) => {
        const searchHistory = JSON.parse(localStorage.getItem('searchHistory')) || [];
        if (!searchHistory.some(x => x.name === suggestion.name)) {
            localStorage.setItem('searchHistory', JSON.stringify([...searchHistory, suggestion]));
        }
    };

    const clearSearchHistory = () => {
        localStorage.removeItem('searchHistory');
        setSearchHistory([]);
    }

    const renderSuggestion = (suggestion) => {
        return (
            <div className="suggestion">
                <div className="is-flex is-align-items-center">
                    <div className="suggestion__icon">
                        <SearchIcon />
                    </div>
                    <div className="suggestion__text">
                        {suggestion.name ? suggestion.name : suggestion} {suggestion.attributes ? suggestion.attributes : ""} {suggestion.sku ? `(${suggestion.sku})` : ""}
                    </div>
                </div>
            </div>
        );
    };

    const renderSearchHistorySidebar = (suggestion) => {
        return (
            <Button
                className="sidebar__suggestions__item"
                sx={{ width: '100%', justifyContent: 'left', color: '#171717' }}
                disableRipple
                onClick={() => onSidebarSuggestionSelected(suggestion)}
            >
                {renderSuggestion(suggestion)}
            </Button>
        )
    }

    const searchInputProps = {
        placeholder: props.searchPlaceholderLabel,
        value: searchTerm,
        className: "search__field",
        onChange: (_, { newValue, method }) => {
            setSearchTerm(newValue)
        },
        onFocus: () => {
            setIsFocused(true);
            setSearchHistory(JSON.parse(localStorage.getItem('searchHistory') || "[]"))
        },
        onBlur: () => {
            setTimeout(() => {
                setIsFocused(false)
            }, 100)
        }
    };

    const noResultInformation = (query) => {
        return (
            <div>
                <div className="has-text-weight-bold pl-1 pb-3">{props.noResultText} "{query}"</div>
                <div className="pl-1 pb-3">{props.changeSearchTermText}</div>
            </div>
        )
    }

    const renderSearchHistory = () => {
        return (
            <div className="history">
                <div className="pb-2 is-flex">
                    <div className="history__text">
                        {props.userSearchHistoryText}
                    </div>
                    <Button
                        className="history__button"
                        disableRipple
                        onClick={clearSearchHistory}
                        sx={{
                            p: 0,
                            minWidth: 0,
                            textDecoration: 'underline',
                            color: '#373f49',
                            '&:hover': {
                                backgroundColor: "#fff",
                                textDecoration: 'underline'
                            }
                        }}
                    >
                        {props.clearText}
                    </Button>
                </div>
                <div>
                    {searchHistory.map((name, index) =>
                        <div key={index}>
                            {renderSearchHistorySidebar(name)}
                        </div>
                    )}
                </div>
            </div>
        )
    }

    function renderSuggestionsContainer({ containerProps, children, query }) {
        {props.overlayDisplaying(false)}
        return (
            <div>
                {!open &&
                    <div>
                        {isFocused &&
                            <div>
                                {searchTerm && searchTerm.length > 0 ?
                                    <div className="relative">
                                        {props.overlayDisplaying(true)}
                                        <div {...containerProps}>
                                            {children ? children :
                                                <div>
                                                    {noResultInformation(query)}
                                                </div>
                                            }
                                        </div>
                                    </div> :
                                    <div>
                                        {searchHistory && searchHistory.length > 0 &&
                                            <div className="relative">
                                                {props.overlayDisplaying(true)}
                                                <div {...containerProps}>
                                                    {renderSearchHistory()}
                                                </div>
                                            </div>
                                        }
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
            <div className="search__text is-flex">
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
                    />
                </div>
                <div className="search__text__remove">
                    {searchTerm.length > 0 &&
                        <Button
                            className="search__text__remove__button"
                            onClick={() => {
                                setSearchTerm("");
                                setSuggestions([]);
                            }}
                            sx={{
                                p: 0,
                                minWidth: '24px'
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
                {searchInput()}
                <div className="search__area">
                    <div className="search__area__desktop">
                        <FormControl fullWidth>
                            <Select
                                className="search__area__select"
                                value={searchArea}
                                onChange={(e) => {
                                    setSearchArea(e.target.value);
                                    setSuggestions([]);
                                }}
                                displayEmpty
                                IconComponent={props => <ArrowShowMoreCategoryIcon {...props} />}
                                sx={{
                                    boxShadow: 'none',
                                    borderRadius: 0,
                                    '.MuiOutlinedInput-notchedOutline': { border: 0 },
                                    '.MuiSelect-select': { p: '0.5rem 0.5rem 0.5rem 1.5rem', textAlign: 'center' },
                                    '.MuiSelect-iconOpen': { right: '16px' }
                                }}
                            >
                                {props.searchAreas && props.searchAreas.length > 0 && props.searchAreas.map((area, index) =>
                                    <MenuItem key={index} value={area.value}>
                                        <div className="is-flex pt-2">
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
            </div>
            <Drawer open={open} PaperProps={{ sx: { width: '100%' } }}>
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
                        {suggestions && suggestions.length > 0 ? suggestions.map((suggestion, index) =>
                            <Button
                                key={index}
                                className="sidebar__suggestions__item"
                                sx={{ width: '100%', justifyContent: 'left', color: '#171717' }}
                                disableRipple
                                onClick={() => onSidebarSuggestionSelected(suggestion)}
                            >
                                {renderSuggestion(suggestion)}
                            </Button>
                        ) :
                            <div>
                                {isFocused && searchTerm ?
                                    noResultInformation(searchTerm)
                                    :
                                    <div>
                                        {searchHistory && searchHistory.length > 0 &&
                                            renderSearchHistory()
                                        }
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

Search.propTypes = {
    searchPlaceholderLabel: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired,
    searchTerm: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
}

export default Search;