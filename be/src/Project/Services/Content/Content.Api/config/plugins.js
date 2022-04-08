module.exports = ({ env }) => ({
    'migrations': {
        enabled: true,
        config: {
            autoStart: true,
            migrationFolderPath : env("MIGRATION_FOLDER_PATH")
        },
    }
})