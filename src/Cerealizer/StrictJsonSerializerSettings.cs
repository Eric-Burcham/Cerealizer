namespace Cerealizer
{
    using Newtonsoft.Json;

    public class StrictJsonSerializerSettings : JsonSerializerSettings
    {
        public StrictJsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error;
        }

        public MissingTypeMemberHandling MissingTypeMemberHandling { get; set; } = MissingTypeMemberHandling.Error;
    }
}
