using System.Globalization;
using System.Resources;

namespace InventoryManagement.Api.Resources;

public class Messages
{
    private static readonly ResourceManager _resourceManager =
        new("InventoryManagement.Api.Resources.Messages", typeof(Program).Assembly);

    public static ResourceManager ResourceManager => _resourceManager;

    public static string? GetString(string name)
        => _resourceManager.GetString(name, CultureInfo.CurrentUICulture);
}