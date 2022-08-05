import React, { useContext, useState, Fragment, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import {
    FormControl, InputLabel, Select, MenuItem, Button,
    Table, TableBody, TableCell, TableContainer, TextField,
    TableHead, TableRow, Paper, CircularProgress, Dialog,
    DialogActions, DialogContent, DialogTitle
} from "@mui/material";
import moment from "moment";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import OrderConstants from "../../../../shared/constants/OrderConstants";;

const OrderItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [orderStatusId, setOrderStatusId] = useState(props.orderStatusId);

    const handleOrderStatusSubmit = (e) => {
        e.preventDefault();

        dispatch({ type: "SET_IS_LOADING", payload: true });

        var orderStatus = {
            orderId: props.id,
            orderStatusId
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(orderStatus)
        };

        fetch(props.updateOrderStatusUrl, requestOptions)
            .then((response) => {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setOrderStatusId(jsonResponse.orderStatusId);
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

    return (
        <section className="section section-small-padding order-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOrderStatusSubmit}>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={''} 
                                variant="standard"
                            />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={''} 
                                variant="standard"
                            />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={''} 
                                variant="standard"
                            />
                        </div>
                    </form>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    );
}

OrderItemForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
};

export default OrderItemForm;