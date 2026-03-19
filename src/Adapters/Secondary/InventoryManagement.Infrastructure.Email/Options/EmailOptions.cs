using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Infrastructure.Email.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Integrations:Email";

    [Required]
    public string BaseUrl { get; init; } = "http://127.0.0.1:65535/";

    [Range(1, 300)]
    public int TimeoutSeconds { get; init; } = 3;

    [Required]
    public string SenderAddress { get; init; } = "noreply@inventory.local";
}