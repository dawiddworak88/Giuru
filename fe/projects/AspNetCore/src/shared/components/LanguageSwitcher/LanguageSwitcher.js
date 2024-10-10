import React, { useState } from "react";
import PropTypes from "prop-types";
import { Button } from "@mui/material";
import ArrowShowMoreIcon from "../../Icons/ArrowShowMore";
import ArrowShowLessIcon from "../../Icons/ArrowShowLess";
import { Unstable_Popup as BasePopup } from '@mui/base/Unstable_Popup';
import ColorConstants from "../../constants/ColorConstants";

function LanguageSwitcher(props) {
    const [anchor, setAnchor] = useState(null);

    const handleClick = (event) => {
        setAnchor(anchor ? null : event.currentTarget);
        props.toggle(open);
    };

    const open = props.isOpen;

    function handleLanguageChange(href) {

        if (typeof window !== "undefined" && href) {
            window.location.href = href;
        }
    }

    return (
        <div>
            <Button
                className="switcher__button"
                type="button"
                onClick={handleClick}
                disableRipple
                sx={{
                    color: ColorConstants.blackbase(),
                    '&:hover': {
                      backgroundColor: ColorConstants.white(),
                    },
                  }}
            >
                <span className="pr-2 switcher__button__text">
                    {props.selectedLanguageText}
                </span>
                {open ?
                    <ArrowShowLessIcon /> : <ArrowShowMoreIcon />
                }
            </Button>
            <BasePopup open={open} anchor={anchor} className="switcher">
                <div className="switcher__body">
                    {props.availableLanguages && props.availableLanguages.length > 0 && props.availableLanguages.map((language, index) =>
                        <a key={index} href={language.url} className="switcher__body__button" onClick={() => handleLanguageChange(language.url)}>{language.text}</a>
                    )}
                </div>
            </BasePopup>
        </div>
    );
}

LanguageSwitcher.propTypes = {
    availableLanguages: PropTypes.array.isRequired,
    selectedLanguageUrl: PropTypes.string.isRequired
};

export default LanguageSwitcher;