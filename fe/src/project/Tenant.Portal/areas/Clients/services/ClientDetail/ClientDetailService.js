export default class ClientDetailService {

    static Save(url, client) {

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(client)
        };

        return fetch(url, requestOptions)
            .then(function (response) {

                return response.json().then(res => {

                    console.log(response);
                    console.log(res);
                })
            }).catch(error => {
                console.log(error);
            });
    }
}
