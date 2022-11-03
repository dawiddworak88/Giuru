import React, { useContext, useState, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { EditorState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import { stateToMarkdown } from "draft-js-export-markdown";
import { stateFromMarkdown } from 'draft-js-import-markdown';
import { 
    TextField, Button, CircularProgress, FormControlLabel, 
    Switch, InputLabel, NoSsr, Autocomplete, FormControl, 
    MenuItem, Select
} from "@mui/material";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import DynamicForm from "../../../../../../shared/components/DynamicForm/DynamicForm";
import QueryStringSerializer from "../../../../../../shared/helpers/serializers/QueryStringSerializer";
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import SearchConstants from "../../../../../../shared/constants/SearchConstants";

function ProductForm(props) {
    const [state, dispatch] = useContext(Context);
    const [convertedToRaw, setConvertedToRaw] = useState(props.description ? props.description : null);
    const [editorState, setEditorState] = useState(EditorState.createEmpty());
    const [primaryProducts, setPrimaryProducts] = useState(props.productBase.primaryProducts ? props.productBase.primaryProducts : []);

    const categoriesProps = {
        options: props.productBase.categories,
        getOptionLabel: (option) => option.name
    };

    const primaryProductsProps = {
        options: primaryProducts,
        getOptionLabel: (option) => option.name
    };

    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        category: { value: props.categoryId ? props.productBase.categories.find((item) => item.id === props.categoryId) : null },
        name: { value: props.name ? props.name : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
        sku: { value: props.sku ? props.sku : "", error: "" },
        primaryProduct: { value: props.primaryProduct ? props.primaryProduct : null },
        images: { value: props.images ? props.images : [] },
        files: { value: props.files ? props.files : [] },
        isNew: { value: props.isNew ? props.isNew : false },
        schema: { value: props.schema ? JSON.parse(props.schema) : {} },
        uiSchema: { value: props.uiSchema ? JSON.parse(props.uiSchema) : {} },
        formData: { value: props.formData ? JSON.parse(props.formData) : {} },
        isPublished: { value: props.isPublished ? props.isPublished : false },
        ean: { value: props.ean ? props.ean : "" },
        groupIds: { value: props.groupIds ? props.groupIds : []},
    };

    const stateValidatorSchema = {
        sku: {
            required: {
                isRequired: true,
                error: props.productBase.skuRequiredErrorMessage
            }
        },
        name: {
            required: {
                isRequired: true,
                error: props.productBase.nameRequiredErrorMessage
            }
        }
    };

    useEffect(() => {
        if (typeof window !== "undefined") {
            if (props.description){
                setEditorState(EditorState.createWithContent(
                    stateFromMarkdown(props.description)
                ))
            }
        }
    }, [])

    const onCategoryChange = (event, newValue) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        setFieldValue({ name: "category", value: newValue });

        var payload = {
            categoryId: newValue.id
        };

        const requestOptions = {
            method: "GET",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
        };

        const getCategorySchemaUrl = props.productBase.getCategorySchemaUrl + "?" + QueryStringSerializer.serialize(payload);

        fetch(getCategorySchemaUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "schema", value: jsonResponse.schema ? JSON.parse(jsonResponse.schema) : {} });
                        setFieldValue({ name: "uiSchema", value: jsonResponse.uiSchema ? JSON.parse(jsonResponse.uiSchema) : {} });
                    }
                    else {
                        toast.error(props.productBase.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.productBase.generalErrorMessage);
            });
    };

    const handleEditorChange = (state) => {
        setEditorState(state);

        const convertedToMarkdown = stateToMarkdown(state.getCurrentContent());
        setConvertedToRaw(convertedToMarkdown);
    }

    const onSubmitForm = (state) => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        var product = {
            id,
            categoryId: category ? category.id : null,
            sku,
            name,
            description: convertedToRaw,
            primaryProductId: primaryProduct ? primaryProduct.id : null,
            images,
            files,
            groupIds,
            isNew,
            ean,
            formData: JSON.stringify(formData),
            isPublished
        };
        
        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(product)
        };

        fetch(props.productBase.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(props.productBase.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.productBase.generalErrorMessage);
            });
    };

    const productsSuggesstionFetchRequest = (e) => {
        const { value } = e.target;

        if (value.length >= SearchConstants.minSearchTermLength()){
            const searchParameters = {
                searchTerm: value,
                pageIndex: 1,
                hasPrimaryProduct: false,
                itemsPerPage: SearchConstants.productsSuggestionItemsPerPage()
            };

            const requestOptions = {
                method: "GET",
                headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" }
            };

            const url = props.productBase.productsSuggestionUrl + "?" + QueryStringSerializer.serialize(searchParameters);
            return fetch(url, requestOptions)
                .then((response) => {

                    AuthenticationHelper.HandleResponse(response);

                    return response.json().then(jsonResponse => {
                        if (response.ok) {
                            setPrimaryProducts(jsonResponse.data);
                        }
                    });
                })
        }
    }

    const uploadCallback = (file) => {
        return new Promise((resolve, reject) => {
            const formData = new FormData();

            formData.append("file", file)

            const requestOptions = {
                method: "POST",
                body: formData
            };

            fetch(props.productBase.saveMediaUrl, requestOptions)
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
                            toast.error(props.productBase.generalErrorMessage);
                        }
                    });
                }).catch(() => {
                    dispatch({ type: "SET_IS_LOADING", payload: false });
                    toast.error(props.productBase.generalErrorMessage);
                });
        });
    }

    const {
        values, errors, dirty, disable,
        setFieldValue, handleOnChange, handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { 
        id, category, sku, name, primaryProduct, images, files, 
        isNew, schema, uiSchema, formData, isPublished, ean, groupIds 
    } = values;

    return (
        <section className="section section-small-padding product">
            <h1 className="subtitle is-4">{props.productBase.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.productBase.idLabel} {id}</InputLabel>
                            </div>}
                        <div className="field">
                            <Autocomplete
                                {...categoriesProps}
                                id="category"
                                name="category"
                                fullWidth={true}
                                value={category}
                                onChange={onCategoryChange}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.productBase.selectCategoryLabel} margin="normal" variant="standard" />}
                            />
                        </div>
                        <div className="field">
                            <FormControl fullWidth={true} variant="standard">
                                <InputLabel id="groups-label">{props.productBase.groupsLabel}</InputLabel>
                                <Select
                                    labelId="groups-label"
                                    id="groupIds"
                                    name="groupIds"
                                    value={groupIds}
                                    multiple={true}
                                    onChange={handleOnChange}>
                                    {props.productBase.groups && props.productBase.groups.length > 0 ? (
                                        props.productBase.groups.map((group, index) => {
                                            return (
                                                <MenuItem key={index} value={group.id}>{group.name}</MenuItem>
                                            );
                                        })
                                    ) : (
                                        <MenuItem disabled>{props.productBase.noGroupsText}</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </div>
                        <div className="field">
                            <TextField 
                                id="sku" 
                                name="sku" 
                                label={props.productBase.skuLabel} 
                                fullWidth={true} 
                                variant="standard"
                                value={sku} 
                                onChange={handleOnChange} 
                                helperText={dirty.sku ? errors.sku : ""} 
                                error={(errors.sku.length > 0) && dirty.sku} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="ean" 
                                name="ean" 
                                label={props.productBase.eanLabel} 
                                fullWidth={true}
                                value={ean} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="name" 
                                name="name" 
                                label={props.productBase.nameLabel} 
                                fullWidth={true} 
                                variant="standard"
                                value={name} 
                                onChange={handleOnChange} 
                                helperText={dirty.name ? errors.name : ""} 
                                error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <InputLabel id="description-label">{props.productBase.descriptionLabel}</InputLabel>
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
                            <Autocomplete
                                {...primaryProductsProps}
                                id="primaryProductId"
                                name="primaryProductId"
                                fullWidth={true}
                                value={primaryProduct}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "primaryProduct", value: newValue });
                                  }}
                                autoComplete
                                renderInput={(params) => (
                                    <TextField 
                                        {...params} 
                                        label={props.productBase.selectPrimaryProductLabel} 
                                        margin="normal" 
                                        variant="standard" 
                                        onChange={productsSuggesstionFetchRequest} />)}
                            />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.productBase.productPicturesLabel}
                                multiple={true}
                                generalErrorMessage={props.productBase.generalErrorMessage}
                                deleteLabel={props.productBase.deleteLabel}
                                dropFilesLabel={props.productBase.dropFilesLabel}
                                dropOrSelectFilesLabel={props.productBase.dropOrSelectFilesLabel}
                                files={images}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.productBase.saveMediaUrl}
                                isUploadInChunksEnabled={props.productBase.isUploadInChunksEnabled}
                                chunkSize={props.productBase.chunkSize}
                                saveMediaChunkUrl={props.productBase.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.productBase.saveMediaChunkCompleteUrl} 
                                accept={{
                                    'image/*': [".png", ".jpg", ".webp"],
                                }}/>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.productBase.productFilesLabel}
                                multiple={true}
                                generalErrorMessage={props.productBase.generalErrorMessage}
                                deleteLabel={props.productBase.deleteLabel}
                                dropFilesLabel={props.productBase.dropFilesLabel}
                                dropOrSelectFilesLabel={props.productBase.dropOrSelectFilesLabel}
                                imagePreviewEnabled={false}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.productBase.saveMediaUrl}
                                isUploadInChunksEnabled={props.productBase.isUploadInChunksEnabled}
                                chunkSize={props.productBase.chunkSize}
                                saveMediaChunkUrl={props.productBase.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.productBase.saveMediaChunkCompleteUrl} 
                                accept={{
                                    "image/*": [".png", ".jpg", ".webp"],
                                    "application/*": [".pdf", ".docx", ".doc", ".zip"]
                                }}/>
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
                                    label={props.productBase.isNewLabel} />
                            </NoSsr>
                        </div>
                        <DynamicForm 
                            jsonSchema={schema} 
                            uiSchema={uiSchema} 
                            formData={formData} 
                            onChange={handleOnChange} />
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
                                        color="secondary" />
                                    }
                                    label={props.productBase.isPublishedLabel} />
                            </NoSsr>
                        </div>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading || disable}>
                                {props.productBase.saveText}
                            </Button>
                            <a href={props.productBase.productsUrl} className="ml-2 button is-text">{props.productBase.navigateToProductsLabel}</a>
                        </div>
                    </form>
                    {state.isLoading && <CircularProgress className="progressBar" />}
                </div>
            </div>
        </section>
    );
}

ProductForm.propTypes = {
    id: PropTypes.string,
    categoryId: PropTypes.string,
    name: PropTypes.string,
    sku: PropTypes.string,
    primaryProductId: PropTypes.string,
    images: PropTypes.array,
    files: PropTypes.array,
    formData: PropTypes.string,
    schema: PropTypes.string,
    uiSchema: PropTypes.string,
    isNewLabel: PropTypes.string.isRequired,
    categories: PropTypes.array.isRequired,
};

export default ProductForm;
