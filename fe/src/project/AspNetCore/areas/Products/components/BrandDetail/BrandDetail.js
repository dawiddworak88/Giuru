import React from "react";
import PropTypes from "prop-types";
import LazyLoad from "react-lazyload";
import Files from "../../../../shared/components/Files/Files";
import LazyLoadConstants from "../../../../../../shared/constants/LazyLoadConstants";

function BrandDetail(props) {

    return (

        <section className="brand-detail section">
            <div className="columns is-tablet">
                <div className="column is-4 is-flex is-flex-centered">
                    {props.logoUrl &&
                        <LazyLoad offset={LazyLoadConstants.defaultOffset()}>
                            <img src={props.logoUrl} alt={props.name} />
                        </LazyLoad>
                    }
                    
                </div>
                <div className="column is-8 is-flex is-flex-centered">
                    <p className="subtitle is-6">{props.description}</p>
                </div>
            </div>
            <Files {...props.files} />
        </section>
    );
}

BrandDetail.propTypes = {
    logoUrl: PropTypes.string,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    files: PropTypes.object.isRequired
};

export default BrandDetail;
