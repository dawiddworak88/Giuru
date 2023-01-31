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
                    labels: props.chartLables,
                    datasets: props.chartDatasets
                }}/>
        </div>
    )
}

SalesAnalytics.propTypes = {
    chartLables: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired
}

export default SalesAnalytics;