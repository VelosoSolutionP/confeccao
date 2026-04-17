using MediatR;
using Roupa.Application.Layouts.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Layouts.Queries;

public class ListarLayoutsQueryHandler :
    IRequestHandler<ListarLayoutsQuery, IEnumerable<LayoutDto>>,
    IRequestHandler<ListarLayoutsPorClienteQuery, IEnumerable<LayoutDto>>,
    IRequestHandler<ObterLayoutPorIdQuery, LayoutDto?>
{
    private readonly ILayoutRepository _layoutRepo;
    private readonly IClienteRepository _clienteRepo;

    public ListarLayoutsQueryHandler(ILayoutRepository layoutRepo, IClienteRepository clienteRepo)
    {
        _layoutRepo = layoutRepo;
        _clienteRepo = clienteRepo;
    }

    public async Task<IEnumerable<LayoutDto>> Handle(ListarLayoutsQuery request, CancellationToken ct)
    {
        var layouts = await _layoutRepo.ListarAsync(ct);
        return await MapMuitosAsync(layouts, ct);
    }

    public async Task<IEnumerable<LayoutDto>> Handle(ListarLayoutsPorClienteQuery request, CancellationToken ct)
    {
        var layouts = await _layoutRepo.ListarPorClienteAsync(request.ClienteId, ct);
        return await MapMuitosAsync(layouts, ct);
    }

    public async Task<LayoutDto?> Handle(ObterLayoutPorIdQuery request, CancellationToken ct)
    {
        var layout = await _layoutRepo.ObterPorIdAsync(request.Id, ct);
        if (layout is null) return null;
        var cliente = await _clienteRepo.ObterPorIdAsync(layout.ClienteId, ct);
        return MapToDto(layout, cliente?.NomeFantasia ?? "");
    }

    private async Task<IEnumerable<LayoutDto>> MapMuitosAsync(IEnumerable<Layout> layouts, CancellationToken ct)
    {
        var lista = layouts.ToList();
        var clienteIds = lista.Select(l => l.ClienteId).Distinct();
        var clientes = new Dictionary<Guid, string>();
        foreach (var id in clienteIds)
        {
            var c = await _clienteRepo.ObterPorIdAsync(id, ct);
            if (c is not null) clientes[id] = c.NomeFantasia;
        }
        return lista.Select(l => MapToDto(l, clientes.GetValueOrDefault(l.ClienteId, "")));
    }

    private static LayoutDto MapToDto(Layout l, string nomeCliente) =>
        new(l.Id, l.ClienteId, nomeCliente, l.Modelo, l.Descricao, l.TipoProduto,
            l.Tecido, l.Cores, l.TipoLogomarca, l.PosicaoLogomarca, l.TamanhoLogomarca,
            l.CorLogomarca, l.Outros, l.UrlImagemFrente, l.UrlImagemCostas, l.CriadoEm,
            Opcoes: l.Opcoes);
}
