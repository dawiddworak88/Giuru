import React, { useContext, useState, useEffect } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import useForm from "../../../../shared/helpers/forms/useForm";
import { EditorState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import { stateToMarkdown } from "draft-js-export-markdown";
import { stateFromMarkdown } from 'draft-js-import-markdown';
import { 
    TextField, Button, CircularProgress, FormControlLabel, 
    Switch, InputLabel, NoSsr, Autocomplete 
} from "@mui/material";
import MediaCloud from "../../../../shared/components/MediaCloud/MediaCloud";
import DynamicForm from "../../../../shared/components/DynamicForm/DynamicForm";
import QueryStringSerializer from "../../../../shared/helpers/serializers/QueryStringSerializer";
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import SearchConstants from "../../../../shared/constants/SearchConstants";
import moment from "moment";

function ProductForm(props) {
    const [state, dispatch] = useContext(Context);
    const [convertedToRaw, setConvertedToRaw] = useState(props.description ? props.description : null);
    const [editorState, setEditorState] = useState(EditorState.createEmpty());
    const [primaryProducts, setPrimaryProducts] = useState(props.primaryProducts ? props.primaryProducts : []);

    const categoriesProps = {
        options: props.categories,
        getOptionLabel: (option) => option.name
    };

    const primaryProductsProps = {
        options: primaryProducts,
        getOptionLabel: (option) => option.name
    };

    const stateSchema = {
        id: { value: props.id ? props.id : null, error: "" },
        category: { value: props.categoryId ? props.categories.find((item) => item.id === props.categoryId) : null },
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
        fulfillmentTime: { value: props.fulfillmentTime ? Number(moment.utc(1000 * props.fulfillmentTime).format("DD") - 1) : null}
    };

    const stateValidatorSchema = {
        sku: {
            required: {
                isRequired: true,
                error: props.skuRequiredErrorMessage
            }
        },
        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
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

        const getCategorySchemaUrl = props.getCategorySchemaUrl + "?" + QueryStringSerializer.serialize(payload);

        fetch(getCategorySchemaUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        setFieldValue({ name: "schema", value: jsonResponse.schema ? JSON.parse(jsonResponse.schema) : {} });
                        setFieldValue({ name: "uiSchema", value: jsonResponse.uiSchema ? JSON.parse(jsonResponse.uiSchema) : {} });
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                setFieldValue({ name: "schema", value: {} });
                setFieldValue({ name: "uiSchema", value: {} });
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
            isNew,
            ean,
            fulfillmentTime: fulfillmentTime ? moment.duration(fulfillmentTime, 'days').asSeconds() : null,
            formData: JSON.stringify(formData),
            isPublished
        };

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json", "X-Requested-With": "XMLHttpRequest" },
            body: JSON.stringify(product)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
                    }
                    else {
                        toast.error(jsonResponse?.message || props.generalErrorMessage);
                    }
                });
            }).catch(() => {
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(props.generalErrorMessage);
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

            const url = props.productsSuggestionUrl + "?" + QueryStringSerializer.serialize(searchParameters);
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

    const { 
        id, category, sku, name, primaryProduct, images, files, 
        isNew, schema, uiSchema, formData, isPublished, ean, fulfillmentTime
    } = values;

    return (
        <section className="section section-small-padding product">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <div className="field">
                                <InputLabel id="id-label">{props.idLabel} {id}</InputLabel>
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
                                renderInput={(params) => <TextField {...params} label={props.selectCategoryLabel} margin="normal" variant="standard" />}
                            />
                        </div>
                        <div className="field">
                            <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true} variant="standard"
                                value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ""} error={(errors.sku.length > 0) && dirty.sku} />
                        </div>
                        <div className="field">
                            <TextField 
                                id="ean" 
                                name="ean" 
                                label={props.eanLabel} 
                                fullWidth={true}
                                value={ean} 
                                variant="standard"
                                onChange={handleOnChange} />
                        </div>
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true} variant="standard"
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField type="number" id="fulfillmentTime" name="fulfillmentTime" label={props.fulfillmentTimeLabel} fullWidth={true} variant="standard"
                            value={fulfillmentTime} onChange={handleOnChange} />
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
                                renderInput={(params) => <TextField {...params} label={props.selectPrimaryProductLabel} margin="normal" variant="standard" onChange={productsSuggesstionFetchRequest} />}
                            />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.productPicturesLabel}
                                multiple={true}
                                videoSizeLimit={props.videoFileSizeLimit}
                                fileSizeLimitErrorMessage={props.fileSizeLimitErrorMessage}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={images}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl}
                                isUploadInChunksEnabled={true}
                                chunkSize={props.chunkSize}
                                saveMediaChunkUrl={props.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.saveMediaChunkCompleteUrl}
                                accept={{
                                    'image/*': [".png", ".jpg", ".webp"],
                                    "video/*": [".mp4"]
                                }}/>
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.productFilesLabel}
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                imagePreviewEnabled={false}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl}
                                isUploadInChunksEnabled={props.isUploadInChunksEnabled}
                                chunkSize={props.chunkSize}
                                saveMediaChunkUrl={props.saveMediaChunkUrl}
                                saveMediaChunkCompleteUrl={props.saveMediaChunkCompleteUrl} 
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
                                    label={props.isNewLabel} />
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
                                    label={props.isPublishedLabel} />
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
                            <a href={props.productsUrl} className="ml-2 button is-text">{props.navigateToProductsLabel}</a>
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
    isNew: PropTypes.bool.isRequired,
    isNewLabel: PropTypes.string.isRequired,
    isPublished: PropTypes.bool.isRequired,
    isPublishedLabel: PropTypes.string.isRequired,
    selectCategoryLabel: PropTypes.string.isRequired,
    selectPrimaryProductLabel: PropTypes.string.isRequired,
    productFilesLabel: PropTypes.string.isRequired,
    productPicturesLabel: PropTypes.string.isRequired,
    skuLabel: PropTypes.string.isRequired,
    nameLabel: PropTypes.string.isRequired,
    descriptionLabel: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    categories: PropTypes.array.isRequired,
    primaryProducts: PropTypes.array,
    dropOrSelectFilesLabel: PropTypes.string.isRequired,
    dropFilesLabel: PropTypes.string.isRequired,
    saveMediaUrl: PropTypes.string.isRequired,
    deleteLabel: PropTypes.string.isRequired,
    getCategorySchemaUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired,
    eanLabel: PropTypes.string.isRequired,
    idLabel: PropTypes.string,
    isUploadInChunksEnabled: PropTypes.bool,
    chunkSize: PropTypes.number,
    saveMediaChunkUrl: PropTypes.string,
    saveMediaChunkCompleteUrl: PropTypes.string,
    productsSuggestionUrl: PropTypes.string.isRequired,
    fileSizeLimitErrorMessage: PropTypes.string,
    videoFileSizeLimit: PropTypes.number
};

export default ProductForm;
