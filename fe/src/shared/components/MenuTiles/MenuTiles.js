import React from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";

function MenuTiles(props) {

    return (
        <nav className="section menu-tiles is-hidden-touch">
            {props.tiles.map((tile, index) => {
                return(
                    <Tile key={index} icon={tile.icon} title={tile.title} url={tile.url} />
                );
            })}
        </nav>
    );
}

MenuTiles.propTypes = {
    tiles: PropTypes.array
};

export default MenuTiles;