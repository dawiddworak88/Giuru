module.exports = ({ env }) => ({
  auth: {
    secret: env('ADMIN_JWT_SECRET', 'f954ceeddf0d400f5152642c61b21987'),
  },
});
