import React, { Fragment, useState, useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button, Tooltip, TablePagination, NoSsr
} from "@mui/material";
import { GetApp, Link, LockOutlined } from "@mui/icons-material";
import moment from "moment";
import { Context } from "../../stores/Store";
import ClipboardHelper from "../../helpers/globals/ClipboardHelper";
import QueryStringSerializer from "../../helpers/serializers/QueryStringSerializer";
import AuthenticationHelper from "../../helpers/globals/AuthenticationHelper";

function Files(props) {
    const [state, dispatch] = useContext(Context);
    const [files, setFiles] = useState(props.files ? props.files.data : []);
    const [total, setTotal] = useState(props.files ? props.files.total : 0);
    const [page, setPage] = useState(0);
    
    const handleCopyClick = (file) => {
        ClipboardHelper.copyToClipboard(file.url);
    };

    const handleChangePage = (event, newPage) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        setPage(newPage);

        const searchParameters = {
            id: props.id,
            pageIndex: newPage + 1,
            itemsPerPage: props.defaultPageSize
        };

        const requestOptions = {
            method: "GET",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFiles(jsonResponse.data);
                        setTotal(jsonResponse.total);
                    } else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    return (
        <Fragment>
            {files &&
                <section className="section files">
                    <h3 className="title is-4">{props.filesLabel}</h3>
                    <div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.filesLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell width="11%"></TableCell>
                                            <TableCell>{props.filenameLabel}</TableCell>
                                            <TableCell>{props.nameLabel}</TableCell>
                                            <TableCell>{props.descriptionLabel}</TableCell>
                                            <TableCell>{props.sizeLabel}</TableCell>
                                            <TableCell>{props.lastModifiedDateLabel}</TableCell>
                                            <TableCell>{props.createdDateLabel}</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {files.map((file, index) => (
                                            <TableRow key={index}>
                                                <TableCell width="11%">
                                                    <div className="files__tooltip">
                                                        <Tooltip title={props.downloadLabel} aria-label={props.downloadLabel}>
                                                            <Fab href={file.url} size="small" color="primary">
                                                                <GetApp />
                                                            </Fab>
                                                        </Tooltip>
                                                        <Tooltip title={props.copyLinkLabel} aria-label={props.copyLinkLabel}>
                                                            <Fab onClick={() => handleCopyClick(file)} size="small" color="secondary">
                                                                <Link />
                                                            </Fab>
                                                        </Tooltip>
                                                    </div>
                                                </TableCell>
                                                <TableCell>
                                                    <Button variant="text" href={file.url}>
                                                        {file.filename}
                                                        {(file.isProtected && !props.isAuthenticated) &&
                                                            <LockOutlined color="primary" />
                                                        }
                                                    </Button>
                                                </TableCell>
                                                <TableCell>{file.name}</TableCell>
                                                <TableCell>{file.description}</TableCell>
                                                <TableCell>{file.size}</TableCell>
                                                <NoSsr>
                                                    <TableCell>{moment(file.lastModifiedDate).local().format("L LT")}</TableCell>
                                                    <TableCell>{moment(file.createdDate).local().format("L LT")}</TableCell>
                                                </NoSsr>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                        <TablePagination 
                            labelDisplayedRows={({ from, to, count }) => `${from} - ${to} ${props.displayedRowsLabel} ${count}`}
                            count={total}
                            rowsPerPageOptions={[props.defaultPageSize]}
                            rowsPerPage={props.defaultPageSize}
                            component="div"
                            page={page}
                            onPageChange={handleChangePage}
                            labelRowsPerPage={props.rowsPerPageLabel}
                        />
                    </div>
                </section>
            }
        </Fragment>
    );
}

Files.propTypes = {
    files: PropTypes.array.isRequired,
    filesLabel: PropTypes.string.isRequired,
    downloadLabel: PropTypes.string.isRequired,
    copyLinkLabel: PropTypes.string.isRequired,
    filenameLabel: PropTypes.string.isRequired,
    sizeLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    lastModifiedDateLabel: PropTypes.string.isRequired,
    createdDateLabel: PropTypes.string.isRequired,
    rowsPerPageLabel: PropTypes.string.isRequired,
    displayedRowsLabel: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    defaultPageSize: PropTypes.number.isRequired,
    searchApiUrl: PropTypes.string.isRequired
};

export default Files;