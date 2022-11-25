# Breakeven | .Net Jr Software Engineer @ Stone
This project was developed as a part of my breakeven as a Jr. Software Engineer on Credit's Team at Stone.

The challenge was to implement an Account Management App (WebApi), where it should be possible to create and deactivate accounts and make transactions.

The project uses Hexagonal Architecture and the following patterns: CQRS, Mediator and Repository.

# API Documentation

## How to run locally
### Installation
1. Install .NET Core v6.0+
2. Clone the repo
```sh
git clone git@github.com:thaifurforo/stone-breakeven.git
```
### Configuring local variables
1. Rename the file ``launchSettings_sample.json`` located at ``src/AccountManagementService.Api/Properties/`` to ``launchSettings.json``
2. Replace the following variables:
```sh
"applicationUrl": "{api_iis_url}",
"sslPort": "{sslPort}"
...
"applicationUrl": "{api_url}",
```
3. Add the connection strings to AccountEventStoreDb and AccountReadModelDb into the environment variables:
```sh
"environmentVariables": {
       "ConnectionStrings__ReadModelSqlConnection": "{readmodel_connectionstring}",
       "ConnectionStrings__EventStoreSqlConnection": "{eventstore_connectionstring}"
      }
```
### Setting up the database
1. Create two new Schemas on your local server: ``AccountEventStoreDb`` and ``AccountReadModelDb``

## Postman Collection
[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/55c9b460d2172a927b73?action=collection%2Fimport)

[Or click here to access the Collection Json](https://www.getpostman.com/collections/55c9b460d2172a927b73)

## API Reference
[Click here to access the API Reference Documentation](https://breakeven-thaifurforo.readme.io/)
