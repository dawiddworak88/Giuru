import NavigationHelper from "./NavigationHelper";
import ResponseStatusConstants from "../../constants/ResponseStatusConstants";

export default class AuthenticationHelper {

    static HandleResponse(response) {

        if (response.status === ResponseStatusConstants.unauthorized())
        {
            NavigationHelper.redirect(response.headers.get("Location"));
        }
    }
}
