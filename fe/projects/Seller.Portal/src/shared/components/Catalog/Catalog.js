import React, { useContext, useEffect } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import moment from "moment";
import { Plus } from "react-feather";
import {
    Delete, Edit, FileCopyOutlined, Link, QrCode2, DragIndicator
} from "@mui/icons-material";
import {
    Button, TextField, Table, TableBody, TableCell, TableContainer,
    TableHead, TableRow, Paper, TablePagination, CircularProgress, Fab,
    Tooltip, NoSsr, Chip
} from "@mui/material";
import KeyConstants from "../../../shared/constants/KeyConstants";
import CatalogConstants from "../../constants/CatalogConstants";
import { Context } from "../../stores/Store";
import QueryStringSerializer from "../../../shared/helpers/serializers/QueryStringSerializer";
import ConfirmationDialog from "../ConfirmationDialog/ConfirmationDialog";
import ClipboardHelper from "../../../shared/helpers/globals/ClipboardHelper";
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import { TextSnippet } from "@mui/icons-material";
import QRCodeDialog from "../QRCodeDialog/QRCodeDialog";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";

function Catalog(props) {
    const [state, dispatch] = useContext(Context);
    const [page, setPage] = React.useState(0);
    const [searchTerm, setSearchTerm] = React.useState(props.searchTerm ? props.searchTerm : "");
    const [items, setItems] = React.useState(props.pagedItems ? props.pagedItems.data : []);
    const [total, setTotal] = React.useState(props.pagedItems ? props.pagedItems.total : 0);
    const [openDeleteDialog, setOpenDeleteDialog] = React.useState(false);
    const [entityToDelete, setEntityToDelete] = React.useState(null);
    const [selectedItem, setSelectedItem] = React.useState(null);
    const [openQRCodeDialog, setOpenQRCodeDialog] = React.useState(false);
    const [isDragableDisable, setIsDragableDisable] = React.useState(true);
    const [draggingItem, setDraggingItem] = React.useState({});
    const [placeholderProps, setPlaceholderProps] = React.useState({});
    const [windowWidth, setWindowWidth] = React.useState(undefined);

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
            itemsPerPage: props.defaultItemsPerPage
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
            itemsPerPage: props.defaultItemsPerPage
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
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const handleChangeEntityOrder = (id, order) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const updateParameters = {
            id,
            order
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(updateParameters)
        };

        return fetch(props.updateOrderApiUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false })

                if (!response.ok) {
                    toast.error(props.generalErrorMessage);
                }
            })
    };

    const copyToClipboard = (text) => {
        ClipboardHelper.copyToClipboard(text);
    };

    const handleQRCodeDialog = (item) => {
        setSelectedItem(item);
        setOpenQRCodeDialog(true);
    };

    const onDragStart = (result) => {
        const item = items.find((x) => x.id === result.draggableId);

        if (item != null) {
            setDraggingItem(item);

            const domQuery = `[data-rbd-drag-handle-draggable-id='${result.draggableId}']`;
            const draggedDOM = document.querySelector(domQuery);

            setPlaceholderProps({ position: result.source.index, height: draggedDOM.clientHeight + 5});
        }
        else {
            setDraggingItem({})
        }
    };

    const onDragEnd = (result) => {
        setIsDragableDisable(true);
        setPlaceholderProps({});

        const { destination, source, draggableId } = result;

        if (!destination) {
            setDraggingItem({});
            return;
        }

        const currentDraggableItemOrder = draggingItem.order - 1 - (page * props.defaultItemsPerPage);

        if (
            destination.droppableId === source.droppableId && 
            destination.index === currentDraggableItemOrder
        ) {
            setDraggingItem({});
            return;
        }

        const newDraggableItemOrder = destination.index + 1 + (page * props.defaultItemsPerPage);

        if(newDraggableItemOrder > 0) {
            draggingItem.order = newDraggableItemOrder;

            const newCategoryArray = reorder(items, source.index, destination.index);

            handleChangeEntityOrder(draggableId, newDraggableItemOrder);
            setItems(newCategoryArray);
    
            setDraggingItem({});
        }
    };

    const reorder = (list, source, destination) => {
        const result = [...list];
        
        result.splice(source, 1);
        result.splice(destination, 0, draggingItem);

        return result;
    };

    const handleChangedPageWhenDragItem = (toPage) => {
        if (draggingItem && !isDragableDisable) {
            handleChangePage(null, toPage);
        }
    };

    useEffect(() => {
        if(typeof window !== 'undefined') {
            setWindowWidth(window.innerWidth);
        
            const handleResize = () => {
                setWindowWidth(window.innerWidth)
            };
    
            window.addEventListener("resize", handleResize);
        }
    }, windowWidth);

    const tableRow = (provided, item) => {
        return (
            <TableRow
                ref={provided.innerRef}
                {...provided.draggableProps}
                {...provided.dragHandleProps}
                className="catalog__table__row"
            >
                {props.table.actions &&
                    <TableCell width="12%" height={placeholderProps.height}>
                        {props.table.actions.map((actionItem, index) => {
                            if (actionItem.isDragDropOrderEnabled && windowWidth >= CatalogConstants.windowSizeDisableDragDrop()) return (
                                <Tooltip title={props.dragLabel} aria-label={props.dragLabel} key={index}>
                                    <Fab onClick={() => setIsDragableDisable(!isDragableDisable)} size="small" color="secondary">
                                        <DragIndicator />
                                    </Fab>
                                </Tooltip>
                            )
                            else if (actionItem.isEdit) return (
                                <Tooltip title={props.editLabel} aria-label={props.editLabel} key={index}>
                                    <Fab href={props.editUrl + "/" + item.id + (searchTerm ? `/?searchTerm=${searchTerm}` : "")} size="small" color="secondary">
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
                            else if (actionItem.qrCode) return (
                                <Tooltip title={props.generateQRCodeLabel} aria-label={props.generateQRCodeLabel} key={index}>
                                    <Fab onClick={() => handleQRCodeDialog(item)} size="small" color="secondary">
                                        <QrCode2 />
                                    </Fab>
                                </Tooltip>)
                            else return (
                                <div></div>)
                        })}
                    </TableCell>
                }
                {props.table.properties && props.table.properties.map((property, index) => {
                    if (property.isPicture) {
                        return (
                            <TableCell key={index}>
                                <div className="property-image">
                                    {item[property.title] ? (
                                        <img src={item[property.title]} />
                                    ) : (
                                        <div className="is-flex is-justify-content-center">
                                            <TextSnippet className="is-size-2" color="primary" />
                                        </div>
                                    )}
                                </div>
                            </TableCell>
                        )
                    }
                    else if (property.isDateTime) {
                        return (
                            <NoSsr key={index}>
                                <TableCell>{moment.utc(item[property.title]).local().format("L LT")}</TableCell>
                            </NoSsr>
                        )
                    }
                    else if (property.isActivityTag) {
                        const isDisabled = item[property.title] === true ? true : false;

                        return (
                            <TableCell><Chip label={isDisabled ? props.inActiveLabel : props.activeLabel} color={isDisabled ? "default" : "success"}/></TableCell>
                        )
                    }
                    else {
                        return (
                            <TableCell key={index}>{item[property.title] !== null ? item[property.title] : "-"}</TableCell>
                        )
                    }
                })}
            </TableRow>
        )
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
                                    <DragDropContext
                                        onDragEnd={(result) => onDragEnd(result)}
                                        onDragStart={(result) => onDragStart(result)}
                                    >
                                        <Droppable
                                            droppableId="categories"
                                            mode="virtual"
                                            renderClone={(provided) => (
                                                tableRow(provided, draggingItem)
                                            )}
                                        >
                                            {(providedDroppable) => (
                                                <TableBody
                                                    ref={providedDroppable.innerRef}
                                                    {...providedDroppable.droppableProps}
                                                >
                                                    {items.map((item, index) => (
                                                        !isDragableDisable && draggingItem.id && placeholderProps.position == index ?
                                                            <TableRow height={placeholderProps.height} /> :
                                                            <Draggable
                                                                key={item.id}
                                                                draggableId={item.id}
                                                                index={index}
                                                                isDragDisabled={isDragableDisable}
                                                            >
                                                                {(providedDraggable) => (
                                                                    tableRow(providedDraggable, item)
                                                                )}
                                                            </Draggable>
                                                    ))}
                                                </TableBody>
                                            )}
                                        </Droppable>
                                    </DragDropContext>
                                </Table>
                            </TableContainer>
                        </div>
                        {!isDragableDisable && draggingItem.id &&
                            <div className="is-flex is-justify-content-center">
                                <div
                                    className={`catalog__change-page-area ${page == 0 ? "catalog__change-page-area__disabled" : ""}`}
                                    onMouseEnter={() => handleChangedPageWhenDragItem(page - 1)}
                                >
                                    {props.prevPageAreaText}
                                </div>
                                <div
                                    className={`catalog__change-page-area ${page == (props.pagedItems.pageCount - 1) ? "catalog__change-page-area__disabled" : ""}`}
                                    onMouseEnter={() => handleChangedPageWhenDragItem(page + 1)}
                                >
                                    {props.nextPageAreaText}
                                </div>
                            </div>
                        }

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
                            ).join(" ") : ( entityToDelete["name"] ? entityToDelete["name"] : entityToDelete["companyName"] ? entityToDelete["companyName"] : "" ): ""
                    )}
                    noLabel={props.noLabel}
                    yesLabel={props.yesLabel}
                />
                {props.qrCodeDialog &&
                    <QRCodeDialog
                        open={openQRCodeDialog}
                        setOpen={setOpenQRCodeDialog}
                        item={selectedItem}
                        labels={props.qrCodeDialog}
                    />
                }
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
    generateQRCodeLabel: PropTypes.string,
    copyLinkLabel: PropTypes.string
}

export default Catalog;