import React, { useState } from "react";
import PropTypes from "prop-types";
import AdapterMoment from '@mui/lab/AdapterMoment';
import { DatePicker, LocalizationProvider } from "@mui/lab";
import { TextField } from "@mui/material";
import { Bar } from "react-chartjs-2";
import { Chart as ChartJs, CategoryScale, Tooltip, Legend, BarElement } from "chart.js";
import ChartsConstants from "../../../constants/ChartsConstants";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";

if (typeof window !== "undefined") {
    ChartJs.register(
        CategoryScale, Legend, Tooltip, BarElement
    )
}

const CountrySalesAnalytics = (props) => {

    const [fromDate, setFromDate] = useState(props.fromDate);
    const [toDate, setToDate] = useState(props.toDate);

    const handleFromDate = (date) => {
        setFromDate(date);

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify({ fromDate: date, toDate })
        };

        fetch(props.saveUrl, requestOptions)
            .then(response => {
                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        // action
                    }
                });

            }).catch(() => {
                // error handling
            });
    }

    const handleToDate = (date) => {
        setToDate(date);

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify({ fromDate, toDate: date })
        };

        fetch(props.saveUrl, requestOptions)
            .then(response => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        // action
                    }
                });

            }).catch(() => {
                // error handling
            });
    }

    return (
        <div className="country-sales">
            <h3 className="subtitle">{props.title}</h3>
            <div className="country-sales-analytics">
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            id="country-sales-from-date"
                            label={props.fromLabel}
                            value={fromDate}
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
                            id="country-sales-to-date"
                            label={props.toLabel}
                            value={toDate}
                            onChange={(date) => {
                                handleToDate(date);
                            }}
                            renderInput={(params) => 
                                <TextField {...params} variant="standard" />}
                            disableFuture={true} />
                    </LocalizationProvider>
                </span>
            </div>
            <Bar
                options={{
                    maintainAspectRatio: false,
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false,
                        }
                    }
                }} 
                data={{
                    labels: props.chartLables,
                    datasets: [{
                        ...props.chartDatasets[0],
                        backgroundColor: ChartsConstants.countrySalesColors()    
                    }]
                }}/>
        </div>
    )
}

CountrySalesAnalytics.propTypes = {
    title: PropTypes.string.isRequired,
    chartLables: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired,
    fromLabel: PropTypes.string.isRequired,
    toLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired
}

export default CountrySalesAnalytics;
