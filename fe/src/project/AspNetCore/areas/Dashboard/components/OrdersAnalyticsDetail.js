import React from "react";
import PropTypes from "prop-types";
import SalesAnalytics from "../../../../../shared/components/AnalyticsDashboard/SalesAnalytics/SalesAnalytics";
import { 
    TableContainer, Paper, Table, TableHead, 
    TableRow, TableCell, TableBody 
} from "@mui/material";

const OrdersAnalyticsDetail = (props) => {
    return (
        <div className="section">
            <h1 className="title is-3">{props.title}</h1>
            <div className="mt-6">
                <div className="columns">
                    <div className="column is-two-thirds">
                        {props.salesAnalytics && 
                            <SalesAnalytics {...props.salesAnalytics} />
                        }
                    </div>
                    <div className="column is-one-third">
                        <h3 className="subtitle">{props.topOrderedProducts}</h3>
                        <div className="table-container">
                            <TableContainer component={Paper} className="orders-analytics__table">
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell>{props.quantityLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {props.products && props.products.length > 0 ? (
                                            props.products.map((product, index) => {
                                                return (
                                                    <TableRow key={index}>
                                                        <TableCell>{product.name} ({product.sku})</TableCell>
                                                        <TableCell>{product.quantity}</TableCell>
                                                    </TableRow>
                                                )
                                            })
                                        ) : (
                                            <TableRow>
                                                <TableCell colSpan={2}>{props.noResultsLabel}</TableCell>
                                            </TableRow>
                                        )}
                                    </TableBody>
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
    nameLabel: PropTypes.string.isRequired,
    quantityLabel: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    numberOfOrdersLabel: PropTypes.string.isRequired,
    topOrderedProducts: PropTypes.string.isRequired,
    chartLables: PropTypes.array.isRequired,
    chartDatasets: PropTypes.array.isRequired
}

export default OrdersAnalyticsDetail;