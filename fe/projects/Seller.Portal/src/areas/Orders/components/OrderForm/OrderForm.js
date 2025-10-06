import React, { useContext, useCallback, useState, Fragment } from "react";
import { toast } from "react-toastify";
import { UploadCloud } from "react-feather";
import { useDropzone } from "react-dropzone";
import PropTypes from "prop-types";
import Autosuggest from "react-autosuggest";
import { Context } from "../../../../shared/stores/Store";
import { Delete, AddShoppingCartRounded } from "@mui/icons-material"
import {
    Fab, Table, TableBody, TableCell, TableContainer, Autocomplete,
    TableHead, TableRow, Paper, TextField, Button, CircularProgress,
    FormControlLabel, Checkbox
} from "@mui/material";
import moment from "moment";
import QueryStringSerializer from "../../../../shared/helpers/serializers/QueryStringSerializer";
import OrderFormConstants from "../../../../shared/constants/OrderFormConstants";
import ConfirmationDialog from "../../../../shared/components/ConfirmationDialog/ConfirmationDialog";
import IconConstants from "../../../../shared/constants/IconConstants";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import ProductPricesHelper from "../../../../shared/helpers/prices/ProductPricesHelper";
import OrderItemsGrouper from "../../../../shared/helpers/orders/OrderItemsGroupHelper"

