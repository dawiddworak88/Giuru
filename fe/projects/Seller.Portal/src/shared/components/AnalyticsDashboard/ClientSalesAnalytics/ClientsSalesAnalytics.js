import React, { useState } from "react";
import PropTypes from "prop-types";
import AdapterMoment from '@mui/lab/AdapterMoment';
import { toast } from "react-toastify";
import { DatePicker, LocalizationProvider } from "@mui/lab";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, TextField } from "@mui/material";
import ChartValidator from "../../../../shared/helpers/validators/ChartValidator";

const ClientsSalesAnalytics = (props) => {
    const [clientsAnalytics, setClientsAnalytics] = useState(props.clients ? props.clients : [])
    const [fromDate, setFromDate] = useState(props.fromDate);
    const [toDate, setToDate] = useState(props.toDate);

    const handleFromDate = (date) => {

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
                            setClientsAnalytics(jsonResponse.data);
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
                            setClientsAnalytics(jsonResponse.data);
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
        <div className="clients-sales">
            <h3 className="subtitle">{props.title}</h3>
            <div className="clients-analytics-date-ranges">
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            id="clients-analytics-from-date"
                            label={props.fromLabel}
                            value={fromDate}
                            name="fromDate"
                            views={props.datePickerViews}
                            onChange={(date) => {
                                handleFromDate(date);
                            }}
                            renderInput={(params) => 
                                <TextField {...params} variant="standard" />} />
                    </LocalizationProvider>
                </span>
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            id="clients-analytics-to-date"
                            label={props.toLabel}
                            value={toDate}
                            views={props.datePickerViews}
                            name="toDate"
                            onChange={(date) => {
                                handleToDate(date);
                            }}
                            renderInput={(params) => 
                                <TextField {...params} variant="standard" />}
                            disableFuture={true} />
                    </LocalizationProvider>
                </span>
            </div>
            <div className="table-container">
                <TableContainer component={Paper} className="clients-analytics__table">
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>{props.nameLabel}</TableCell>
                                <TableCell>{props.quantityLabel}</TableCell>
                            </TableRow>
                        </TableHead>
                        {clientsAnalytics && clientsAnalytics.length > 0 ? (
                            <TableBody>
                                {clientsAnalytics.map((client, index) => {
                                    return (
                                        <TableRow key={index}>
                                            <TableCell>{client.name}</TableCell>
                                            <TableCell>{client.quantity}</TableCell>
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

ClientsSalesAnalytics.propTypes = {
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

export default ClientsSalesAnalytics;