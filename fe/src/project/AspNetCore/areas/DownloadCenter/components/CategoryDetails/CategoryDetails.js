import React from "react";
import PropTypes from "prop-types";
import Files from "../../../../../../shared/components/Files/Files";

const CategoryDetails = (props) => {
    return (
        <div className="section dc-category">
            <div className="container">
                {props.categories ? (
                    <div className="dc-category-container">
                        <h3 className="is-size-5 has-text-weight-bold is-uppercase">{props.title}</h3>
                        <div className="is-flex is-flex-wrap-wrap mt-5">
                            {props.categories.length > 0 && props.categories.map((category, index) => {
                                return (
                                    <a href={category.url} className="dc-category__list-item is-flex is-justify-content-center is-align-items-center m-2" key={index}>
                                        <span className="subtitle is-6">{category.name}</span>
                                    </a>
                                )
                            })}
                        </div>
                        {props.files &&
                            <Files {...props.files} />
                        }
                    </div>
                ) : (
                    <h1 className="title">{props.noCategoriesLabel}</h1>
                )}
            </div>
        </div>
    )
}

CategoryDetails.propTypes = {
    title: PropTypes.string.isRequired,
    noCategoriesLabel: PropTypes.string,
    categories: PropTypes.array,
    files: PropTypes.object
}

export default CategoryDetails;