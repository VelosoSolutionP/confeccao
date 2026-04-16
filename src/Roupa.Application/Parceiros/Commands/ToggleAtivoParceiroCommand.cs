using MediatR;
using Roupa.Application.Common;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Parceiros.Commands;

public record ToggleAtivoParceiroCommand(Guid Id) : IRequest<Result>;

public class ToggleAtivoParceiroCommandHandler : IRequestHandler<ToggleAtivoParceiroCommand, Result>
{
    private readonly IParceiroRepository _repo;
    private readonly IUnitOfWork _uow;

    public ToggleAtivoParceiroCommandHandler(IParceiroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result> Handle(ToggleAtivoParceiroCommand request, CancellationToken ct)
    {
        var p = await _repo.ObterPorIdAsync(request.Id, ct);
        if (p is null) return Result.Falha("Parceiro não encontrado.");

        if (p.Ativo) p.Desativar(); else p.Ativar();
        _repo.Atualizar(p);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }
}
