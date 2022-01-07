import React from "react";
import PropTypes from "prop-types";
import IconConstants from "../../constants/IconConstants";
import * as Icon from "react-feather";

function Tile(props) {

    const IconTag = Icon[props.icon];

    return (
        <a href={props.url} className="tile">
            <div className="tile__icon">
                <IconTag size={IconConstants.defaultSize()} />
            </div>
            <div className="tile__title">
                {props.title}
            </div>
        </a>
    );
}

Tile.propTypes = {
    title: PropTypes.string.isRequired,
    icon: PropTypes.string.isRequired,
    url: PropTypes.string
};

export default Tile;