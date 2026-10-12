/**
 * The query of a catalog delete request. A catalog identifies a row by its id alone unless the page supplies its own
 * parameters, which lets a page delete against more than the id (for example the version the row was loaded with).
 */
const getDeleteParameters = (entity, getCustomDeleteParameters) => {
    return typeof getCustomDeleteParameters === "function"
        ? getCustomDeleteParameters(entity)
        : { id: entity.id };
};

export default {
    getDeleteParameters
};
