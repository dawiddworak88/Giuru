import React from "react"
import PropTypes from "prop-types";
import { toast } from "react-toastify";

const ToastSuccessAddProductToBasket = (props) => {

    toast.success(
        <div className="is-flex">
            {props.title &&
                <div>
                    {props.title}
                </div>
            }
            {props.basketUrl &&
                <div className="ml-1 mr-3">
                    <a
                        href={props.basketUrl}
                        aria-label={props.title}
                    >
                        {props.showText}
                    </a>
                </div>
            }
        </div>,
        {
            className: "toast-success"
        }
    )
}

export default ToastSuccessAddProductToBasket

ToastSuccessAddProductToBasket.propTypes = {
    title: PropTypes.string.isRequired,
    moveToBasketText: PropTypes.string.isRequired,
    basketUrl: PropTypes.string.isRequired
}