# Authentica

Secure your app with Authentica, featuring OAuth 2.0 authorization code grant, client credentials grant, device code grant, refresh token grant.

[Technical Documentation](https://chris-briddock.github.io/Authentica/api/Api.Constants.html)

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
* **Passkeys Support (FIDO2, Fingerprint, FaceID)**
* **External OAuth2.0 Logins (Microsoft, Google)**

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
| **Hashed User Passwords**             | Protection of user passwords through irreversible hashing, using Argon2.                                               |
| **Hashed Application Secrets**             | Protection of application secrets through irreversible hashing, using Argon2.                                               |
| **HTTP/3 Support**                       | Utilizes HTTP/3 protocol with fallback to HTTP/2 and HTTP/1 1.                                 |
| **Soft delete entries**                       | Utilizes soft deletions to ensure no data is permanently lost.                                 |
| **Auditable Entries** | To enable auditable entries, I have used CreatedBy, CreatedOn, ModifiedOn, and ModifiedBy columns. These columns help track who created and modified each record and when the changes occurred, ensuring comprehensive audit trails for data changes. |
| **Data purge after 7 years, if soft deleted**                       | Data purges happen once an entry is over 7 years old. These are implemented as Background Services and run on a separate thread, using a Periodic Timer.                       |
| **Temporal tables**                       | When an entry in a table changes, we can tell when that changed, but also have a record of what changed, through these system versioned history tables                                 |
| **Worker Services**                       | Utilizes loosely coupled worker services so this is independently scalable from the main service                                 |
| **Security Features** | Implementations of security features such as concurrency stamps, security stamps, last logged in IP address, and last logged in date/time ensure security by providing mechanisms to detect and prevent unauthorized access, track user activity, and maintain data integrity. |
| **Activity Logging** | Utilized Activities to log every user action, ensuring the system state is rebuildable and replayable. Collected information includes the action taken, the request payload, and the timestamp. By implementing session middleware that generates a GUID as a Sequence ID, it is possible to track all actions a user takes within their session. |
| **Integration Testing** | Utilized NUnit and Moq for integration testing to ensure system behavior is as expected by testing the interaction between various components in a production-like environment. This approach helps in identifying issues that may not be apparent in unit tests and ensures that the system works correctly as a whole. |
| **Unit Testing** | Utilized NUnit and Moq for unit testing to ensure individual components function correctly by isolating each part of the code and testing it independently. This approach helps in identifying bugs early in the development process, verifying logic, and ensuring code reliability and maintainability. |

### Endpoints

All endpoints are prefixed with `/api/v{version}/` where `{version}` is the API version.

#### OAuth2 Endpoints

* **GET /oauth2/authorize**: User authorization for OAuth 2.0 flows.
* **POST /oauth2/token**: Obtain OAuth tokens.
* **GET /oauth2/device**: Create tokens for device code flow.

#### User Endpoints

* **POST /users/logout**: Logout a user.
* **POST /users/login**: Login a user.
* **GET /users**: Retrieve user details by email (for the authenticated user).
* **POST /users/register**: Register a new user.
* **DELETE /users/delete**: Delete a user by email.
* **POST /users/confirm-email**: Confirm a user's email address.
* **POST /users/reset-password**: Reset a user's password.

#### Code Endpoints

* **POST /users/codes/mfa**: Send a multi-factor authentication token via email.
* **POST /users/codes/reset-password**: Send a password reset token via email.
* **POST /users/codes/confirm-email**: Send a email confirmation token via email.
* **POST /users/codes/update-email**: Send a email update token via email.
* **POST /users/codes/update-phonenumber**: Send a phone number update token via email..

#### Multi-Factor Authentication Endpoints

* **POST /users/mfa/login/email**: Login with email-based MFA.
* **POST /users/mfa/login/authenticator**: Login with authenticator app-based MFA.
* **POST /users/mfa/manage**: Enable/disable multi-factor authentication settings.
* **POST /users/mfa/manage/authenticator**: Manage authenticator-based MFA.
* **GET /users/mfa/recovery/codes**: Generate MFA recovery codes.
* **POST /users/mfa/recovery**: Redeem MFA recovery codes.

#### Passkey Authentication Endpoints

* **POST /users/passkeys/attestation/options**: Get passkey attestation options.
* **POST /users/passkeys/attestation**: Register a new passkey.
* **POST /users/passkeys/assertion**: Authenticate with a passkey.
* **POST /users/passkeys/assertion/options**: Get passkey assertion options.

#### User Details Endpoints

* **PUT /users/details/email**: Update a user's email address.
* **PUT /users/details/number**: Update a user's phone number.
* **PUT /users/details/address**: Update a user's address.

#### Application Endpoints

* **GET /applications**: Retrieve application by name.
* **GET /applications/all**: Retrieve all applications.
* **POST /applications**: Create an application.
* **PUT /applications**: Update an application.
* **DELETE /applications**: Delete an application.
* **PUT /applications/secrets**: Manage application secrets.

#### Session Endpoints

* **GET /sessions**: Get all sessions associated with a user.
* **DELETE /sessions**: Delete a session by ID.

#### Admin Endpoints

* **POST /admin/reset-password**: Admin-only endpoint to reset any user's password.
* **POST /admin/mfa/disable**: Admin-only endpoint to disable MFA for any user.
* **POST /admin/register**: Create a new admin user.
* **GET /admin/users**: Get all users in the system.
* **GET /admin/activities**: Get all activity logs in the system.
* **GET /admin/applications**: Get all applications in the system.

#### Admin Role Management Endpoints

* **POST /admin/roles/add**: Add a user to a role.
* **POST /admin/roles/create**: Create a new role.
* **DELETE /admin/roles/delete**: Delete a role.
* **PUT /admin/roles/update**: Update a role.
* **GET /admin/roles**: Get role information.

### Getting Started

To get started with Authentica, follow these steps:

1. Clone the repository: `git clone https://github.com/chris-briddock/Authentica.git`
2. Open the solution.
3. Ensure you have added migrations for the project with Entity Framework Core.
4. Replace all required values in appsettings.development.json. SQL Server, Redis and RabbitMQ will be configured already.
5. Using the docker compose file, will start a local SQL Server container, RabbitMQ, Redis and a .NET Aspire Standalone Dashboard.
6. Start hacking, enjoy!

### License

This project is licensed under the MIT License.
