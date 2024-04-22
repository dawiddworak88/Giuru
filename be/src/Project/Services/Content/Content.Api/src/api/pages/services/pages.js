'use strict';

/**
 * pages service
 */

const { createCoreService } = require('@strapi/strapi').factories;

module.exports = createCoreService('api::pages.pages');
