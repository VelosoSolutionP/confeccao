using MediatR;
using Roupa.Application.Common;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Clientes.Commands;

public record ToggleAtivoClienteCommand(Guid Id) : IRequest<Result>;

public class ToggleAtivoClienteCommandHandler : IRequestHandler<ToggleAtivoClienteCommand, Result>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public ToggleAtivoClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result> Handle(ToggleAtivoClienteCommand request, CancellationToken ct)
    {
        var c = await _repo.ObterPorIdAsync(request.Id, ct);
        if (c is null) return Result.Falha("Cliente não encontrado.");

        if (c.Ativo) c.Desativar(); else c.Ativar();
        _repo.Atualizar(c);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }
}
