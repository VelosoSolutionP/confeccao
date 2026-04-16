using MediatR;
using Roupa.Application.Parceiros.Commands;
using Roupa.Application.Parceiros.DTOs;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Parceiros.Queries;

public class ListarParceirosQueryHandler : IRequestHandler<ListarParceirosQuery, IEnumerable<ParceiroDto>>
{
    private readonly IParceiroRepository _repo;

    public ListarParceirosQueryHandler(IParceiroRepository repo) => _repo = repo;

    public async Task<IEnumerable<ParceiroDto>> Handle(ListarParceirosQuery request, CancellationToken ct)
    {
        var parceiros = await _repo.ListarAsync(request.ApenasAtivos, ct);
        return parceiros.Select(CriarParceiroCommandHandler.Map);
    }
}
