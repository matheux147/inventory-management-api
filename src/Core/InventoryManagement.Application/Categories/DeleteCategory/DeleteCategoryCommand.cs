using InventoryManagement.Application.Abstractions.Messaging;

namespace InventoryManagement.Application.Categories.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid CategoryId) : ICommand;