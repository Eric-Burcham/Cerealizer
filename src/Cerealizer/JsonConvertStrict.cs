namespace Cerealizer
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    public static class JsonConvertStrict
    {
        public static Func<StrictJsonSerializerSettings>? DefaultSettings { get; set; }

        public static object DeserializeObject(string value, Type type, StrictJsonSerializerSettings settings)
        {
            var jsonSerializer = StrictJsonSerializer.CreateDefault(settings);

            using (var reader = new JsonTextReader(new StringReader(value)))
            {
                return jsonSerializer.Deserialize(reader, type);
            }
        }
    }
}
