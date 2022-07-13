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
        ///     The default value is Ignore.
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

        public new static StrictJsonSerializer CreateDefault()
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

        public new object Deserialize(JsonReader reader, Type objectType)
        {
            if (reader is not RewindableJsonTextReader rewindableJsonTextReader)
            {
                throw new InvalidOperationException("The reader passed to this method must be rewindable");
            }

            return Deserialize(rewindableJsonTextReader, objectType);
        }

        public object Deserialize(RewindableJsonTextReader reader, Type objectType)
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
            typeof(StrictJsonSerializer).BaseType.InvokePrivateStaticMethod("ApplySerializerSettings", serializer, settings);

            if (settings.MissingTypeMemberHandling.HasValue)
            {
                serializer.MissingTypeMemberHandling = settings.MissingTypeMemberHandling.Value;
            }
        }

        private static JsonObjectContract GetContract(Type type)
        {
            return (JsonObjectContract)new DefaultContractResolver().ResolveContract(type);
        }

        private static JsonObjectContract GetContractSafe(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return GetContract(type);
        }

        private static void ValidateJsonPayloadMembers(JsonReader reader, Type objectType)
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
