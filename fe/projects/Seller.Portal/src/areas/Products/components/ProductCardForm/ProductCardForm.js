import React, { useContext, useState } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../shared/stores/Store";
import { Button, CircularProgress } from "@mui/material";
import { Edit, Delete, DragIndicator } from "@mui/icons-material"
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import AuthenticationHelper from "../../../../shared/helpers/globals/AuthenticationHelper";
import useForm from "../../../../shared/helpers/forms/useForm";
import ProductCardModal from "../../components/ProductCardModal/ProductCardModal";
import ConfirmationDialog from "../../../../shared/components/ConfirmationDialog/ConfirmationDialog";
import ProductCardConstants from "../../../../shared/constants/ProductCardConstants";
import CamelcaseHelper from "../../../../shared/helpers/globals/CamelCaseHelper";

const ProductCardForm = (props) => {
    const [state, dispatch] = useContext(Context);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [productAttribute, setProductAttribute] = useState(null);
    const [definitions, setDefinitions] = useState({});
    const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
    const [entityToDelete, setEntityToDelete] = useState(null);
    const stateSchema = {
        id: { value: props.id ? props.id : null },
        schema: { value: props.schema ? JSON.parse(props.schema) : null },
        uiSchema: { value: props.uiSchema ? JSON.parse(props.uiSchema) : null }
    }
    
    const stateValidatorSchema = {
        schema: {
            required: {
                isRequired: true,
                error: props.fieldRequiredErrorMessage
            }
        }
    }

    const onSubmitForm = (state) => {
        const requestPayload = {
            id: state.id,
            schema: JSON.stringify(state.schema),
            uiSchema: state.uiSchema ? JSON.stringify(state.uiSchema) : null
        }

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(requestPayload)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {
                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
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
    }

    const generateElementsFromSchema = (schema) => {
        if (!schema.properties)
            return [];

        const objects = [];
        const elementDict = {};

        setDefinitions(schema.definitions);

        Object.entries(schema.properties).forEach(([parameter, element]) => {
            let newElement = {
                name: parameter,
                required: requiredNames.includes(parameter)
            };

            if (element && typeof element === "object"){
                newElement.dataOptions = {
                    title: element.title,
                    type: element.type
                }
            }

            if (element.$ref !== undefined || element.items !== undefined) {
                const definitionArr = element.$ref ? element.$ref.split("/") : element.items.$ref.split("/");

                const definitionId = definitionArr[2];

                if (definitionId != null) {

                    newElement = {
                        ...newElement,
                        definitionId: definitionId
                    }
                }
            }

            elementDict[newElement.name] = newElement;
        })

        Object.keys(elementDict).forEach((name) => {
            objects.push(elementDict[name]);
        });

        return objects;
    }

    const Card = (props) => {
        return (
            <div className="card p-4 mb-2 is-flex is-justify-content-space-between is-align-items-center">
                <div className="card-title">{props.data.title}</div>
                <div className="card-content is-flex">
                    <div className="card-icon" onClick={() => handleProductAttribute(props.data)}><Edit/></div>
                    <div className="card-icon" onClick={() => handleDeleteClick(props.data)}><Delete/></div> 
                </div>
            </div>
        )
    }

    const generateElementComponentsFromSchema = (schema) => {
        if (!schema.properties)
            return [];

        const elementsAttributes = generateElementsFromSchema(schema);

        const elementList = elementsAttributes.map((elementProps, index) => {
            return (
                <Card 
                    key={index}
                    index={index}
                    schema={schema}
                    data={{
                        name: elementProps.name,
                        title: elementProps.dataOptions.title,
                        type: elementProps.dataOptions.type,
                        required: elementProps.required,
                        definitionId: elementProps.definitionId
                    }}
                />
            )
        })

        return elementList;
    }

    const generateSchemaElementFromElement = (element) => {
        if (element.definitionId != undefined) {

            return {
                title: element.dataOptions.title,
                $ref: `#/definitions/${element.definitionId}`
            }
        }
        else {
            const prop = {};

            Object.keys(element.dataOptions).forEach((key) => {
            if (element.dataOptions[key] !== '')
                prop[key] = element.dataOptions[key];
            });

            return prop;
        }
    }

    const generateSchemaFromElementProps = (elements) => {
        if (!elements) 
            return {};

        const newSchema = {};
      
        const props = {};
        const elementDict = {};
        const dependentElements = new Set([]);

        for (let i = 0; i < elements.length; i += 1) {
            let element = elements[i];

            elementDict[element.name] = { ...element };
        }

        Object.keys(elementDict).forEach((elementName) => {
            const element = elementDict[elementName];
            
            if (!dependentElements.has(elementName)) {
                props[element.name] = generateSchemaElementFromElement(element);
            }
        });
      
        newSchema.properties = props;
        newSchema.required = elements.filter(({ required, dependent }) => required && !dependent).map(({ name }) => name);
      
        return newSchema;
    }

    const updateSchema = (newElement, schema) => {
        const newSchema = Object.assign({...schema}, generateSchemaFromElementProps(newElement));

        newSchema.type = "object";
        newSchema.definitions = definitions;

        setFieldValue({ name: "schema", value: newSchema })
    }

    const dragEnd = (result, schema) => {
        const src = result.source.index;
        const dest = result.destination.index;

        const newElements = generateElementsFromSchema(schema);

        const tmpBlock = newElements[src];
        newElements[src] = newElements[dest];
        newElements[dest] = tmpBlock;

        updateSchema(newElements, schema)
    }

    const handleProductAttribute = (attribute) => {
        setProductAttribute(attribute);
        setIsModalOpen(true);
    }

    const handleCloseModal = () => {
        setIsModalOpen(false);
    }

    const handleOpenModal = () => {
        const newElement = {
            title: props.defaultInputName,
            name: CamelcaseHelper.replace(props.defaultInputName),
            type: ProductCardConstants.defaultInputType(),
            required: false,
        }

        setProductAttribute(newElement)
        setIsModalOpen(true);
    }

    const handleDefinitionSchema = (id) => {
        dispatch({ type: "SET_IS_LOADING", payload: true });

        if (definitions[id] !== undefined) {
            dispatch({ type: "SET_IS_LOADING", payload: false });
            return;
        }

        const requestOptions = {
            method: "GET",
            headers: { 
                "Content-Type": "application/json"
            }
        };

        fetch(`${props.definitionUrl}/${id}`, requestOptions)
            .then((response) => {
                dispatch({ type: "SET_IS_LOADING", payload: false });

                AuthenticationHelper.HandleResponse(response);

                return response.json().then(jsonResponse => {
                    if (response.ok) {
                        const newDefinitions = definitions;

                        newDefinitions[id] = jsonResponse.data;

                        setDefinitions(newDefinitions);
                    }
                    else {
                        toast.error(props.generalErrorMessage);
                    }
                })
            });
    }

    const handleSaveCard = (schema) => {
        const newElements = generateElementsFromSchema(schema);

        if (productAttribute != null) {

            let newElement = {
                name: productAttribute.name,
                required: false,
                dataOptions: {
                    title: productAttribute.title,
                    type: productAttribute.type,
                    default: ""
                }
            }

            if (productAttribute.definitionId !== undefined) {
                newElement = {
                    ...newElement,
                    definitionId: productAttribute.definitionId
                }

                handleDefinitionSchema(productAttribute.definitionId);
            }

            const existingProductAttributeIndex = newElements.findIndex((element) => element.name === productAttribute.name);

            if (existingProductAttributeIndex != -1) {
                newElements[existingProductAttributeIndex] = newElement;
            }
            else {
                newElements.splice(0, 0, newElement)
            }

            updateSchema(newElements, schema)
    
            setIsModalOpen(false);
            setProductAttribute(null);
        } 
    }

    const handleDeleteDialogClose = () => {
        setOpenDeleteDialog(false);
        setEntityToDelete(null);
    };

    const handleDeleteClick = (item) => {
        setEntityToDelete(item);
        setOpenDeleteDialog(true);
    };

    const handleDeleteEntity = () => {
        const newElements = generateElementsFromSchema(schema);

        const index = newElements.findIndex((element) => element.name === entityToDelete.name);

        const element = newElements[index];

        if (element.definitionId !== undefined) {
            const definitionItems = newElements.filter((item) => item.definitionId === element.definitionId);

            if (!(definitionItems.length > 1)) {
                const newDefinitions = definitions;

                delete newDefinitions[element.definitionId];

                setDefinitions(newDefinitions);
            }
        }

        newElements.splice(index, 1);

        updateSchema(newElements, schema);

        handleDeleteDialogClose();
    }

    const { values, setFieldValue, handleOnSubmit } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { schema } = values;
    const requiredNames = schema.required ? schema.required : [];

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <Button type="button" color="primary" variant="contained" className="mb-2" onClick={handleOpenModal}>{props.newText}</Button>
                    <form className="is-modern-form" onSubmit={handleOnSubmit}>
                        <DragDropContext onDragEnd={(result) => dragEnd(result, schema)}>
                            <Droppable droppableId="droppable">
                                {(providedDroppable) => (
                                    <div ref={providedDroppable.innerRef} {...providedDroppable.droppableProps}>
                                        {generateElementComponentsFromSchema(schema).map((element, index) => {
                                            return (
                                                <Draggable
                                                    key={element.key}
                                                    draggableId={element.key}
                                                    index={index}
                                                >
                                                    {(providedDraggable) => (
                                                        <div ref={providedDraggable.innerRef} {...providedDraggable.draggableProps} {...providedDraggable.dragHandleProps}>
                                                            {element}
                                                        </div>
                                                    )}
                                                </Draggable>
                                            )
                                        })}
                                        {providedDroppable.placeholder}
                                    </div>
                                )}
                            </Droppable>
                        </DragDropContext>
                        <div className="field">
                            <Button 
                                type="submit" 
                                variant="contained" 
                                color="primary" 
                                disabled={state.isLoading}>
                                {props.saveText}
                            </Button>
                            <a href={props.productCardsUrl} className="ml-2 button is-text">{props.navigateToProductCardsLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
            <ConfirmationDialog
                open={openDeleteDialog}
                handleClose={handleDeleteDialogClose}
                handleConfirm={handleDeleteEntity}
                titleId="delete-from-basket-title"
                title={props.deleteConfirmationLabel}
                textId="delete-from-basket-text"
                text={props.areYouSureLabel + ((entityToDelete ? (": " + entityToDelete.title) : "") + "?")}
                noLabel={props.noLabel}
                yesLabel={props.yesLabel}
            />
            <ProductCardModal 
                isOpen={isModalOpen}
                attribute={productAttribute}
                setAttribute={setProductAttribute}
                handleClose={handleCloseModal}
                handleSave={() => handleSaveCard(schema)}
                labels={props.productCardModal}
            />
            {state.isLoading && <CircularProgress className="progressBar" />}
        </section>
    )
}

ProductCardForm.propTypes = {
    title: PropTypes.string.isRequired,
    schema: PropTypes.string.isRequired,
    saveText: PropTypes.string.isRequired,
    saveUrl: PropTypes.string.isRequired,
    navigateToProductCardsLabel: PropTypes.string.isRequired,
    productCardsUrl: PropTypes.string.isRequired,
    defaultInputName: PropTypes.string.isRequired,
    newText: PropTypes.string.isRequired,
    yesLabel: PropTypes.string.isRequired,
    noLabel: PropTypes.string.isRequired,
    productAttributeExistsMessage: PropTypes.string.isRequired
}

export default ProductCardForm;