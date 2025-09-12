import React, { useState, useRef } from "react"
import { Dialog } from "@mui/material"
import PropTypes from "prop-types"
import ZoomModalConstants from "../../../../shared/constants/ZoomModalConstants"

const ProductZoomModal = (props) => {
    const [scale, setScale] = useState(ZoomModalConstants.defaultScale())
    const [position, setPosition] = useState(ZoomModalConstants.defaultPosition())
    const [isZoomed, setIsZoomed] = useState(false)

    const imageRef = useRef(null)
    const lastTapRef = useRef(0)
    const lastPositionRef = useRef(ZoomModalConstants.defaultPosition())

    const { isOpen, handleClose, mediaItems, index } = props
    if (!isOpen || !mediaItems || !mediaItems[index]) return null;

    const item = mediaItems[index];
    if (!item.mimeType?.startsWith('image')) return null;
    const { mediaSrc, mediaAlt } = item

    const resetZoom = () => {
        setScale(ZoomModalConstants.defaultScale())
        setPosition(ZoomModalConstants.defaultPosition())
    };

    const constrainPosition = (newPosition) => {
        if (!imageRef.current) return newPosition

        const image = imageRef.current.getBoundingClientRect()

        const maxX = Math.max(0, image.width / 4)
        const maxY = Math.max(0, image.height / 4)

        return {
            x: Math.max(-maxX, Math.min(maxX, newPosition.x)),
            y: Math.max(-maxY, Math.min(maxY, newPosition.y))
        }
    };

    const handleTouchStart = (e) => {
        
        if (e.touches.length > 1) {
            e.preventDefault()
            return
        }

        const touch = e.touches[0]
        lastPositionRef.current = { x: touch.clientX, y: touch.clientY }

        const now = Date.now()
        if (now - lastTapRef.current < ZoomModalConstants.timeToSecondTap()) {
            handleDoubleTap(touch)
        }

        lastTapRef.current = now
    };

    const handleTouchMove = (e) => {

        if (e.touches.length > 1) {
            e.preventDefault()
            return
        }

        if (isZoomed) {
            const touch = e.touches[0]

            const deltaX = touch.clientX - lastPositionRef.current.x
            const deltaY = touch.clientY - lastPositionRef.current.y

            const newPosition = {
                x: position.x + deltaX,
                y: position.y + deltaY
            }

            setPosition(constrainPosition(newPosition))
            lastPositionRef.current = { x: touch.clientX, y: touch.clientY }
        }
    };

    const handleDoubleTap = (touch) => {
        if (scale > 1) {
            resetZoom()
            setIsZoomed(false)
        } else {
            const rect = imageRef.current.getBoundingClientRect()
            const centerX = touch.clientX - rect.left - rect.width / 2
            const centerY = touch.clientY - rect.top - rect.height / 2

            const defaultScale = ZoomModalConstants.defaultZoom()
            setScale(defaultScale)
            setIsZoomed(true)
            setPosition({
                x: -centerX * (defaultScale - 1) / defaultScale,
                y: -centerY * (defaultScale - 1) / defaultScale
            })
        }
    };

    const handleOnClose = () => {
        resetZoom()
        handleClose()
    };

    if (!mediaItems || !mediaItems[index]) return null

    return (
        <Dialog
            open={isOpen}
            onClose={handleOnClose}
        >
            <img
                ref={imageRef}
                src={mediaSrc}
                alt={mediaAlt}
                className="zoom-modal-image"
                style={{
                    transform: `translate(${position.x}px, ${position.y}px) scale(${scale})`,
                    transformOrigin: 'center center'
                }}
                onTouchStart={handleTouchStart}
                onTouchMove={handleTouchMove}
                draggable={false}
            />
        </Dialog>
    )
}

ProductZoomModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    handleClose: PropTypes.func,
    mediaItems: PropTypes.array.isRequired,
    index: PropTypes.number.isRequired
}

export default ProductZoomModal