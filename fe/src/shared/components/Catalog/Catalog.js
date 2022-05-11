
import React, { useContext } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import moment from "moment";
import { Plus } from "react-feather";
import {
    Delete, Edit
} from "@material-ui/icons";
import FileCopyOutlinedIcon from "@material-ui/icons/FileCopyOutlined";
import {
    Button, TextField, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TablePagination, CircularProgress, Fab
} from "@material-ui/core";
import KeyConstants from "../../constants/KeyConstants";
import { Context } from "../../stores/Store";
import QueryStringSerializer from "../../helpers/serializers/QueryStringSerializer";
import PaginationConstants from "../../constants/PaginationConstants";
import ConfirmationDialog from "../ConfirmationDialog/ConfirmationDialog";
import AuthenticationHelper from "../../helpers/globals/AuthenticationHelper";

function Catalog(props) {

    const [state, dispatch] = useContext(Context);
    const [page, setPage] = React.useState(0);
    const [itemsPerPage,] = React.useState(PaginationConstants.defaultRowsPerPage());
    const [searchTerm, setSearchTerm] = React.useState("");
    const [items, setItems] = React.useState(props.pagedItems ? props.pagedItems.data : []);
    const [total, setTotal] = React.useState(props.pagedItems ? props.pagedItems.total : 0);
    const [openDeleteDialog, setOpenDeleteDialog] = React.useState(false);
    const [entityToDelete, setEntityToDelete] = React.useState(null);

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
            itemsPerPage
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setItems(() => []);
                        setItems(() => jsonResponse.data);
                        setTotal(() => jsonResponse.total);
                    } else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const search = () => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const searchParameters = {

            searchTerm,
            pageIndex: 1,
            itemsPerPage
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setPage(() => 0);

                        setItems(() => []);
                        setItems(() => jsonResponse.data);
                        setTotal(() => jsonResponse.total);
                    } else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
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
    
    return (
        <section className="section section-small-padding catalog">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div>
              {props.newUrl &&
                <a href={props.newUrl} className="button is-primary">
                  <span className="icon">
                    <Plus />
                  </span>
                  <span>
                    {props.newText}
                  </span>
                </a>
              }
            </div>
            <div>
                {props.searchLabel &&
                    <div className="catalog__search is-flex-centered">
                        <TextField id="search" name="search" value={searchTerm} onChange={handleOnChange} onKeyPress={handleSearchTermKeyPress} className="catalog__search-field" label={props.searchLabel} type="search" autoComplete="off" />
                        <Button onClick={search} type="button" variant="contained" color="primary">
                            {props.searchLabel}
                        </Button>
                    </div>
                }
                {(items && items.length > 0 && props.table) ?
                    (<div className="table-container">
                        <div className="catalog__table">
                            <TableContainer component={Paper}>
                                <Table aria-label={props.title}>
                                    <TableHead>
                                        <TableRow>
                                            {props.table.actions &&
                                                <TableCell width="11%"></TableCell>
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
                                                    <TableCell width="11%">
                                                        {props.table.actions.map((actionItem, index) => {
                                                            if (actionItem.isEdit) return (
                                                                <Fab href={props.editUrl + "/" + item.id} size="small" color="secondary" aria-label={props.editLabel} key={index}>
                                                                    <Edit />
                                                                </Fab>)
                                                            else if (actionItem.isDelete) return (
                                                                <Fab onClick={() => handleDeleteClick(item)} size="small" color="primary" aria-label={props.deleteLabel} key={index}>
                                                                    <Delete />
                                                                </Fab>)
                                                            else if (actionItem.isDuplicate) return (
                                                                <Fab href={props.duplicateUrl + "/" + item.id} size="small" color="secondary" aria-label={props.duplicateLabel} key={index}>
                                                                    <FileCopyOutlinedIcon />
                                                                </Fab>)
                                                            else return (
                                                                <div></div>)})}
                                                    </TableCell>
                                                }

                                                {props.table.properties && props.table.properties.map((property, index) => {

                                                    if (property.isDateTime) return (
                                                        <TableCell key={index}>{moment.utc(item[property.title]).local().format("L LT")}</TableCell>
                                                    )
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
                                backIconButtonText={props.backIconButtonText}
                                nextIconButtonText={props.nextIconButtonText}
                                rowsPerPageOptions={[PaginationConstants.defaultRowsPerPage()]}
                                component="div"
                                count={total}
                                page={page}
                                onChangePage={handleChangePage}
                                rowsPerPage={PaginationConstants.defaultRowsPerPage()}
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
                    text={props.areYouSureLabel + (entityToDelete ? (entityToDelete.name ? ": " + entityToDelete.name : ": " + entityToDelete.productName) : "")}
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
    backIconButtonText: PropTypes.string.isRequired,
    nextIconButtonText: PropTypes.string.isRequired,
    editUrl: PropTypes.string,
    deleteUrl: PropTypes.string,
    duplicateUrl: PropTypes.string,
    noResultsLabel: PropTypes.string.isRequired,
    table: PropTypes.object.isRequired
}

export default Catalog;