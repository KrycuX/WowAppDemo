using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WowApi.Infrastructure.BlizzardApi.ExternalApiModels.Resposne;
public class Heirlooms: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class Key: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class Mounts: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class Pets: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class Toys: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}

public class Transmogs: IBaseResponse
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }
}
public class CharacterCollectionResponse
{
    [JsonPropertyName("pets")]
    public Pets? Pets { get; set; }
    [JsonPropertyName("mounts")]
    public Mounts? Mounts { get; set; }
    [JsonPropertyName("heirlooms")]
    public Heirlooms? Heirlooms { get; set; }
    [JsonPropertyName("toys")]
    public Toys? Toys { get; set; }
    [JsonPropertyName("transmogs")]
    public Transmogs? Transmogs { get; set; }
}



