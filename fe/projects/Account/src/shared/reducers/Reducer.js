const Reducer = (state, action) => {
    switch (action.type) {
        case "SET_IS_LOADING":
            return {
                ...state,
                isLoading: action.payload
            };
        case "SET_TOTAL_BASKET":
            return {
                ...state,
                totalBasketItems: action.payload
            }
        default:
            return state;
    }
};

export default Reducer;
