using Newtonsoft.Json;

namespace Cerealizer;

/// <remarks>
///     A "missing Type member" is the situation where the target Type for deserialization has a member that is not present
///     in the JSON payload.
/// </remarks>
/// <summary>
///     Specifies missing Type member handling options for the <see cref="JsonSerializer" />.
/// </summary>
public enum MissingTypeMemberHandling
{
    /// <summary>
    ///     Ignore when a member of the target Type is missing from the JSON payload.
    /// </summary>
    Ignore = 0,

    /// <summary>
    ///     Throw a <see cref="JsonSerializationException" /> when a member of the target Type is missing from the JSON
    ///     payload.
    /// </summary>
    Error = 1
}