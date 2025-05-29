using System.Text.Json.Serialization;

public class FileItem
{
    [JsonPropertyName("name")] public string Name { get; set; } = default!;

    [JsonPropertyName("type")] public string Type { get; set; } = default!;

    [JsonPropertyName("size")] public long? Size { get; set; }
}