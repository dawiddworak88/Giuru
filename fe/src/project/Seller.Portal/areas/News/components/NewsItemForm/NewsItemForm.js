import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import NavigationHelper from "../../../../../../shared/helpers/globals/NavigationHelper";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import { EditorState, convertToRaw, ContentState, convertFromHTML} from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import { 
    TextField, Select, FormControl, InputLabel, MenuItem, Button, CircularProgress 
} from "@material-ui/core";

const NewsItemForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [showBackToNewsListButton, setShowBackToNewsListButton] = useState(false);
    const [convertedToRaw, setConvertedToRaw] = useState(null);
    const [editorState, setEditorState] = useState(props.content ? EditorState.createWithContent(
        ContentState.createFromText(
            convertFromHTML(props.content)
        )
    ) : EditorState.createEmpty());
    
    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        title: { value: props.newsTitle ? props.newsTitle : "", error: "" },
        heroImage: { value: props.images ? props.images : [] },
        content: { value: editorState, error: "" }
    }

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestData = {
            heroImage:  state.heroImage[0].id,
            title: state.title,
            content: convertedToRaw
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(requestData)
        };
    }

    const stateValidatorSchema = () => {

    }

    const handleEditorChange = (state) => {
        setEditorState(state);

        const converted = draftToHtml(convertToRaw(editorState.getCurrentContent()))
        
        setConvertedToRaw(converted);
    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);
    
    const { title, heroImage } = values;
    return (
        <section className="section section-small-padding category">
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
                            <Editor 
                                editorState={editorState} 
                                onEditorStateChange={handleEditorChange}
                            />
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
    navigateToNewsLabel: PropTypes.string
}

export default NewsItemForm;