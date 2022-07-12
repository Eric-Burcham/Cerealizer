namespace Cerealizer
{
    using System;

    public static class StrictJsonConvert
    {
        public static Func<StrictJsonSerializerSettings>? DefaultSettings { get; set; }

        public static object DeserializeObject(string value, Type type, StrictJsonSerializerSettings settings)
        {
            var jsonSerializer = StrictJsonSerializer.CreateDefault(settings);

            if (!jsonSerializer.IsCheckAdditionalContentSet())
            {
                jsonSerializer.CheckAdditionalContent = true;
            }

            using (var reader = new RewindableJsonTextReader(new RewindableStringReader(value)))
            {
                return jsonSerializer.Deserialize(reader, type);
            }
        }
    }
}
