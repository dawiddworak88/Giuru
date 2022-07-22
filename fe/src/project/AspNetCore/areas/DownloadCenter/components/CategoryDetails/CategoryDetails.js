import React from "react";
import PropTypes from "prop-types";
import DownloadCenterFiles from "../../../../shared/components/DownloadCenterFiles/DownloadCenterFiles";

const CategoryDetails = (props) => {
    return (
        <div className="section dc-category">
            <div className="container">
                <h3 className="title is-3">{props.title}</h3>
                <div className="is-flex is-flex-wrap-wrap">
                    {props.subcategories.length > 0 && props.subcategories.map((subcategory, index) => {
                        return (
                            <a href={subcategory.url} className="dc-category__subcategory-link is-flex is-justify-content-center is-align-items-center m-2" key={index}>
                                <span className="subtitle is-6">{subcategory.name}</span>
                            </a>
                        )
                    })}
                </div>
                {props.files &&
                    <DownloadCenterFiles {...props.files} />
                }
            </div>
        </div>
    )
}

CategoryDetails.propTypes = {
    title: PropTypes.string.isRequired,
    subcategories: PropTypes.array,
    files: PropTypes.object
}

export default CategoryDetails;