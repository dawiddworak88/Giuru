import { toast } from 'react-toastify';
import FetchErrorHandler from '../../../../../../shared/helpers/errorHandlers/FetchErrorHandler';

toast.configure();

export default class ClientDetailService {

    static Save(url, client, generalErrorMessage) {

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(client)
        };

        return fetch(url, requestOptions)
            .then(function (response) {

                FetchErrorHandler.handleError(response);

                return response.json().then(jsonResponse => {

                    toast.success(jsonResponse.message);
                })
            }).catch(error => {

                console.log(error);
                toast.error(generalErrorMessage);
            });
    }
}
