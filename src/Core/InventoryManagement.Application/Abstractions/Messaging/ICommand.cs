using InventoryManagement.Domain.Abstractions;
using MediatR;

namespace InventoryManagement.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;