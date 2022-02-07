import React, { useCallback, useEffect, useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import { Button } from "@material-ui/core";
import { useDropzone } from "react-dropzone";
import { Context } from "../../../shared/stores/Store";
import IconConstants from "../../constants/IconConstants";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";

function MediaCloud(props) {

    const [, dispatch] = useContext(Context);
    const { setFieldValue, files, mediaId } = props;

    function deleteMedia(e, id) {

        e.preventDefault();

        setFieldValue({ name: props.name, value: files.filter((item) => item.id !== id) });
    }

    const onDrop = useCallback(acceptedFiles => {
        
        dispatch({ type: "SET_IS_LOADING", payload: true });

        if (props.multiple) {
            const formData = new FormData();
            if (props.mediaId){
                formData.append("id", mediaId)
            }

            acceptedFiles.forEach((file) => {
                formData.append("files", file);
            });

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.saveMediaUrl, requestOptions)
                .then(function (response) {

                    dispatch({ type: "SET_IS_LOADING", payload: false });

                    return response.json().then((media) => {

                        if (response.ok) {

                            dispatch({ type: "SET_IS_LOADING", payload: false });
                            setFieldValue({ name: props.name, value: [...files, ...media] });
                        }
                        else {
                            toast.error(props.generalErrorMessage);
                        }
                    });
                }).catch((error) => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.generalErrorMessage);
                });
        }
        else {
            acceptedFiles.forEach((file) => {
                const formData = new FormData();

                formData.append("file", file);
                if (props.mediaId){
                    formData.append("id", mediaId)
                }

                const requestOptions = {
                    method: "POST",
                    body: formData
                };

                fetch(props.saveMediaUrl, requestOptions)
                    .then(function (response) {

                        dispatch({ type: "SET_IS_LOADING", payload: false });

                        return response.json().then((media) => {

                            if (response.ok) {

                                dispatch({ type: "SET_IS_LOADING", payload: false });
                                setFieldValue({ name: props.name, value: [media] });
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
        } // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [files]); 

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: props.accept,
        multiple: props.multiple
    });

    useEffect(() => () => {
        files.forEach(file => URL.revokeObjectURL(file.url));
    }, [files]);

    const reorder = (list, startIndex, endIndex) => {
        const result = Array.from(list);
        const [removed] = result.splice(startIndex, 1);
        result.splice(endIndex, 0, removed);
        return result;
    };

    const getItemStyle = (isDragging, draggableStyle) => ({
        opacity: isDragging ? 0.5 : 1,
        ...draggableStyle,
    });

    const getListStyle = isDraggingOver => ({
        background: isDraggingOver ? "lightgray" : "white",
        display: "flex",
        overflow: "auto"
    });

    const onDragEnd = (result) => {
        if (!result.destination) {
            return;
        }

        const items = reorder(
            props.files,
            result.source.index,
            result.destination.index
        );

        props.setFieldValue({ name: props.name, value: items });
    }

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
                    <p>{isDragActive ? props.dropFilesLabel : props.dropOrSelectFilesLabel}</p>
                </div>
            </div>
            {files && files.length > 0 &&
                <DragDropContext onDragEnd={onDragEnd}>
                    <Droppable droppableId={props.name} direction="horizontal">
                        {(provided, snapshot) => (
                            <aside className="dropzone__preview"
                                {...provided.droppableProps}
                                ref={provided.innerRef}
                                style={getListStyle(snapshot.isDraggingOver)}>
                                {files.map((file, index) =>
                                    <Draggable key={file.id} draggableId={file.id} index={index}>
                                        {(provided, snapshot) => (
                                            <div className="dropzone__preview-thumbnail"
                                                ref={provided.innerRef}
                                                {...provided.draggableProps}
                                                {...provided.dragHandleProps}
                                                style={getItemStyle(
                                                    snapshot.isDragging,
                                                    provided.draggableProps.style
                                                )}>
                                                <div>
                                                    {file.mimeType === "image/jpeg" || file.mimeType === "image/png" ?
                                                        <img src={file.url} alt={file.name} /> :
                                                        <div className="dropzone__preview-tile">
                                                            <a href={file.url} alt={file.name}>{file.filename}</a>
                                                        </div>
                                                    }
                                                </div>
                                                <div className="is-flex is-flex-centered has-text-cenetered">
                                                    <Button type="button" color="primary" onClick={(e) => deleteMedia(e, file.id)}>
                                                        {props.deleteLabel}
                                                    </Button>
                                                </div>
                                            </div>
                                        )}
                                    </Draggable>
                                )}
                                {provided.placeholder}
                            </aside>)}
                    </Droppable>
                </DragDropContext>
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
    imagePreviewEnabled: PropTypes.bool.isRequired,
    setFieldValue: PropTypes.func.isRequired,
    files: PropTypes.array
};

export default MediaCloud;
