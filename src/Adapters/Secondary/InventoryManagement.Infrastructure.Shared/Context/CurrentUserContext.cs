using InventoryManagement.Domain.Ports.Context;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Infrastructure.Shared.Context;

public sealed class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    private static readonly Guid DefaultUserId = Guid.NewGuid();
    private const string DefaultEmail = "system@inventory.local";

    public Guid UserId
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
                return DefaultUserId;

            var rawUserId = httpContext.Request.Headers["X-User-Id"].FirstOrDefault();

            if (Guid.TryParse(rawUserId, out var userId))
                return userId;

            return DefaultUserId;
        }
    }

    public string Email
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext is null)
                return DefaultEmail;

            var email = httpContext.Request.Headers["X-User-Email"].FirstOrDefault();

            return string.IsNullOrWhiteSpace(email)
                ? DefaultEmail
                : email.Trim();
        }
    }
}