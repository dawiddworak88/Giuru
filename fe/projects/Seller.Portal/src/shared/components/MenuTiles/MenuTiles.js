import React from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";
import { Splide, SplideSlide } from '@splidejs/react-splide';
import MenuTilesConstants from "../../../../../../shared/constants/MenuTilesConstants";
import '@splidejs/react-splide/css';

function MenuTiles(props) {
    return (
        <nav className="section menu-tiles is-hidden-touch">
            <Splide
                options={{
                    perPage: MenuTilesConstants.defaultTilesSize(),
                    breakpoints: MenuTilesConstants.defaultTilesResponsive(),
                    pagination: false
                }}
            >
                {props.tiles.map((tile, index) => {
                    return(
                        <SplideSlide key={index}>
                            <Tile icon={tile.icon} title={tile.title} url={tile.url} target={tile.target} />
                        </SplideSlide>
                    );
                })}
            </Splide>
        </nav>
    );
}

MenuTiles.propTypes = {
    tiles: PropTypes.array
};

export default MenuTiles;