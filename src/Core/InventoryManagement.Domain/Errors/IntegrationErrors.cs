using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Domain.Errors;

public static class IntegrationErrors
{
    public static readonly Error Cancelled =
        new("integration.cancelled", ErrorType.Failure);

    public static readonly Error WmsInvalidInput =
        new("wms.invalid_input", ErrorType.Validation);

    public static readonly Error WmsCreateFailed =
        new("wms.create_failed", ErrorType.Failure);

    public static readonly Error WmsDispatchFailed =
        new("wms.dispatch_failed", ErrorType.Failure);

    public static readonly Error AuditLogInvalidInput =
        new("auditlog.invalid_input", ErrorType.Validation);

    public static readonly Error AuditLogCreateFailed =
        new("auditlog.create_failed", ErrorType.Failure);

    public static readonly Error EmailInvalidInput =
        new("email.invalid_input", ErrorType.Validation);

    public static readonly Error EmailSendFailed =
        new("email.send_failed", ErrorType.Failure);
}