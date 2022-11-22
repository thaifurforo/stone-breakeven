# Breakeven | .Net Jr Software Engineer @ Stone
This project was developed as a part of my breakeven as a Jr. Software Engineer on Credit's Team at Stone.

The challenge was to implement an Account Management App (WebApi).

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
2. Replace the ``https_port`` and ``http_port`` inside the curly brackets to an available port
```sh
"applicationUrl": "https://localhost:{https_port};http://localhost:{http_port}",
```
3. Replace the variables ``localhost,port``, ``localhost_user`` and ``localhost_password`` inside the curly brackets according to your local environment variables:
```sh
"ConnectionStrings__ReadModelSqlConnection": "Server={localhost,port};Initial Catalog=AccountReadModelDb;Persist Security Info=False;User ID={localhost_user};Password={localhost_password};TrustServerCertificate=True",
"ConnectionStrings__EventStoreSqlConnection": "Server={localhost,port};Initial Catalog=AccountEventStoreDb;Persist Security Info=False;User ID={localhost_user};Password={localhost_password};TrustServerCertificate=True"
```
### Setting up the database
1. Create two new Schemas on your local server: ``AccountEventStoreDb`` and ``AccountReadModelDb``

## Functional tests
[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/55c9b460d2172a927b73?action=collection%2Fimport)

[Or click here to access the Json to run in Postman](https://www.getpostman.com/collections/55c9b460d2172a927b73)

## API Reference
[Click here to access the API Reference Documentation](https://breakeven-thaifurforo.readme.io/)