function OrderForm(props) {
    const [state, dispatch] = useContext(Context);
    const [id, setId] = useState(props.id ? props.id : null);
    const [basketId, setBasketId] = useState(null);
    const [client, setClient] = useState(props.clientId ? props.clients.find((item) => item.id === props.clientId) : null);
    const [deliveryAddress, setDeliveryAddress] = useState(null);
    const [billingAddress, setBillingAddress] = useState(null);
    const [clientAddresses, setClientAddresses] = useState([]);
    const [searchTerm, setSearchTerm] = useState("");
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [externalReference, setExternalReference] = useState("");
    const [moreInfo, setMoreInfo] = useState("");
    const [orderItems, setOrderItems] = useState([]);
    const [suggestions, setSuggestions] = useState([]);
    const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
    const [entityToDelete, setEntityToDelete] = useState(null);
    const [disableSaveButton, setDisableSaveButton] = useState(false);
    const [productFromOutlet, setProductFromOutlet] = useState(false);

    const onSuggestionsFetchRequested = (args) => {

        if (args.value && args.value.length >= OrderFormConstants.minSuggestionSearchTermLength()) {

            const searchParameters = {
                clientId: client ? client.id : null,
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
                            setSuggestions(() => jsonResponse);
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
        let items = orderItems.filter(item => item.productId === suggestion.id);

        if (items.length > 0) {
            suggestion.stockQuantity -= items.reduce((sum, item) => sum + item.stockQuantity, 0);
            suggestion.outletQuantity -= items.reduce((sum, item) => sum + item.outletQuantity, 0); 
        }

        setProduct(suggestion);
    };

    const getProductSuggestionValue = (suggestion) => {
        return "(" + suggestion.sku + ")" + " " + suggestion.name;
    };

    const handleAddOrderItemClick = async () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const orderItem = {
            productId: product.id,
            sku: product.sku,
            name: product.name,
            imageId: product.images ? product.images[0] : null,
            quantity: quantity,
            stockQuantity: 0,
            outletQuantity: 0,
            unitPrice: product.price ? parseFloat(product.price).toFixed(2) : null,
            price: product.price ? parseFloat(product.price * quantity).toFixed(2) : null,
            currency: product.currency,
            externalReference,
            moreInfo
        };

        if (productFromOutlet) {
            orderItem.outletQuantity = quantity;
            orderItem.stockQuantity = 0;
            orderItem.quantity = 0;

            const outletPrice = await  ProductPricesHelper.getPriceByProductSku(props.getProductPriceUrl, client.id, product.sku);

            if (outletPrice) {
                orderItem.unitPrice = outletPrice.price
                orderItem.price = parseFloat(outletPrice.price * quantity).toFixed(2);
                orderItem.currency = outletPrice.currency;
            }
        } else if (product.stockQuantity > 0) {
            if (quantity > product.stockQuantity) {
                orderItem.quantity = quantity - product.stockQuantity;
                orderItem.stockQuantity = product.stockQuantity; 
            }
            else {
                orderItem.stockQuantity = quantity;
                orderItem.quantity = 0;
            }
        }

        if (props.maxAllowedOrderQuantity && 
           (quantity > props.maxAllowedOrderQuantity)) {
                toast.error(props.maxAllowedOrderQuantityErrorMessage);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                return;
        };

        const basket = {
            id: basketId,
            items: [...orderItems, orderItem]
        };

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
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
                            setMoreInfo("");
                            setOrderItems(OrderItemsGrouper.groupOrderItems(jsonResponse.items));
                            setQuantity(1);
                            setProductFromOutlet(false);
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

                            setOrderItems(OrderItemsGrouper.groupOrderItems(jsonResponse.items));
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

        if (deliveryAddress) {
            order = {
                ...order,
                shippingAddressId: deliveryAddress.id,
                shippingCompany: deliveryAddress.company,
                shippingFirstName: deliveryAddress.firstName,
                shippingLastName: deliveryAddress.lastName,
                shippingRegion: deliveryAddress.region,
                shippingPostCode: deliveryAddress.postCode,
                shippingCity: deliveryAddress.city,
                shippingStreet: deliveryAddress.street,
                shippingPhoneNumber: deliveryAddress.phoneNumber,
                shippingCountryId: deliveryAddress.countryId
            }
        }

        if (billingAddress) {
            order = {
                ...order,
                billingAddressId: billingAddress.id,
                billingCompany: billingAddress.company,
                billingFirstName: billingAddress.firstName,
                billingLastName: billingAddress.lastName,
                billingRegion: billingAddress.region,
                billingPostCode: billingAddress.postCode,
                billingCity: billingAddress.city,
                billingStreet: billingAddress.street,
                billingPhoneNumber: billingAddress.phoneNumber,
                billingCountryId: billingAddress.countryId
            }
        }

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

    const handleChangeClient = (value) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });
        setClient(value);

        const searchParameters = {
            clientId: value.id,
            pageIndex: 1,
            itemsPerPage: props.defaultItemsPerPage
        };

        const requestOptions = {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            }
        };

        const url = props.getClientAddressesUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        fetch(url, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setClientAddresses(jsonResponse.data);

                        if (value.defaultDeliveryAddressId) {
                            const defaultDeliveryAddress = jsonResponse.data.find(x => x.id == value.defaultDeliveryAddressId);

                            setDeliveryAddress(defaultDeliveryAddress);
                        }

                        if (value.defaultBillingAddressId) {
                            const defaultBillingAddress = jsonResponse.data.find(x => x.id == value.defaultBillingAddressId);

                            setBillingAddress(defaultBillingAddress);
                        }
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const onDrop = useCallback(acceptedFiles => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        acceptedFiles.forEach((file) => {

            const formData = new FormData();

            formData.append("file", file);
            formData.append("clientId", client.id)

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
                            setOrderItems(OrderItemsGrouper.groupOrderItems([...orderItems, ...jsonResponse.items]));
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
    }, [client]);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        multiple: false,
        accept: {
            "application/*": [".xls", ".xlsx"]
        }
    });

    const getTotalQuantities = (item) => {
        return item.quantity + item.stockQuantity + item.outletQuantity;
    }

    const maxOutlet = product ? product.outletQuantity : 0;

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
                                options={props.clients}
                                getOptionLabel={(option) => option.name}
                                id="client"
                                name="client"
                                fullWidth={true}
                                value={client}
                                onChange={(event, newValue) => {
                                    handleChangeClient(newValue);
                                }}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectClientLabel} margin="normal" variant="standard" />}
                            />
                        </div>
                        {clientAddresses && clientAddresses.length > 0 &&
                            <Fragment>
                                <div className="field">
                                    <Autocomplete
                                        options={clientAddresses}
                                        getOptionLabel={(option) => `${option.company}, ${option.firstName} ${option.lastName}, ${option.postCode} ${option.city}`}
                                        id="deliveryAddress"
                                        name="deliveryAddress"
                                        fullWidth={true}
                                        value={deliveryAddress}
                                        onChange={(event, newValue) => {
                                            setDeliveryAddress(newValue);
                                        }}
                                        autoComplete
                                        renderInput={(params) => <TextField {...params} label={props.deliveryAddressLabel} margin="normal" variant="standard" />}
                                    />
                                </div>
                                <div className="field">
                                    <Autocomplete
                                        options={clientAddresses}
                                        getOptionLabel={(option) => `${option.company}, ${option.firstName} ${option.lastName}, ${option.postCode} ${option.city}`}
                                        id="billingAddress"
                                        name="billingAddress"
                                        fullWidth={true}
                                        value={billingAddress}
                                        onChange={(event, newValue) => {
                                            setBillingAddress(newValue);
                                        }}
                                        autoComplete
                                        renderInput={(params) => <TextField {...params} label={props.billingAddressLabel} margin="normal" variant="standard" />}
                                    />
                                </div>
                            </Fragment>
                        }
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
                        <div className="columns is-tablet is-justify-content-center">
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
                                                {getProductSuggestionValue(suggestion)}
                                            </div>
                                        );
                                    }}
                                    inputProps={searchInputProps} />
                            </div>
                            <div className="column is-1 is-flex is-align-items-flex-end">
                                <TextField 
                                    id="quantity" 
                                    name="quantity" 
                                    type="number" 
                                    inputProps={{ 
                                        min: "1", 
                                        step: "1" 
                                    }} 
                                    variant="standard"
                                    label={productFromOutlet ? `${props.quantityLabel} ${maxOutlet > 0 ? `(${props.maximalLabel} ${maxOutlet})` : ""}` : props.quantityLabel} 
                                    fullWidth={true} 
                                    disabled={product == null} 
                                    value={quantity} 
                                    onChange={(e) => {
                                        setQuantity(e.target.value);
                                    }}
                                    onBlur={() => {
                                        let numericValue = Number(quantity);

                                        if (isNaN(numericValue) || numericValue < 1) {
                                            numericValue = 1;
                                        }

                                        if (productFromOutlet && numericValue > maxOutlet) {
                                            numericValue = maxOutlet;
                                        }

                                        setQuantity(numericValue);
                                    }}
                                />
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end pb-2">
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={productFromOutlet}
                                            onChange={(e) => {
                                                setProductFromOutlet(e.target.checked);

                                                if (e.target.checked && (quantity > maxOutlet)) {
                                                    setQuantity(maxOutlet);
                                                }
                                            }} />
                                    }
                                    label={props.outletProductLabel}
                                    disabled={!product || product.outletQuantity == 0}
                                />
                            </div>    
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <TextField id="externalReference" name="externalReference" type="text" label={props.externalReferenceLabel} variant="standard"
                                    fullWidth={true} value={externalReference} onChange={(e) => {

                                        e.preventDefault();
                                        setExternalReference(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-2 is-flex is-align-items-flex-end">
                                <TextField id="moreInfo" name="moreInfo" type="text" label={props.moreInfoLabel} variant="standard"
                                    fullWidth={true} value={moreInfo} onChange={(e) => {
                                        e.preventDefault();
                                        setMoreInfo(e.target.value);
                                    }} />
                            </div>
                            <div className="column is-1 is-flex is-align-items-flex-end">
                                <Button 
                                    type="button" 
                                    variant="contained" 
                                    color="primary" 
                                    onClick={handleAddOrderItemClick}
                                    disabled={state.isLoading || !(quantity > 0 && product !== null)}
                                >
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
                                                        <TableCell>{props.stockQuantityLabel}</TableCell>
                                                        <TableCell>{props.outletQuantityLabel}</TableCell>
                                                        <TableCell className="has-text-weight-bold">{props.inTotalLabel}</TableCell>
                                                        <TableCell>{props.externalReferenceLabel}</TableCell>
                                                        <TableCell>{props.deliveryFromLabel}</TableCell>
                                                        <TableCell>{props.deliveryToLabel}</TableCell>
                                                        <TableCell>{props.moreInfoLabel}</TableCell>
                                                        <TableCell>{props.unitPriceLabel}</TableCell>
                                                        <TableCell>{props.priceLabel}</TableCell>
                                                        <TableCell>{props.currencyLabel}</TableCell>
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
                                                            <TableCell>{item.stockQuantity}</TableCell>
                                                            <TableCell>{item.outletQuantity}</TableCell>
                                                            <TableCell className="has-text-weight-bold">{getTotalQuantities(item)}</TableCell>
                                                            <TableCell>{item.externalReference}</TableCell>
                                                            <TableCell>{item.deliveryFrom && <span>{moment(item.deliveryFrom).format("L")}</span>}</TableCell>
                                                            <TableCell>{item.deliveryTo && <span>{moment(item.deliveryTo).format("L")}</span>}</TableCell>
                                                            <TableCell>{item.moreInfo}</TableCell>
                                                            <TableCell>{item.unitPrice}</TableCell>
                                                            <TableCell>{item.price}</TableCell>
                                                            <TableCell>{item.currency}</TableCell>
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
    moreInfoLabel: PropTypes.string.isRequired,
    selectClientLabel: PropTypes.string.isRequired,
    getSuggestionsUrl: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
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
