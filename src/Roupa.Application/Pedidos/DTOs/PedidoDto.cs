using Roupa.Domain.Enums;

namespace Roupa.Application.Pedidos.DTOs;

public record PedidoDto(
    Guid Id,
    string Numero,
    Guid ClienteId,
    string NomeCliente,
    StatusPedido Status,
    string StatusNome,
    DateTime? DataEntrega,
    string? Observacoes,
    decimal Total,
    List<PedidoItemDto> Itens,
    DateTime CriadoEm);

public record PedidoItemDto(
    Guid Id,
    Guid LayoutId,
    string ModeloLayout,
    string Tamanho,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Total);

public record CriarPedidoDto(
    Guid ClienteId,
    DateTime? DataEntrega,
    string? Observacoes);

public record AdicionarItemDto(
    Guid LayoutId,
    string Tamanho,
    int Quantidade,
    decimal PrecoUnitario);
