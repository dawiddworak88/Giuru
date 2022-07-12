import React from "react";

const CategoryDetails = (props) => {
    return (
        <div className="section download-catalog">
            <div className="container">
                {props.categories ? (
                    <div className="list">
                        <h3>{props.title}</h3>
                        <div className="list-box">
                            {props.categories.length > 0 && props.categories.map((category, index) => {
                                return (
                                    <a href={category.url} className="list-box__item" key={index}>
                                        <span className="list-box__title">{category.name}</span>
                                    </a>
                                )
                            })}
                            {props.files && props.files.length > 0 && props.files.map((file, index) => {
                                return (
                                    <img src={file.url} alt={file.name} key={index}/>
                                )
                            })}
                        </div>
                    </div>
                ) : (
                    <div>Brak</div>
                )}
            </div>
        </div>
    )
}

export default CategoryDetails;