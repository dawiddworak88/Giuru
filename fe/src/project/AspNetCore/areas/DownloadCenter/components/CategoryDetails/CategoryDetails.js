import React, { useState } from "react";
import {
    NoSsr, FormControlLabel, Checkbox,
} from "@mui/material"

const CategoryDetails = (props) => {
    const [selectedFiles, setSelectedFiles] = useState([]);

    const handleSelectItem = (id) => {
        const selectedIndex = selectedFiles.indexOf(id);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selectedFiles, id);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selectedFiles.slice(1));
        } else if (selectedIndex === selectedFiles.length - 1) {
            newSelected = newSelected.concat(selectedFiles.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selectedFiles.slice(0, selectedIndex), selectedFiles.slice(selectedIndex + 1));
        }

        setSelectedFiles(newSelected);
    }

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
                        </div>
                        {props.files &&
                            <div className="dc-category__files">
                                <div className="is-flex is-justify-content-space-between is-align-items-center dc-category__files-info">
                                    <h3 className="is-size-6 has-text-weight-bold is-uppercase">Materiały do pobrania</h3>
                                    <div className="dc-category__files-buttons">
                                        <button className="button is-text">Pobierz wybrane</button>
                                        <button className="button is-text">Pobierz wszystko</button>
                                    </div>
                                </div>
                                <div className="dc-category__files-list">
                                    {props.files.length > 0 && props.files.map((file, index) => {
                                        const isFileSelected = selectedFiles.indexOf(file.id) !== -1;

                                        return (
                                            <div className="dc-category__file" key={index}>
                                                {file.mimeType.startsWith("image") &&
                                                    <a href={file.url}>
                                                        <img src={file.url} alt={file.name} />
                                                    </a>
                                                }
                                                <div className="dc-category__file-controls">
                                                    <NoSsr>
                                                        <FormControlLabel 
                                                            control={
                                                                <Checkbox 
                                                                    checked={isFileSelected}
                                                                    onChange={() => handleSelectItem(file.id)}
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
                        }
                    </div>
                ) : (
                    <div>Brak</div>
                )}
            </div>
        </div>
    )
}

export default CategoryDetails;