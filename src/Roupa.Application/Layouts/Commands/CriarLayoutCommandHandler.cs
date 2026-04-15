using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Layouts.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Layouts.Commands;

public class CriarLayoutCommandHandler : IRequestHandler<CriarLayoutCommand, Result<LayoutDto>>
{
    private readonly ILayoutRepository _layoutRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;

    public CriarLayoutCommandHandler(ILayoutRepository layoutRepo, IClienteRepository clienteRepo, IUnitOfWork uow)
    {
        _layoutRepo = layoutRepo;
        _clienteRepo = clienteRepo;
        _uow = uow;
    }

    public async Task<Result<LayoutDto>> Handle(CriarLayoutCommand request, CancellationToken ct)
    {
        var cliente = await _clienteRepo.ObterPorIdAsync(request.ClienteId, ct);
        if (cliente is null)
            return Result<LayoutDto>.Falha("Cliente não encontrado.");

        var layout = Layout.Criar(
            request.ClienteId, request.Modelo, request.Descricao, request.TipoProduto,
            request.Tecido, request.Cores, request.TipoLogomarca, request.PosicaoLogomarca,
            request.TamanhoLogomarca, request.CorLogomarca, request.Outros);

        await _layoutRepo.AdicionarAsync(layout, ct);
        await _uow.SalvarAsync(ct);

        return Result<LayoutDto>.Ok(MapToDto(layout, cliente.NomeFantasia));
    }

    private static LayoutDto MapToDto(Layout l, string nomeCliente) =>
        new(l.Id, l.ClienteId, nomeCliente, l.Modelo, l.Descricao, l.TipoProduto,
            l.Tecido, l.Cores, l.TipoLogomarca, l.PosicaoLogomarca, l.TamanhoLogomarca,
            l.CorLogomarca, l.Outros, l.UrlImagemFrente, l.UrlImagemCostas, l.CriadoEm);
}
