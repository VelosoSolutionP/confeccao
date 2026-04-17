using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Layouts.DTOs;
using Roupa.Domain.Enums;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Layouts.Commands;

public record AtualizarLayoutCommand(
    Guid Id,
    string Modelo,
    string Descricao,
    TipoProduto TipoProduto,
    string Tecido,
    string Cores,
    TipoLogomarca TipoLogomarca,
    string PosicaoLogomarca,
    string TamanhoLogomarca,
    string CorLogomarca,
    string? Outros,
    string? Opcoes = null) : IRequest<Result<LayoutDto>>;

public class AtualizarLayoutCommandHandler : IRequestHandler<AtualizarLayoutCommand, Result<LayoutDto>>
{
    private readonly ILayoutRepository _repo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;

    public AtualizarLayoutCommandHandler(ILayoutRepository repo, IClienteRepository clienteRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _clienteRepo = clienteRepo;
        _uow = uow;
    }

    public async Task<Result<LayoutDto>> Handle(AtualizarLayoutCommand request, CancellationToken ct)
    {
        var layout = await _repo.ObterPorIdAsync(request.Id, ct);
        if (layout is null) return Result<LayoutDto>.Falha("Ficha não encontrada.");

        layout.Atualizar(request.Modelo, request.Descricao, request.TipoProduto,
                         request.Tecido, request.Cores, request.TipoLogomarca,
                         request.PosicaoLogomarca, request.TamanhoLogomarca,
                         request.CorLogomarca, request.Outros, request.Opcoes);
        _repo.Atualizar(layout);
        await _uow.SalvarAsync(ct);

        var cliente = await _clienteRepo.ObterPorIdAsync(layout.ClienteId, ct);
        return Result<LayoutDto>.Ok(new LayoutDto(
            layout.Id, layout.ClienteId, cliente?.NomeFantasia ?? "",
            layout.Modelo, layout.Descricao, layout.TipoProduto,
            layout.Tecido, layout.Cores, layout.TipoLogomarca,
            layout.PosicaoLogomarca, layout.TamanhoLogomarca, layout.CorLogomarca,
            layout.Outros, layout.UrlImagemFrente, layout.UrlImagemCostas, layout.CriadoEm,
            Opcoes: layout.Opcoes));
    }
}
