import React from "react";
import PropTypes from "prop-types";
import { Edit, Delete } from "@mui/icons-material"

const ProductCardForm = (props) => {

    const schema = props.schema ? JSON.parse(props.schema) : null;
    const requiredNames = schema.required ? schema.required : [];

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

    return (
        <section className="section section-small-padding category">
            <h1 className="subtitle is-4">{props.title}</h1>
            <div className="columns is-desktop">
                <div className="column is-half">
                    {generateElementComponentsFromSchema(schema)}
                    <div className="mt-2">
                        <a href={props.productCardsUrl} className="button is-text">{props.navigateToProductCardsLabel}</a>
                    </div>
                </div>
            </div>
        </section>
    )
}

ProductCardForm.propTypes = {
    title: PropTypes.string.isRequired,
    schema: PropTypes.string.isRequired,
    navigateToProductCardsLabel: PropTypes.string.isRequired
}

export default ProductCardForm;