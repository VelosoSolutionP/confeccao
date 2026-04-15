using MediatR;
using Roupa.Application.Clientes.DTOs;

namespace Roupa.Application.Clientes.Queries;

public record ListarClientesQuery(bool ApenasAtivos = true) : IRequest<IEnumerable<ClienteDto>>;

public record ObterClientePorIdQuery(Guid Id) : IRequest<ClienteDto?>;
