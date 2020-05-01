import { toast } from 'react-toastify';
import FetchErrorHandler from '../../../../../../shared/helpers/errorHandlers/FetchErrorHandler';

export default class ClientDetailService {

    static Save(url, client, generalErrorMessage, dispatch) {

        toast.configure();

        dispatch({type: 'SET_IS_LOADING', payload: true});

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(client)
        };

        return fetch(url, requestOptions)
            .then(function (response) {

                FetchErrorHandler.handleError(response);

                return response.json().then(jsonResponse => {

                    dispatch({type: 'SET_IS_LOADING', payload: false});
                    toast.success(jsonResponse.message);
                })
            }).catch(error => {

                console.log(error);
                dispatch({type: 'SET_IS_LOADING', payload: false});
                toast.error(generalErrorMessage);
            });
    }
}
