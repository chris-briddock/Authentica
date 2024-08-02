namespace Application.Attributes;

/// <summary>
/// Marks a property as sensitive data.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SensitiveDataAttribute : Attribute
{
}
