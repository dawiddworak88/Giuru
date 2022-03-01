import React, { useContext, useState, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import NoSsr from '@material-ui/core/NoSsr';
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import { EditorState, convertToRaw, ContentState, convertFromHTML} from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import { 
    TextField, Select, FormControl, FormControlLabel, Switch, InputLabel, MenuItem, Button, CircularProgress, TextareaAutosize
} from "@material-ui/core";

const NewsItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [showBackToNewsListButton, setShowBackToNewsListButton] = useState(false);
    const [tags, setTags] = useState([]);
    const [convertedToRaw, setConvertedToRaw] = useState(null);
    const [editorState, setEditorState] = useState(props.content ? EditorState.createWithContent(
        ContentState.createFromText(
            convertFromHTML(props.content)
        )
    ) : EditorState.createEmpty());
    
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        title: { value: props.newsTitle ? props.newsTitle : "", error: "" },
        heroImage: { value: props.images ? props.images : null },
        description: { value: props.description ? props.description : null },
        content: { value: props.content, error: "" },
        images: { value: props.images ? props.images : [] },
        files: { value: props.files ? props.files : [] },
        isNew: {value: props.isNew ? props.isNew : false, error: ""},
        isPublished: {value: props.isPublished ? props.isPublished : false, error: ""}
    }

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestData = {
            heroImage: state.heroImage ? state.heroImage[0].id : null,
            title: state.title,
            description: state.description,
            content: convertedToRaw,
            images: state.images,
            files: state.files,
            isNew: state.isNew,
            isPublished: state.isPublished
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(requestData)
        };


    }

    const stateValidatorSchema = () => {

    }

    const createTag = () => {
        const ul = document.querySelector("ul");
        ul.querySelectorAll("li").forEach(li => li.remove());
        tags.forEach(tag => {
            ul.insertAdjacentHTML("afterbegin", `<li>${tag}</li>`)
        })
    }

    const onChangeTag = (e) => {
        console.log("asdasd")
        let tag = e.target.value.replace("/\s+/g", " ");
        if (tag.length > 1 && !tags.includes(tag)){
            tag.split(", ").forEach(tag => {
                setTags([...tags, tag])
                createTag();
            });
        }
    }

    const handleEditorChange = (state) => {
        setEditorState(state);

        console.log(draftToHtml(convertToRaw(state.getCurrentContent())));

        const converted = draftToHtml(convertToRaw(state.getCurrentContent()))
        setConvertedToRaw(converted);
    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);
    
    const { title, heroImage, description, isNew, isPublished, files, images } = values;
    return (
        <section className="section section-small-padding news-item">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        <div className="field">
                            <MediaCloud
                                id="heroImage"
                                name="heroImage"
                                label={props.heroImageLabel}
                                accept=".png, .jpg, .webp"
                                multiple={false}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                files={heroImage}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="title"
                                name="title"
                                label={props.titleLabel} 
                                fullWidth={true}
                                value={title} 
                                onChange={handleOnChange} 
                            />
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
                            />
                        </div>
                        <div className="field">
                            <Editor 
                                editorState={editorState} 
                                onEditorStateChange={handleEditorChange}
                            />
                        </div>
                        {/* <div className="field">
                            <InputLabel id="language-label">Tagi</InputLabel>
                            <div className="news-item__tags">
                                <ul>
                                    <li>NOWOŚĆ</li>
                                    <li>NOWE</li>
                                </ul>
                                <input name="tags" id="tags" onKeyPress={onChangeTag}/>
                            </div>
                        </div> */}
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.imagesLabel}
                                accept=".png, .jpg, .webp"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                imagePreviewEnabled={false}
                                files={images}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.filesLabel}
                                accept=".pdf, .docx, .zip"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectImagesLabel}
                                imagePreviewEnabled={false}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <NoSsr>
                                <FormControlLabel
                                    control={
                                        <Switch
                                            onChange={e => {
                                                setFieldValue({ name: "isNew", value: e.target.checked });
                                            }}
                                            checked={isNew}
                                            id="isNew"
                                            name="isNew"
                                            color="secondary" />
                                        }
                                        label={props.isNewLabel} 
                                    />
                            </NoSsr>
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
                            {showBackToNewsListButton ? (
                                <Button 
                                    type="button" 
                                    variant="contained" 
                                    color="primary" 
                                    onClick={(e) => {
                                        e.preventDefault();
                                        NavigationHelper.redirect(props.newsUrl);
                                    }}>
                                    {props.navigateToNewsLabel}
                                </Button> 
                            ) : (
                                <Button 
                                    type="submit" 
                                    variant="contained" 
                                    color="primary"
                                    disabled={false}
                                    >
                                    {props.saveText}
                                </Button>
                            )}
                        </div>
                    </form>
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
    isNewLabel: PropTypes.string
}

export default NewsItemForm;