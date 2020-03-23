import React from 'react';
import PropTypes from 'prop-types';
import Tile from './Tile';

function MenuTiles(props) {

    return (
        <nav className="section menu-tiles">
            <Tile icon="ShoppingCart" title={props.ordersText} />
            <Tile icon="Package" title={props.productsText} />
            <Tile icon="Users" title={props.clientsText} />
            <Tile icon="Settings" title={props.settingsText} />            
        </nav>
    );
}

MenuTiles.propTypes = {
    ordersText: PropTypes.string.isRequired,
    productsText: PropTypes.string.isRequired,
    clientsText: PropTypes.string.isRequired,
    settingsText: PropTypes.string.isRequired
};

export default MenuTiles;