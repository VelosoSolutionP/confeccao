using MediatR;
using Roupa.Application.Common;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Layouts.Commands;

public record ExcluirLayoutCommand(Guid Id) : IRequest<Result>;

public class ExcluirLayoutCommandHandler : IRequestHandler<ExcluirLayoutCommand, Result>
{
    private readonly ILayoutRepository _repo;
    private readonly IUnitOfWork _uow;

    public ExcluirLayoutCommandHandler(ILayoutRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result> Handle(ExcluirLayoutCommand request, CancellationToken ct)
    {
        var layout = await _repo.ObterPorIdAsync(request.Id, ct);
        if (layout is null) return Result.Falha("Ficha não encontrada.");

        _repo.Remover(layout);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }
}
