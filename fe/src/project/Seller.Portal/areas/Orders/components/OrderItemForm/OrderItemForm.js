import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import {
    FormControl, InputLabel, Select, MenuItem, Button, TextField, CircularProgress
} from "@mui/material";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";

const OrderItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [orderStatusId, setOrderStatusId] = useState(props.orderStatusId);
    const [orderStatusComment, setOrderStatusComment] = useState("")
    const [a, sa] = useState(false);

    const handleSubmitForm = (e) => {
        e.preventDefault();

        const requestPayload = {
            id: props.id,
            orderStatusId,
            orderStatusComment
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(requestPayload)
        }

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        sa(true)
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
    }

    return (
        <section className="section section-small-padding order-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleSubmitForm}>
                        {props.id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {props.id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField 
                                id="productSku" 
                                name="productSku" 
                                label={props.skuLabel} 
                                fullWidth={true}
                                value={props.productSku}
                                variant="standard"
                                InputProps={{
                                    readOnly: true,
                                }}
                            />
                        </div>
                        <div className="field">
                            <TextField 
                                id="productName" 
                                name="productName" 
                                label={props.nameLabel} 
                                fullWidth={true}
                                value={props.productName} 
                                variant="standard"
                                InputProps={{
                                    readOnly: true,
                                }}
                            />
                        </div>
                        <div className="field">
                            <FormControl variant="standard" fullWidth={true}>
                                <InputLabel id="orderItemStatus-label">{props.orderStatusLabel}</InputLabel>
                                <Select
                                    id="orderItemStatus"
                                    name="orderItemStatus"
                                    value={orderStatusId ? orderStatusId : props.orderStatusId}
                                    onChange={(e) => {
                                        sa(false);
                                        setOrderStatusId(e.target.value)
                                    }}
                                    >
                                    {props.orderItemStatuses.map((status, index) => {
                                        return (
                                            <MenuItem key={index} value={status.id}>{status.name}</MenuItem>
                                        );
                                    })}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <TextField
                                id="ordertatusComment"
                                name="orderStatusComment"
                                label={props.orderStatusCommentLabel}
                                variant="standard"
                                fullWidth={true}
                                multiline={true}
                                value={orderStatusComment}
                                onChange={(e) => setOrderStatusComment(e.target.value)}
                            />
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || props.orderStatusId === orderStatusId || a}>
                                {props.saveText}
                            </Button>
                            <Button 
                                className="ml-2"
                                type="button" 
                                variant="contained" 
                                color="secondary" 
                                onClick={(e) => {
                                    e.preventDefault();
                                    NavigationHelper.redirect(props.orderUrl);
                                }}>
                                {props.navigateToOrderLabel}
                            </Button> 
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