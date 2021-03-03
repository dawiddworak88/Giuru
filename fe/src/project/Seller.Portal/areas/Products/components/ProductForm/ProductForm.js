import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import NoSsr from '@material-ui/core/NoSsr';
import Autocomplete from "@material-ui/lab/Autocomplete";
import { Context } from "../../../../../../shared/stores/Store";
import useForm from "../../../../../../shared/helpers/forms/useForm";
import { TextField, Button, CircularProgress, FormControlLabel, Switch } from "@material-ui/core";
import MediaCloud from "../../../../../../shared/components/MediaCloud/MediaCloud";
import DynamicForm from "../../../../../../shared/components/DynamicForm/DynamicForm";

function ProductForm(props) {

    const uiSchema = {};

    const jsonSchema = {
        "definitions": {
            "Color": {
                "title": "Color",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "#ff0000"
                        ],
                        "title": "Red"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "#00ff00"
                        ],
                        "title": "Green"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "#0000ff"
                        ],
                        "title": "Blue"
                    }
                ]
            },
            "Fabrics": {
                "title": "Fabrics",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "1"
                        ],
                        "title": "Monolith 77"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "2"
                        ],
                        "title": "Sawana 14"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "3"
                        ],
                        "title": "Berlin 01"
                    }
                ]
            },
            "Shape": {
                "title": "Shape",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "111"
                        ],
                        "title": "L Shape"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "112"
                        ],
                        "title": "U Shape"
                    }
                ]
            },
            "Orientation": {
                "title": "Orientation",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "1111"
                        ],
                        "title": "Left"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "1112"
                        ],
                        "title": "Right"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "1113"
                        ],
                        "title": "Ambiguous"
                    }
                ]
            },
            "UpholsteryMaterial": {
                "title": "Upholstery Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "191"
                        ],
                        "title": "Polyester"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "192"
                        ],
                        "title": "Velvet"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "193"
                        ],
                        "title": "Chenile"
                    }
                ]
            },
            "SeatFillMaterial": {
                "title": "Seat Fill Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91"
                        ],
                        "title": "Foam"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "92"
                        ],
                        "title": "Bonnel"
                    }
                ]
            },
            "BackFillMaterial": {
                "title": "Back Fill Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91"
                        ],
                        "title": "Foam"
                    }
                ]
            },
            "CushionsFillMaterial": {
                "title": "Cushions Fill Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91"
                        ],
                        "title": "Foam"
                    }
                ]
            },
            "FrameMaterial": {
                "title": "Frame Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91"
                        ],
                        "title": "Wood"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "92"
                        ],
                        "title": "Metal"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "93"
                        ],
                        "title": "Plastics"
                    }
                ]
            },
            "FrameWoodType": {
                "title": "Frame Wood Type",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "913"
                        ],
                        "title": "pine"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "923"
                        ],
                        "title": "Spruce"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "933"
                        ],
                        "title": "Oak"
                    }
                ]
            },
            "LegMaterial": {
                "title": "Leg Material",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91"
                        ],
                        "title": "Wood"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "92"
                        ],
                        "title": "Metal"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "93"
                        ],
                        "title": "Plastics"
                    }
                ]
            },
            "UnfoldMechanism": {
                "title": "Unfold Mechanism",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "31"
                        ],
                        "title": "Dolphine"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "32"
                        ],
                        "title": "DL"
                    }
                ]
            },
            "ArmType": {
                "title": "Arm Type",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "31"
                        ],
                        "title": "Flared Arms"
                    }
                ]
            },
            "BackType": {
                "title": "Back Type",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "31"
                        ],
                        "title": "Pillow back"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "32"
                        ],
                        "title": "Cushion back"
                    }
                ]
            },
            "Style": {
                "title": "Style",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "551131"
                        ],
                        "title": "Modern"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "551132"
                        ],
                        "title": "Traditional"
                    }
                ]
            },
            "Country": {
                "title": "Country",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "41131"
                        ],
                        "title": "Poland"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "51132"
                        ],
                        "title": "Germany"
                    }
                ]
            },
            "ApprovedUse": {
                "title": "Approved Use",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "15131"
                        ],
                        "title": "Residential Use"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "17132"
                        ],
                        "title": "Commercial Use"
                    }
                ]
            },
            "LevelOfAssembly": {
                "title": "Level of Assembly",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "91888"
                        ],
                        "title": "Partial Assembly"
                    },
                    {
                        "type": "string",
                        "enum": [
                            "92889"
                        ],
                        "title": "Full Assembly"
                    }
                ]
            },
            "Warranty": {
                "title": "Warranty",
                "type": "string",
                "anyOf": [
                    {
                        "type": "string",
                        "enum": [
                            "918888"
                        ],
                        "title": "2 years"
                    }
                ]
            }
        },
        "type": "object",
        "properties": {
            "width": {
                "type": "number",
                "title": "Width [cm]:"
            },
            "height": {
                "type": "number",
                "title": "Height [cm]:"
            },
            "depth": {
                "type": "number",
                "title": "Depth [cm]:"
            },
            "shape": {
                "$ref": "#/definitions/Shape",
                "title": "Shape:"
            },
            "orientation": {
                "$ref": "#/definitions/Orientation",
                "title": "Orientation:"
            },
            "style": {
                "$ref": "#/definitions/Style",
                "title": "Style:"
            },
            "primaryColor": {
                "$ref": "#/definitions/Color",
                "title": "Primary Color:"
            },
            "secondaryColor": {
                "$ref": "#/definitions/Color",
                "title": "Secondary Color:"
            },
            "primaryFabrics": {
                "$ref": "#/definitions/Fabrics",
                "title": "Primary Fabrics:"
            },
            "secondaryFabrics": {
                "$ref": "#/definitions/Fabrics",
                "title": "Secondary Fabrics:"
            },
            "upholsteryMaterial": {
                "$ref": "#/definitions/UpholsteryMaterial",
                "title": "Upholstery Material:"
            },
            "seatAreaWidth": {
                "type": "number",
                "title": "Seat Area Width [cm]:"
            },
            "seatAreaHeight": {
                "type": "number",
                "title": "Seat Area Height [cm]:"
            },
            "seatAreaDepth": {
                "type": "number",
                "title": "Seat Area Depth [cm]:"
            },
            "seatCapacity": {
                "type": "number",
                "title": "Seat Capacity:"
            },
            "weightCapacity": {
                "type": "number",
                "title": "Weight Capacity [kg]:"
            },
            "sleepFunction": {
                "type": "boolean",
                "title": "Sleep Function"
            },
            "sleepAreaWidth": {
                "type": "number",
                "title": "Sleep Area Width [cm]:"
            },
            "sleepAreaDepth": {
                "type": "number",
                "title": "Sleep Area Depth [cm]:"
            },
            "unfoldMechanism": {
                "$ref": "#/definitions/UnfoldMechanism",
                "title": "Unfold Mechanism:"
            },
            "cushionsNumber": {
                "type": "number",
                "title": "Number of Cushions:"
            },
            "cushionsRemovable": {
                "type": "boolean",
                "title": "Cushions Removable"
            },
            "cushionsUpholsteryMaterial": {
                "$ref": "#/definitions/UpholsteryMaterial",
                "title": "Cushions Upholstery Material:"
            },
            "cushionsFillMaterial": {
                "$ref": "#/definitions/CushionsFillMaterial",
                "title": "Cushions Fill Material:"
            },
            "tossPillowsIncluded": {
                "type": "boolean",
                "title": "Toss Pillows Included"
            },
            "legWidth": {
                "type": "number",
                "title": "Leg Width [cm]:"
            },
            "legHeight": {
                "type": "number",
                "title": "Leg Height [cm]:"
            },
            "legDepth": {
                "type": "number",
                "title": "Leg Depth [cm]:"
            },
            "legColor": {
                "$ref": "#/definitions/Color",
                "title": "Leg Color:"
            },
            "armType": {
                "$ref": "#/definitions/ArmType",
                "title": "Arm Type:"
            },
            "backType": {
                "$ref": "#/definitions/BackType",
                "title": "Back Type:"
            },
            "isStandalone": {
                "type": "boolean",
                "title": "Standalone"
            },
            "regulatedHeadrest": {
                "type": "boolean",
                "title": "Regulated Headrest"
            },
            "frameMaterial": {
                "$ref": "#/definitions/FrameMaterial",
                "title": "Frame Material:"
            },
            "frameWoodType": {
                "$ref": "#/definitions/FrameWoodType",
                "title": "Frame Wood Type:"
            },        
            "storageContainers": {
                "type": "boolean",
                "title": "Storage Containers"
            },
            "storageContainersNumber": {
                "type": "number",
                "title": "Number of Storage Containers:"
            },
            "fireResistant": {
                "type": "boolean",
                "title": "Fire Resistant"
            },
            "approvedUse": {
                "type": "array",
                "uniqueItems": true,
                "items": {
                  "$ref": "#/definitions/ApprovedUse"
                },
                "title": "Approved Use:"
            },
            "piecesNumber": {
                "type": "number",
                "title": "Number of Pieces:"
            },
            "totalWeight": {
                "type": "number",
                "title": "Total Weight [kg]:"
            },
            "adultAssemblyRequired": {
                "type": "boolean",
                "title": "Adult Assembly Required"
            },
            "assemblyLevel": {
                "$ref": "#/definitions/LevelOfAssembly",
                "title": "Level of Assembly:"
            },
            "adultAssemblyNumber": {
                "type": "number",
                "title": "Number of Adults for Assembly:"
            },
            "warranty": {
                "$ref": "#/definitions/Warranty",
                "title": "Warranty:"
            },
            "countryOfOrigin": {
                "$ref": "#/definitions/Country",
                "title": "Country of Origin:"
            }
        }
    };

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
        isNew: { value: props.isNew ? props.isNew : false },
        formData: { value: props.formData ? JSON.parse(props.formData) : {} }
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

    const onCategoryChange = (event, newValue) => {

        dispatch({ type: "SET_IS_LOADING", payload: true });

        setFieldValue({ name: "category", value: newValue });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(newValue)
        };

        fetch(props.getCategorySchemaUrl, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                return response.json().then(jsonResponse => {

                    if (response.ok) {

                        
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

    const onSubmitForm = (state) => {

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

    const { id, category, sku, name, description, primaryProduct, images, files, isNew, formData } = values;

    return (
        <section className="section section-small-padding product">
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
                                onChange={onCategoryChange}
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
                            jsonSchema={jsonSchema} 
                            uiSchema={uiSchema} 
                            formData={formData} 
                            onChange={handleOnChange} />
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
    getCategorySchemaUrl: PropTypes.string.isRequired,
    generalErrorMessage: PropTypes.string.isRequired
};

export default ProductForm;
