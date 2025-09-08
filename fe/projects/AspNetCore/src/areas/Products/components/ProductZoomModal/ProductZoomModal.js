import React, { useState, useRef } from "react"
import { Dialog } from "@mui/material"
import ZoomModalConstants from "../../../../shared/constants/ZoomModalConstants"

const ProductZoomModal = (props) => {
    const [scale, setScale] = useState(ZoomModalConstants.defaultScale())
    const [position, setPosition] = useState(ZoomModalConstants.defaultPosition())
    const [isZoomed, setIsZoomed] = useState(false)

    const { isOpen, handleClose, mediaItems, index } = props
    const { mediaSrc, mediaAlt } = mediaItems[index]

    const imageRef = useRef(null)
    const lastTapRef = useRef(0)
    const lastPositionref = useRef(ZoomModalConstants.defaultPosition())

    const resetZoom = () => {
        setScale(ZoomModalConstants.defaultScale())
        setPosition(ZoomModalConstants.defaultPosition())
    };

    const constrainPosition = (newPosition) => {
        if (!imageRef.current) return newPosition

        const image = imageRef.current.getBoundingClientRect()

        const maxX = Math.max(0, image.width / 10)
        const maxY = Math.max(0, image.height / 10)

        return {
            x: Math.max(-maxX, Math.min(maxX, newPosition.x)),
            y: Math.max(-maxY, Math.min(maxY, newPosition.y))
        }
    };

    const handleTouchStart = (e) => {
        e.preventDefault()
        const touch = e.touches[0]
        lastPositionref.current = { x: touch.clientX, y: touch.clientY }

        const now = Date.now()
        if (now - lastTapRef.current < ZoomModalConstants.timeToSecondTap()) {
            handleDoubleTap(touch)
        }

        lastTapRef.current = now
    };

    const handleTouchMove = (e) => {
        e.preventDefault()
        if (isZoomed) {
            const touch = e.touches[0]

            const deltaX = touch.clientX - lastPositionref.current.x
            const deltaY = touch.clientY - lastPositionref.current.y

            const newPosition = {
                x: position.x + deltaX,
                y: position.y + deltaY
            }

            setPosition(constrainPosition(newPosition))
            lastPositionref.current = { x: touch.clientX, y: touch.clientY }
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
                    transform: `scale(${scale}) translate(${position.x}px, ${position.y}px)`,
                    transformOrigin: 'center center'
                }}
                onTouchStart={handleTouchStart}
                onTouchMove={handleTouchMove}
                draggable={false}
            />
        </Dialog>
    )
}

export default ProductZoomModal