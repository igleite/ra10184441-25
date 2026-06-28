using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BuildingBlocks.Infrastructure.Serialization;

public static class JsonSerialization
{
    public static readonly JsonSerializerSettings Default = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Include,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
}