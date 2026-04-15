using MediatR;
using Roupa.Application.Pedidos.Commands;
using Roupa.Application.Pedidos.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Pedidos.Queries;

public class ListarPedidosQueryHandler :
    IRequestHandler<ListarPedidosQuery, IEnumerable<PedidoDto>>,
    IRequestHandler<ListarPedidosPorClienteQuery, IEnumerable<PedidoDto>>,
    IRequestHandler<ListarPedidosPorStatusQuery, IEnumerable<PedidoDto>>,
    IRequestHandler<ObterPedidoPorIdQuery, PedidoDto?>
{
    private readonly IPedidoRepository _pedidoRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly ILayoutRepository _layoutRepo;

    public ListarPedidosQueryHandler(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo, ILayoutRepository layoutRepo)
    {
        _pedidoRepo = pedidoRepo;
        _clienteRepo = clienteRepo;
        _layoutRepo = layoutRepo;
    }

    public async Task<IEnumerable<PedidoDto>> Handle(ListarPedidosQuery request, CancellationToken ct)
        => await MapMuitosAsync(await _pedidoRepo.ListarAsync(ct), ct);

    public async Task<IEnumerable<PedidoDto>> Handle(ListarPedidosPorClienteQuery request, CancellationToken ct)
        => await MapMuitosAsync(await _pedidoRepo.ListarPorClienteAsync(request.ClienteId, ct), ct);

    public async Task<IEnumerable<PedidoDto>> Handle(ListarPedidosPorStatusQuery request, CancellationToken ct)
        => await MapMuitosAsync(await _pedidoRepo.ListarPorStatusAsync(request.Status, ct), ct);

    public async Task<PedidoDto?> Handle(ObterPedidoPorIdQuery request, CancellationToken ct)
    {
        var pedido = await _pedidoRepo.ObterPorIdAsync(request.Id, ct);
        if (pedido is null) return null;
        var cliente = await _clienteRepo.ObterPorIdAsync(pedido.ClienteId, ct);
        var layoutsMap = await CarregarLayoutsMapAsync(pedido.Itens, ct);
        return PedidoMapper.ToDto(pedido, cliente?.NomeFantasia ?? "", layoutsMap);
    }

    private async Task<IEnumerable<PedidoDto>> MapMuitosAsync(IEnumerable<Pedido> pedidos, CancellationToken ct)
    {
        var lista = pedidos.ToList();
        var clienteIds = lista.Select(p => p.ClienteId).Distinct();
        var clientes = new Dictionary<Guid, string>();
        foreach (var id in clienteIds)
        {
            var c = await _clienteRepo.ObterPorIdAsync(id, ct);
            if (c is not null) clientes[id] = c.NomeFantasia;
        }
        var allItems = lista.SelectMany(p => p.Itens);
        var layoutsMap = await CarregarLayoutsMapAsync(allItems, ct);
        return lista.Select(p => PedidoMapper.ToDto(p, clientes.GetValueOrDefault(p.ClienteId, ""), layoutsMap));
    }

    private async Task<Dictionary<Guid, string>> CarregarLayoutsMapAsync(IEnumerable<PedidoItem> items, CancellationToken ct)
    {
        var map = new Dictionary<Guid, string>();
        foreach (var layoutId in items.Select(i => i.LayoutId).Distinct())
        {
            var l = await _layoutRepo.ObterPorIdAsync(layoutId, ct);
            if (l is not null) map[layoutId] = l.Modelo;
        }
        return map;
    }
}
