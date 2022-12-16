module.exports = ({ env }) => ({
    "upload": {
        enabled: true,
        config: {
            provider: "giuru",
            providerOptions: {
                appSecret: env("GIURU_API_SECRET"),
                appEmail: env("GIURU_API_EMAIL"),
                appOrganisationId: env("GIURU_API_ORGANISATION_ID"),
                mediaApiUrl: env("GIURU_MEDIA_API_URL"),
                identityTokenApiUrl: env("GIURU_IDENTITY_API_URL")
            }
        }
    }
})