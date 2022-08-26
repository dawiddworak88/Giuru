import React, { useCallback, useEffect, useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { UploadCloud } from "react-feather";
import { Button } from "@mui/material";
import { useDropzone } from "react-dropzone";
import { Context } from "../../../shared/stores/Store";
import IconConstants from "../../constants/IconConstants";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import AuthenticationHelper from "../../helpers/globals/AuthenticationHelper";

function MediaCloud(props) {
    const [, dispatch] = useContext(Context);
    const { setFieldValue, files, mediaId } = props;

    const [fileToUploadInChunksIndex, setFileToUploadinChunksIndex] = useState(0);
    const [chunkCounter, setChunkCounter] = useState(0);
    const [progress, setProgress] = useState(0);
    const [filesToUploadInChunks, setFilesToUploadInChunks] = useState([]);
    const [fileToUploadInChunks, setFileToUploadInChunks] = useState(null);
    const [beginingOfTheChunk, setBeginingOfTheChunk] = useState(0);
    const [endOfTheChunk, setEndOfTheChunk] = useState(props.chunkSize);
    const [fileToUploadInChunksSize, setFileToUploadInChunksSize] = useState(0);
    const [fileToUploadInChunksFilename, setFileToUploadInChunksFilename] = useState("");
    const [chunkCount, setChunkCount] = useState(0);

    function deleteMedia(e, id) {

        e.preventDefault();

        setFieldValue({ name: props.name, value: files.filter((item) => item.id !== id) });
    }

    const onDrop = useCallback(acceptedFiles => {
        
        dispatch({ type: "SET_IS_LOADING", payload: true });

        if (props.multiple) {

            if (props.isUploadInChunksEnabled) {
                console.log("props.isUploadInChunksEnabled");
                console.log(acceptedFiles);
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

                if (props.mediaId) {
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
            console.log("filesToUploadInChunks filesToUploadInChunks.length > 0")
            filesInChunksUpload();
        }
      }, [filesToUploadInChunks, fileToUploadInChunks, progress]);

    const filesInChunksUpload = () => {
        console.log("filesToUploadInChunks");
        console.log(filesToUploadInChunks);

        console.log("filesInChunksUpload");
        console.log(fileToUploadInChunks);

        if (fileToUploadInChunks) {
            
            setChunkCounter(chunkCounter + 1);
            if (chunkCounter <= chunkCount) {
                console.log("filesInChunksUpload chunkCounter <= chunkCount");
                console.log(chunkCounter);
                uploadChunk(fileToUploadInChunks.slice(beginingOfTheChunk, endOfTheChunk));
            }
        }
        else {
            console.log("filesInChunksUpload else");
            console.log(filesToUploadInChunks[fileToUploadInChunksIndex].name);
            
            setFileToUploadInChunksSize(filesToUploadInChunks[fileToUploadInChunksIndex].size);
            setFileToUploadInChunksFilename(filesToUploadInChunks[fileToUploadInChunksIndex].name);
            setChunkCount(filesToUploadInChunks[fileToUploadInChunksIndex].size % props.chunkSize == 0 ? filesToUploadInChunks[fileToUploadInChunksIndex].size / props.chunkSize : Math.floor(filesToUploadInChunks[fileToUploadInChunksIndex].size / props.chunkSize) + 1);
            setFileToUploadInChunks(filesToUploadInChunks[fileToUploadInChunksIndex]);
        }
    };

    const uploadChunk = (chunk) => {

        var formData = new FormData();

        console.log(chunk);
        console.log(chunkCounter);
        console.log(fileToUploadInChunksFilename);

        formData.append("chunk", chunk);
        formData.append("chunkNumber", chunkCounter);
        formData.append("filename", fileToUploadInChunksFilename);
        
        console.log("upload chunk");
        console.log(fileToUploadInChunksFilename);
        console.log(chunkCounter);

        const requestOptions = {
            method: "POST",
            body: formData
        };

        fetch(props.saveMediaUrl, requestOptions)
            .then(function (response) {
                if (response.ok) {
                    console.log("upload chunk response.ok")

                    setBeginingOfTheChunk(endOfTheChunk);
                    setEndOfTheChunk(endOfTheChunk + props.chunkSize);

                    console.log(chunkCount);
                    console.log(chunkCounter);

                    if (chunkCounter == chunkCount) {
                        uploadInChunksComplete();
                    }
                    else {
                        console.log("setProgress")
                        setProgress(((chunkCounter + 1) / chunkCount) * 100);
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

        console.log("uploadInChunksComplete");
        console.log(fileToUploadInChunks.name);

        if (fileToUploadInChunksIndex < filesToUploadInChunks.length) {
            setFileToUploadinChunksIndex(fileToUploadInChunksIndex + 1);
            setFileToUploadInChunks(filesToUploadInChunks[fileToUploadInChunksIndex + 1]);
            setFileToUploadInChunksSize(filesToUploadInChunks[fileToUploadInChunksIndex + 1].size);
            setFileToUploadInChunksFilename(filesToUploadInChunks[fileToUploadInChunksIndex + 1].name);
            setProgress(0);
        }
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
    accept: PropTypes.string.isRequired,
    multiple: PropTypes.bool.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    setFieldValue: PropTypes.func.isRequired,
    isUploadInChunksEnabled: PropTypes.bool,
    chunkSize: PropTypes.number,
    files: PropTypes.array
};

export default MediaCloud;
