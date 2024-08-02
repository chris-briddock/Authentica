# Authentica

Secure your app with Authentica, featuring OAuth 2.0 authorization code grant, client credentials grant, refresh token grant.

![Azure DevOps build](https://img.shields.io/azure-devops/build/chris1997/91f2d938-549b-497e-980d-188da969448a/7)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/chris1997/ChristopherBriddock.Identity/7)

## Table of Contents

* [Introduction](#introduction)
* [Planned Improvements](#planned-improvements)
* [User Functionality](#user-functionality)
* [Technical Functionality](#technical-functionality)
* [Endpoints](#endpoints)
* [Getting Started](#getting-started)
* [License](#license)

### Introduction

Authentica plays a crucial role in your application's security infrastructure, providing essential features for user authentication, authorization, and identity management. This README provides an overview of the service, available endpoints, and instructions on getting started.

### Planned Improvements

* **Multiple Tenants**
* **FIDO2 Support**
* **Application based 2FA Codes**

Authorization Flows:

* **Device Authorization Flow (Device Grant)**

### User Functionality

| Feature                         | Description                                                                                   |
| ------------------------------- | --------------------------------------------------------------------------------------------- |
| **Application Authorization**  | Secure access to applications using OAuth 2.0.                                              |
| **Create Application**         | Register new applications within the system.                                                |
| **Delete Application**         | Remove applications from the system.                                                         |
| **Read All Applications**      | Retrieve a list of all applications associated with your user account.                      |
| **Read Application by Name**   | Fetch details of a specific application by its name.                                         |
| **Update Application Details** | Modify the properties and settings of an existing application.                               |
| **Confirm Email**              | Verify and confirm your email address.                                                       |
| **Delete Account**             | Permanently remove your user account from the system.                                        |
| **Logout**                     | Sign out from the application.                                                                |
| **Refresh Bearer Token**       | Obtain a new bearer token to maintain authenticated sessions.                                |
| **Register**                   | Create a new user account.                                                                    |
| **Update Password**            | Change your account password.                                                                 |
| **Reset Password**             | Initiate a password reset process.                                                            |
| **Update Email Address**       | Modify the email address associated with your account.                                       |
| **Two-Step Verification**      | Enable or manage two-step authentication for enhanced security.                              |
| **Two-Factor Recovery Codes**  | Generate and manage recovery codes for two-factor authentication.                            |
| **Account Lockout**            | Temporary lockout of the account after 3 failed login attempts for 10 minutes.               |

### Technical Functionality

| Feature                                   | Description                                                                                     |
| ----------------------------------------- | ----------------------------------------------------------------------------------------------- |
| **RabbitMQ and Azure Service Bus Support** | Integration with RabbitMQ and Azure Service Bus for messaging.                                |
| **API Health Checks**                    | Regular health checks to monitor the API's status.                                             |
| **Secure JWT Bearer Authentication**      | Implementation of secure JWT bearer tokens for authentication.                                 |
| **Optional Azure Application Insights**  | Optional monitoring and diagnostics using Azure Application Insights.                         |
| **Logging, Tracing, and Metrics**        | Advanced logging, tracing, and metrics with .NET Aspire.                                      |
| **Resilient SQL Server Connections**      | Reliable connections to Microsoft SQL Server using Entity Framework Core.                      |
| **Encrypted User Passwords**             | Protection of user passwords through encryption.                                               |
| **HTTP/3 Support**                       | Utilizes HTTP/3 protocol with fallback to HTTP/2 and HTTP/1.1.                                 |

### Endpoints

* **/oauth2/authorize**: Endpoint for user authorization.
* **/oauth2/token**: Endpoint for obtaining OAuth tokens.
* **/users/logout**: Endpoint for user logout.
* **/users/login**: Endpoint for user login.
* **/users**: Endpoint for reading a user by email.
* **/users/register**: Endpoint for user registration.
* **/users/delete**: Endpoint for deleting a user by email.
* **/users/confirm-email**: Endpoint for confirming user email.
* **/users/reset-password**: Endpoint for resetting user passwords.
* **/users/2fa/login**: Endpoint for logging in with two-factor authentication.
* **/users/2fa/manage**: Endpoint for managing two-factor authentication settings.
* **/users/2fa/recovery/codes**: Endpoint for generating two-factor recovery codes.
* **/users/2fa/recovery**: Endpoint for redeeming two-factor recovery codes.
* **/users/details/email**: Endpoint for updating a user's email.
* **/users/details/number**: Endpoint for updating a user's phone number.
* **/users/details/address**: Endpoint for updating a user's address.
* **/users/tokens**: Endpoint for managing user tokens.
* **/applications**: Endpoint for reading an application by name.
* **/applications/all**: Endpoint for reading all applications.
* **/applications**: Endpoint for creating a new application.
* **/applications**: Endpoint for updating an application by name.
* **/applications**: Endpoint for deleting an application by name.
* **/applications/secrets**: Endpoint for managing application secrets.

### Getting Started

To get started with Authentica, follow these steps:

1. Clone the repository: `git clone https://github.com/chris-briddock/ChristopherBriddock.Identity.git`
2. Open the solution.
3. Ensure you have added migrations for the project with Entity Framework Core.
4. Replace all required values in appsettings.development.json. SQL Server, Redis and RabbitMQ will be configured already.
5. Using the docker compose file, will start a local SQL Server container, RabbitMQ, Redis and a .NET Aspire Standalone Dashboard.
6. Start hacking, enjoy!

### License

This project is licensed under the ![GitHub License](https://img.shields.io/github/license/chris-briddock/ChristopherBriddock.Identity)
See the [LICENSE](LICENSE) file for details
