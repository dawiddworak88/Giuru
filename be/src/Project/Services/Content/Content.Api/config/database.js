module.exports = ({ env }) => ({
  connection: {
    client: process.env.DATABASE_CLIENT ?? 'postgres',
    connection: {
      host: env('DATABASE_HOST', process.env.DATABASE_HOST ?? 'host.docker.internal'),
      port: env.int('DATABASE_PORT', process.env.DATABASE_PORT ?? 5438),
      database: env('DATABASE_NAME', process.env.DATABASE_NAME ?? 'postgres'),
      user: env('DATABASE_USERNAME', process.env.DATABASE_USERNAME ?? 'postgres'),
      password: env('DATABASE_PASSWORD', process.env.DATABASE_PASSWORD ?? 'postgres'),
      ssl: env.bool('DATABASE_SSL', process.env.DATABASE_SSL ?? false),
    },
  },
});
