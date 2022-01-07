import React, { Fragment } from "react";
import PropTypes from "prop-types";

function Breadcrumbs(props) {

    return (
        <Fragment>
            {props.items &&
                <div className="breadcrumb-container">
                    <nav className="breadcrumb" aria-label="breadcrumbs">
                        <ul>
                            {props.items.map((item, index) =>
                                <li className={item.isActive ? "is-active" : null} key={index}>
                                    <a aria-current={item.isActive ? "page" : null} href={item.url}>{item.name}</a>
                                </li>
                            )}
                        </ul>
                    </nav>
                </div>
            }
        </Fragment>
    );
}

Breadcrumbs.propTypes = {
    items: PropTypes.array.isRequired
};

export default Breadcrumbs;
