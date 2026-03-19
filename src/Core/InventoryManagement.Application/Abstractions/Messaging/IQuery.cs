using InventoryManagement.Domain.Abstractions;
using MediatR;

namespace InventoryManagement.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;