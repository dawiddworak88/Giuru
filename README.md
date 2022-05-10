# Giuru &middot; [![GitHub license](https://img.shields.io/badge/license-GPL-blue.svg)](https://github.com/dawiddworak88/Giuru/blob/master/LICENSE.md) ![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/8ee660cb3f7b42c1aa7ca8d7d8158b85)](https://www.codacy.com/manual/dawiddworak88/Giuru?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=dawiddworak88/Giuru&amp;utm_campaign=Badge_Grade)

Giuru is a user-friendly, fast and complete B2B Commerce that connects with your existing ERP, CRM and CMS systems. 

## Installation

### Prerequisites

* **[.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0):** download and install the latest version
* **[Node 12](https://nodejs.org/en/download/):** download and install the latest Node 12 LTS version
* **[Docker](http://hub.docker.com/):** to run ASP.NET Core web app, Node for SSR and Storybook in containers. Download and install the latest version

### Configuration file

Add the following configuration to `.env` file in the root of your project and fill with your details

```env
COMPOSE_PROJECT_NAME=GIURU
GIURU_EXTERNAL_DNS_NAME_OR_IP=host.docker.internal
SUPPORTED_CULTURES=en,de,pl
DEFAULT_CULTURE=en
ORGANISATION_ID=[PUT_YOUR_ORGANISATION_ID]
ELK_VERSION=7.9.1
SENDER_EMAIL=[PUT_YOUR_APPLICATION_EMAIL]
SENDER_NAME=[PUT_YOUR_EMAIL_SENDER_NAME]
SENDGRID_API_KEY=[PUT_YOUR_SENDGRID_API_KEY_HERE_AND_NEVER_COMMIT]
SENDGRID_CREATE_ACCOUNT_TEMPLATE_ID=[PUT_YOUR_SENDGRID_CREATE_ACCOUNT_TEMPLATE_ID_HERE_AND_NEVER_COMMIT]
SENDGRID_RESET_PASSWORD_TEMPLATE_ID=[PUT_YOUR_SENDGRID_RESET_ACCOUNT_TEMPLATE_ID_HERE_AND_NEVER_COMMIT]
SENDGRID_CUSTOM_ORDER_TEMPLATE_ID=[PUT_YOUR_SENGRID_CUSTOM_ORDER_TEMPLATE_ID_HERE_AND_NEVER_COMMIT]
```

### Quickstart

1. Clone this repository
2. Execute the following commands from the /fe folder to build fe:

    npm install

    npm run build-fe

3. Open Visual Studio, set the docker-compose project as a startup project and hit F5

## Documentation

You will learn more about Giuru [on the website](https://giuru.com).

## Contributing

Found a serious bug? Your pull request is very welcome :)

## Support

To get support [contact me directly](https://spincoding.com/contact/).

### License

Giuru is [GNU GPL licensed](./LICENSE.md).
