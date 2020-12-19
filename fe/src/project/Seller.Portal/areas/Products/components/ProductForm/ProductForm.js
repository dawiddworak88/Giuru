import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress, FormControlLabel, Switch } from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";

function ProductForm(props) {

    const categoriesProps = {
        options: props.categories,
        getOptionLabel: (option) => option.name
    };

    const primaryProductsProps = {
        options: props.primaryProducts,
        getOptionLabel: (option) => option.name
    };

    const [state, dispatch] = useContext(Context);

    const stateSchema = {

        id: { value: props.id ? props.id : null, error: "" },
        category: { value: props.categoryId ? props.categories.find((item) => item.id === props.categoryId) : null },
        name: { value: props.name ? props.name : "", error: "" },
        description: { value: props.description ? props.description : "", error: "" },
        sku: { value: props.sku ? props.sku : "", error: "" },
        primaryProduct: { value: props.primaryProductId ? props.primaryProducts.find((item) => item.id === props.primaryProductId) : null },
        images: { value: props.images ? props.images : [] },
        files: { value: props.files ? props.files : [] },
        isNew: { value: props.isNew ? props.isNew : false }
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

    function onSubmitForm(state) {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        setFieldValue({ name: "id", value: jsonResponse.id });
                        toast.success(jsonResponse.message);
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

    const {
        values,
        errors,
        dirty,
        disable,
        setFieldValue,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm, !props.id);

    const { id, category, sku, name, description, primaryProduct, images, files, isNew } = values;

    return (
        <section className="section section-small-padding">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                        {id &&
                            <input id="id" name="id" type="hidden" value={id} />
                        }
                        <div className="field">
                            <Autocomplete
                                {...categoriesProps}
                                id="category"
                                name="category"
                                fullWidth={true}
                                value={category}
                                onChange={(event, newValue) => {
                                    setFieldValue({ name: "category", value: newValue });
                                  }}
                                autoComplete
                                renderInput={(params) => <TextField {...params} label={props.selectCategoryLabel} margin="normal" />}
                            />
                        </div>
                        <div className="field">
                            <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                                value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ""} error={(errors.sku.length > 0) && dirty.sku} />
                        </div>
                        <div className="field">
                            <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                                value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ""} error={(errors.name.length > 0) && dirty.name} />
                        </div>
                        <div className="field">
                            <TextField id="description" name="description" label={props.descriptionLabel} fullWidth={true}
                                value={description} onChange={handleOnChange} multiline />
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
                                renderInput={(params) => <TextField {...params} label={props.selectPrimaryProductLabel} margin="normal" />}
                            />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="images"
                                name="images"
                                label={props.productPicturesLabel}
                                accept=".png, .jpg"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                files={images}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
                            <MediaCloud
                                id="files"
                                name="files"
                                label={props.productFilesLabel}
                                accept=".png, .jpg, .pdf, .docx, .zip"
                                multiple={true}
                                generalErrorMessage={props.generalErrorMessage}
                                deleteLabel={props.deleteLabel}
                                dropFilesLabel={props.dropFilesLabel}
                                dropOrSelectFilesLabel={props.dropOrSelectFilesLabel}
                                imagePreviewEnabled={false}
                                files={files}
                                setFieldValue={setFieldValue}
                                saveMediaUrl={props.saveMediaUrl} />
                        </div>
                        <div className="field">
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
                        </div>
                        <div className="field">
                            <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
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
    isNewLabel: PropTypes.string.isRequired,
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
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductForm;
