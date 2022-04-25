import React, { useEffect } from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";
import {
    ArrowBackIos, ArrowForwardIos
} from "@material-ui/icons";

function MenuTiles(props) {
    if (typeof window !== "undefined") {
        const tilesWrapper = document.querySelector(".menu-tiles__wrapper");
        const tiles = document.getElementsByClassName("tile");

        const prevButton = document.getElementById("prev");
        const nextButton = document.getElementById("next");

        const toRight = () => {
            const lastTile = tiles[tiles.length - 1];

            tilesWrapper.scrollLeft = lastTile.offsetLeft;
        }

        const toLeft = () => {
            tilesWrapper.scrollLeft = 0;
        }

        useEffect(() => {
            nextButton.addEventListener("click", toRight);
            prevButton.addEventListener("click", toLeft);
        }, []);
    }

    return (
        <nav className="section menu-tiles is-hidden-touch">
            <div className="menu-tiles__prev" id="prev"><ArrowBackIos /></div>
            <div className="menu-tiles__next" id="next"><ArrowForwardIos /></div>
            <div className="menu-tiles__wrapper">
                {props.tiles.map((tile, index) => {
                    return(
                        <Tile key={index} icon={tile.icon} title={tile.title} url={tile.url} />
                    );
                })}
            </div>
        </nav>
    );
}

MenuTiles.propTypes = {
    tiles: PropTypes.array
};

export default MenuTiles;