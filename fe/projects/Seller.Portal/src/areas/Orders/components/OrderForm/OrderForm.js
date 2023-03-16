import React, { useContext, useCallback, useState, Fragment } from "react";
import { toast } from "react-toastify";
import { UploadCloud } from "react-feather";
import { useDropzone } from "react-dropzone";
import PropTypes from "prop-types";
import { DatePicker, LocalizationProvider } from "@mui/lab";
import Autosuggest from "react-autosuggest";
import { Context } from "../../../../../../../shared/stores/Store";
import { Delete, AddShoppingCartRounded } from "@mui/icons-material"
import {
    Fab, Table, TableBody, TableCell, TableContainer, Autocomplete, 
    TableHead, TableRow, Paper, TextField, Button, CircularProgress
} from "@mui/material";
import moment from "moment";
import AdapterMoment from '@mui/lab/AdapterMoment';
import QueryStringSerializer from "../../../../../../../shared/helpers/serializers/QueryStringSerializer";
import OrderFormConstants from "../../../../../../../shared/constants/OrderFormConstants";
import ConfirmationDialog from "../../../../../../../shared/components/ConfirmationDialog/ConfirmationDialog";
import IconConstants from "../../../../../../../shared/constants/IconConstants";
import AuthenticationHelper from "../../../../../../../shared/helpers/globals/AuthenticationHelper";

