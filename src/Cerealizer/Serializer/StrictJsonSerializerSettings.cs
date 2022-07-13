namespace Cerealizer
{
    using Newtonsoft.Json;

    public class StrictJsonSerializerSettings : JsonSerializerSettings
    {
        public MissingTypeMemberHandling? MissingTypeMemberHandling { get; set; }
    }
}
