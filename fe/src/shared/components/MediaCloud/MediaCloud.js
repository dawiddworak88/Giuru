import React, { useCallback, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import { Button } from "@material-ui/core";
import { useDropzone } from "react-dropzone";
import IconConstants from "../../constants/IconConstants";

function MediaCloud(props) {

    const { setFieldValue, dispatch, files } = props;

    function deleteMedia(e, id) {

        e.preventDefault();
        setFieldValue({ name: props.stateCollectionName, value: files.filter((item) => item.id !== id) });
    }

    const onDrop = useCallback(acceptedFiles => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        acceptedFiles.forEach((file) => {

            const formData = new FormData();

            formData.append("file", file);

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.saveMediaUrl, requestOptions)
                .then(function (response) {

                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    return response.json().then((jsonResponse) => {

                        if (response.ok) {

                            dispatch({ type: "SET_IS_LOADING", payload: false });
                            setFieldValue({ name: props.stateCollectionName, value: [jsonResponse] });
                        }
                        else {
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch(() => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.generalErrorMessage);
                });
        });
    }, []);

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: props.accept,
        multiple: props.multiple
    });

    return (
        
        <div className="dropzone">
            {props.label &&
                <label className="dropzone__title" for={props.id}>{props.label}</label>
            }
            <div className="dropzone__pond-container" {...getRootProps()}>
                <input id={props.id} name={props.name} {...getInputProps()} />
                <div className={isDragActive ? "dropzone__pond dropzone--active" : "dropzone__pond"}>
                    <p>
                        <UploadCloud size={IconConstants.defaultSize()} />
                    </p>
                    <p>{isDragActive ? props.dropOrSelectFilesLabel : props.dropFilesLabel}</p>
                </div>
            </div>
            {props.previewEnabled && files && files.length > 0 &&
                <aside className="dropzone__preview">
                    {files.map((file) =>
                        <div className="dropzone__preview-thumbnail">
                            <div>
                                <img src={file.url} />
                            </div>
                            <div className="is-flex is-flex-centered has-text-cenetered">
                                <Button type="button" type="contained" color="primary" onClick={(e) => deleteMedia(e, file.id)}>
                                    {props.deleteLabel}
                                </Button>
                            </div>
                        </div>
                    )}
                </aside>
            }
        </div>
    );
}

MediaCloud.propTypes = {
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    label: PropTypes.string,
    accept: PropTypes.string.isRequired,
    multiple: PropTypes.bool.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    previewEnabled: PropTypes.bool.isRequired,
    files: PropTypes.array,
    stateCollectionName: PropTypes.string.isRequired,
    setFieldValue: PropTypes.func.isRequired,
    dispatch: PropTypes.func.isRequired
};

export default MediaCloud;
