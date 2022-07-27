import React, { Fragment, useState } from "react";
import PropTypes from "prop-types";
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button, Tooltip,
    FormControlLabel, Checkbox
} from "@mui/material";
import { GetApp, Link, LockOutlined } from "@mui/icons-material";
import moment from "moment";
import ClipboardHelper from "../../../../../shared/helpers/globals/ClipboardHelper";
import JsZip from 'jszip';
import { saveAs } from 'file-saver';

const DownloadCenterFiles = (props) => {
    const [selectedFiles, setSelectedFiles] = useState([]);
    
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
            setSelectedFiles(props.files.map((f) => f))
            return;
        }

        setSelectedFiles([]);
    }

    const handleDownloadFiles = async (checkedFiles) => {
        let files = props.files;

        if (checkedFiles && selectedFiles.length > 0){
            files = selectedFiles;
        }

        if (files.length > 0){

            const zip = new JsZip();
            const folder = zip.folder(`${props.filesLabel}-${currentDate}`);

            for(let i = 0; i < files.length; i++ ) {
                let file = files[i];

                const blobPromise = fetch(file.url).then((r) => {
                    if (r.status === 200) return r.blob();
                    return Promise.reject(new Error(r.statusText));
                });

                const name = file.url.substring(file.url.lastIndexOf("/") + 1);

                folder.file(name, blobPromise);
            }

            zip.generateAsync({ type: "blob" }).then((blob) => saveAs(blob, `${props.filesLabel}-${new Date().getTime()}.zip`));
        }
    }

    return (
        <Fragment>
            {props.files &&
                <section className="section files pt-5">
                    <div className="is-flex is-justify-content-space-between is-align-items-center files__header">
                        <h3 className="title is-4">{props.filesLabel}</h3>
                            <div className="files__buttons">
                                <button className="button is-text" type="button" onClick={() => handleDownloadFiles(true)} disabled={selectedFiles.length > 0 ? false : true}>{props.downloadSelectedLabel}</button>
                                <button className="button is-text" type="button" onClick={() => handleDownloadFiles()}>{props.downloadEverythingLabel}</button>
                            </div>
                    </div>
                    <div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.filesLabel}>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell width="11%">
                                                <Checkbox
                                                    indeterminate={selectedFiles.length > 0 && selectedFiles.length < props.files.length}
                                                    checked={props.files.length > 0 && selectedFiles.length === props.files.length}
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
                                        {props.files.map((file, index) => {
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
                    </div>
                </section>
            }
        </Fragment>
    );
}

DownloadCenterFiles.propTypes = {
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
    downloadEverythingLabel: PropTypes.string.isRequired
};

export default DownloadCenterFiles;