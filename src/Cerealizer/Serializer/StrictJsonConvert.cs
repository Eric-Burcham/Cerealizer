namespace Cerealizer
{
    using System;

    using Newtonsoft.Json;

    public static class StrictJsonConvert
    {
        public static Func<StrictJsonSerializerSettings> DefaultSettings { get; set; } = () => new StrictJsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error, MissingTypeMemberHandling = MissingTypeMemberHandling.Error };

        public static object DeserializeObject(string value, Type type, StrictJsonSerializerSettings settings)
        {
            var jsonSerializer = StrictJsonSerializer.CreateDefault(settings);

            if (!jsonSerializer.IsCheckAdditionalContentSet())
            {
                jsonSerializer.CheckAdditionalContent = true;
            }

            using (var reader = new RewindingJsonTextReader(value))
            {
                return jsonSerializer.Deserialize(reader, type);
            }
        }
    }
}
