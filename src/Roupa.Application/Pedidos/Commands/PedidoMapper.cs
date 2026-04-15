using Roupa.Application.Pedidos.DTOs;
using Roupa.Domain.Entities;

namespace Roupa.Application.Pedidos.Commands;

internal static class PedidoMapper
{
    public static PedidoDto ToDto(Pedido p, string nomeCliente, Dictionary<Guid, string> layoutsMap) =>
        new(p.Id, p.Numero, p.ClienteId, nomeCliente, p.Status, p.Status.ToString(),
            p.DataEntrega, p.Observacoes, p.Total,
            p.Itens.Select(i => new PedidoItemDto(
                i.Id, i.LayoutId,
                layoutsMap.GetValueOrDefault(i.LayoutId, ""),
                i.Tamanho, i.Quantidade, i.PrecoUnitario, i.Total)).ToList(),
            p.CriadoEm);
}
