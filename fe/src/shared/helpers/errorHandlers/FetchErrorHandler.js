import ResponseStatusConstants from "../../constants/ResponseStatusConstants";

export default class FetchErrorHandler {

    static handleError(response) {

        if (!response.ok) {

            if (response.status == ResponseStatusConstants.Unauthorized())
            {
                if (typeof window !== 'undefined') {

                    window.location.reload();
                }
            }

            throw new Error(response.status);
        }
    }
}