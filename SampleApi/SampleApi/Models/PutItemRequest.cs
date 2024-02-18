using System.ComponentModel.DataAnnotations;

namespace SampleApi.Models;

public record PutItemRequest : PostItemRequest
{
    /// <summary>
    /// The id of the item. This must be a valid GUID and must match the id in the route.
    /// </summary>
    [Required(ErrorMessage = "The id field is required.")]
    public Guid Id { get; init; }
}