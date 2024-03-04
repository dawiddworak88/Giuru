'use strict';

/**
 * global-configuration router
 */

const { createCoreRouter } = require('@strapi/strapi').factories;

module.exports = createCoreRouter('api::global-configuration.global-configuration');
