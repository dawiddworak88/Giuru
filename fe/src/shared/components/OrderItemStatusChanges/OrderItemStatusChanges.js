import React from "react";
import PropTypes from "prop-types";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, NoSsr
} from "@mui/material";
import moment from "moment";

const OrderItemStatusChanges = (props) => {
    const { labels, statusChanges } = props;
    
    return (
        props.statusChanges &&
            <div className="order-history mt-5 p-5">
                <h3 className="title is-4">{labels.title}</h3>
                <div className="table-container">
                    <TableContainer component={Paper}>
                        <Table aria-label={labels.title}>
                            <TableHead>
                                <TableRow>
                                    <TableCell>{labels.orderStatusLabel}</TableCell>
                                    <TableCell>{labels.orderStatusCommentLabel}</TableCell>
                                    <TableCell>{labels.lastModifiedDateLabel}</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {props.statusChanges.map((statusChange, index) => 
                                    <TableRow key={index}>
                                        <TableCell>{statusChange.orderItemStatusName}</TableCell>
                                        <TableCell>{statusChange.orderItemStatusChangeComment}</TableCell>
                                        <NoSsr>
                                            <TableCell>{moment.utc(statusChange.createdDate).local().format("L LT")}</TableCell>
                                        </NoSsr>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </div>
            </div>
    )
}

OrderItemStatusChanges.propTypes = {
    labels: PropTypes.object.isRequired,
    statusChanges: PropTypes.array.isRequired
}

export default OrderItemStatusChanges;