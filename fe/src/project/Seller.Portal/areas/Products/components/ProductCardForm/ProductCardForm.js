import React from "react";
import PropTypes from "prop-types";
import { Edit, Delete } from "@mui/icons-material"

const ProductCardForm = (props) => {

    const requiredNames = props.schema.required ? props.schema.required : [];

    const generateElementsFromSchema = (schema) => {
        if (!schema.properties)
            return [];

        const objects = [];
        const elementDict = {};

        Object.entries(schema.properties).forEach(([p, e]) => {
            const newElement = {};
            const elementDetails  = e && typeof e === "object" ? (
                {
                    ...e,
                    $ref: ""
                }
            ) : {}


            newElement.name = p;
            newElement.required = requiredNames.includes(p);
            newElement.dataOptions = elementDetails;

            elementDict[newElement.name] = newElement;
        })

        Object.keys(elementDict).forEach((name) => {
            objects.push(elementDict[name]);
        });

        return objects;
    }

    const Card = (props) => {
        return (
            <div className="card p-4 mb-2 is-flex is-justify-content-space-between">
                {props.title}
                <div>
                    <Edit /> <Delete />
                </div>
            </div>
        )
    }

    const generateElementComponentsFromSchema = (schema) => {
        if (!schema.properties)
            return [];

        const elementsAttributes = generateElementsFromSchema(schema);

        const elementList = elementsAttributes.map((elementProp, index) => {
            return (
                <Card 
                    name={elementProp.name}
                    title={elementProp.dataOptions.title}
                    type={elementProp.dataOptions.type}
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
                    {props.schema && generateElementComponentsFromSchema(props.schema)}
                </div>
            </div>
        </section>
    )
}

ProductCardForm.propTypes = {
    title: PropTypes.string.isRequired,
    schema: PropTypes.string.isRequired
}

export default ProductCardForm;