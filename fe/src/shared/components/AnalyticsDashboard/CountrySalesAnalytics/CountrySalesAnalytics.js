import React from "react";
import PropTypes from "prop-types";
import { Doughnut } from "react-chartjs-2";
import { Chart as ChartJs, ArcElement, Tooltip, Legend } from "chart.js";

if (typeof window !== "undefined") {
    ChartJs.register(
        ArcElement, Legend, Tooltip
    )
}

const CountrySalesAnalytics = (props) => {
    return (
        <div className="country-sales">
            <h3 className="subtitle">{props.title}</h3>
            <Doughnut
                options={{
                    responsive: true,
                }} 
                data={{
                    labels: props.chartLables,
                    datasets: props.chartDatasets
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