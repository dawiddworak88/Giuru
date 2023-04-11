class QuantityValidator {

    static validateQuantity(quantity){
        if (quantity <= -1){
            return false;
        }

        return true;
    }
}

export default QuantityValidator;