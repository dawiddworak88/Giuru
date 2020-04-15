import { toast } from 'react-toastify';

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

                if (!response.ok) {
                    
                    throw new Error(response.status);
                }
                else {

                    const contentType = response.headers.get("content-type");
                    if (contentType && contentType.indexOf("application/json") !== -1) {

                        return response.json().then(jsonResponse => {

                            toast.success(jsonResponse.message);
                        })
                    }
                }
            }).catch(error => {

                console.log(error);
                toast.error(generalErrorMessage);
            });
    }
}
