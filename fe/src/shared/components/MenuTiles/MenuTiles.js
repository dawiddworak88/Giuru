import React, { useEffect } from "react";
import PropTypes from "prop-types";
import Tile from "./Tile";

function MenuTiles(props) {
    if (typeof window !== "undefined") {
        const tiles = document.querySelector(".menu-tiles__wrapper");
        let isDown = false;
        let scrollStartX;
        let scrollLeft;

        const startScrolling = (e) => {
            isDown = true;
            scrollStartX = e.pageX || e.touches[0].pageX - tiles.offsetLeft;
            scrollLeft = tiles.scrollLeft;	
        }

        const endScrolling = (e) => {
            isDown = false;
        }

        const scrolling = (e) => {
            if(!isDown) return;
            e.preventDefault();

            const x = e.pageX || e.touches[0].pageX - tiles.offsetLeft;
            const dist = (x - scrollStartX);
            tiles.scrollLeft = scrollLeft - dist;
        }

        useEffect(() => {
            tiles.addEventListener('mousedown', startScrolling);
            tiles.addEventListener('touchstart', startScrolling);
            tiles.addEventListener('mousemove', scrolling);
            tiles.addEventListener('touchmove', scrolling);
            tiles.addEventListener('mouseleave', endScrolling);
	        tiles.addEventListener('mouseup', endScrolling);
	        tiles.addEventListener('touchend', endScrolling);

            return () => {
                tiles.removeEventListener('mousedown', startScrolling);
                tiles.removeEventListener('touchstart', startScrolling);
                tiles.removeEventListener('mousemove', scrolling);
                tiles.removeEventListener('touchmove', scrolling);
                tiles.removeEventListener('mouseleave', endScrolling);
	            tiles.removeEventListener('mouseup', endScrolling);
	            tiles.removeEventListener('touchend', endScrolling);
            }
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