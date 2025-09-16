import React from 'react';
import PropTypes from 'prop-types';
import { 
    Dialog, 
    DialogContent, 
    DialogTitle, 
    IconButton, 
    List, 
    ListItem, 
    Typography
} from '@mui/material';
import { CloseIcon } from '../../icons';

const PriceModal = ({
    open,
    onClose,
    title,
    note,
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
                    maxWidth: "46.875rem",
                    width: "100%",
                    padding: "2.5rem",
                    borderRadius: "0.5rem",
                    '& .MuiDialogTitle-root + .MuiDialogContent-root': {
                        paddingTop: "1.5rem"
                    }
                    
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
                {title}
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
                    padding: 0
                }}
            >
                {priceInclusions && (
                    <List
                        dense
                        disablePadding
                        sx={{
                            listStyle: "inside",
                            listStyleType: "disc",
                            "> li": {
                                display: "list-item",
                                fontSize: "1rem",
                                fontWeight: "400",
                                lineHeight: "1.5rem",
                                padding: 0
                            }
                        }}
                    >
                        {priceInclusions.map((inclusion) => {
                            return (
                                <ListItem>{inclusion.text} {inclusion.underlinedText && <span style={{textDecoration: "underline"}}>{inclusion.underlinedText}*</span>}.</ListItem>
                            )
                        })}
                    </List>
                )}
                {note && 
                    <Typography 
                        className='mt-5'
                        sx={{
                            fontSize: "0.875rem"
                        }}
                    >
                        {note}
                    </Typography>
                }
            </DialogContent>
        </Dialog>
    )
}
PriceModal.propTypes = {
    open: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    
};

export default PriceModal;