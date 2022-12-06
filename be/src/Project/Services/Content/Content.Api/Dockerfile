FROM node:18-alpine

RUN apk update && apk add  build-base gcc autoconf automake zlib-dev libpng-dev nasm bash vips-dev

WORKDIR /content

COPY . .

RUN npm install --platform=linuxmusl --arch=x64 sharp

RUN yarn install

ARG DATABASE_CLIENT
ARG DATABASE_NAME
ARG DATABASE_HOST
ARG DATABASE_PORT
ARG DATABASE_USERNAME
ARG DATABASE_PASSWORD
ARG DATABASE_SSL

ENV NODE_ENV production

EXPOSE 1337

RUN npm run build

CMD ["npm", "run", "start"]
