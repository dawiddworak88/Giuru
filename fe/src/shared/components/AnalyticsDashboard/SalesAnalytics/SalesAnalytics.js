import React from "react";
import PropTypes from "prop-types";
import { Line } from "react-chartjs-2";
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
    return (
        <div className="sales-analytics">
            <h3 className="subtitle">{props.title}</h3>
            <Line 
                options={{
                    responsive: true,
                    plugins: {
                        legend: {
                            display: false,
                        }
                    }
                }} 
                data={{
                    labels: props.chartLables,
                    datasets: props.chartDatasets
                }}/>
        </div>
    )
}

SalesAnalytics.propTypes = {
    title: PropTypes.string.isRequired,
    chartLables: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired
}

export default SalesAnalytics;