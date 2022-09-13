import React, { Fragment, useState, useContext } from "react";
import PropTypes from "prop-types";
import { Context } from "../../../../../shared/stores/Store";
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button, Tooltip,
    FormControlLabel, Checkbox, TablePagination,
    CircularProgress, TextField
} from "@mui/material";
import { GetApp, Link, LockOutlined } from "@mui/icons-material";
import moment from "moment";
import ClipboardHelper from "../../../../../shared/helpers/globals/ClipboardHelper";
import JsZip from 'jszip';
import { saveAs } from 'file-saver';
import { toast } from "react-toastify";
import ResponseStatusConstants from "../../../../../shared/constants/ResponseStatusConstants";
import QueryStringSerializer from "../../../../../shared/helpers/serializers/QueryStringSerializer";
import AuthenticationHelper from "../../../../../shared/helpers/globals/AuthenticationHelper";

const DownloadCenterFiles = (props) => {
    const [state, dispatch] = useContext(Context);
    const [searchTerm, setSearchTerm] = useState("");
    const [selectedFiles, setSelectedFiles] = useState([]);
    const [total, setTotal] = useState(props.files ? props.files.total : 0);
    const [files, setFiles] = useState(props.files ? props.files.data : []);
    const [page, setPage] = useState(0);
    
    const handleCopyClick = (file) => {
        ClipboardHelper.copyToClipboard(file.url);
    };

    const handleSelectItem = (file) => {
        const selectedIndex = selectedFiles.indexOf(file);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selectedFiles, file);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selectedFiles.slice(1));
        } else if (selectedIndex === selectedFiles.length - 1) {
            newSelected = newSelected.concat(selectedFiles.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selectedFiles.slice(0, selectedIndex), selectedFiles.slice(selectedIndex + 1));
        }

        setSelectedFiles(newSelected);
    }

    const handleSelectAllItems = e => {
        if (e.target.checked){
            setSelectedFiles(files.map((f) => f))
            return;
        }

        setSelectedFiles([]);
    }

    const handleDownloadFiles = checkedFiles => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        let filesToDownload = files;

        if (checkedFiles && selectedFiles.length > 0) {
            filesToDownload = selectedFiles;
        }

        if (filesToDownload.length > 0) {
            const zip = new JsZip();
            const filename = `${props.filesLabel}-${moment().local().toISOString()}`;
            const folder = zip.folder(`${filename}`);

            for(let i = 0; i < filesToDownload.length; i++) {
                const blobPromise = fetch(filesToDownload[i].url).then((r) => {
                    if (r.status === ResponseStatusConstants.ok()) {
                        return r.blob();
                    }

                    return Promise.reject(new Error(r.statusText));
                });

                folder.file(filesToDownload[i].filename, blobPromise);
            }

            zip.generateAsync({ type: "blob" })
                .then((blob) => saveAs(blob, `${filename}.zip`))
                .then(() => dispatch({ type: "SET_IS_LOADING", payload: false }));
        }
    }

    const handleChangePage = (event, newPage) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        setPage(newPage);

        const searchParameters = {
            id: props.id,
            searchTerm,
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

    const handleSearchButton = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const searchParameters = {
            id: props.id,
            searchTerm,
            pageIndex: 1,
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
                        setPage(0)
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

    const handleOnChange = (event) => {
        setSearchTerm(event.target.value);
    }

    return (
        <Fragment>
            {files && 
                <section className="section files pt-5">
                    <div className="files__content-box">
                        <div className="files__search">
                            <TextField id="search" name="search" label={props.searchLabel} className="files__search-field" value={searchTerm} onChange={handleOnChange} variant="standard" type="search" autoComplete="off" />
                            <Button onClick={handleSearchButton} className="ml-5" type="button" variant="contained" color="primary">
                                {props.searchLabel}
                            </Button>
                        </div>
                        <div className="is-flex is-justify-content-space-between is-align-items-center files__header">
                            <h3 className="title is-4">{props.filesLabel}</h3>
                                <div className="files__buttons">
                                    <button className="button is-text" type="button" onClick={() => handleDownloadFiles(true)} disabled={selectedFiles.length > 0 ? false : true}>{props.downloadSelectedLabel}</button>
                                    <button className="button is-text" type="button" onClick={() => handleDownloadFiles()}>{props.downloadEverythingLabel}</button>
                                </div>
                        </div>
                    </div>
                    {files.length > 0 ? (
                        <div className="table-container">
                            <div className="catalog__table">
                                <TableContainer component={Paper}>
                                    <Table aria-label={props.filesLabel}>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell width="11%">
                                                    <Checkbox
                                                        indeterminate={selectedFiles.length > 0 && selectedFiles.length < total}
                                                        checked={total > 0 && selectedFiles.length === total}
                                                        onChange={handleSelectAllItems} 
                                                    />
                                                </TableCell>
                                                <TableCell>{props.filenameLabel}</TableCell>
                                                <TableCell>{props.nameLabel}</TableCell>
                                                <TableCell>{props.descriptionLabel}</TableCell>
                                                <TableCell>{props.sizeLabel}</TableCell>
                                                <TableCell>{props.lastModifiedDateLabel}</TableCell>
                                                <TableCell>{props.createdDateLabel}</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {files.map((file, index) => {
                                                const isFileSelected = selectedFiles.indexOf(file) !== -1;

                                                return (
                                                    <TableRow key={index}>
                                                        <TableCell width="20%">
                                                            <div className="files__tooltip">
                                                                <Tooltip title={props.selectFileLabel} aria-label={props.selectFileLabel}>
                                                                    <FormControlLabel 
                                                                        control={
                                                                            <Checkbox 
                                                                                checked={isFileSelected}
                                                                                onChange={() => handleSelectItem(file)}
                                                                            />
                                                                        }
                                                                    />
                                                                </Tooltip>
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
                                                        <TableCell>{moment(file.lastModifiedDate).local().format("L LT")}</TableCell>
                                                        <TableCell>{moment(file.createdDate).local().format("L LT")}</TableCell>
                                                    </TableRow>
                                                )
                                            })}
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
                    ) : (
                        <section className="is-flex-centered">
                            <span className="is-title is-5">{props.noResultsLabel}</span>
                        </section>
                    )}
                </section>
            }
            {state.isLoading && <CircularProgress className="progressBar" />}
        </Fragment>
    );
}

DownloadCenterFiles.propTypes = {
    id: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    defaultPageSize: PropTypes.number.isRequired,
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
    selectFileLabel: PropTypes.string.isRequired,
    downloadSelectedLabel: PropTypes.string.isRequired,
    downloadEverythingLabel: PropTypes.string.isRequired,
    displayedRowsLabel: PropTypes.string.isRequired,
    rowsPerPageLabel: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired
};

export default DownloadCenterFiles;