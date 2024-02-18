namespace SampleApi.Models;

public record Item(Guid Id, string Name, string? Description, DateTime CreatedAt, DateTime? UpdatedAt);