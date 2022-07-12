namespace Cerealizer
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class StrictJsonSerializer : JsonSerializer
    {
        private MissingTypeMemberHandling _missingTypeMemberHandling;

        /// <summary>
        ///     Gets or sets how missing members (e.g. JSON contains a property that isn't a member on the object) are handled
        ///     during deserialization.
        ///     The default value is <see cref="MissingTypeMemberHandling.Ignore" />.
        /// </summary>
        public virtual MissingTypeMemberHandling MissingTypeMemberHandling
        {
            get => _missingTypeMemberHandling;
            set
            {
                if (value < MissingTypeMemberHandling.Ignore || value > MissingTypeMemberHandling.Error)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _missingTypeMemberHandling = value;
            }
        }

        public static StrictJsonSerializer Create(StrictJsonSerializerSettings settings)
        {
            var serializer = Create();

            if (settings != null)
            {
                ApplySerializerSettings(serializer, settings);
            }

            return serializer;
        }

        public new static StrictJsonSerializer Create()
        {
            return new StrictJsonSerializer();
        }

        public static StrictJsonSerializer CreateDefault()
        {
            // copy static to local variable to avoid concurrency issues
            var defaultSettings = JsonConvertStrict.DefaultSettings?.Invoke();

            return Create(defaultSettings);
        }

        public static StrictJsonSerializer CreateDefault(StrictJsonSerializerSettings settings)
        {
            var serializer = CreateDefault();
            if (settings != null)
            {
                ApplySerializerSettings(serializer, settings);
            }

            return serializer;
        }

        public new object? Deserialize(JsonReader reader, Type? objectType)
        {
            if (_missingTypeMemberHandling == MissingTypeMemberHandling.Error)
            {
                ValidateJsonPayloadMembers(reader, objectType);
            }

            return base.Deserialize(reader, objectType);
        }

        private static void ApplySerializerSettings(StrictJsonSerializer serializer, StrictJsonSerializerSettings settings)
        {
            if (!settings.Converters.IsNullOrEmpty())

                // insert settings converters at the beginning so they take precedence
                // if user wants to remove one of the default converters they will have to do it manually
            {
                for (var i = 0; i < settings.Converters.Count; i++)
                {
                    serializer.Converters.Insert(i, settings.Converters[i]);
                }
            }

            serializer.MissingTypeMemberHandling = settings.MissingTypeMemberHandling;
            serializer.TypeNameHandling = settings.TypeNameHandling;
            serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
            serializer.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
            serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
            serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            serializer.MissingMemberHandling = settings.MissingMemberHandling;
            serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
            serializer.NullValueHandling = settings.NullValueHandling;
            serializer.DefaultValueHandling = settings.DefaultValueHandling;
            serializer.ConstructorHandling = settings.ConstructorHandling;
            serializer.Context = settings.Context;
            serializer.CheckAdditionalContent = settings.CheckAdditionalContent;
            serializer.Error += settings.Error;
            serializer.ContractResolver = settings.ContractResolver!;
            if (settings.ReferenceResolverProvider != null)
            {
                serializer.ReferenceResolver = settings.ReferenceResolverProvider();
            }

            serializer.TraceWriter = settings.TraceWriter;
            serializer.EqualityComparer = settings.EqualityComparer;
            if (settings.SerializationBinder != null)
            {
                serializer.SerializationBinder = settings.SerializationBinder;
            }

            serializer.Formatting = settings.Formatting;
            serializer.DateFormatHandling = settings.DateFormatHandling;
            serializer.DateTimeZoneHandling = settings.DateTimeZoneHandling;
            serializer.DateParseHandling = settings.DateParseHandling;
            serializer.DateFormatString = settings.DateFormatString;
            serializer.FloatFormatHandling = settings.FloatFormatHandling;
            serializer.FloatParseHandling = settings.FloatParseHandling;
            serializer.StringEscapeHandling = settings.StringEscapeHandling;
            serializer.Culture = settings.Culture;
            serializer.MaxDepth = settings.MaxDepth;
        }

        private static JsonObjectContract GetContract(Type type)
        {
            return (JsonObjectContract)new DefaultContractResolver().ResolveContract(type);
        }

        private static JsonObjectContract? GetContractSafe(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            return GetContract(type);
        }

        private static void ValidateJsonPayloadMembers(JsonReader reader, Type? objectType)
        {
            var contract = GetContractSafe(objectType);
            var properties = contract!.Properties.ToList();
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
            }
            while (reader.Read() && properties.Any());

            if (properties.Any())
            {
                throw new JsonSerializationException("Could not find property 'MyPrice' on object of type 'ProductLong' in JSON payload.");
            }
        }
    }
}
