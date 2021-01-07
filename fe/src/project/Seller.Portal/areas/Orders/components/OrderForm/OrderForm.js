import React, { useContext, useState, Fragment } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import Grid from '@material-ui/core/Grid';
import MomentUtils from '@date-io/moment';
import {
    MuiPickersUtilsProvider,
    KeyboardDatePicker,
} from '@material-ui/pickers';
import Autosuggest from "react-autosuggest";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import { TextField, Button, IconButton, CircularProgress } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import ClearIcon from "@material-ui/icons/Clear";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper } from "@material-ui/core";

function OrderForm(props) {

    const clientsProps = {
        options: props.clients,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);
    const [disable, setDisable] = useState(false);
    const [id, setId] = useState(props.id ? props.id : null);
    const [client, setClient] = useState(props.clientId ? props.clients.find((item) => item.id === props.clientId) : null);
    const [searchTerm, setSearchTerm] = useState("");
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState([]);
    const [referenceId, setReferenceId] = useState("");
    const [deliveryFrom, setDeliveryFrom] = useState(null);
    const [deliveryTo, setDeliveryTo] = useState(null);
    const [moreInfo, setMoreInfo] = useState("");
    const [orderItems, setOrderItems] = useState([]);
    const [suggestions, setSuggestions] = useState([]);

    const onSuggestionsFetchRequested = (args) => {

        if (args.value) {

            dispatch({ type: "SET_IS_LOADING", payload: true });

            const searchParameters = {

                searchTerm: args.value
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

        setProduct(suggestion);
    };

    function onSubmitForm() {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setId(jsonResponse.id);
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
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
        <section className="section section-small-padding client">
            <h1 className="subtitle is-4">{props.title}</h1>
            <form className="is-modern-form" method="post">
                {id &&
                    <input id="id" name="id" type="hidden" value={id} />
                }
                <div className="columns is-desktop">
                    <div className="column is-half">
                        <div className="field">
                            <Autocomplete
                                {...clientsProps}
                                id="client"
                                name="client"
                                fullWidth={true}
                                value={client}
                                onChange={(event, newValue) => {
                                    setClient(newValue);
                                }}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectClientLabel} margin="normal" />}
                            />
                        </div>
                    </div>
                </div>
                {client &&
                    <Fragment>
                        <div className="columns is-desktop">
                            <div className="column is-3">
                                <Autosuggest
                                    suggestions={suggestions}
                                    onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                                    onSuggestionsClearRequested={() => setSuggestions([])}
                                    getSuggestionValue={(suggestion) => {
                                        return suggestion;
                                    }}
                                    onSuggestionSelected={onSuggestionSelected}
                                    renderSuggestion={(suggestion) => {
                                        return (
                                            <div className="suggestion">
                                                {suggestion}
                                            </div>
                                        );
                                    }}
                                    inputProps={searchInputProps} />
                            </div>
                            <div className="column is-2">
                                <TextField id="quantity" name="quantity" type="number" inputProps={{ min: "1", step: "1" }} 
                                    label={props.quantityLabel} fullWidth={true} value={quantity} onChange={(e) => {

                                        e.preventDefault();
                                        setQuantity(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-1">
                                <TextField id="referenceId" name="referenceId" type="text" 
                                    label={props.referenceIdLabel} fullWidth={true}
                                    value={referenceId} onChange={(e) => {

                                        e.preventDefault();
                                        setReferenceId(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-1">
                                <MuiPickersUtilsProvider utils={MomentUtils}>
                                    <Grid container justify="space-around">
                                        <KeyboardDatePicker
                                            margin="normal"
                                            id="deliveryFrom"
                                            label={props.deliveryFromLabel}
                                            value={deliveryFrom}
                                            onChange={(e) => {

                                                e.preventDefault();
                                                setDeliveryFrom(e.target.value);
                                            }}
                                            InputProps={{
                                                endAdornment: (
                                                  <IconButton onClick={() => handleDeliveryFromChange(null)}>
                                                    <ClearIcon />
                                                  </IconButton>
                                                )
                                              }}
                                              InputAdornmentProps={{
                                                position: "start"
                                              }}
                                            KeyboardButtonProps={{
                                                'aria-label': props.changeDeliveryFromLabel,
                                            }} />
                                    </Grid>
                                </MuiPickersUtilsProvider>
                            </div>
                            <div className="column is-1">
                                <MuiPickersUtilsProvider utils={MomentUtils}>
                                    <Grid container justify="space-around">
                                        <KeyboardDatePicker
                                            margin="normal"
                                            id="deliveryTo"
                                            label={props.deliveryToLabel}
                                            value={deliveryTo}
                                            onChange={(e) => {

                                                e.preventDefault();
                                                setDeliveryTo(e.target.value);
                                            }}
                                            InputProps={{
                                                endAdornment: (
                                                  <IconButton onClick={() => handleDeliveryToChange(null)}>
                                                    <ClearIcon />
                                                  </IconButton>
                                                )
                                              }}
                                              InputAdornmentProps={{
                                                position: "start"
                                              }}
                                            KeyboardButtonProps={{
                                                'aria-label': props.changeDeliveryToLabel,
                                            }} />
                                    </Grid>
                                </MuiPickersUtilsProvider>
                            </div>
                            <div className="column is-2">
                                <TextField id="moreInfo" name="moreInfo" type="text" label={props.moreInfoLabel} 
                                fullWidth={true} value={moreInfo} multiline onChange={(e) => {

                                    e.preventDefault();
                                    setReferenceId(e.target.value);
                                }} />
                            </div>
                            <div className="column is-2">
                                <Button type="button" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                    {props.addText}
                                </Button>
                            </div>
                        </div>
                        <div className="orderItems">
                            {(orderItems && orderItems.length > 0) ?
                                (<div className="table-container">
                                    <div className="orderitems__table">
                                        <TableContainer component={Paper}>
                                            <Table aria-label={props.orderItemsLabel}>
                                                <TableHead>
                                                    <TableRow>
                                                        <TableCell></TableCell>
                                                        <TableCell>{props.skuLabel}</TableCell>
                                                        <TableCell>{props.nameLabel}</TableCell>
                                                        <TableCell>{props.quantityLabel}</TableCell>
                                                        <TableCell>{props.referenceIdLabel}</TableCell>
                                                        <TableCell>{props.deliveryFromLabel}</TableCell>
                                                        <TableCell>{props.deliveryToLabel}</TableCell>
                                                        <TableCell>{props.moreInfoLabel}</TableCell>
                                                    </TableRow>
                                                </TableHead>
                                                <TableBody>
                                                    {orderItems.map((item) => (
                                                        <TableRow key={item.id}>
                                                            <TableCell width="11%">
                                                                <Fab onClick={() => handleDeleteClick(item)} size="small" color="primary" aria-label={props.deleteLabel}>
                                                                    <DeleteIcon />
                                                                </Fab>
                                                            </TableCell>
                                                            <TableCell>{item.sku}</TableCell>
                                                            <TableCell>{item.name}</TableCell>
                                                            <TableCell>{item.quantity}</TableCell>
                                                            <TableCell>{item.referenceId}</TableCell>
                                                            <TableCell>{moment(item.deliveryFrom).local().format("L LT")}</TableCell>
                                                            <TableCell>{moment(item.deliveryTo).local().format("L LT")}</TableCell>
                                                            <TableCell>{item.moreInfo}</TableCell>
                                                        </TableRow>
                                                    ))}
                                                </TableBody>
                                            </Table>
                                        </TableContainer>
                                    </div>
                                </div>) :
                                (<section className="section is-flex-centered">
                                    <span className="is-title is-5">{props.noOrderItemsLabel}</span>
                                </section>)
                            }
                        </div>
                    </Fragment>
                }
                <div className="field">
                    <Button type="button" variant="contained" color="primary" disabled={state.isLoading || disable}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section >
    );
}

OrderForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    referenceIdLabel: PropTypes.string.isRequired,
    deliveryFromLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    selectClientLabel: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
    changeDeliveryFromLabel: PropTypes.string.isRequired,
    changeDeliveryToLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    clientRequiredErrorMessage: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    addText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    clients: PropTypes.array
};

export default OrderForm;
