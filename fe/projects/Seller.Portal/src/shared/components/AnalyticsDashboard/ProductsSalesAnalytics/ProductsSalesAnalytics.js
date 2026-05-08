import React, { useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import { toast } from "react-toastify";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody } from "@mui/material";
import ChartValidator from "../../../../shared/helpers/validators/ChartValidator";

const toMomentValue = (value) => {
    if (!value) {
        return null;
    }

    if (moment.isMoment(value)) {
        return value;
    }

    const parsedValue = moment(value);

    return parsedValue.isValid() ? parsedValue : null;
}

const ProductsSalesAnalytics = (props) => {
    const [productsAnalytics, setProductsAnalytics] = useState(props.products ? props.products : [])
    const [fromDate, setFromDate] = useState(toMomentValue(props.fromDate));
    const [toDate, setToDate] = useState(toMomentValue(props.toDate));

    const handleFromDate = (date) => {
        if (!date) {
            setFromDate(null);
            return;
        }

        if (ChartValidator.validate(date, toDate)) {
            setFromDate(date);

            const requestOptions = {
                method: "POST",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
                body: JSON.stringify({ fromDate: date, toDate })
            };
    
            fetch(props.saveUrl, requestOptions)
                .then((response) => {
                    AuthenticationHelper.HandleResponse(response);
    
                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            setProductsAnalytics(jsonResponse.data);
                        }
                    });
    
                }).catch(() => {
                    toast.error(props.generalErrorMessage);
                });
        }
        else {
            toast.error(props.invalidDateRangeErrorMessage);
            return;
        }
    }

    const handleToDate = (date) => {
        if (!date) {
            setToDate(null);
            return;
        }

        if (ChartValidator.validate(fromDate, date)) {
            setToDate(date);

            const requestOptions = {
                method: "POST",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
                body: JSON.stringify({ fromDate, toDate: date })
            };
    
            fetch(props.saveUrl, requestOptions)
                .then((response) => {
                    AuthenticationHelper.HandleResponse(response);
    
                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            setProductsAnalytics(jsonResponse.data);
                        }
                    });
    
                }).catch(() => {
                    toast.error(props.generalErrorMessage);
                });
        }
        else {
            toast.error(props.invalidDateRangeErrorMessage)
            return;
        }
    }

    return (
        <div className="products-sales">
            <h3 className="subtitle">{props.title}</h3>
            <div className="products-analytics-date-ranges">
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            label={props.fromLabel}
                            value={fromDate}
                            views={props.datePickerViews}
                            onChange={(date) => {
                                handleFromDate(date);
                            }}
                            slotProps={{
                                textField: {
                                    id: "products-analytics-from-date",
                                    name: "fromDate",
                                    variant: "standard"
                                }
                            }} />
                    </LocalizationProvider>
                </span>
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            label={props.toLabel}
                            value={toDate}
                            views={props.datePickerViews}
                            onChange={(date) => {
                                handleToDate(date);
                            }}
                            disableFuture={true}
                            slotProps={{
                                textField: {
                                    id: "products-analytics-to-date",
                                    name: "toDate",
                                    variant: "standard"
                                }
                            }} />
                    </LocalizationProvider>
                </span>
            </div>
            <div className="table-container">
                <TableContainer component={Paper} className="products-analytics__table">
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>{props.nameLabel}</TableCell>
                                <TableCell>{props.skuLabel}</TableCell>
                                <TableCell>{props.quantityLabel}</TableCell>
                            </TableRow>
                        </TableHead>
                        {productsAnalytics && productsAnalytics.length > 0 ? (
                            <TableBody>
                                {productsAnalytics.map((product, index) => {
                                    return (
                                        <TableRow key={index}>
                                            <TableCell>{product.name}</TableCell>
                                            <TableCell>{product.sku}</TableCell>
                                            <TableCell>{product.quantity}</TableCell>
                                        </TableRow>
                                    )
                                })}
                            </TableBody>
                        ) : (
                            <TableRow>
                                <TableCell colSpan={3} align="center">{props.noResultsLabel}</TableCell>
                            </TableRow>
                        )}
                    </Table>
                </TableContainer>
            </div>
        </div>
    )
}

ProductsSalesAnalytics.propTypes = {
    noResultsLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    fromLabel: PropTypes.string.isRequired,
    toLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    invalidDateRangeErrorMessage: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
}

export default ProductsSalesAnalytics;