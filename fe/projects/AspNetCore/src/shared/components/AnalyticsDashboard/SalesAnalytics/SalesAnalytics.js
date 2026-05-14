import React, { useState } from "react";
import PropTypes from "prop-types";
import moment from "moment";
import { Line } from "react-chartjs-2";
import { AdapterMoment } from '@mui/x-date-pickers/AdapterMoment';
import { toast } from "react-toastify";
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { 
    Chart as ChartJs, Tooltip, Legend, CategoryScale, 
    LinearScale, PointElement, LineElement
} from "chart.js";
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

if (typeof window !== "undefined") {
    ChartJs.register(
        Legend, Tooltip, CategoryScale, LinearScale, 
        PointElement, LineElement
    )
}

const SalesAnalytics = (props) => {
    const [salesAnalytics, setSalesAnalytics] = useState(props.chartDatasets ? props.chartDatasets : [])
    const [salesAnalyticsLabels, setSalesAnalyticsLabels] = useState(props.chartLabels ? props.chartLabels : [])
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
                            setSalesAnalytics(jsonResponse.data.chartDatasets);
                            setSalesAnalyticsLabels(jsonResponse.data.chartLabels);
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
                            setSalesAnalytics(jsonResponse.data.chartDatasets);
                            setSalesAnalyticsLabels(jsonResponse.data.chartLabels);
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
        <div className="sales-analytics">
            {props.title &&
                <h3 className="subtitle">{props.title}</h3>
            }
            <div className="sales-analytics-date-ranges">
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
                                    id: "sales-analytics-from-date",
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
                                    id: "sales-analytics-to-date",
                                    name: "toDate",
                                    variant: "standard"
                                }
                            }} />
                    </LocalizationProvider>
                </span>
            </div>
            <Line 
                options={{
                    maintainAspectRatio: false,
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    },
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
    chartDatasets: PropTypes.array.isRequired,
    fromLabel: PropTypes.string.isRequired,
    toLabel: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    invalidDateRangeErrorMessage: PropTypes.string.isRequired
}

export default SalesAnalytics;