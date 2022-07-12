using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cerealizer
{
    public static class JsonConvertStrict
    {
        public static object DeserializeObject(string value, Type type, StrictJsonSerializerSettings settings)
        {
            if (settings.MissingTypeMemberHandling == MissingTypeMemberHandling.Error)
            {
                ValidateJsonPayloadMembers(value, type);
            }

            return JsonConvert.DeserializeObject(value, type, settings);
        }

        private static void ValidateJsonPayloadMembers(string value, Type type)
        {
            var contract = GetContractSafe(type);
            var properties = contract.Properties.ToList();
            var reader = new JsonTextReader(new StringReader(value));

            do
            {
                if (reader.TokenType != JsonToken.PropertyName)
                {
                    continue;
                }

                var payloadPropertyName = reader.Value!.ToString()!;
                var foundProperty = properties.SingleOrDefault(x => x.PropertyName == payloadPropertyName);
                if (foundProperty != null)
                {
                    properties.Remove(foundProperty);
                }

            } while (reader.Read() && properties.Any());

            if (properties.Any())
            {
                throw new JsonSerializationException(
                    "Could not find property 'MyPrice' on object of type 'ProductLong' in JSON payload.");
            }
        }

        private static JsonObjectContract? GetContractSafe(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            return GetContract(type);
        }

        private static JsonObjectContract GetContract(Type type)
        {
            return (JsonObjectContract)new DefaultContractResolver().ResolveContract(type);
        }
    }

    public class StrictJsonSerializer : JsonSerializer
    {
        
        public new object? Deserialize(JsonReader reader, Type? objectType)
        {
            return base.Deserialize(reader, objectType);
        }
    }

    public class StrictJsonSerializerSettings : JsonSerializerSettings
    {
        public StrictJsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error;
        }

        public MissingTypeMemberHandling MissingTypeMemberHandling { get; set; } = MissingTypeMemberHandling.Error;
    }

    /// <remarks>
    /// A "missing Type member" is the situation where the target Type for deserialization has a member that is not present in the JSON payload.
    /// </remarks>
    /// <summary>
    /// Specifies missing Type member handling options for the <see cref="JsonSerializer"/>.
    /// </summary>
    public enum MissingTypeMemberHandling
    {
        /// <summary>
        /// Ignore when a member of the target Type is missing from the JSON payload.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// Throw a <see cref="JsonSerializationException"/> when a member of the target Type is missing from the JSON payload.
        /// </summary>
        Error = 1
    }

}
