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
            var defaultSettings = StrictJsonConvert.DefaultSettings?.Invoke();

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
            if (reader is not RewindableJsonTextReader rewindableJsonTextReader)
            {
                throw new InvalidOperationException("The reader passed to this method must be rewindable");
            }

            return Deserialize(rewindableJsonTextReader, objectType);
        }

        public object? Deserialize(RewindableJsonTextReader reader, Type? objectType)
        {
            if (_missingTypeMemberHandling == MissingTypeMemberHandling.Error)
            {
                ValidateJsonPayloadMembers(reader, objectType);
            }

            reader.Rewind();

            return base.Deserialize(reader, objectType);
        }

        internal bool IsCheckAdditionalContentSet()
        {
            var checkAdditionalContent = this.GetPrivateFieldValue<bool?>("_checkAdditionalContent");

            return checkAdditionalContent != null;
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

            if (settings.ContractResolver != null)
            {
                serializer.ContractResolver = settings.ContractResolver;
            }

            if (settings.HasConstructorHandlingValue)
            {
                serializer.ConstructorHandling = settings.ConstructorHandling;
            }

            if (settings.HasCheckAdditionalContentValue)
            {
                serializer.CheckAdditionalContent = settings.CheckAdditionalContent;
            }

            if (settings.HasContextValue)
            {
                serializer.Context = settings.Context;
            }

            if (settings.HasCultureValue)
            {
                serializer.Culture = settings.Culture;
            }

            if (settings.HasDateFormatHandlingValue)
            {
                serializer.DateFormatHandling = settings.DateFormatHandling;
            }

            if (settings.HasDateFormatStringValue)
            {
                serializer.DateFormatString = settings.DateFormatString;
            }

            if (settings.HasDateParseHandlingValue)
            {
                serializer.DateParseHandling = settings.DateParseHandling;
            }

            if (settings.HasDateTimeZoneHandlingValue)
            {
                serializer.DateTimeZoneHandling = settings.DateTimeZoneHandling;
            }

            if (settings.HasDefaultValueHandlingValue)
            {
                serializer.DefaultValueHandling = settings.DefaultValueHandling;
            }

            if (settings.EqualityComparer != null)
            {
                serializer.EqualityComparer = settings.EqualityComparer;
            }

            if (settings.SerializationBinder != null)
            {
                serializer.SerializationBinder = settings.SerializationBinder;
            }

            if (settings.Error != null)
            {
                serializer.Error += settings.Error;
            }

            if (settings.HasFloatFormatHandlingValue)
            {
                serializer.FloatFormatHandling = settings.FloatFormatHandling;
            }

            if (settings.HasFloatParseHandlingValue)
            {
                serializer.FloatParseHandling = settings.FloatParseHandling;
            }

            if (settings.HasFormattingValue)
            {
                serializer.Formatting = settings.Formatting;
            }

            if (settings.HasMaxDepthValue)
            {
                serializer.MaxDepth = settings.MaxDepth;
            }

            if (settings.HasMetadataPropertyHandlingValue)
            {
                serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
            }

            if (settings.HasMissingMemberHandlingValue)
            {
                serializer.MissingMemberHandling = settings.MissingMemberHandling;
            }

            serializer.MissingTypeMemberHandling = settings.MissingTypeMemberHandling;
            if (settings.HasNullValueHandlingValue)
            {
                serializer.NullValueHandling = settings.NullValueHandling;
            }

            if (settings.HasObjectCreationHandlingValue)
            {
                serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
            }

            if (settings.HasPreserveReferencesHandlingValue)
            {
                serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
            }

            if (settings.HasReferenceLoopHandlingValue)
            {
                serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            }

            if (settings.ReferenceResolverProvider != null)
            {
                serializer.ReferenceResolver = settings.ReferenceResolverProvider();
            }

            if (settings.SerializationBinder != null)
            {
                serializer.SerializationBinder = settings.SerializationBinder;
            }

            if (settings.HasStringEscapeHandlingValue)
            {
                serializer.StringEscapeHandling = settings.StringEscapeHandling;
            }

            if (settings.TraceWriter != null)
            {
                serializer.TraceWriter = settings.TraceWriter;
            }

            if (settings.HasTypeNameAssemblyFormatHandlingValue)
            {
                serializer.TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
            }

            if (settings.HasTypeNameHandlingValue)
            {
                serializer.TypeNameHandling = settings.TypeNameHandling;
            }
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
                var propertyNames = string.Join(",", properties.OrderBy(x => x.PropertyName).Select(x => x.PropertyName));

                throw new JsonSerializationException($"Could not find these properties on object of type '{objectType.FullName}' in JSON payload:  {propertyNames}.");
            }
        }
    }
}
