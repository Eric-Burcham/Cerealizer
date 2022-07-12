using Newtonsoft.Json;

namespace Cerealizer;

public class StrictJsonSerializerSettings : JsonSerializerSettings
{
    public StrictJsonSerializerSettings()
    {
        MissingMemberHandling = MissingMemberHandling.Error;
    }

    public MissingTypeMemberHandling MissingTypeMemberHandling { get; set; } = MissingTypeMemberHandling.Error;
}