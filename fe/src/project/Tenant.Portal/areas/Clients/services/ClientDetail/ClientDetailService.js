import { toast } from 'react-toastify';

export default class ClientDetailService {

    static Save(url, client) {

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(client)
        };

        return fetch(url, requestOptions)
            .then(function (response) {

                console.log(response);

                const contentType = response.headers.get("content-type");
                if (contentType && contentType.indexOf("application/json") !== -1) {
                
                    return response.json().then(res => {

                        toast.success("Yes");
                    })
                }                
            }).catch(error => {
                console.log(error);
            });
    }
}
