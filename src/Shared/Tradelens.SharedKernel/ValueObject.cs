namespace Tradelens.SharedKernel;

public abstract record ValueObject;


// /// <summary>
// /// https://github.com/dotnet/dotnet/blob/main/src/runtime/src/libraries/System.Private.CoreLib/src/System/Object.cs
// /// 
// /// Value objects not only define data that do not contain a unique identity, but also help us realize the domain better.
// /// A value object models a concept where the value itself defines what it is. Perfect examples of this within the
// /// CompanyFundamentals domain are the CentralIndexKey, LegalEntityIdentifier, CountryCode (ISO 3166), etc. For instance,
// /// we're interested that two legal business entities that are registered in the United States have the same CountryCode
// /// of "US", and therefore the two CountryCode objects are structurally equal.
// /// 
// /// Structs and records have this functionality out of the box, but they have their drawbacks. A CountryCode isn't just
// /// an object that happens to be/contain a string, but we can really think of how it fits in our domain and why we
// /// define it as a value object in the first place. We centralize and enforce business invariants such as, its length must
// /// equal to 2, adhere to the ISO 3166-1 alpha-2, must be uppercase, etc., so we don't find this logic scattered elsewhere
// /// in the application. In order to do this as well as control object creation, we will prefer a private constructor and a
// /// factory method. A record also has "nondestructive mutation" for arbitrary parameter value re-assignment such as:
// /// 
// /// var countryCodeUsa = new CountryCode("US");
// /// var invalid = countryCodeUsa with
// /// {
// ///     Value = "banana"
// /// }
// /// 
// /// skipping any invariants and rules defined in the constructor or elsewhere. This can be avoided by establishing best
// /// practices or guidelines within an organization or team, but this language feature allows for it to happen. Ultimately,
// /// and we want to ensure that any domain object or concept that exists in our system at any point is valid.
// ///
// /// Because
// /// </summary>
// public class ValueObject
// {
// }