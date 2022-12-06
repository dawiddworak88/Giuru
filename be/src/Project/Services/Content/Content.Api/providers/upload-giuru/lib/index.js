const FormData = require('form-data');
const fetch = require("node-fetch");

const accessToken = ({ 
  appSecret, 
  email, 
  organisationId,
  tokenApiUrl
}) => {
  return new Promise((resolve, reject) => {
    const requestPayload = {
      email,
      appSecret,
      organisationId
    }

    const requestOptions = {
      method: "POST",
      headers: { 
        "Content-Type": "application/json" 
      },
      body: JSON.stringify(requestPayload)
    }

    fetch(`${tokenApiUrl}`, requestOptions)
      .then((response) => {
        return response.text().then((token) => {
          if (response.ok) {
            resolve(token)
          }
        });
      })
  });
}

module.exports = {
    provider: "storage",
    name: "giuru",

    init(providerOptions) {
      return {
        async upload(file) {
          const token = await accessToken({
            appSecret: providerOptions.appSecret,
            organisationId: providerOptions.appOrganisationId,
            email: providerOptions.appEmail,
            tokenApiUrl: providerOptions.identityTokenApiUrl
          });
          
          return new Promise((resolve, reject) => {

            const formData = new FormData();

            formData.append("file", file.buffer, {
              filename: file.name
            });

            const requestOptions = {
              method: "POST",
              headers: {
                "Authorization": `Bearer ${token}`
              },
              body: formData
            }

            fetch(`${providerOptions.mediaApiUrl}/api/v1/files`, requestOptions)
              .then((response) => {

                return response.json().then((media) => {
                  if (response.ok) {
                    const filePath = `${providerOptions.mediaApiUrl}/api/v1/files/${media.id}`;

                    file.url = filePath;
                    file.path = filePath;

                    resolve();
                  }
                });
              });
          });
        },
        delete(file) {
          // delete the file in the provider
        },
      };
    },
};