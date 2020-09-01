import { toast } from "react-toastify";
import FetchErrorHandler from "../../../../../../shared/helpers/errorHandlers/FetchErrorHandler";

export default class ClientDetailService {

    static Save(url, client, generalErrorMessage, dispatch) {

        toast.configure();

        dispatch({ type: "SET_IS_LOADING", payload: true });

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(client)
        };

        return fetch(url, requestOptions)
            .then(function (response) {

                dispatch({ type: "SET_IS_LOADING", payload: false });

                FetchErrorHandler.handleUnauthorizedResponse(response);

                return response.json().then(jsonResponse => {

                    if (response.ok) {
                        toast.success(jsonResponse.message);
                    }
                    else {
                        FetchErrorHandler.consoleLogResponseDetails(client, response, jsonResponse);
                        toast.error(jsonResponse.message);
                    }
                });
            }).catch(error => {

                console.log(error);
                dispatch({ type: "SET_IS_LOADING", payload: false });
                toast.error(generalErrorMessage);
            });
    }
}
