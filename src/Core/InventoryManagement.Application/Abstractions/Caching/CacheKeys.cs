namespace InventoryManagement.Application.Abstractions.Caching;

public static class CacheKeys
{
    public static string CategoriesList(int pageNumber, int pageSize)
        => $"categories:list:{pageNumber}:{pageSize}";

    public static string SuppliersList(int pageNumber, int pageSize)
        => $"suppliers:list:{pageNumber}:{pageSize}";
}
