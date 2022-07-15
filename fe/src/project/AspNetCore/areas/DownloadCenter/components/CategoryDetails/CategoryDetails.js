import React, { useState } from "react";
import {
    NoSsr, FormControlLabel, Checkbox,
} from "@mui/material"
import JsZip from 'jszip';
import PropTypes from "prop-types";
import { saveAs } from 'file-saver';

const CategoryDetails = (props) => {
    const [selectedFiles, setSelectedFiles] = useState([]);

    const handleSelectItem = (file) => {
        const selectedIndex = selectedFiles.indexOf(file);
        let newSelected = [];

        if (selectedIndex === -1) {
            newSelected = newSelected.concat(selectedFiles, file);
        } else if (selectedIndex === 0) {
            newSelected = newSelected.concat(selectedFiles.slice(1));
        } else if (selectedIndex === selectedFiles.length - 1) {
            newSelected = newSelected.concat(selectedFiles.slice(0, -1));
        } else if (selectedIndex > 0) {
            newSelected = newSelected.concat(selectedFiles.slice(0, selectedIndex), selectedFiles.slice(selectedIndex + 1));
        }

        setSelectedFiles(newSelected);
    }

    const handleDownloadFiles = (checkedFiles) => {
        let files = props.files;

        if (checkedFiles && selectedFiles.length > 0){
            files = selectedFiles;
        }

        if (files.length > 0){
            const zip = JsZip();

            for(let i = 0; i < files.length; i++ ){
                let file = files[i]

                const t = fetch(file.url, { mode: "no-cors" })
                    .then(response => response.blob())
                    .then(blob => {
                        return blob;
                    });

                    zip.file(`${file.filename}`, t)
            }

            zip.generateAsync({type: 'blob'}).then(zipFile => {
                const currentDate = new Date().getTime();
                const fileName = `${props.title}-${currentDate}.zip`;

                return saveAs(zipFile, fileName);
            });
        }
    }

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
                            <div className="dc-category__files">
                                <div className="is-flex is-justify-content-space-between is-align-items-center dc-category__files-info">
                                    <h3 className="is-size-6 has-text-weight-bold is-uppercase">Materiały do pobrania</h3>
                                    <div className="dc-category__files-buttons">
                                        <button className="button is-text" type="button" onClick={() => handleDownloadFiles(true)} disabled={selectedFiles.length > 0 ? false : true}>{props.downloadSelectedLabel}</button>
                                        <button className="button is-text" type="button" onClick={() => handleDownloadFiles()}>{props.downloadEverythingLabel}</button>
                                    </div>
                                </div>
                                <div className="dc-category__files-list">
                                    {props.files.length > 0 && props.files.map((file, index) => {
                                        const isFileSelected = selectedFiles.indexOf(file) !== -1;

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
                                                                    onChange={() => handleSelectItem(file)}
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
                    <h1 className="title">{props.noCategoriesLabel}</h1>
                )}
            </div>
        </div>
    )
}

CategoryDetails.propTypes = {
    title: PropTypes.string.isRequired,
    downloadEverythingLabel: PropTypes.string.isRequired,
    downloadSelectedLabel: PropTypes.string.isRequired,
    noCategoriesLabel: PropTypes.string,
    categories: PropTypes.array,
    files: PropTypes.array
}

export default CategoryDetails;