import React from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";
import { Swiper, SwiperSlide } from "swiper/react";
import MenuTilesConstants from "./MenuTilesConstants";

function MenuTiles(props) {

    return (
        <nav className="section menu-tiles is-hidden-touch">
            <Swiper
                slidesPerView={MenuTilesConstants.tilesCountPerSlide()}
            >
                {props.tiles.map((tile, index) => {
                    return(
                        <SwiperSlide>
                            <Tile key={index} icon={tile.icon} title={tile.title} url={tile.url} />
                        </SwiperSlide>
                    );
                })}
            </Swiper>
        </nav>
    );
}

MenuTiles.propTypes = {
    tiles: PropTypes.array
};

export default MenuTiles;