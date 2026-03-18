using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Domain.Errors;

public class IntegrationErrors
{
    public static readonly Error Cancelled = new("INTEGRATION_CANCELLED");

    public static readonly Error WmsInvalidInput = new("WMS_INVALID_INPUT");
    public static readonly Error WmsCreateFailed = new("WMS_CREATE_FAILED");
    public static readonly Error WmsDispatchFailed = new("WMS_DISPATCH_FAILED");

    public static readonly Error AuditLogInvalidInput = new("AUDITLOG_INVALID_INPUT");
    public static readonly Error AuditLogCreateFailed = new("AUDITLOG_CREATE_FAILED");

    public static readonly Error EmailInvalidInput = new("EMAIL_INVALID_INPUT");
    public static readonly Error EmailSendFailed = new("EMAIL_SEND_FAILED");
}
