import React, { useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { CircularProgress } from "@material-ui/core";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper
} from "@material-ui/core";
import moment from "moment";

function EditOrderForm(props) {

    const [state, dispatch] = useContext(Context);
    const [orderStatusId, setOrderStatusId] = useState(props.statusId);

    const handleOrderStatusSubmit = () => {

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

                        setFieldValue({ name: "id", value: jsonResponse.id });
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
        <section className="section section-small-padding edit-order">
            <h1 className="subtitle is-4">{props.title}</h1>
            <form className="is-modern-form" onSubmit={handleOrderStatusSubmit} method="post">
                <div className="columns is-desktop">
                    <div className="column is-3">
                        <div className="field">
                            <FormControl>
                                <InputLabel id="order-status-label">{props.orderStatusLabel}</InputLabel>
                                <Select
                                    labelId="order-status-label"
                                    id="orderStatus"
                                    name="orderStatus"
                                    value={orderStatusId}
                                    onChange={(e) => {

                                        e.preventDefault();
                                        setOrderStatusId(e.target.value);
                                    }}>
                                    {props.orderStatuses.map(status => {
                                        return (
                                            <MenuItem key={status.id} value={status.id}>{status.name}</MenuItem>
                                        );
                                    })}
                                </Select>
                            </FormControl>
                        </div>
                    </div>
                    <div className="column is-3">
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading}>
                                {props.saveText}
                            </Button>
                        </div>
                    </div>
                </div>
            </form>
            <div>
                <h2 className="subtitle is-5 edit-order__items-subtitle">{props.clientLabel}</h2>
            </div>
            <div>
                <h2 className="subtitle is-5 edit-order__items-subtitle">{props.orderItemsLabel}</h2>
                <div className="edit-order__items">
                    <section className="section">
                        <div className="orderitems__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.orderItemsLabel}>
                                    <TableHead>
                                        <TableRow>
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
                                        {props.orderItems && props.orderItems.map((item, index) => (
                                            <TableRow key={index}>
                                                <TableCell><a href={item.productUrl} target="_blank"><img className="order__item-product-image" src={item.imageSrc} alt={item.imageAlt} /></a></TableCell>
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
                    </section>
                </div>
            </div>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section >
    );
}

EditOrderForm.propTypes = {
    title: PropTypes.string.isRequired,
    id: PropTypes.string,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    externalReferenceLabel: PropTypes.string.isRequired,
    deliveryFromLabel: PropTypes.string.isRequired,
    deliveryToLabel: PropTypes.string.isRequired,
    moreInfoLabel: PropTypes.string.isRequired,
    orderItemsLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatuses: PropTypes.array.isRequired
};

export default EditOrderForm;
