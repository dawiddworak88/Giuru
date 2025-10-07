import React, { useContext, useEffect, useState } from "react";
import { toast } from "react-toastify";
import { PropTypes } from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import { Button, InputLabel, NoSsr, TextField } from "@mui/material";
import { Editor } from "react-draft-wysiwyg";
import { EditorState } from "draft-js";
import { stateToMarkdown } from "draft-js-export-markdown";
import { stateFromMarkdown } from "draft-js-import-markdown";

const ClientApprovalForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [convertedToRaw, setConvertedToRaw] = useState(props.description ? props.description : null);
    const [editorState, setEditorState] = useState(EditorState.createEmpty());

    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        name: { value: props.name ? props.name : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
    }

    const stateValidatorSchema = {
        name: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    };

    const handleEditorChange = (state) => {
        setEditorState(state);

        const convertedToMarkdown = stateToMarkdown(state.getCurrentContent());
        setConvertedToRaw(convertedToMarkdown);
    }

    const onSubmitForm = (state) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        var approval = {
            id,
            name,
            description: convertedToRaw
        }

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(approval)
        }

        fetch(props.saveUrl, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: true});

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

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, name } = values;

    useEffect(() => {
        if(typeof window !== "undefined") {
            if (props.description) {
                setEditorState(EditorState.createWithContent(
                    stateFromMarkdown(props.description)
                ));
            }
        }
    }, []);

    return (
        <section className="section section-small-padding category">
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
                                id="name"
                                name="name"
                                label={props.nameLabel}
                                fullWidth={true}
                                value={name}
                                onChange={handleOnChange}
                                variant="standard"
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <InputLabel id="description-label">{props.descriptionLabel}</InputLabel>
                            <NoSsr>
                                <Editor 
                                    editorState={editorState}
                                    onEditorStateChange={handleEditorChange}
                                    localization={{
                                        locale: props.locale
                                    }}
                                />
                            </NoSsr>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit"
                                variant="contained"
                                color="primary"
                                disabled={state.isLoading || disable}>
                                    {props.saveText}
                            </Button>
                            <a href={props.clientApprovalsUrl} className="ml-2 button is-text">{props.navigateToClientApprovals}</a>
                        </div>
                    </form>
                </div>
            </div>
        </section>
    );
}

ClientApprovalForm.propTypes = {
    id: PropTypes.string,
    saveUrl: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    idLabel: PropTypes.string.isRequired,
    name: PropTypes.string,
    nameLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    clientApprovalsUrl: PropTypes.string.isRequired,
    navigateToClientApprovals: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    fieldRequiredErrorMessage: PropTypes.string.isRequired
}

export default ClientApprovalForm;