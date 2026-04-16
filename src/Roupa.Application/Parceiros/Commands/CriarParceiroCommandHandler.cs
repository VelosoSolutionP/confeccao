using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Parceiros.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Parceiros.Commands;

public class CriarParceiroCommandHandler : IRequestHandler<CriarParceiroCommand, Result<ParceiroDto>>
{
    private readonly IParceiroRepository _repo;
    private readonly IUnitOfWork _uow;

    public CriarParceiroCommandHandler(IParceiroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result<ParceiroDto>> Handle(CriarParceiroCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            return Result<ParceiroDto>.Falha("Nome do parceiro é obrigatório.");

        var parceiro = Parceiro.Criar(
            request.Nome, request.CpfCnpj, request.Telefone,
            request.Email, request.Especialidade, request.Cidade, request.Observacoes);

        await _repo.AdicionarAsync(parceiro, ct);
        await _uow.SalvarAsync(ct);

        return Result<ParceiroDto>.Ok(Map(parceiro));
    }

    internal static ParceiroDto Map(Parceiro p) =>
        new(p.Id, p.Nome, p.CpfCnpj, p.Telefone, p.Email,
            p.Especialidade, p.Cidade, p.Observacoes, p.Ativo, p.CriadoEm);
}
