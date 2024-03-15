import { Button } from "@mui/material";
import React, {useState} from "react";
import UserIcon from "../../Icons/User";
import ArrowShowMoreIcon from "../../Icons/ArrowShowMore";
import ArrowShowLessIcon from "../../Icons/ArrowShowLess";
import { Unstable_Popup as BasePopup } from '@mui/base/Unstable_Popup';

const UserPopup = (props) => {
    const [anchor, setAnchor] = useState(null);

    const handleClick = (event) => {
        setAnchor(anchor ? null : event.currentTarget);
    };

    const open = Boolean(anchor);
    const id = open ? 'simple-popup' : undefined;

    return (
        <div>
            <Button
                className="popup__button"
                aria-describedby={id}
                type="button"
                onClick={handleClick}
                disableRipple
            >
                <span className="pr-2">
                    <UserIcon />
                </span>
                {open ?
                    <ArrowShowLessIcon /> : <ArrowShowMoreIcon />
                }
            </Button>
            <BasePopup id={id} open={open} anchor={anchor}>
                <div className="popup__body">
                    {props.isLoggedIn ? (
                        props.signOutLink &&
                        <div>
                            <div className="popup__body__welcome-text">{props.welcomeText}, {props.name}!</div>
                            {props.actions && props.actions.length > 0 && props.actions.map((action, index) => 
                                <a key={index} href={action.url} className="popup__body__button">{action.text}</a>
                            )}
                            <a href={props.signOutLink.url} className="popup__body__button sign-in-out-button">{props.signOutLink.text}</a>
                        </div>
                    ) : (
                        props.signInLink &&
                        <div>
                            <a className="popup__body__button" href={props.signInLink.url}>
                                {props.signInLink.text}
                            </a>
                        </div>
                    )}
                </div>
            </BasePopup>
        </div>
    );
}

export default UserPopup;