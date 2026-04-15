using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Pedidos.DTOs;

namespace Roupa.Application.Pedidos.Commands;

public record CriarPedidoCommand(Guid ClienteId, DateTime? DataEntrega, string? Observacoes)
    : IRequest<Result<PedidoDto>>;

public record AdicionarItemPedidoCommand(Guid PedidoId, Guid LayoutId, string Tamanho, int Quantidade, decimal PrecoUnitario)
    : IRequest<Result<PedidoDto>>;

public record ConfirmarPedidoCommand(Guid PedidoId) : IRequest<Result>;

public record IniciarProducaoCommand(Guid PedidoId) : IRequest<Result>;

public record ConcluirPedidoCommand(Guid PedidoId) : IRequest<Result>;

public record CancelarPedidoCommand(Guid PedidoId) : IRequest<Result>;
