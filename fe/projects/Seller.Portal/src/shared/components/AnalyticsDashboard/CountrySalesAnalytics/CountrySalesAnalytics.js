import React, { useState } from "react";
import PropTypes from "prop-types";
import AdapterMoment from '@mui/lab/AdapterMoment';
import { toast } from "react-toastify";
import { DatePicker, LocalizationProvider } from "@mui/lab";
import { TextField } from "@mui/material";
import { Bar } from "react-chartjs-2";
import { Chart as ChartJs, CategoryScale, Tooltip, Legend, BarElement } from "chart.js";
import ChartsConstants from "../../../../shared/constants/ChartsConstants";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import ChartValidator from "../../../../shared/helpers/validators/ChartValidator";

if (typeof window !== "undefined") {
    ChartJs.register(
        CategoryScale, Legend, Tooltip, BarElement
    )
}

const CountrySalesAnalytics = (props) => {
    const [countriesSales, setCountriesSales] = useState(props.chartDatasets ? props.chartDatasets : [])
    const [countriesLabels, setCountriesLabels] = useState(props.chartLabels ? props.chartLabels : [])
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
                            setCountriesSales(jsonResponse.data.chartDatasets);
                            setCountriesLabels(jsonResponse.data.chartLabels);
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
                            setCountriesSales(jsonResponse.data.chartDatasets);
                            setCountriesLabels(jsonResponse.data.chartLabels);
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
        <div className="country-sales">
            <h3 className="subtitle">{props.title}</h3>
            <div className="country-sales-analytics">
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            id="country-sales-from-date"
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
                            id="country-sales-to-date"
                            label={props.toLabel}
                            value={toDate}
                            name="toDate"
                            views={props.datePickerViews}
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
                    labels: countriesLabels,
                    datasets: [{
                        ...countriesSales[0],
                        backgroundColor: ChartsConstants.countrySalesColors()    
                    }]
                }}/>
        </div>
    )
}

CountrySalesAnalytics.propTypes = {
    title: PropTypes.string.isRequired,
    chartLabels: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired,
    fromLabel: PropTypes.string.isRequired,
    toLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    invalidDateRangeErrorMessage: PropTypes.string.isRequired
}

export default CountrySalesAnalytics;
