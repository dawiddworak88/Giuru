module.exports = async () => {

    const homePage = await strapi.db.query("api::homePage");

    if (!homePage) {

        await homePage.update({
            where: { id: 1, locale: "en" },
            data: {
            title: "Title"
            }
        });
    }
}