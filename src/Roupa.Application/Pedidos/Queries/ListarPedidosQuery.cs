using MediatR;
using Roupa.Application.Pedidos.DTOs;
using Roupa.Domain.Enums;

namespace Roupa.Application.Pedidos.Queries;

public record ListarPedidosQuery : IRequest<IEnumerable<PedidoDto>>;

public record ListarPedidosPorClienteQuery(Guid ClienteId) : IRequest<IEnumerable<PedidoDto>>;

public record ListarPedidosPorStatusQuery(StatusPedido Status) : IRequest<IEnumerable<PedidoDto>>;

public record ObterPedidoPorIdQuery(Guid Id) : IRequest<PedidoDto?>;
