namespace InventoryManagement.Domain.Ports.Repositories.Common;

public sealed record PaginationParams(int PageNumber = 1, int PageSize = 10)
{
    public int NormalizedPageNumber => PageNumber < 1 ? 1 : PageNumber;
    public int NormalizedPageSize => PageSize < 1 ? 10 : PageSize > 100 ? 100 : PageSize;
    public int Skip => (NormalizedPageNumber - 1) * NormalizedPageSize;
}