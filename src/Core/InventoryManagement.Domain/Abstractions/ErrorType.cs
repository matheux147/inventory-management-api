namespace InventoryManagement.Domain.Abstractions;

public enum ErrorType
{
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Failure = 4,
    Unauthorized = 5,
    Forbidden = 6
}