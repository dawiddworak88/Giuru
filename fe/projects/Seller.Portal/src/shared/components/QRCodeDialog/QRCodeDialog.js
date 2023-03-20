import React from "react";
import PropTypes from "prop-types";
import { QRCodeCanvas } from "qrcode.react";
import DialogConstants from "../../../../../../shared/constants/DialogConstants";
import {
    Button, Dialog, DialogActions, 
    DialogContent, DialogTitle
} from "@mui/material";

const QRCodeDialog = (props) => {
    const { open, item, setOpen, labels } = props;

    const handleDownloadQRCode = () => {
        const qrCode = document.getElementById("qr-code");
        const qrCodeUrl = qrCode.toDataURL("image/png");
        const downloadLink = document.createElement("a");

        downloadLink.href = qrCodeUrl;
        downloadLink.hidden = true;
        downloadLink.download = `${item.filename}-QRCode.png`;

        document.body.append(downloadLink);
        downloadLink.click();
        document.body.removeChild(downloadLink);

        setOpen(false);
    }

    const handleCloseDialog = () => {
        setOpen(false);
    }

    return (
        <Dialog
            open={open}
            onClose={handleCloseDialog}
        >
            <DialogTitle>{labels.title}</DialogTitle>
            <DialogContent>
                <QRCodeCanvas
                    id="qr-code"
                    value={props.item ? props.item.url : ""}
                    level={"H"}
                    size={DialogConstants.defaultQRCodeSize()}
                    includeMargin={true}/>
            </DialogContent>
            <DialogActions>
                <Button type="button" onClick={handleCloseDialog}>{labels.closeLabel}</Button>
                <Button 
                    type="button" 
                    variant="contained" 
                    color="primary" 
                    onClick={handleDownloadQRCode}>
                        {labels.downloadLabel}
                </Button>
            </DialogActions>
        </Dialog>
    )
}

QRCodeDialog.propTypes = {
    labels: PropTypes.object.isRequired,
    open: PropTypes.bool.isRequired,
    item: PropTypes.object,
    setOpen: PropTypes.func.isRequired
}

export default QRCodeDialog;