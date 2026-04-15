using MediatR;
using Roupa.Application.Clientes.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Clientes.Queries;

public class ListarClientesQueryHandler :
    IRequestHandler<ListarClientesQuery, IEnumerable<ClienteDto>>,
    IRequestHandler<ObterClientePorIdQuery, ClienteDto?>
{
    private readonly IClienteRepository _repo;

    public ListarClientesQueryHandler(IClienteRepository repo) => _repo = repo;

    public async Task<IEnumerable<ClienteDto>> Handle(ListarClientesQuery request, CancellationToken ct)
    {
        var clientes = await _repo.ListarAsync(request.ApenasAtivos, ct);
        return clientes.Select(MapToDto);
    }

    public async Task<ClienteDto?> Handle(ObterClientePorIdQuery request, CancellationToken ct)
    {
        var cliente = await _repo.ObterPorIdAsync(request.Id, ct);
        return cliente is null ? null : MapToDto(cliente);
    }

    private static ClienteDto MapToDto(Cliente c) =>
        new(c.Id, c.RazaoSocial, c.NomeFantasia, c.Cnpj, c.Email, c.Telefone, c.Ativo, c.CriadoEm);
}
