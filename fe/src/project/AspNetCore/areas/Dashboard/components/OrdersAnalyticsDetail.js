import React from "react";
import PropTypes from "prop-types";
import { Line } from "react-chartjs-2";
import { Chart as ChartJs, Title, CategoryScale, LinearScale, BarElement, PointElement, LineElement} from "chart.js";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody } from "@mui/material";

ChartJs.register(
    Title, CategoryScale, LinearScale, BarElement, PointElement, LineElement
)

const OrdersAnalyticsDetail = (props) => {
    return (
        <div className="section container">
            <h1 className="title is-3">{props.title}</h1>
            <div className="mt-6">
                <div className="columns">
                    <div className="column is-two-thirds">
                        <h3 className="subtitle">{props.numberOfOrdersLabel}</h3>
                        <Line 
                            options={{
                                responsive: true,
                            }} 
                            data={{
                                labels: props.chartLables,
                                datasets: props.chartDatasets
                            }}/>
                    </div>
                    <div className="column is-one-third">
                        <h3 className="subtitle">{props.top10OrderedProducts}</h3>
                        <div className="table-container">
                            <TableContainer component={Paper} className="orders-analytics__table">
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{props.productNameLabel}</TableCell>
                                            <TableCell>{props.productQuantityLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    {props.products && props.products.length > 0 ? (
                                        <TableBody>
                                            {props.products.map((product, index) => {
                                                return (
                                                    <TableRow key={index}>
                                                        <TableCell>{product.name} ({product.sku})</TableCell>
                                                        <TableCell>{product.quantity}</TableCell>
                                                    </TableRow>
                                                )
                                            })}
                                        </TableBody>
                                    ) : (
                                        <TableRow>
                                            <TableCell colSpan={2}>{props.noResultsLabel}</TableCell>
                                        </TableRow>
                                    )}
                                </Table>
                            </TableContainer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

OrdersAnalyticsDetail.propTypes = {
    title: PropTypes.string.isRequired,
    productNameLabel: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    numberOfOrdersLabel: PropTypes.string.isRequired,
    top10OrderedProducts: PropTypes.string.isRequired,
    chartLables: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired
}

export default OrdersAnalyticsDetail;