import React from 'react';
import PropTypes from 'prop-types';
import { 
    Dialog, 
    DialogContent, 
    DialogTitle, 
    IconButton, 
    List, 
    ListItem 
} from '@mui/material';
import { CloseIcon } from '../../icons';

const PriceModal = ({
    open,
    onClose,
    priceInclusions
}) => {
    return (
        <Dialog
            className='price-modal'
            open={open}
            onClose={onClose}
            aria-label='price-modal'
            PaperProps={{
                sx: {
                    maxWidth: "750px",
                    width: "100%",
                    padding: "40px",
                    borderRadius: "8px"
                }
            }}
        >
           <DialogTitle 
                className='price-modal__title'
                sx={{
                    fontSize: "1.625rem",
                    lineHeight: "130%",
                    fontWeight: 400,
                    p: 0,
                    letterSpacing: 0,
                    boxShadow: "none"
                }}
            >
                Cena zawiera
                <IconButton 
                    disableRipple
                    aria-label="close"
                    onClick={onClose}
                    sx={{
                        position: 'absolute',
                        right: "1rem",
                        top: "1rem"
                    }}
                >
                    <CloseIcon />
                </IconButton>
            </DialogTitle> 
            <DialogContent
                sx={{
                    padding: "1.5rem 0 0 0"
                }}
            >
                {priceInclusions && (
                    <List
                        dense
                        disablePadding
                        sx={{
                            listStyleType: "disc",
                            "> li": {
                                display: "list-item"
                            }
                        }}
                    >
                        {priceInclusions.map((inclusion) => {
                            return (
                                <ListItem>{inclusion}</ListItem>
                            )
                        })}
                    </List>
                )}
                <p className='mt-5'>*Więcej informacji znajduje się w regulaminie dostępnym na stronie b2b.eltap.com</p>
            </DialogContent>
        </Dialog>
    )
}
PriceModal.propTypes = {
    open: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired
};

export default PriceModal;