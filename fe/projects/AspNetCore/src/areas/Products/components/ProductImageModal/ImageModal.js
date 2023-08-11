import React from "react";
import { Button, Dialog, DialogTitle } from "@mui/material";
import { Close } from "@mui/icons-material";
import { Splide, SplideSlide } from "@splidejs/react-splide";
import LazyLoad from "react-lazyload";
import LazyLoadConstants from "../../../../shared/constants/LazyLoadConstants";
import ResponsiveImage from "../../../../shared/components/Picture/ResponsiveImage";

const ImageModal = (props) => {
    const { isOpen, handleClose, images } = props;  

    return (
        <Dialog
            open={isOpen}
            onClose={handleClose}
            maxWidth='md'
            PaperProps={{
                className: "image-modal"            
            }}
        >
            <div className="is-flex is-justify-content-end">
                <Button variant="text" color="inherit" onClick={() => handleClose()} className="image-modal__close-button"><Close /></Button>
            </div>
            <div className="image-modal__container">
                {images && images.length > 0 &&
                    <Splide
                        className="image-modal__splide"
                        options={{
                            type: "slide",
                            pagination: false,                              
                            keyboard: 'global'                                             
                        }}
                    >
                        {images.map((image, index) => {
                            return (
                                <SplideSlide>
                                    <LazyLoad offset={LazyLoadConstants.defaultOffset()} className="image-modal__image" key={index}>
                                        <ResponsiveImage sources={image.sources} imageSrc={image.src} imageAlt={image.alt} />
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

export default ImageModal;