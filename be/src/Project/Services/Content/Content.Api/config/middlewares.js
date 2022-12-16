module.exports = ({ env }) => [
  'strapi::errors',
  {
    name: "strapi::security",
    config: {
      contentSecurityPolicy: {
        useDefaults: true,
        directives: {
          'img-src': ["'self'", 'data:', 'blob:', `${env("GIURU_MEDIA_API_URL")}`, `${env("GIURU_CONTENT_API_URL")}`],
        }
      }
    }
  },
  'strapi::cors',
  'strapi::poweredBy',
  'strapi::cors',
  'strapi::logger',
  'strapi::query',
  'strapi::body',
  'strapi::session',
  'strapi::favicon',
  'strapi::public'
];
