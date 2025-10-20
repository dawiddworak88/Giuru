
import React, { useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import moment from "moment";
import { Plus } from "react-feather";
import {
    Delete, Edit, FileCopyOutlined, Link
} from "@mui/icons-material";
import {
    Button, TextField, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TablePagination, CircularProgress, Fab,
    Tooltip, NoSsr
} from "@mui/material";
import KeyConstants from "../../../shared/constants/KeyConstants";
import { Context } from "../../stores/Store";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import ConfirmationDialog from "../ConfirmationDialog/ConfirmationDialog";
import ClipboardHelper from "../../../shared/helpers/globals/ClipboardHelper";
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import { TextSnippet } from "@mui/icons-material";
import OrdersStatusFilters from "../OrdersStatusFilters/OrdersStatusFilters";

function Catalog(props) {
    const [state, dispatch] = useContext(Context);
    const [page, setPage] = React.useState(0);
    const [searchTerm, setSearchTerm] = React.useState(props.searchTerm ? props.searchTerm : "");
    const [items, setItems] = React.useState(props.pagedItems ? props.pagedItems.data : []);
    const [total, setTotal] = React.useState(props.pagedItems ? props.pagedItems.total : 0);
    const [openDeleteDialog, setOpenDeleteDialog] = React.useState(false);
    const [entityToDelete, setEntityToDelete] = React.useState(null);
    const [selectedStatusId, setSelectedStatusId] = React.useState(null);

    const handleSearchTermKeyPress = (event) => {
        if (event.key === KeyConstants.enter()) {
            search();
        }
    };

    const handleOnChange = (event) => {
        setSearchTerm(event.target.value);
    };

    const handleChangePage = (event, newPage) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        setPage(() => newPage);

        const searchParameters = {
            searchTerm,
            pageIndex: newPage + 1,
            itemsPerPage: props.defaultItemsPerPage,
            orderStatusId: selectedStatusId
        };

        fetch(searchParameters);
    };

    const search = () => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const searchParameters = {
            searchTerm,
            pageIndex: 1,
            itemsPerPage: props.defaultItemsPerPage,
            orderStatusId: selectedStatusId
        };

        fetch(searchParameters, () => setPage(0));
    };

    const handleDeleteClick = (item) => {
        setEntityToDelete(() => item);
        setOpenDeleteDialog(() => true);
    };

    const handleDeleteDialogClose = () => {
        setOpenDeleteDialog(() => false);
        setEntityToDelete(() => null);
    };

    const handleDeleteEntity = () => {

        dispatch({ type: "SET_IS_LOADING", payload: true });
        
        const deleteParameters = {
            id: entityToDelete.id
        };

        const requestOptions = {
            method: "DELETE",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const url = props.deleteApiUrl + "?" + QueryStringSerializer.serialize(deleteParameters);

        return fetch(url, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        toast.success(jsonResponse.message);
                        setItems(() => items.filter(item => item.id !== entityToDelete.id));
                        setTotal(() => total - 1);
                        setOpenDeleteDialog(() => false);
                        setEntityToDelete(() => null);
                    } else {
                        toast.error(jsonResponse.message);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const copyToClipboard = (text) => {
        ClipboardHelper.copyToClipboard(text);
    }

    const handleStatusChange = (statusId) => {
        setSelectedStatusId(statusId);

        dispatch({ type: "SET_IS_LOADING", payload: true });

        let searchParameters = {
            searchTerm,
            pageIndex: 1,
            itemsPerPage: props.defaultItemsPerPage,
            orderStatusId: statusId
        }

        fetch(searchParameters, () => setPage(0));
    }

    const fetch = (searchParameters, onSuccess) => {
        const requestOptions = {
            method: "GET",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setItems(() => jsonResponse.data);
                        setTotal(() => jsonResponse.total);

                        if (typeof onSuccess === "function") {
                            onSuccess();
                        }
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
        <section className="section section-small-padding catalog">
            <h1 className="subtitle is-4">{props.title}</h1>
            {props.newUrl &&
                <div>
                    <a href={props.newUrl} className="button is-primary">
                    <span className="icon">
                        <Plus />
                    </span>
                    <span>
                        {props.newText}
                    </span>
                    </a>
                </div>
            }
            <div>
                {props.searchLabel &&
                    <div className="catalog__search is-flex-centered">
                        <TextField id="search" name="search" value={searchTerm} onChange={handleOnChange} variant="standard" onKeyPress={handleSearchTermKeyPress} className="catalog__search-field" label={props.searchLabel} type="search" autoComplete="off" />
                        <Button onClick={search} type="button" variant="contained" color="primary">
                            {props.searchLabel}
                        </Button>
                    </div>
                }
                {props.ordersStatuses && 
                    <OrdersStatusFilters 
                        ordersStatuses={props.ordersStatuses}
                        selectedStatusId={selectedStatusId}
                        onStatusChange={handleStatusChange}
                    />
                }
                {(items && items.length > 0 && props.table) ?
                    (<div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.title}>
                                    <TableHead>
                                        <TableRow>
                                            {props.table.actions &&
                                                <TableCell width="12%"></TableCell>
                                            }
                                            {props.table.labels.map((item, index) =>
                                                <TableCell key={index} value={item}>{item}</TableCell>
                                            )}
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {items.map((item, index) => (
                                            <TableRow key={index}>
                                                {props.table.actions &&
                                                    <TableCell width="12%">
                                                        {props.table.actions.map((actionItem, index) => {
                                                            if (actionItem.isEdit) return (
                                                                <Tooltip title={props.editLabel} aria-label={props.editLabel} key={index}>
                                                                    <Fab href={props.editUrl + "/" + item.id + (searchTerm ? `?searchTerm=${searchTerm}` : "")} size="small" color="secondary">
                                                                        <Edit />
                                                                    </Fab>
                                                                </Tooltip>)
                                                            else if (actionItem.isDelete) return (
                                                                <Tooltip title={props.deleteLabel} aria-label={props.deleteLabel} key={index}>
                                                                    <Fab onClick={() => handleDeleteClick(item)} size="small" color="primary">
                                                                        <Delete />
                                                                    </Fab>
                                                                </Tooltip>)
                                                            else if (actionItem.isDuplicate) return (
                                                                <Tooltip title={props.duplicateLabel} aria-label={props.duplicateLabel} key={index}>
                                                                    <Fab href={props.duplicateUrl + "/" + item.id} size="small" color="secondary">
                                                                        <FileCopyOutlined />
                                                                    </Fab>
                                                                </Tooltip>)
                                                            else if (actionItem.isPicture) return (
                                                                <Tooltip title={props.copyLinkLabel} aria-label={props.copyLinkLabel} key={index}>
                                                                    <Fab onClick={() => copyToClipboard(item.url)} size="small" color="secondary">
                                                                        <Link />
                                                                    </Fab>
                                                                </Tooltip>)
                                                            else return (
                                                                <div></div>)})}
                                                    </TableCell>
                                                }
                                                {props.table.properties && props.table.properties.map((property, index) => {
                                                    if (property.isPicture){
                                                        return (
                                                            <TableCell key={index}>
                                                                <div className="property-image">
                                                                    {item[property.title] ? (
                                                                        <img src={item[property.title]}/>
                                                                    ) : (
                                                                        <div className="is-flex is-justify-content-center">
                                                                            <TextSnippet className="is-size-2" color="primary"/>
                                                                        </div>
                                                                    )}
                                                                </div>
                                                            </TableCell>
                                                        )
                                                    }
                                                    else if (property.isDateTime){
                                                        return (
                                                            <NoSsr key={index}>
                                                                <TableCell>{moment.utc(item[property.title]).local().format("L LT")}</TableCell>
                                                            </NoSsr>
                                                        )
                                                    }
                                                    else {
                                                        return (
                                                            <TableCell key={index}>{item[property.title] !== null ? item[property.title] : "-"}</TableCell>
                                                        )}})}
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                        <div className="catalog__pagination is-flex-centered">
                            <TablePagination
                                labelDisplayedRows={({ from, to, count }) => `${from} - ${to} ${props.displayedRowsLabel} ${count}`}
                                labelRowsPerPage={props.rowsPerPageLabel}
                                rowsPerPageOptions={[props.defaultItemsPerPage]}
                                component="div"
                                count={total}
                                page={page}
                                onPageChange={handleChangePage}
                                rowsPerPage={props.defaultItemsPerPage}
                            />
                        </div>
                    </div>) :
                    (<section className="section is-flex-centered">
                        <span className="is-title is-5">{props.noResultsLabel}</span>
                    </section>)
                }
                <ConfirmationDialog
                    open={openDeleteDialog}
                    handleClose={handleDeleteDialogClose}
                    handleConfirm={handleDeleteEntity}
                    titleId="alert-dialog-title"
                    title={props.deleteConfirmationLabel}
                    textId="alert-dialog-description"
                    text={props.areYouSureLabel + ": " + (
                        entityToDelete ? props.confirmationDialogDeleteNameProperty && props.confirmationDialogDeleteNameProperty.length > 0 ? 
                            props.confirmationDialogDeleteNameProperty.map((property) => {
                                return entityToDelete[`${property}`]
                            }
                        ).join(" ") : entityToDelete["name"] : ""
                    )}
                    noLabel={props.noLabel}
                    yesLabel={props.yesLabel}
                />
                {state.isLoading && <CircularProgress className="progressBar" />}
            </div>
        </section>
    );
}

Catalog.propTypes = {
    title: PropTypes.string.isRequired,
    newText: PropTypes.string,
    newUrl: PropTypes.string,
    noLabel: PropTypes.string,
    yesLabel: PropTypes.string,
    deleteConfirmationLabel: PropTypes.string,
    areYouSureLabel: PropTypes.string,
    generalErrorMessage: PropTypes.string.isRequired,
    searchLabel: PropTypes.string.isRequired,
    searchApiUrl: PropTypes.string.isRequired,
    editLabel: PropTypes.string,
    deleteLabel: PropTypes.string,
    duplicateLabel: PropTypes.string,
    displayedRowsLabel: PropTypes.string.isRequired,
    rowsPerPageLabel: PropTypes.string.isRequired,
    editUrl: PropTypes.string,
    deleteUrl: PropTypes.string,
    duplicateUrl: PropTypes.string,
    noResultsLabel: PropTypes.string.isRequired,
    table: PropTypes.object.isRequired,
    confirmationDialogDeleteNameProperty: PropTypes.array,
    defaultItemsPerPage: PropTypes.number.isRequired,
    copyLinkLabel: PropTypes.string
}

export default Catalog;