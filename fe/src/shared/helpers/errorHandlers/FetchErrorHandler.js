import ResponseStatusConstants from "../../constants/ResponseStatusConstants";

export default class FetchErrorHandler {

    static handleUnauthorizedResponse(response) {

        if (!response.ok) {

            if (response.status === ResponseStatusConstants.Unauthorized())
            {
                if (typeof window !== "undefined") {

                    window.location.reload();
                }
            }
        }
    }

    static consoleLogResponseDetails(entity, response, jsonResponse)
    {
        console.log(entity);
        console.log(response);
        console.log(jsonResponse);
    }
}