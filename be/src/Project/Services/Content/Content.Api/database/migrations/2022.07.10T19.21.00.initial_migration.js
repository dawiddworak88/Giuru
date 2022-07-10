'use strict'

async function up(knex) {

    const tables = await knex('pg_catalog.pg_tables')
    .select('tablename')
    .where({schemaname:'public'});

    console.log(tables);

    const entries = await knex('home_pages').select();

    console.log(entries);
}

async function down(knex) {}

module.exports = { up, down };