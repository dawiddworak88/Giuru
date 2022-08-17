import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import {
    Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper
} from "@mui/material";
import moment from "moment";

const OrderItemStatusChanges = (props) => {
    const [isMounted, setMounted] = useState(false);

    useEffect(() => {
        setMounted(true)
    }, [])

    return (
        props.orderItemStatusChanges &&
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
                                {props.orderItemStatusChanges.map((statusChange, index) => 
                                    <TableRow key={index}>
                                        <TableCell>{statusChange.orderItemStatusName}</TableCell>
                                        <TableCell>{statusChange.orderItemStatusChangeComment}</TableCell>
                                        <TableCell>{isMounted ? moment.utc(statusChange.createdDate).local().format("L LT") : moment.utc(statusChange.createdDate).format("L LT")}</TableCell>
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
    title: PropTypes.string.isRequired,
    orderStatusLabel: PropTypes.string.isRequired,
    orderStatusCommentLabel: PropTypes.string.isRequired,
    lastModifiedDateLabel: PropTypes.string.isRequired,
    orderItemStatusChanges: PropTypes.array
}

export default OrderItemStatusChanges;