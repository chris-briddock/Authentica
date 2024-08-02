namespace Domain.ValueObjects;

/// <summary>
/// Represents an address value object.
/// </summary>
public class Address
{
    /// <summary>
    /// Gets or sets the name of the building, if there is one.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the door number, if there is one.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Gets the street address.
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// Gets the city.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets the state or province.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Gets the post code.
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    /// Gets the country.
    /// </summary>
    public string? Country { get; set; }

     /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    public Address()
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class.
    /// </summary>
    /// <param name="street">The street address.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state or province.</param>
    /// <param name="postCode">The postal code.</param>
    /// <param name="country">The country.</param>
    public Address(
        string? street,
        string? city,
        string? state,
        string? postCode,
        string? country)
    {
        Street = street;
        City = city;
        State = state;
        PostCode = postCode;
        Country = country;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> class with building name and door number.
    /// </summary>
    /// <param name="street">The street address.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state or province.</param>
    /// <param name="postalCode">The postal code.</param>
    /// <param name="country">The country.</param>
    /// <param name="name">The name of the building.</param>
    /// <param name="number">The door number.</param>
    public Address(
        string? street,
        string? city,
        string? state,
        string? postalCode,
        string? country,
        string? name,
        string? number)
        : this(street, city, state, postalCode, country)
    {
        Name = name;
        Number = number;
    }
}