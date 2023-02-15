import React from "react";
import PropTypes from "prop-types";
import { Bar } from "react-chartjs-2";
import { Chart as ChartJs, CategoryScale, Tooltip, Legend, BarElement } from "chart.js";
import ChartsConstants from "../../../constants/ChartsConstants";

if (typeof window !== "undefined") {
    ChartJs.register(
        CategoryScale, Legend, Tooltip, BarElement
    )
}

const CountrySalesAnalytics = (props) => {
    return (
        <div className="country-sales">
            <h3 className="subtitle">{props.title}</h3>
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
    chartDatasets: PropTypes.array.isRequired
}

export default CountrySalesAnalytics;