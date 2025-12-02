import React, { useCallback, useEffect, useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import { Button } from "@mui/material";
import { useDropzone } from "react-dropzone";
import { Context } from "../../../shared/stores/Store";
import IconConstants from "../../../shared/constants/IconConstants";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import AuthenticationHelper from "../../../shared/helpers/globals/AuthenticationHelper";
import { v4 as uuidv4 } from "uuid";

function MediaCloud(props) {

    const [, dispatch] = useContext(Context);
    const { setFieldValue, files, mediaId } = props;

    const [fileToUploadInChunksIndex, setFileToUploadinChunksIndex] = useState(0);
    const [fileUploadProgress, setFileUploadProgress] = useState(0);
    const [filesToUploadInChunks, setFilesToUploadInChunks] = useState([]);
    const [fileToUploadInChunks, setFileToUploadInChunks] = useState(null);
    const [beginingOfTheChunk, setBeginingOfTheChunk] = useState(0);
    const [endOfTheChunk, setEndOfTheChunk] = useState(props.chunkSize);
    const [fileToUploadInChunksFilename, setFileToUploadInChunksFilename] = useState("");
    const [chunkCounter, setChunkCounter] = useState(0);
    const [chunkCount, setChunkCount] = useState(0);
    const [uploadId, setUploadId] = useState(null);

    function deleteMedia(e, id) {

        e.preventDefault();

        setFieldValue({ name: props.name, value: files.filter((item) => item.id !== id) });
    }

    const onDrop = useCallback(acceptedFiles => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        if (props.videoSizeLimit) {
            const oversizedVideoFile = acceptedFiles.find(file => file.size > props.videoSizeLimit && file.type.startsWith("video"));

            if (oversizedVideoFile){
                toast.error(props.fileSizeLimitErrorMessage);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                return;
            }
        }

        if (props.multiple) {
            if (props.isUploadInChunksEnabled) {
                setFilesToUploadInChunks(acceptedFiles);
            }
            else {
                const formData = new FormData();

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

                        AuthenticationHelper.HandleResponse(response);

                        return response.json().then((media) => {

                            if (response.ok) {

                                dispatch({ type: "SET_IS_LOADING", payload: false });
                                setFieldValue({ name: props.name, value: [...files, ...media] });
                            }
                            else {
                                toast.error(props.generalErrorMessage);
                            }
                        });
                    }).catch(() => {
                        dispatch({ type: "SET_IS_LOADING", payload: false });
                        toast.error(props.generalErrorMessage);
                    });
            }
        }
        else {
            if (props.isUploadInChunksEnabled) {
                setFilesToUploadInChunks(acceptedFiles);
            }
            else {
                const formData = new FormData();

                if (mediaId) {
                    formData.append("id", mediaId)
                }

                acceptedFiles.forEach((file) => {
                    formData.append("file", file);

                    const requestOptions = {
                        method: "POST",
                        body: formData
                    };

                    fetch(props.saveMediaUrl, requestOptions)
                        .then(function (response) {
                            dispatch({ type: "SET_IS_LOADING", payload: false });

                            AuthenticationHelper.HandleResponse(response);

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
            }
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

    useEffect(() => {
        if (filesToUploadInChunks.length > 0) {
            filesInChunksUpload();
        }
      }, [filesToUploadInChunks, fileToUploadInChunks, fileUploadProgress]);

    const filesInChunksUpload = () => {
        if (fileToUploadInChunks) {        
            if (chunkCounter < chunkCount) {
                const chunk = fileToUploadInChunks.slice(beginingOfTheChunk, endOfTheChunk);

                uploadChunk(chunk);
            }
        }
        else {
            const file = filesToUploadInChunks[fileToUploadInChunksIndex];

            const newUploadId = uuidv4();

            setUploadId(newUploadId);

            setFileToUploadInChunksFilename(file.name);

            const countChunks = file.size % props.chunkSize === 0
            ? file.size / props.chunkSize
            : Math.floor(file.size / props.chunkSize) + 1;

            setChunkCount(countChunks);
            setChunkCounter(0);
            setBeginingOfTheChunk(0);
            setEndOfTheChunk(props.chunkSize);
            setFileToUploadInChunks(file);
        }
    };

    const uploadChunk = (chunk) => {
        var formData = new FormData();
        formData.append("chunk", chunk);
        formData.append("chunkNumber", chunkCounter);
        formData.append("filename", fileToUploadInChunksFilename);
        formData.append("uploadId", uploadId);

        const requestOptions = {
            method: "POST",
            body: formData
        };

        fetch(props.saveMediaChunkUrl, requestOptions)
            .then(function (response) {

                AuthenticationHelper.HandleResponse(response);

                if (response.ok) {
                    setBeginingOfTheChunk(endOfTheChunk);
                    setEndOfTheChunk(endOfTheChunk + props.chunkSize);

                    if (chunkCounter + 1 === chunkCount) {
                        uploadInChunksComplete();
                    }
                    else {
                        setChunkCounter(prev => prev + 1);
                        setFileUploadProgress(((chunkCounter + 1) / chunkCount) * 100);
                    } 
                }
                else {
                    toast.error(props.generalErrorMessage);
                }
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

    const uploadInChunksComplete = () => {
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify({
                id: mediaId,
                filename: fileToUploadInChunksFilename,
                uploadId: uploadId
            })
        };

        fetch(props.saveMediaChunkCompleteUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(media => {
                    if (response.ok) {
                        if (props.multiple) {
                            setFieldValue({ name: props.name, value: [...files, media] });
                        }
                        else {
                            setFieldValue({ name: props.name, value: [media] });
                        }

                        if ((fileToUploadInChunksIndex + 1) < filesToUploadInChunks.length) {
                            setFileToUploadinChunksIndex(fileToUploadInChunksIndex + 1);
                            setFileToUploadInChunks(filesToUploadInChunks[fileToUploadInChunksIndex + 1]);
                            setFileToUploadInChunksFilename(filesToUploadInChunks[fileToUploadInChunksIndex + 1].name);
                            setBeginingOfTheChunk(0);
                            setEndOfTheChunk(props.chunkSize);
                            setChunkCounter(0);
                            setChunkCount(0);
                            setFileUploadProgress(0);
                            setUploadId(null);
                        }
                        else {
                            setFilesToUploadInChunks([]);
                            setFileToUploadinChunksIndex(0);
                            setFileToUploadInChunks(null);
                            setFileToUploadInChunksFilename("");
                            setBeginingOfTheChunk(0);
                            setEndOfTheChunk(props.chunkSize);
                            setChunkCounter(0);
                            setChunkCount(0);
                            setFileUploadProgress(0);
                            setUploadId(null);
                        }
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
            });
    };

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
    };

    return (
        <div className="dropzone">
            {props.label &&
                <label className="dropzone__title">{props.label}</label>
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
                                            <div className="dropzone__preview-thumbnail m-2"
                                                ref={provided.innerRef}
                                                {...provided.draggableProps}
                                                {...provided.dragHandleProps}
                                                style={getItemStyle(
                                                    snapshot.isDragging,
                                                    provided.draggableProps.style
                                                )}>
                                                <div>
                                                    {file.mimeType.startsWith("image") ?
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
    accept: PropTypes.object.isRequired,
    multiple: PropTypes.bool.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    saveMediaChunkUrl: PropTypes.string,
    saveMediaChunkCompleteUrl: PropTypes.string,
    deleteLabel: PropTypes.string.isRequired,
    setFieldValue: PropTypes.func.isRequired,
    isUploadInChunksEnabled: PropTypes.bool,
    chunkSize: PropTypes.number,
    files: PropTypes.array,
    videoSizeLimit: PropTypes.number,
    fileSizeLimitErrorMessage: PropTypes.string
};

export default MediaCloud;
