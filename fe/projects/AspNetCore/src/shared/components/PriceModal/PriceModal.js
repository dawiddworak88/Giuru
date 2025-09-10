import React from 'react';
import PropTypes from 'prop-types';
import { Dialog, DialogTitle } from '@mui/material';

const PriceModal = ({
    open,
    onClose
}) => {
    return (
        <Dialog 
            open={open}
            onClose={onClose}
            aria-label='price-modal'
            PaperProps={{
                maxWidth: "750px",
                px: "40px",
                py: "40px"
            }}
        >
           <DialogTitle>
                Cena zawiera
            </DialogTitle> 
        </Dialog>
    )
}
PriceModal.propTypes = {
    open: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired
};

export default PriceModal;