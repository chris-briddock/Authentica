using Microsoft.AspNetCore.Identity;

namespace Application.Results;

/// <summary>
/// Represents the result of an operation, indicating success or failure and any associated errors.
/// </summary>
public class BaseResult<TResult>
where TResult : BaseResult<TResult>, new()
{
    /// <summary>
    /// A static readonly instance representing a successful operation.
    /// </summary>
    private static readonly TResult _success = new() { Succeeded = true };

    /// <summary>
    /// A readonly list of errors associated with the result of an operation.
    /// </summary>
    private readonly List<IdentityError> _errors = [];

    /// <summary>
    /// Gets a static instance representing a successful operation.
    /// </summary>
    public static TResult Success => _success;

    /// <summary>
    /// Gets an enumeration of errors associated with the result of an operation.
    /// </summary>
    public IEnumerable<IdentityError> Errors => _errors;

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; protected set; }

    /// <summary>
    /// Creates a new result indicating a failed operation with the specified errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="IdentityError"/> objects representing the errors.</param>
    /// <returns>A new result indicating a failed operation.</returns>
    public static TResult Failed(params IdentityError[] errors)
    {
        var result = new TResult { Succeeded = false };
        if (errors != null)
        {
            result._errors.AddRange(errors);
        }
        return result;
    }
}