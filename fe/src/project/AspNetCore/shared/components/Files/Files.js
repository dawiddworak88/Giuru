import React, { Fragment } from "react";
import PropTypes from "prop-types";
import {
    Fab, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, Button
} from "@material-ui/core";
import { GetApp, Link, LockOutlined } from "@material-ui/icons";
import moment from "moment";

function Files(props) {

    const handleDownloadClick = (file) => {
    };

    const handleCopyClick = (file) => {
    }

    return (
        
        <Fragment>
            {props.files &&
                <section className="section files">
                    <h3 className="files__title">{props.downloadLabel}</h3>
                    <div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.downloadLabel}>
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
                                        {props.files.map((file) => (
                                            <TableRow key={file.id}>
                                                <TableCell width="11%">
                                                    <Fab onClick={() => handleDownloadClick(file)} size="small" color="primary" aria-label={props.deleteLabel}>
                                                        <GetApp />
                                                    </Fab>
                                                    <Fab onClick={() => handleCopyClick(file)} size="small" color="secondary" aria-label={props.copyLinkLabel}>
                                                        <Link />
                                                    </Fab>
                                                </TableCell>
                                                <TableCell>
                                                    <Button variant="text" onClick={() => handleDownloadClick(file)}>
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
                                        ))}
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

Files.propTypes = {
    files: PropTypes.array.isRequired,
    downloadLabel: PropTypes.string.isRequired,
    filenameLabel: PropTypes.string.isRequired,
    sizeLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    lastModifiedDateLabel: PropTypes.string.isRequired,
    createdDateLabel: PropTypes.string.isRequired
}

export default Files;