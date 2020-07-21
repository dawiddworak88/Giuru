import React from 'react';
import PropTypes from 'prop-types';

function Catalog(props) {

    return (

        <section className="section">
            <h1 className="title is-3">{props.title}</h1>
            {props.resultsCount &&
                <p className="subtitle is-6">{props.resultsCount} {props.resultsLabel}</p>
            }
            {props.items ?
            (
                <div className="columns is-tablet is-multiline">
                    {props.items.map((item, index) => 
                        <div className="column is-3">
                            <div className="card">
                                <a key={item.id} href={item.url}>
                                    <div className="card-image">
                                        <figure className="image is-4by3">
                                            <img src={item.imageUrl} alt={item.imageAlt} />
                                        </figure>
                                    </div>
                                    <div className="media-content">
                                        <p className="content-grid-card__title title is-5 has-text-centered">{item.title}</p>
                                    </div>
                                </a>
                            </div>
                        </div>
                    )}
                </div>
            ) :
            (
                <section className="section is-flex-centered">
                    <span className="is-title is-5">{props.noResultsLabel}</span>
                </section>
            )}
            
        </section>
    );
}

Catalog.propTypes = {
    title: PropTypes.string.isRequired,
    noResultsLabel: PropTypes.string.isRequired,
    resultsCount: PropTypes.number.isRequired,
    resultsLabel: PropTypes.string.isRequired,
    items: PropTypes.array
}

export default Catalog;