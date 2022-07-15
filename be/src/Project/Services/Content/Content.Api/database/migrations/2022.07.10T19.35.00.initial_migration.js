'use strict'

async function up(knex) {

    const tables = await knex('pg_catalog.pg_tables')
    .select('tablename')
    .where({schemaname:'public'});

    console.log(tables);

    const homePages = await knex('home_pages').select();

    console.log(homePages);

    const homePagesComponents = await knex('home_pages_components').select();

    console.log(homePagesComponents);

    const hpll = await knex('home_pages_localizations_links').select();

    console.log(hpll);

    const hpvl = await knex('home_pages_localizations_links').select();

    console.log(hpvl);
}

async function down(knex) {}

module.exports = { up, down };