module.exports = ({ env }) => ({
    'migrations': {
        enabled: true,
        config: {
            autoStart: true,
            migrationFolderPath : env("MIGRATION_FOLDER_PATH")
        },
    },
    "upload": {
        enabled: true,
        config: {
            provider: "giuru",
            providerOptions: {}
        }
    }
})