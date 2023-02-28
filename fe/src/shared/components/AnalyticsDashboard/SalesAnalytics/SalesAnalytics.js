import React, { useState } from "react";
import PropTypes from "prop-types";
import { Line } from "react-chartjs-2";
import AdapterMoment from '@mui/lab/AdapterMoment';
import { toast } from "react-toastify";
import { DatePicker, LocalizationProvider } from "@mui/lab";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { TextField } from "@mui/material";
import { 
    Chart as ChartJs, Tooltip, Legend, CategoryScale, 
    LinearScale, PointElement, LineElement
} from "chart.js";

if (typeof window !== "undefined") {
    ChartJs.register(
        Legend, Tooltip, CategoryScale, LinearScale, 
        PointElement, LineElement
    )
}

const SalesAnalytics = (props) => {
    const [salesAnalytics, setSalesAnalytics] = useState(props.chartDatasets ? props.chartDatasets : [])
    const [salesAnalyticsLabels, setSalesAnalyticsLabels] = useState(props.chartLabels ? props.chartLabels : [])
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
            .then((response) => {
                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setSalesAnalytics(jsonResponse.data.chartDatasets);
                        setSalesAnalyticsLabels(jsonResponse.data.chartLabels);
                    }
                });

            }).catch(() => {
                toast.error(props.generalErrorMessage);
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
            .then((response) => {
                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setSalesAnalytics(jsonResponse.data.chartDatasets);
                        setSalesAnalyticsLabels(jsonResponse.data.chartLabels);
                    }
                });

            }).catch(() => {
                toast.error(props.generalErrorMessage);
            });
    }

    return (
        <div className="sales-analytics">
            {props.title &&
                <h3 className="subtitle">{props.title}</h3>
            }
            <div className="sales-analytics-date-ranges">
                <span>
                    <LocalizationProvider dateAdapter={AdapterMoment}>
                        <DatePicker
                            id="sales-analytics-from-date"
                            label={props.fromLabel}
                            value={fromDate}
                            name="fromDate"
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
                            id="sales-analytics-to-date"
                            label={props.toLabel}
                            value={toDate}
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
            <Line 
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
                    labels: salesAnalyticsLabels,
                    datasets: salesAnalytics
                }}/>
        </div>
    )
}

SalesAnalytics.propTypes = {
    chartLabels: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired
}

export default SalesAnalytics;