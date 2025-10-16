import React, { useContext, useState, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import MediaCloud from "../../../../shared/components/MediaCloud/MediaCloud";
import { EditorState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import { 
    TextField, Select, FormControl, FormControlLabel, Switch, NoSsr,
    InputLabel, MenuItem, Button, CircularProgress, FormHelperText
} from "@mui/material";
import { stateToMarkdown } from "draft-js-export-markdown";
import { stateFromMarkdown } from 'draft-js-import-markdown';

const NewsItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [convertedToRaw, setConvertedToRaw] = useState(props.content ? props.content : null);
    const [editorState, setEditorState] = useState(EditorState.createEmpty());
    
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        categoryId: { value: props.categoryId ? props.categoryId : null, error: ""},
        title: { value: props.newsTitle ? props.newsTitle : "", error: "" },
        previewImage: { value: props.previewImages ? props.previewImages : [], error: "" },
        thumbnailImage: { value: props.thumbnailImages ? props.thumbnailImages : [], error: "" },
        description: { value: props.description ? props.description : null, error: "" },
        content: { value: props.content ? props.content : "", error: "" },
        files: { value: props.files ? props.files : [], error: "" },
        isPublished: {value: props.isPublished ? props.isPublished : false, error: ""}
    }

    useEffect(() => {
        if (typeof window !== "undefined") {
            if (props.content){
                setEditorState(EditorState.createWithContent(
                    stateFromMarkdown(props.content, {
                        parserOptions: {
                            atomicImages: true
                        }
                    })
                ))
            }
        }
    }, [])

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestData = {
            id: state.id,
            thumbnailImageId: state.thumbnailImage.length > 0 ? state.thumbnailImage[0].id : null,
            previewImageId: state.previewImage.length > 0 ? state.previewImage[0].id : null,
            categoryId: state.categoryId,
            title: state.title,
            description: state.description,
            content: convertedToRaw,
            files: state.files,
            isPublished: state.isPublished
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(requestData)
        };

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                
                AuthenticationHelper.HandleResponse(response);
                
                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        toast.success(jsonResponse.message);
                        setFieldValue({ name: "id", value: jsonResponse.id });
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            });
    }

    const stateValidatorSchema = {
        title: {
            required: {
                isRequired: true,
                error: props.titleRequiredErrorMessage
            }
        },
        categoryId: {
            required: {
                isRequired: true,
                error: props.categoryRequiredErrorMessage
            }
        },
        description: {
            required: {
                isRequired: true,
                error: props.descriptionRequiredErrorMessage
            }
        }
    }

    const handleEditorChange = (state) => {
        setEditorState(state);

        const convertedToMarkdown = stateToMarkdown(state.getCurrentContent());
        setConvertedToRaw(convertedToMarkdown);
    }

    const uploadCallback = (file) => {
        return new Promise((resolve, reject) => {
            const formData = new FormData();

            formData.append("file", file)
            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.saveMediaUrl, requestOptions)
                .then(function (response) {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    
                    AuthenticationHelper.HandleResponse(response);

                    response.json().then((media) => {
                        if (response.ok) {

                            resolve({
                                data: {
                                    link: media.url
                                }
                            })
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

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);
    
    const { id, title, previewImage, thumbnailImage, description, isPublished, files, categoryId } = values;
    
    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
                            </div>
                        }
                        <div className="field">
                            <TextField 
                                id="title"
                                name="title"
                                label={props.titleLabel} 
                                fullWidth={true}
                                value={title} 
                                onChange={handleOnChange} 
                                variant="standard"
                                helperText={dirty.title ? errors.title : ""} 
                                error={(errors.title.length > 0) && dirty.title} 
                            />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="category">{props.categoryLabel}</InputLabel>
                                <Select
                                    labelId="category"
                                    id="categoryId"
                                    name="categoryId"
                                    value={categoryId}
                                    onChange={handleOnChange}>
                                    <MenuItem key={0} value="">{props.selectCategoryLabel}</MenuItem>
                                    {props.categories && props.categories.map(category =>
                                        <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                    )}
                                </Select>
                                {errors.categoryId && dirty.categoryId && (
                                    <FormHelperText>{errors.categoryId}</FormHelperText>
                                )}
                            </FormControl>
                        </div>
                        <div className="field">
                            <TextField 
                                id="description" 
                                name="description"
                                label={props.descriptionLabel} 
                                fullWidth={true}
                                value={description} 
                                multiline={true}
                                onChange={handleOnChange}
                                variant="standard"
                                helperText={dirty.description ? errors.description : ""} 
                                error={(errors.description.length > 0) && dirty.description} 
                            />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="thumbnailImage"
                                name="thumbnailImage"
                                label={props.thumbImageLabel}
                                multiple={false}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={thumbnailImage}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"]
                                }}/>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="previewImage"
                                name="previewImage"
                                label={props.previewImageLabel}
                                multiple={false}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={previewImage}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"]
                                }}/>
                        </div>
                        <div className="field">
                            <NoSsr>
                                <Editor 
                                    editorState={editorState} 
                                    onEditorStateChange={handleEditorChange}
                                    localization={{
                                        locale: props.locale
                                    }}
                                    toolbar={{
                                        image: {
                                            uploadEnabled: true,
                                            previewImage: true,
                                            inputAccept: 'image/jpeg,image/jpg,image/png,image/webp',
                                            uploadCallback: uploadCallback
                                        }
                                    }}
                                />
                            </NoSsr>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.filesLabel}
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} 
                                accept={{
                                    "application/*": [".pdf", ".docx", ".doc", ".zip"]
                                }}/>
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={e => {
                                                setFieldValue({ name: "isPublished", value: e.target.checked });
                                            }}
                                            checked={isPublished}
                                            id="isPublished"
                                            name="isPublished"
                                            color="secondary" 
                                        />
                                    }
                                    label={props.isPublishedLabel} />
                            </NoSsr>
                        </div>
                        <div className="field">
                            <Button 
                                    type="submit" 
                                    variant="contained" 
                                    color="primary"
                                    disabled={state.isLoading || disable || !convertedToRaw}
                                >
                                {props.saveText}
                            </Button>
                            <a href={props.newsUrl} className="ml-2 button is-text">{props.navigateToNewsLabel}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    )
}

NewsItemForm.propTypes = {
    id: PropTypes.string,
    title: PropTypes.string.isRequired,
    newsUrl: PropTypes.string,
    saveText: PropTypes.string,
    titleLabel: PropTypes.string,
    heroImageLabel: PropTypes.string,
    generalErrorMessage: PropTypes.string,
    deleteLabel: PropTypes.string,
    dropFilesLabel: PropTypes.string,
    dropOrSelectImagesLabel: PropTypes.string,
    saveMediaUrl: PropTypes.string,
    navigateToNewsLabel: PropTypes.string,
    descriptionLabel: PropTypes.string,
    filesLabel: PropTypes.string,
    imagesLabel: PropTypes.string,
    isPublishedLabel: PropTypes.string,
    isNewLabel: PropTypes.string,
    categoryLabel: PropTypes.string,
    selectCategoryLabel: PropTypes.string,
    thumbImageLabel: PropTypes.string,
    idLabel: PropTypes.string
}

export default NewsItemForm;