// Represents a file or folder in a directory listing returned by the API
// Used by the BrowseController to serialize file metadata

using System.Text.Json.Serialization;

public class FileItem
{
    [JsonPropertyName("name")] public string Name { get; set; } = default!;

    [JsonPropertyName("type")] public string Type { get; set; } = default!;

    [JsonPropertyName("size")] public long? Size { get; set; }
}