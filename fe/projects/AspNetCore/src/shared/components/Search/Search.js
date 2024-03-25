import React, { useState, useContext } from "react";
import Autosuggest from "react-autosuggest";
import { toast } from "react-toastify";
import { Button, FormControl, MenuItem, Select, Drawer } from "@mui/material";
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
    const [searchArea, setSearchArea] = useState(1);
    const [searchTerm, setSearchTerm] = useState(props.searchTerm ? props.searchTerm : "");
    const [state, dispatch] = useContext(Context);
    const [open, setOpen] = useState(false);

    const getSuggestionValue = (suggestion) => {
        return suggestion;
    };

    const renderSuggestion = (suggestion) => {
        return (
            <div className="suggestion">
                {suggestion && suggestion.length > 0 ?
                    <div className="is-flex">
                        {console.log("siema")}
                        <div className="suggestion__icon">
                            <SearchIcon />
                        </div>
                        <div className="suggestion__text">
                            {suggestion}
                        </div>
                    </div>
                    :
                    <div className="suggestion">
                        {console.log("lol")}
                        No results found for the searched phrase "{searchTerm}"
                    </div>
                }
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
            setSearchTerm(newValue)
            setSuggestions(['Narożnik Amando', 'Narożnik Ume',]);
        }
    };

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
                        onSuggestionsClearRequested={() => setSuggestions([])}
                        getSuggestionValue={getSuggestionValue}
                        onSuggestionSelected={onSuggestionSelected}
                        renderSuggestion={renderSuggestion}
                        inputProps={searchInputProps}
                        shouldRenderSuggestions={(value, reason) => {
                            return reason == 'input-focused';
                        }}
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
                {searchInput()}
                <div className="search__area">
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
                            <MenuItem value={1}>
                                <div className="is-flex pt-2 is-justify-content-center">
                                    <div className="pr-2">
                                        <CategoryIcon />
                                    </div>
                                    <div className="search__area__select__text">
                                        Wszytkie
                                    </div>
                                </div>
                            </MenuItem>
                            <MenuItem value={2}>
                                <div className="is-flex pt-2">
                                    <div className="pr-2">
                                        <CategoryIcon />
                                    </div>
                                    <div className="search__area__select__text">
                                        Stany magazynowe
                                    </div>
                                </div>
                            </MenuItem>
                        </Select>
                    </FormControl>
                </div>
            </div>
            <Drawer open={open} PaperProps={{ sx: { width: '100%' } }}>
                <div className="sidebar">
                    <div className="sidebar__header is-flex">
                        <div className="sidebar__header__back">
                            <Button
                                onClick={() => setOpen(false)}
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
                        <Button disableRipple className="sidebar__header__areas__button">
                            Wszytko
                        </Button>
                        <Button disableRipple className="sidebar__header__areas__button">
                            Stany magazynowe
                        </Button>
                    </div>
                </div>
            </Drawer>
        </div>
    )
}

export default Search;