import React from "react";
import {
    NoSsr, FormControlLabel, Checkbox
} from "@mui/material"

const CategoryDetails = (props) => {
    return (
        <div className="section download-catalog">
            <div className="container">
                {props.categories ? (
                    <div className="list">
                        <h3 className="is-size-5 has-text-weight-bold is-uppercase">{props.title}</h3>
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
                                    <div className="fileasd">
                                        {file.mimeType.startsWith("image") &&
                                            <a href={file.url}>
                                                <img src={file.url} alt={file.name} />
                                            </a>
                                        }
                                        <div className="flex">
                                            <NoSsr>
                                                <FormControlLabel 
                                                    control={
                                                        <Checkbox 
                                                            checked={false}
                                                            />
                                                    }
                                                    label={file.name}
                                                />
                                            </NoSsr>
                                        </div>
                                    </div>
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