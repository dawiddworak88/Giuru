import React from "react";
import { Button, Dialog } from "@mui/material";
import { Close } from "@mui/icons-material";
import { useEffect } from 'react';

function Product3dModelModal(props) {
  const { isOpen, handleClose, modelSrc, modelAlt } = props;

  useEffect(() => {
    import('@google/model-viewer');
  }, [])

  return (
    <Dialog
      open={isOpen}
      onClose={handleClose}
      slotProps={{
        paper: { className: "modal-3d-model" }
      }}
    >
      <div className="modal-3d-model__wrapper">
        <model-viewer
          src={modelSrc}
          alt={modelAlt}
          camera-controls
          loading="lazy"
          auto-rotate
          className="modal-3d-model__model-viewer"
        />
      </div>
      <Button
        disableRipple
        onClick={handleClose}
        className="modal-3d-model__close-button"
      >
        <Close />
      </Button>
    </Dialog>
  )
}

export default Product3dModelModal;
