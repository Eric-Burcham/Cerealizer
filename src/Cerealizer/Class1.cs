using System;
using Newtonsoft.Json;

namespace Cerealizer
{
    public static class JsonConvertStrict
    {
        public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(value, type, settings);
        }
    }
}
