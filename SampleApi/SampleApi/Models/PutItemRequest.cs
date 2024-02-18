using System.ComponentModel.DataAnnotations;

namespace SampleApi.Models;

public record PutItemRequest : PostItemRequest
{
    [Required(ErrorMessage = "The id field is required.")]
    public Guid Id { get; init; }
}