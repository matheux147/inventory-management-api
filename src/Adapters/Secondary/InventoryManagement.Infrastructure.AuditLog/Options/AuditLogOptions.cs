using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Infrastructure.AuditLog.Options;

public sealed class AuditLogOptions
{
    public const string SectionName = "Integrations:AuditLog";

    [Required]
    public string BaseUrl { get; init; } = "http://127.0.0.1:65535/";

    [Range(1, 300)]
    public int TimeoutSeconds { get; init; } = 3;
}