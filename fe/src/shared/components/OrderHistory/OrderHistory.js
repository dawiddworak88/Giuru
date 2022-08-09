import React from "react";
import PropTypes from "prop-types";
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button, Tooltip,
} from "@mui/material";
import moment from "moment";

const OrderHistory = (props) => {

    return (
        props.orderStatusesHistory &&
            <div className="order-history mt-5 p-5">
                <h3 className="title is-4">{props.title}</h3>
                <div className="table-container">
                    <TableContainer component={Paper}>
                        <Table aria-label={props.title}>
                            <TableHead>
                                <TableRow>
                                    <TableCell>{props.orderStatusLabel}</TableCell>
                                    <TableCell>{props.orderStatusCommentLabel}</TableCell>
                                    <TableCell>{props.lastModifiedDateLabel}</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {props.orderStatusesHistory.map((historyItem, index) => 
                                    <TableRow key={index}>
                                        <TableCell>{historyItem.orderStatusName}</TableCell>
                                        <TableCell>{historyItem.orderStatusComment}</TableCell>
                                        <TableCell>{moment(historyItem.createdDate).local().format("L LT")}</TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </div>
            </div>
    )
}

OrderHistory.propTypes = {
    title: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatusCommentLabel: PropTypes.string.isRequired,
    lastModifiedDateLabel: PropTypes.string.isRequired,
    orderStatusesHistory: PropTypes.array
}

export default OrderHistory;