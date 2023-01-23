import React, { useContext } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";
import { Context } from "../../../../../../shared/stores/Store";
import { Button, CircularProgress } from "@mui/material";
import { Edit, Delete } from "@mui/icons-material"
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import AuthenticationHelper from "../../../../../../shared/helpers/globals/AuthenticationHelper";
import useForm from "../../../../../../shared/helpers/forms/useForm";

const ProductCardForm = (props) => {
    const [state, dispatch] = useContext(Context);
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
        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { 
                "Content-Type": "application/json", 
                "X-Requested-With": "XMLHttpRequest" 
            },
            body: JSON.stringify(state)
        };

        fetch(props.saveUrl, requestOptions)
            .then(function (response) {
                dispatch({ type: "SET_IS_LOADING", payload: false });

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

        Object.entries(schema.properties).forEach(([parameter, element]) => {
            const newElement = {
                name: parameter,
                required: requiredNames.includes(parameter)
            };

            if (element && typeof element === "object"){
                newElement.dataOptions = {
                    title: element.title,
                    type: element.type
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
                <div className="card-title">{props.title}</div>
                <div className="card-content is-flex">
                    <div className="card-icon"><Edit/></div>
                    <div className="card-icon"><Delete/></div>
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
                    name={elementProps.name}
                    title={elementProps.dataOptions.title}
                    type={elementProps.dataOptions.type}
                />
            )
        })

        return elementList;
    }

    const generateSchemaElementFromElement = (element) => {
        const prop = {};

        Object.keys(element.dataOptions).forEach((key) => {
          if (element.dataOptions[key] !== '')
            prop[key] = element.dataOptions[key];
        });

        return prop;
    }

    const generateSchemaFromElementProps = (elements) => {
        if (!elements) 
            return {};

        const newSchema = {};
      
        const props = {};
        const elementDict = {};
        const dependentElements = new Set([]);

        for (let i = 0; i < elements.length; i += 1) {
            const element = elements[i];

            elementDict[element.name] = { ...element };

            if (element.dependents){
                element.dependents.forEach((possibility) => {
                    possibility.children.forEach((dependentElement) => {
                        dependentElements.add(dependentElement);
                    });
                });
            }
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

    const getIdFromElements = (elements) => {
        const names = elements.map((element) => element.name);
        const defaultNameLength = props.defaultInputName.length;
      
        return names.length > 0
          ? Math.max(
              ...names.map((name) => {
                if (name.startsWith(props.defaultInputName)) {
                  const index = name.substring(defaultNameLength, name.length);
                  const value = Number.parseInt(index);
      
                  if (!isNaN(value)) {
                    return value;
                  }
                }
      
                return 0;
              }),
            ) + 1
          : 1;
      }

    const addCard = (schema) => {

        const newElements = generateElementsFromSchema(schema);
        const i = getIdFromElements(newElements);

        const newElement = {
            name: `${props.defaultInputName}${i}`,
            required: false,
            dataOptions: {
                title: `${props.defaultInputName}${i}`,
                type: "string",
                default: ""
            }
        }

        newElements.splice(0, 0, newElement)

        updateSchema(newElements, schema)
    }

    const {
        values, disable, setFieldValue
    } = useForm(stateSchema, stateValidatorSchema, onSubmitForm);

    const { schema } = values;
    const requiredNames = schema.required ? schema.required : [];

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    <Button type="button" color="primary" variant="contained" className="mb-2" onClick={() => addCard(schema)}>{props.newText}</Button>
                    <form className="is-modern-form">
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
                                disabled={state.isLoading || disable}>
                                {props.saveText}
                            </Button>
                            <a href={props.productCardsUrl} className="ml-2 button is-text">{props.navigateToProductCardsLabel}</a>
                        </div>
                    </form>
                </div>
            </div>
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
    newText: PropTypes.string.isRequired
}

export default ProductCardForm;