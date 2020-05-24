import React, { useContext, useState } from 'react';
import PropTypes from 'prop-types';
import { Context } from '../../../../../../shared/stores/Store';
import useForm from '../../../../../../shared/helpers/forms/useForm';
import { TextField, Button, CircularProgress } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';
import DynamicForm from '../../../../../../shared/components/DynamicForm/DynamicForm';

function ProductDetailForm(props) {

    const [state] = useContext(Context);

    const stateSchema = {

        name: { value: '', error: '' },
        sku: { value: '', error: '' },
        formData: {}
    };

    const stateValidatorSchema = {

        name: {
            required: {
                isRequired: true,
                error: props.nameRequiredErrorMessage
            }
        },
        sku: {
            required: {
                isRequired: true,
                error: props.skuRequiredErrorMessage
            }
        }
    };

    function onSubmitForm(state) {
        console.log(state);
    }

    const {
        values,
        errors,
        dirty,
        disable,
        handleOnChange,
        handleOnSubmit
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { name, sku, formData } = values;

    var tempSchemas = [
        { name: "Narożniki bez strony", value: "1" },
        { name: "Narożniki ze stroną", value: "2" },
        { name: "Sofy", value: "3" },
        { name: "Szafy", value: "4" }
    ];

    const [tempSchema, setTempSchema] = useState(null);

    function changeSchema(schema) {

        if (schema.value === "1") {

            var schema1 = {
                id: "1",
                jsonSchema: {
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
                        "Material": {
                            "title": "Material",
                            "type": "string",
                            "anyOf": [
                                {
                                    "type": "string",
                                    "enum": [
                                        "11"
                                    ],
                                    "title": "Foam-T25"
                                },
                                {
                                    "type": "string",
                                    "enum": [
                                        "12"
                                    ],
                                    "title": "Frame-Wood"
                                },
                                {
                                    "type": "string",
                                    "enum": [
                                        "13"
                                    ],
                                    "title": "Legs-Plastic"
                                }
                            ]
                        }
                    },
                    "properties": {
                        "primaryColor": {
                            "type": "array",
                            "uniqueItems": true,
                            "items": {
                                "$ref": "#/definitions/Color"
                            },
                            "title": "Primary Color:"
                        },
                        "secondaryColor": {
                            "type": "array",
                            "uniqueItems": true,
                            "items": {
                                "$ref": "#/definitions/Color"
                            },
                            "title": "Secondary Color:"
                        },
                        "primaryFabrics": {
                            "type": "array",
                            "uniqueItems": true,
                            "items": {
                                "$ref": "#/definitions/Fabrics"
                            },
                            "title": "Primary Fabrics:"
                        },
                        "secondaryFabrics": {
                            "type": "array",
                            "uniqueItems": true,
                            "items": {
                                "$ref": "#/definitions/Fabrics"
                            },
                            "title": "Secondary Fabrics:"
                        },
                        "materials": {
                            "type": "array",
                            "uniqueItems": true,
                            "items": {
                                "$ref": "#/definitions/Material"
                            },
                            "title": "Materials:"
                        },
                        "description": {
                            "type": "string",
                            "title": "Description:"
                        },
                        "minimumQuantity": {
                            "type": "number",
                            "title": "Minimum Quantity:"
                        },
                        "packages": {
                            "type": "number",
                            "title": "Packages:"
                        },
                        "width": {
                            "type": "number",
                            "title": "Width:"
                        },
                        "height": {
                            "type": "number",
                            "title": "Height:"
                        },
                        "depth": {
                            "type": "number",
                            "title": "Depth:"
                        },
                        "sleepingAreaWidth": {
                            "type": "number",
                            "title": "Width of Sleeping Area:"
                        },
                        "sleepingAreaLength": {
                            "type": "number",
                            "title": "Length of Sleeping Area:"
                        },
                        "heightOfSitting": {
                            "type": "number",
                            "title": "Height of Sitting:"
                        },
                        "package1Volume": {
                            "type": "number",
                            "title": "Package 1 Volume:"
                        },
                        "package1GrossWeight": {
                            "type": "number",
                            "title": "Package 1 Gross Weight:"
                        },
                        "package1Length": {
                            "type": "number",
                            "title": "Package 1 Length:"
                        },
                        "package1Width": {
                            "type": "number",
                            "title": "Package 1 Width:"
                        },
                        "package1Height": {
                            "type": "number",
                            "title": "Package 1 Height:"
                        },
                        "package2Volume": {
                            "type": "number",
                            "title": "Package 2 Volume:"
                        },
                        "package2GrossWeight": {
                            "type": "number",
                            "title": "Package 2 Gross Weight:"
                        },
                        "package2Length": {
                            "type": "number",
                            "title": "Package 2 Length:"
                        },
                        "package2Width": {
                            "type": "number",
                            "title": "Package 2 Width:"
                        },
                        "package2Height": {
                            "type": "number",
                            "title": "Package 2 Height:"
                        },
                        "package3Volume": {
                            "type": "number",
                            "title": "Package 3 Volume:"
                        },
                        "package3GrossWeight": {
                            "type": "number",
                            "title": "Package 3 Gross Weight:"
                        },
                        "package3Length": {
                            "type": "number",
                            "title": "Package 3 Length:"
                        },
                        "package3Width": {
                            "type": "number",
                            "title": "Package 3 Width:"
                        },
                        "package3Height": {
                            "type": "number",
                            "title": "Package 3 Height:"
                        },
                        "package4Volume": {
                            "type": "number",
                            "title": "Package 4 Volume:"
                        },
                        "package4GrossWeight": {
                            "type": "number",
                            "title": "Package 4 Gross Weight:"
                        },
                        "package4Length": {
                            "type": "number",
                            "title": "Package 4 Length:"
                        },
                        "package4Width": {
                            "type": "number",
                            "title": "Package 4 Width:"
                        },
                        "package4Height": {
                            "type": "number",
                            "title": "Package 4 Height:"
                        },
                        "package5Volume": {
                            "type": "number",
                            "title": "Package 5 Volume:"
                        },
                        "package5GrossWeight": {
                            "type": "number",
                            "title": "Package 5 Gross Weight:"
                        },
                        "package5Length": {
                            "type": "number",
                            "title": "Package 5 Length:"
                        },
                        "package5Width": {
                            "type": "number",
                            "title": "Package 5 Width:"
                        },
                        "package5Height": {
                            "type": "number",
                            "title": "Package 5 Height:"
                        },
                        "intrastat": {
                            "type": "string",
                            "title": "Intrastat:"
                        }
                    }
                }
            }

            setTempSchema(schema1);
        }

        if (schema.value === "2") {
            var schema2 = {
                id: "2",
                jsonSchema: {
                    "type": "object",
                    "properties": {
                        "side": {
                            "type": "string",
                            "title": "Strona:"
                        },
                        "threadColor": {
                            "type": "string",
                            "title": "Kolor nici:"
                        }
                    }
                }
            }

            setTempSchema(schema2);
        }
    }

    return (
        <div>
            <form className="is-modern-form" onSubmit={handleOnSubmit} method="post">
                {tempSchemas &&
                    <Autocomplete
                        id="select-schema"
                        options={tempSchemas}
                        onChange={(event, schema) => {
                            changeSchema(schema);
                        }}
                        getOptionLabel={(option) => option.name}
                        renderInput={(params) => <TextField {...params} label={props.selectSchemaLabel} variant="outlined" />} />
                }
                <div className="field">
                    <TextField id="sku" name="sku" label={props.skuLabel} fullWidth={true}
                        value={sku} onChange={handleOnChange} helperText={dirty.sku ? errors.sku : ''} error={(errors.sku.length > 0) && dirty.sku} />
                </div>
                <div className="field">
                    <TextField id="name" name="name" label={props.nameLabel} fullWidth={true}
                        value={name} onChange={handleOnChange} helperText={dirty.name ? errors.name : ''} error={(errors.name.length > 0) && dirty.name} />
                </div>
                {tempSchema &&
                    <DynamicForm schema={tempSchema} formData={formData} onChange={handleOnChange} />
                }
                <div className="field">
                    <Button type="submit" variant="contained" color="primary" disabled={state.isLoading || disable}>
                        {props.saveText}
                    </Button>
                </div>
            </form>
            {state.isLoading && <CircularProgress className="progressBar" />}
        </div>
    );
}

ProductDetailForm.propTypes = {
    selectLabel: PropTypes.string,
    schema: PropTypes.object.isRequired
};

export default ProductDetailForm;