import React, { useEffect } from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";

function MenuTiles(props) {
    if (typeof window !== "undefined") {
        const tiles = document.querySelector(".menu-tiles__wrapper");
        let isDown = false;
        let scrollStartX;
        let scrollLeft;

        const dragStart = (e) => {
            isDown = true;
            scrollStartX = e.pageX || e.touches[0].pageX - tiles.offsetLeft;
            scrollLeft = tiles.scrollLeft;	

            tiles.classList.add('active');
        }

        const dragEnd = (e) => {
            isDown = false;

            tiles.classList.remove('active');
        }

        const scrolling = (e) => {
            if(!isDown) return;
            e.preventDefault();

            const x = e.pageX || e.touches[0].pageX - tiles.offsetLeft;
            const dist = (x - scrollStartX);
            tiles.scrollLeft = scrollLeft - dist;
        }

        useEffect(() => {
            tiles.addEventListener('mousedown', dragStart);
            tiles.addEventListener('touchstart', dragStart);
            tiles.addEventListener('mousemove', scrolling);
            tiles.addEventListener('touchmove', scrolling);
            tiles.addEventListener('mouseleave', dragEnd);
	        tiles.addEventListener('mouseup', dragEnd);
	        tiles.addEventListener('touchend', dragEnd);
        }, [])
    }

    return (
        <nav className="section menu-tiles is-hidden-touch">
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