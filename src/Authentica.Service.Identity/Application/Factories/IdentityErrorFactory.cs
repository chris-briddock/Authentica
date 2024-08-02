using Microsoft.AspNetCore.Identity;

namespace Application.Factories;

/// <summary>
/// Provides custom error messages for identity operations, 
/// including detailed exception information.
/// </summary>
public class IdentityErrorFactory : IdentityErrorDescriber
{
    /// <summary>
    /// Returns an error message indicating an exception occurred with details.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An <see cref="IdentityError"/> with a detailed error message.</returns>
    public static IdentityError ExceptionOccurred(Exception exception)
    {
        return new IdentityError
        {
            Code = nameof(ExceptionOccurred),
            Description = $"An exception occurred: {exception.Message}. StackTrace: {exception.StackTrace}"
        };
    }

    /// <summary>
    /// Returns an error message indicating that the specified user was not found.
    /// </summary>
    /// <returns>An <see cref="IdentityError"/> indicating that the user was not found.</returns>
    public static IdentityError UserNotFound()
    {
        return new IdentityError
        {
            Code = nameof(UserNotFound),
            Description = $"User was not found."
        };
    }

    /// <summary>
    /// Returns an error message indicating that the specified email was not found.
    /// </summary>
    /// <returns>An <see cref="IdentityError"/> with a message indicating the email was not found.</returns>
    public static IdentityError EmailNotFound()
    {
        return new IdentityError
        {
            Code = nameof(EmailNotFound),
            Description = "No user was found with the supplied email address."
        };
    }

    /// <summary>
    /// Returns an error message indicating the application was not found.
    /// </summary>
    /// <returns>An <see cref="IdentityError"/> indicating application not found.</returns>
    public static IdentityError ApplicationNotFound()
    {
        return new IdentityError
        {
            Code = nameof(ApplicationNotFound),
            Description = "The application was not found."
        };
    }
}
