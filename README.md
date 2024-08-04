# Authentica

Secure your app with Authentica, featuring OAuth 2.0 authorization code grant, client credentials grant, refresh token grant.

Technical documentation:
https://chris-briddock.github.io/Authentica/api/Api.Constants.html

![Azure DevOps build](https://img.shields.io/azure-devops/build/Authentica/c98bacd5-0436-4667-a21d-828fb19fb305/1)
![Azure DevOps Code Coverage](https://img.shields.io/azure-devops/coverage/Authentica/Authentica/1)

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
* **Passkeys Support**
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
| **Optional Azure Application Insights Support**  | Optional monitoring and diagnostics using Azure Application Insights.                         |
| **Logging, Tracing, and Metrics**        | Advanced logging, tracing, and metrics with .NET Aspire.                                      |
| **Resilient SQL Server Connections**      | Reliable connections to Microsoft SQL Server using Entity Framework Core.                      |
| **Encrypted User Passwords**             | Protection of user passwords through irreversable encryption.                                               |
| **Encrypted Application Secrets**             | Protection of application secrets through irreversable encryption.                                               |
| **HTTP/3 Support**                       | Utilizes HTTP/3 protocol with fallback to HTTP/2 and HTTP/1 1.                                 |
| **Soft deletable entries**                       | Utilizes soft deletions to ensure no data is permenantly lost.                                 |
| **Auditable Entries** | To enable auditable entries, I have used CreatedBy, CreatedOn, ModifiedOn, and ModifiedBy columns. These columns help track who created and modified each record and when the changes occurred, ensuring comprehensive audit trails for data changes. |
| **Data purge after 7 years, if soft deleted**                       | Data purges happen once an entry is over 7 years old. These are implemented as Background Services and run on a seperate thread, using a Periodic Timer.                       |
| **Temporal tables**                       | When an entry in a table changes, we can tell when that happend changed, but also have a record of what changed, through these system versioned history tables                                 |
| **Worker Services**                       | Utilizes loosley coupled worker services so this is independently scalable from the main service                                 |
| **Security Features** | Implementations of security features such as concurrency stamps, security stamps, last logged in IP address, and last logged in date/time ensure security by providing mechanisms to detect and prevent unauthorized access, track user activity, and maintain data integrity. |
| **Event Logging** | Utilized Domain Events and Integration Events to log every user action, ensuring the system state is rebuildable and replayable. Collected information includes the action taken, the request payload, and the timestamp. By implementing session middleware that generates a GUID as a Sequence ID, it is possible to track all actions a user takes within their session. |
| **Integration Testing** | Utilized NUnit and Moq for integration testing to ensure system behavior is as expected by testing the interaction between various components in a production-like environment. This approach helps in identifying issues that may not be apparent in unit tests and ensures that the system works correctly as a whole. |
| **Unit Testing** | Utilized NUnit and Moq for unit testing to ensure individual components function correctly by isolating each part of the code and testing it independently. This approach helps in identifying bugs early in the development process, verifying logic, and ensuring code reliability and maintainability. |

### Endpoints

* **/oauth2/authorize**: GET Endpoint for user authorization.
* **/oauth2/token**: POST Endpoint for obtaining OAuth tokens.
* **/users/logout**: POST Endpoint for user logout.
* **/users/login**: POST Endpoint for user login.
* **/users**: GET Endpoint for reading a user by email.
* **/users/register**: POST Endpoint for user registration.
* **/users/delete**: POST Endpoint for deleting a user by email.
* **/users/confirm-email**: POST Endpoint for confirming user email.
* **/users/reset-password**: POST Endpoint for resetting user passwords.
* **/users/2fa/login**: POST Endpoint for logging in with two-factor authentication.
* **/users/2fa/manage**: POST Endpoint for managing two-factor authentication settings.
* **/users/2fa/recovery/codes**: GET Endpoint for generating two-factor recovery codes.
* **/users/2fa/recovery**: POST Endpoint for redeeming two-factor recovery codes.
* **/users/details/email**: PUT Endpoint for updating a user's email.
* **/users/details/number**: PUT Endpoint for updating a user's phone number.
* **/users/details/address**: PUT Endpoint for updating a user's address.
* **/users/tokens**: POST Endpoint for managing user tokens.
* **/applications**: GET Endpoint for reading an application by name.
* **/applications/all**: GET Endpoint for reading all applications.
* **/applications**: POST Endpoint for creating a new application.
* **/applications**: PUT Endpoint for updating an application by name.
* **/applications**: DELETE Endpoint for deleting an application by name.
* **/applications/secrets**: PUT Endpoint for managing application secrets.

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
