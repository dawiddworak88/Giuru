
import React, { useContext } from "react";
import { toast } from "react-toastify";
import moment from "moment";
import Fab from "@material-ui/core/Fab";
import DeleteIcon from "@material-ui/icons/Delete";
import EditIcon from "@material-ui/icons/Edit";
import {
    Button, TextField, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TablePagination, CircularProgress,
    Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle
} from "@material-ui/core";
import FetchErrorHandler from "../../../../../../shared/helpers/errorHandlers/FetchErrorHandler";
import KeyConstants from "../../../../../../shared/constants/KeyConstants";
import { Context } from "../../../../../../shared/stores/Store";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";
import PaginationConstants from "../../../../../../shared/constants/PaginationConstants";

function ProductCatalog(props) {

    const [state, dispatch] = useContext(Context);
    const [page, setPage] = React.useState(0);
    const [itemsPerPage,] = React.useState(PaginationConstants.defaultRowsPerPage());
    const [searchTerm, setSearchTerm] = React.useState("");
    const [products, setProducts] = React.useState(props.pagedProducts.data);
    const [total, setTotal] = React.useState(props.pagedProducts.total);
    const [openDeleteDialog, setOpenDeleteDialog] = React.useState(false);
    const [entityToDelete, setEntityToDelete] = React.useState(null);

    const handleSearchTermKeyPress = (event) => {

        if (event.key === KeyConstants.enter()) {
            search();
        }
    }

    const handleOnChange = (event) => {
        setSearchTerm(event.target.value);
    }

    const handleChangePage = (event, newPage) => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        setPage(() => newPage);

        const searchParameters = {

            searchTerm: searchTerm,
            pageIndex: newPage + 1,
            itemsPerPage
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setProducts(() => []);
                        setProducts(() => jsonResponse.data.pagedProducts.data);
                        setTotal(() => jsonResponse.data.pagedProducts.total);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(searchParameters, response, jsonResponse);
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(error => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const search = () => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const searchParameters = {

            searchTerm: searchTerm,
            pageIndex: 1,
            itemsPerPage: itemsPerPage
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        };

        const url = props.searchApiUrl + "?" + QueryStringSerializer.serialize(searchParameters);

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setPage(() => 0);

                        setProducts(() => []);
                        setProducts(() => jsonResponse.data.pagedProducts.data);
                        setTotal(() => jsonResponse.data.pagedProducts.total);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(searchParameters, response, jsonResponse);
                        toast.error(props.generalErrorMessage);
                    }
                })
            }).catch(error => {
                console.log(error);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    }

    const handleDeleteClick = (product) => {
        setEntityToDelete(() => product);
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
            headers: { "Content-Type": "application/json" }
        };

        const url = props.deleteApiUrl + "?" + QueryStringSerializer.serialize(deleteParameters);

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        toast.success(jsonResponse.message);
                        setProducts(() => products.filter(item => item.id !== entityToDelete.id));
                        setTotal(() => total - 1);
                        setOpenDeleteDialog(() => false);
                        setEntityToDelete(() => null);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(url, response, jsonResponse);
                        toast.error(props.generalErrorMessage);
                    }
                })
            }).catch(error => {
                console.log(error);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    return (
        <div>
            <div className="catalog__search is-flex-centered">
                <TextField id="search" name="search" value={searchTerm} onChange={handleOnChange} onKeyPress={handleSearchTermKeyPress} className="catalog__search-field" label={props.searchLabel} type="search" autoComplete="off" />
                <Button onClick={search} type="button" variant="contained" color="primary">
                    {props.searchLabel}
                </Button>
            </div>
            {(products && products.length > 0) ?
                (<div className="table-container">
                    <div className="catalog__table">
                        <TableContainer component={Paper}>
                            <Table aria-label={props.title}>
                                <TableHead>
                                    <TableRow>
                                        <TableCell width="11%"></TableCell>
                                        <TableCell>{props.skuLabel}</TableCell>
                                        <TableCell>{props.nameLabel}</TableCell>
                                        <TableCell>{props.lastModifiedDateLabel}</TableCell>
                                        <TableCell>{props.createdDateLabel}</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {products.map((product) => (
                                        <TableRow key={product.name}>
                                            <TableCell width="11%">
                                                <a href={props.editUrl + "/" + product.id}>
                                                    <Fab size="small" color="secondary" aria-label={props.editLabel}>
                                                        <EditIcon />
                                                    </Fab>
                                                </a>
                                                <Fab onClick={() => handleDeleteClick(product)} size="small" color="primary" aria-label={props.deleteLabel}>
                                                    <DeleteIcon />
                                                </Fab>
                                            </TableCell>
                                            <TableCell>{product.sku}</TableCell>
                                            <TableCell>{product.name}</TableCell>
                                            <TableCell>{moment(product.lastModifiedDate).local().format("L LT")}</TableCell>
                                            <TableCell>{moment(product.createdDate).local().format("L LT")}</TableCell>
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
                            rowsPerPageOptions={[ PaginationConstants.defaultRowsPerPage() ]}
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
            <Dialog
                open={openDeleteDialog}
                onClose={handleDeleteDialogClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description">
                <DialogTitle id="alert-dialog-title">{props.deleteConfirmationLabel}</DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        {props.areYouSureLabel}: {entityToDelete ? entityToDelete.name : ""}?
                </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleDeleteDialogClose} color="primary">
                        {props.noLabel}
                    </Button>
                    <Button disabled={state.isLoading} onClick={handleDeleteEntity} color="primary" autoFocus>
                        {props.yesLabel}
                    </Button>
                </DialogActions>
            </Dialog>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

export default ProductCatalog;