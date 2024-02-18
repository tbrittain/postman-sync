using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SampleApi.Models;

public record PostItemRequest
{
    [Required(ErrorMessage = "The name field is required.")]
    [MaxLength(100, ErrorMessage = "The name field must not exceed 100 characters.")]
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [MaxLength(500, ErrorMessage = "The description field must not exceed 500 characters.")]
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}