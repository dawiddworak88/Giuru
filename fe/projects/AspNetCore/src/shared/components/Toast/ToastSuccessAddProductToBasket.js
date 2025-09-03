import React from "react"
import PropTypes from "prop-types";
import { toast } from "react-toastify";

const ToastSuccessAddProductToBasket = (props) => {

    toast.success(
        <>
            {props.title &&
                <p>{props.title}</p>
            }
            {props.basketUrl &&
                <a
                    className="toast_link"
                    href={props.basketUrl}
                    aria-label={props.title}
                >
                    {props.showText}
                </a>
            }
        </>
    )
}

export default ToastSuccessAddProductToBasket

ToastSuccessAddProductToBasket.propTypes = {
    title: PropTypes.string.isRequired,
    moveToBasketText: PropTypes.string.isRequired,
    basketUrl: PropTypes.string.isRequired
}