function OrderForm(props) {
    const clientsProps = {
        options: props.clients,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);
    const [id, setId] = useState(props.id ? props.id : null);
    const [basketId, setBasketId] = useState(null);
    const [client, setClient] = useState(props.clientId ? props.clients.find((item) => item.id === props.clientId) : null);
    const [searchTerm, setSearchTerm] = useState("");
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [externalReference, setExternalReference] = useState("");
    const [deliveryFrom, setDeliveryFrom] = useState(null);
    const [deliveryTo, setDeliveryTo] = useState(null);
    const [moreInfo, setMoreInfo] = useState("");
    const [orderItems, setOrderItems] = useState([]);
    const [suggestions, setSuggestions] = useState([]);
    const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
    const [entityToDelete, setEntityToDelete] = useState(null);
    const [disableSaveButton, setDisableSaveButton] = useState(false);

    const onSuggestionsFetchRequested = (args) => {

        if (args.value && args.value.length >= OrderFormConstants.minSuggestionSearchTermLength()) {

            const searchParameters = {
                searchTerm: args.value,
                pageIndex: 1,
                hasPrimaryProduct: true,
                itemsPerPage: OrderFormConstants.productSuggestionsNumber()
            };

            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
            };

            const url = props.getSuggestionsUrl + "?" + QueryStringSerializer.serialize(searchParameters);

            return fetch(url, requestOptions)
                .then(function (response) {

                    AuthenticationHelper.HandleResponse(response);

                    return response.json().then(jsonResponse => {

                        if (response.ok) {
                            setId(jsonResponse.id);
                            setSuggestions(() => []);
                            setSuggestions(() => jsonResponse.data);
                        }
                        else {
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch(() => {
                    toast.error(props.generalErrorMessage);
                });
        }
    };

    const onSuggestionSelected = (event, { suggestion }) => {
        setProduct(suggestion);
    };

    const getProductSuggestionValue = (suggestion) => {

        return "(" + suggestion.sku + ")" + " " + suggestion.name;
    };

    const handleAddOrderItemClick = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const orderItem = {
            productId: product.id,
            sku: product.sku,
            name: product.name,
            imageId: product.images ? product.images[0] : null,
            quantity,
            externalReference,
            deliveryFrom: moment(deliveryFrom).startOf("day"),
            deliveryTo: moment(deliveryTo).startOf("day"),
            moreInfo
        };

        const basket = {

            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setBasketId(jsonResponse.id);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {

                            setProduct(null);
                            setSearchTerm("");
                            setExternalReference("");
                            setQuantity(1);
                            setOrderItems(jsonResponse.items);
                        }
                        else {

                            setOrderItems([]);
                        }
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

    const handleDeleteClick = (item) => {
        setEntityToDelete(item);
        setOpenDeleteDialog(true);
    };

    const handleDeleteDialogClose = () => {
        setOpenDeleteDialog(false);
        setEntityToDelete(null);
    };

    const handleDeleteEntity = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const basket = {
            id: basketId,
            items: orderItems.filter((orderItem) => orderItem !== entityToDelete)
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(basket)
        };

        fetch(props.updateBasketUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setBasketId(jsonResponse.id);
                        setOpenDeleteDialog(false);

                        if (jsonResponse.items && jsonResponse.items.length > 0) {

                            setOrderItems(jsonResponse.items);
                        }
                        else {

                            setOrderItems([]);
                        }
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

    const handlePlaceOrder = () => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        var order = {
            basketId,
            clientId: client.id,
            clientName: client.name
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(order)
        };

        fetch(props.placeOrderUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        toast.success(jsonResponse.message);
                        setDisableSaveButton(true);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };
    
    const onDrop = useCallback(acceptedFiles => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        acceptedFiles.forEach((file) => {

            const formData = new FormData();

            formData.append("file", file);

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.uploadOrderFileUrl, requestOptions)
                .then(function (response) {
                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    return response.json().then((jsonResponse) => {

                        if (response.ok) {

                            dispatch({ type: "SET_IS_LOADING", payload: false });

                            setBasketId(jsonResponse.id);
                            setOrderItems([...orderItems, ...jsonResponse.items]);
                        }
                        else {
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch(() => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.generalErrorMessage);
                });
        });
    }, []);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        multiple: false,
        accept: {
            "application/*": [".xls", ".xlsx"]
        }
    });

    return (
        <section className="section section-small-padding order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="is-modern-form">
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
                                renderInput={(params) => <TextField {...params} label={props.selectClientLabel} margin="normal" variant="standard" />}
                            />
                        </div>
                    </div>
                </div>
                {client &&
                    <Fragment>
                        <h2 className="subtitle is-5 pb-2">{props.orderItemsLabel}</h2>
                        <div className="container">
                            <div className="dropzone__pond-container" {...getRootProps()}>
                                <input id={props.id} name={props.name} {...getInputProps()} />
                                <div className={isDragActive ? "dropzone__pond dropzone--active" : "dropzone__pond"}>
                                    <p>
                                        <UploadCloud size={IconConstants.defaultSize()} />
                                    </p>
                                    <p>{isDragActive ? props.dropFilesLabel : props.dropOrSelectFilesLabel}</p>
                                </div>
                            </div>
                        </div>
                        <div className="container mt-5 mb-5 has-text-centered">
                            {props.orLabel}
                        </div>
                        <div className="columns is-tablet">
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <Autosuggest
                                    suggestions={suggestions}
                                    onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                                    onSuggestionsClearRequested={() => setSuggestions([])}
                                    getSuggestionValue={(suggestion) => {
                                        return getProductSuggestionValue(suggestion);
                                    }}
                                    onSuggestionSelected={onSuggestionSelected}
                                    renderSuggestion={(suggestion) => {
                                        return (
                                            <div className="suggestion">
                                                { getProductSuggestionValue(suggestion)}
                                            </div>
                                        );
                                    }}
                                    inputProps={searchInputProps} />
                            </div>
                            <div className="column is-1 is-flex is-align-items-flex-end">
                                <TextField id="quantity" name="quantity" type="number" inputProps={{ min: "1", step: "1" }} variant="standard"
                                    label={props.quantityLabel} fullWidth={true} value={quantity} onChange={(e) => {

                                        e.preventDefault();
                                        setQuantity(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <TextField id="externalReference" name="externalReference" type="text" label={props.externalReferenceLabel} variant="standard"
                                    fullWidth={true} value={externalReference} onChange={(e) => {

                                        e.preventDefault();
                                        setExternalReference(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <LocalizationProvider dateAdapter={AdapterMoment}>
                                    <DatePicker
                                        id="deliveryFrom"
                                        label={props.deliveryFromLabel}
                                        value={deliveryFrom}
                                        onChange={(date) => {
                                            setDeliveryFrom(date);
                                        }}
                                        renderInput={(params) => 
                                            <TextField {...params} variant="standard" />}
                                        disablePast={true}/>
                                </LocalizationProvider>
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <LocalizationProvider dateAdapter={AdapterMoment}>
                                    <DatePicker
                                        id="deliveryTo"
                                        label={props.deliveryToLabel}
                                        value={deliveryTo}
                                        onChange={(date) => {
                                            setDeliveryTo(date);
                                        }}
                                        renderInput={(params) => 
                                            <TextField {...params} variant="standard" />}
                                       disablePast={true}/>
                                </LocalizationProvider>
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <TextField id="moreInfo" name="moreInfo" type="text" label={props.moreInfoLabel} variant="standard"
                                    fullWidth={true} value={moreInfo} onChange={(e) => {
                                        e.preventDefault();
                                        setMoreInfo(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-1 is-flex is-align-items-flex-end">
                                <Button type="button" variant="contained" color="primary" onClick={handleAddOrderItemClick} disabled={state.isLoading || quantity < 1 || product === null}>
                                    {props.addText}
                                </Button>
                            </div>
                        </div>
                        <div className="order__items">
                            {(orderItems && orderItems.length > 0) ?
                                (<Fragment>
                                    <div className="orderitems__table">
                                        <TableContainer component={Paper}>
                                            <Table aria-label={props.orderItemsLabel}>
                                                <TableHead>
                                                    <TableRow>
                                                        <TableCell></TableCell>
                                                        <TableCell></TableCell>
                                                        <TableCell>{props.skuLabel}</TableCell>
                                                        <TableCell>{props.nameLabel}</TableCell>
                                                        <TableCell>{props.quantityLabel}</TableCell>
                                                        <TableCell>{props.externalReferenceLabel}</TableCell>
                                                        <TableCell>{props.deliveryFromLabel}</TableCell>
                                                        <TableCell>{props.deliveryToLabel}</TableCell>
                                                        <TableCell>{props.moreInfoLabel}</TableCell>
                                                    </TableRow>
                                                </TableHead>
                                                <TableBody>
                                                    {orderItems.map((item, index) => (
                                                        <TableRow key={index}>
                                                            <TableCell width="11%">
                                                                <Fab onClick={() => handleDeleteClick(item)} size="small" color="primary" aria-label={props.deleteLabel}>
                                                                    <Delete />
                                                                </Fab>
                                                            </TableCell>
                                                            <TableCell><a href={item.productUrl} target="_blank"><img className="order__basket-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
                                                            <TableCell>{item.sku}</TableCell>
                                                            <TableCell>{item.name}</TableCell>
                                                            <TableCell>{item.quantity}</TableCell>
                                                            <TableCell>{item.externalReference}</TableCell>
                                                            <TableCell>{item.deliveryFrom && <span>{moment(item.deliveryFrom).format("L")}</span>}</TableCell>
                                                            <TableCell>{item.deliveryTo && <span>{moment(item.deliveryTo).format("L")}</span>}</TableCell>
                                                            <TableCell>{item.moreInfo}</TableCell>
                                                        </TableRow>
                                                    ))}
                                                </TableBody>
                                            </Table>
                                        </TableContainer>
                                    </div>
                                </Fragment>) :
                                (<section className="section is-flex-centered has-text-centered is-flex-direction-column">
                                    <AddShoppingCartRounded fontSize="large" className="m-2" />
                                    <span className="is-title is-5">{props.noOrderItemsLabel}</span>
                                </section>)
                            }
                        </div>
                    </Fragment>
                }
                <div className="field">
                    <Button 
                        type="button" 
                        variant="contained"
                        color="primary"
                        onClick={handlePlaceOrder}
                        disabled={state.isLoading || orderItems.length === 0 || disableSaveButton}>
                        {props.saveText}
                    </Button>
                    <a href={props.ordersUrl} className="ml-2 button is-text">{props.navigateToOrdersListText}</a>
                </div>
            </div>
            <ConfirmationDialog
                open={openDeleteDialog}
                handleClose={handleDeleteDialogClose}
                handleConfirm={handleDeleteEntity}
                titleId="delete-from-basket-title"
                title={props.deleteConfirmationLabel}
                textId="delete-from-basket-text"
                text={props.areYouSureLabel + ((entityToDelete ? (": " + entityToDelete.name) : "") + "?")}
                noLabel={props.noLabel}
                yesLabel={props.yesLabel}
            />
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section >
    );
}

OrderForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    searchPlaceholderLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    externalReferenceLabel: PropTypes.string.isRequired,
    deliveryFromLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    selectClientLabel: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
    changeDeliveryFromLabel: PropTypes.string.isRequired,
    changeDeliveryToLabel: PropTypes.string.isRequired,
    clientRequiredErrorMessage: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    addText: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    noOrderItemsLabel: PropTypes.string.isRequired,
    okLabel: PropTypes.string.isRequired,
    cancelLabel: PropTypes.string.isRequired,
    clients: PropTypes.array,
    deleteConfirmationLabel: PropTypes.string.isRequired,
    areYouSureLabel: PropTypes.string.isRequired,
    yesLabel: PropTypes.string.isRequired,
    noLabel: PropTypes.string.isRequired,
    ordersUrl: PropTypes.string.isRequired,
    placeOrderUrl: PropTypes.string.isRequired,
    navigateToOrdersListText: PropTypes.string.isRequired,
    uploadOrderFileUrl: PropTypes.string.isRequired,
    orLabel: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired
};

export default OrderForm;
