import React from "react";
import PropTypes from "prop-types";
import { Plus } from "react-feather";

const Ordering = (props) => {
    return (
        <div>
            <a href={props.newUrl} className="button is-primary">
                <span className="icon">
                    <Plus />
                </span>
                <span>
                    {props.newText}
                </span>
            </a>
        </div>
    );
};

Ordering.propTypes = {
    newText: PropTypes.string.isRequired,
    newUrl: PropTypes.string.isRequired
}

export default Ordering;