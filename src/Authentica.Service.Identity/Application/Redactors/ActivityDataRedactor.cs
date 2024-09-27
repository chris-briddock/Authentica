using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using Application.Attributes;

namespace Application.Redactors;

/// <summary>
/// Provides functionality to redact sensitive data from objects.
/// </summary>
public static class ActivityDataRedactor
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> CachedProperties = new();

    /// <summary>
    /// Redacts sensitive data from the given object.
    /// </summary>
    /// <typeparam name="T">The type of the object to redact.</typeparam>
    /// <param name="obj">The object to redact.</param>
    /// <returns>The redacted object.</returns>
    public static T RedactSensitiveData<T>(T obj) where T : class
    {
        var visitedObjects = new HashSet<object>(new ReferenceEqualityComparer());
        return (T)RedactObject(obj, visitedObjects);
    }

    /// <summary>
    /// Recursively redacts sensitive data from an object.
    /// </summary>
    /// <param name="obj">The object to redact.</param>
    /// <param name="visitedObjects">A set of already visited objects to prevent infinite recursion.</param>
    /// <returns>The redacted object.</returns>
    private static object RedactObject(object obj, HashSet<object> visitedObjects)
    {
        if (obj is null || visitedObjects.Contains(obj))
        {
            return obj!;
        }

        visitedObjects.Add(obj);

        var type = obj.GetType();

        if (type.IsPrimitive || type == typeof(string))
        {
            return obj;
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
        {
            return RedactCollection((IEnumerable)obj, visitedObjects);
        }

        var properties = CachedProperties.GetOrAdd(type, t => t
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToArray());

        foreach (var property in properties)
        {
            if (property.GetCustomAttributes(typeof(SensitiveDataAttribute), true).Any())
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, "REDACTED");
                }
            }
            else
            {
                var value = property.GetValue(obj)!;
                var redactedValue = RedactObject(value, visitedObjects);
                property.SetValue(obj, redactedValue);
            }
        }

        return obj;
    }

    /// <summary>
    /// Redacts sensitive data from a collection.
    /// </summary>
    /// <param name="collection">The collection to redact.</param>
    /// <param name="visitedObjects">A set of already visited objects to prevent infinite recursion.</param>
    /// <returns>A new collection with redacted items.</returns>
    private static object RedactCollection(IEnumerable collection, HashSet<object> visitedObjects)
    {
        var type = collection.GetType();
        var elementType = type.IsGenericType ?
            type.GetGenericArguments()[0] :
            type.GetElementType();

        if (elementType == null)
        {
            return collection;
        }

        var listType = typeof(List<>).MakeGenericType(elementType);
        var newList = (IList)Activator.CreateInstance(listType)!;

        foreach (var item in collection)
        {
            var redactedItem = RedactObject(item, visitedObjects);
            newList.Add(redactedItem);
        }

        return newList;
    }
    /// <summary>
    /// Compares objects by reference equality.
    /// </summary>
    public class ReferenceEqualityComparer : IEqualityComparer<object>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public new bool Equals(object? x, object? y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}