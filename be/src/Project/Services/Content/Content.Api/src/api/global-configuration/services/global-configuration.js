'use strict';

/**
 * global-configuration service
 */

const { createCoreService } = require('@strapi/strapi').factories;

module.exports = createCoreService('api::global-configuration.global-configuration');
