import React from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";
import { Swiper, SwiperSlide } from "swiper/react";
import SwiperCore, { Navigation } from "swiper";
import MenuTilesConstants from "./MenuTilesConstants";

function MenuTiles(props) {
    
    SwiperCore.use([Navigation]);

    return (
        <nav className="section menu-tiles is-hidden-touch">
            <Swiper
                slidesPerView={MenuTilesConstants.tilesCountPerSlide()}
                navigation={{
                    prevEl: ".menu-tiles__prev",
                    nextEl: ".menu-tiles__next"
                }}
            >
                {props.tiles.map((tile, index) => {
                    return(
                        <SwiperSlide  key={index}>
                            <Tile icon={tile.icon} title={tile.title} url={tile.url} />
                        </SwiperSlide>
                    );
                })}
            </Swiper>
            <div className="swiper-button-prev menu-tiles__prev"></div>
            <div className="swiper-button-next menu-tiles__next"></div>
        </nav>
    );
}

MenuTiles.propTypes = {
    tiles: PropTypes.array
};

export default MenuTiles;