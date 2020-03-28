import React from 'react';
import PropTypes from 'prop-types';
import { Plus } from 'react-feather';

function Catalog(props) {
    return (
        <section className="section catalog">
            <h1 className="title is-4">{props.title}</h1>
            <div>
                <a href="/" class="button is-primary">
                    <span className="icon">
                        <Plus />
                    </span>
                    <span>
                        {props.newText}
                    </span>
                </a>
            </div>
        </section>
    );
}

Catalog.propTypes = {
    title: PropTypes.string.isRequired,
    newText: PropTypes.string
};

export default Catalog;