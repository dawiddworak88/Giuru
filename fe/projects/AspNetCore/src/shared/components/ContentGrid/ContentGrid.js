import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import ResponsiveImage from "../../../shared/components/Picture/ResponsiveImage";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";

function ContentGrid(props) {

    return (
        <section className="content-grid section">
            <h1 className="title is-3">{props.title}</h1>
            {props.items && props.items.length > 0 &&
                <div>
                    <div className="columns is-tablet is-multiline">
                        {props.items.map((item, index) =>
                            <div key={item.id} className="column is-4">
                                <div className="content-grid-item card">
                                    <a href={item.url}>
                                        <div className="card-image">
                                            <figure className="image is-4by3">
                                                <LazyLoad offset={LazyLoadConstants.contentGridOffset()}>
                                                    <ResponsiveImage imageAlt={item.imageAlt} imageSrc={item.imageUrl} sources={item.sources} />
                                                </LazyLoad>
                                            </figure>
                                        </div>
                                    </a>
                                    <div className="media-content is-flex-centered">
                                        <h2 className="content-grid-item__title"><a href={item.url}>{item.title}</a></h2>
                                    </div>
                                </div>
                            </div>)}
                    </div>
                </div>}
        </section>
    );
}

ContentGrid.propTypes = {
    title: PropTypes.string.isRequired,
    items: PropTypes.array
};

export default ContentGrid;
