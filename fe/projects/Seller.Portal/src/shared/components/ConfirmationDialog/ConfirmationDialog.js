
import React, { useContext } from "react";
import PropTypes from "prop-types";
import {
    Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle
} from "@mui/material";
import { Context } from "../../stores/Store";

function ConfirmationDialog(props) {

    const [state,] = useContext(Context);

    return (
        <Dialog
            open={props.open}
            onClose={props.handleClose}
            aria-labelledby={props.titleId}
            aria-describedby={props.textId}>
            <DialogTitle id={props.titleId}>{props.title}</DialogTitle>
            <DialogContent>
                <DialogContentText id={props.textId}>
                    {props.text}
            </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={props.handleClose} color="primary">
                    {props.noLabel}
                </Button>
                <Button disabled={state.isLoading} onClick={props.handleConfirm} color="primary" autoFocus>
                    {props.yesLabel}
                </Button>
            </DialogActions>
        </Dialog>
    );
}

ConfirmationDialog.propTypes = {
    open: PropTypes.bool.isRequired,
    handleClose: PropTypes.func.isRequired,
    titleId: PropTypes.string.isRequired,
    textId: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    noLabel: PropTypes.string.isRequired,
    yesLabel: PropTypes.string.isRequired,
    handleConfirm: PropTypes.func.isRequired
};

export default ConfirmationDialog;
