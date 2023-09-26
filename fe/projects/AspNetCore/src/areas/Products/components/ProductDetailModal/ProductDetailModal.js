import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import { Button, Dialog } from "@mui/material";
import { Close } from "@mui/icons-material";
import { Splide, SplideSlide } from "@splidejs/react-splide";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";

const ProductDetailModal = (props) => {
    const { isOpen, handleClose, mediaItems, index } = props;
    const [splideContext, setSplideContext] = useState(null);     
        
    useEffect(() => {
        if(splideContext) {                        
            splideContext.go(index)                  
        }        
    })

    return (
        <Dialog
            open={isOpen}
            onClose={handleClose}
            maxWidth='md'
            PaperProps={{
                className: "image-modal"            
            }}
        >
            <div className="is-flex is-justify-content-end p-2">
                <Button 
                    variant="text" 
                    color="inherit" 
                    onClick={() => handleClose()} 
                    className="image-modal__close-button">
                        <Close />
                </Button>
            </div>
            <div className="image-modal__container">
                {mediaItems && mediaItems.length > 0 &&
                    <Splide                                     
                        onVisible={(context) => setSplideContext(context)}                        
                        options={{
                            type: 'fade',
                            pagination: false,                              
                            keyboard: 'global',                                                                                           
                        }}
                    >
                        {mediaItems.map((mediaItem, index) => {
                            return (
                                <SplideSlide>                                    
                                    <LazyLoad offset={LazyLoadConstants.defaultOffset()} className="image-modal__image" key={index}>
                                        {mediaItem.mimeType.startsWith("image") ? (
                                            <ResponsiveImage sources={mediaItem.sources} mediaItemsrc={mediaItem.mediaSrc} imageAlt={mediaItem.mediaAlt} imageTitle={props.title} />
                                        ) : (
                                            <video autoPlay loop muted playsInline preload='auto'>
                                                <source src={mediaItem.mediaSrc} type="video/mp4" />
                                            </video>
                                        )}
                                    </LazyLoad>                                                                      
                                </SplideSlide>
                            )
                        })
                        }
                    </Splide>
                }
            </div>
        </Dialog>
    )
}

ProductDetailModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    handleClose: PropTypes.func.isRequired,
    mediaItems: PropTypes.array.isRequired,
    title: PropTypes.string.isRequired,
    index: PropTypes.number.isRequired
}

export default ProductDetailModal;