using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Parceiros.DTOs;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Parceiros.Commands;

public record AtualizarParceiroCommand(
    Guid Id,
    string Nome,
    string? CpfCnpj,
    string? Telefone,
    string? Email,
    string? Especialidade,
    string? Cidade,
    string? Observacoes) : IRequest<Result<ParceiroDto>>;

public class AtualizarParceiroCommandHandler : IRequestHandler<AtualizarParceiroCommand, Result<ParceiroDto>>
{
    private readonly IParceiroRepository _repo;
    private readonly IUnitOfWork _uow;

    public AtualizarParceiroCommandHandler(IParceiroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result<ParceiroDto>> Handle(AtualizarParceiroCommand request, CancellationToken ct)
    {
        var p = await _repo.ObterPorIdAsync(request.Id, ct);
        if (p is null) return Result<ParceiroDto>.Falha("Parceiro não encontrado.");

        p.Atualizar(request.Nome, request.CpfCnpj, request.Telefone, request.Email,
                    request.Especialidade, request.Cidade, request.Observacoes);
        _repo.Atualizar(p);
        await _uow.SalvarAsync(ct);

        return Result<ParceiroDto>.Ok(new ParceiroDto(p.Id, p.Nome, p.CpfCnpj, p.Telefone, p.Email,
                                                       p.Especialidade, p.Cidade, p.Observacoes, p.Ativo, p.CriadoEm));
    }
}
