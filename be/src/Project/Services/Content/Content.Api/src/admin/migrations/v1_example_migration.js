module.exports = async () => {
    const pluginStore = strapi.store({
        environment: '',
        type: 'plugin',
        name: 'example-migration',
    });

    const values = await pluginStore.get({ key: "advanced" });

    if (values) {
        // Place for set values

        await pluginStore.set({ key: 'advanced', value : values });
    }
}