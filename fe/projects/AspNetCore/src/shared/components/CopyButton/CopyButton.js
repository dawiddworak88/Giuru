import React, { useState } from "react";
import PropTypes from "prop-types";
import { ContentCopy } from "@mui/icons-material";
import { IconButton, Tooltip } from "@mui/material";
import { toast } from "react-toastify";
import CopyConstants from "../../constants/CopyConstants";

const CopyButton = ({
    copiedText,
    copyTextError,
    copyToClipboardText,
    text,
    label
}) => {
    const [copied, setCopied] = useState(false);

    const handleOnCopy = async () => {
        try {
            if (navigator.clipboard?.writeText && window.isSecureContext) {
                await navigator.clipboard.writeText(text)
                setCopied(true)
                setTimeout(() => setCopied(false), CopyConstants.copyFeedbackDuration())
                return
            }
        } catch (err) {
            toast.error(copyTextError)
        }
        fallbackCopyTextToClipboard()
    };

    const fallbackCopyTextToClipboard = () => {
        const ta = document.createElement('textarea')
        ta.value = text
        ta.setAttribute('readonly', '')
        ta.style.position = 'absolute'
        ta.style.left = '-9999px'
        document.body.appendChild(ta)
        ta.select()
        try {
            const successful = document.execCommand('copy')
            if (successful)
            {
                setCopied(true)
                setTimeout(() => setCopied(false), CopyConstants.copyFeedbackDuration())
            } else {
                toast.error(copyTextError)
            }
        } catch (err) {
            toast.error(copyTextError)
        } finally {
            document.body.removeChild(ta)
        }
    };

    return (
        <Tooltip title={copied ? copiedText : copyToClipboardText}>
            <IconButton
                size="small"
                disableRipple
                className="copy-button"
                onClick={handleOnCopy}
                aria-label={`${copyToClipboardText}: ${label}`}
            >
                <ContentCopy fontSize="small" />
            </IconButton>
        </Tooltip>
    )
}

CopyButton.propTypes = {
    copiedText: PropTypes.string.isRequired,
    copyTextError: PropTypes.string.isRequired,
    copyToClipboardText: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    label: PropTypes.string.isRequired
}

export default CopyButton