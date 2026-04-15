using MediatR;
using Roupa.Application.Layouts.DTOs;

namespace Roupa.Application.Layouts.Queries;

public record ListarLayoutsQuery : IRequest<IEnumerable<LayoutDto>>;

public record ListarLayoutsPorClienteQuery(Guid ClienteId) : IRequest<IEnumerable<LayoutDto>>;

public record ObterLayoutPorIdQuery(Guid Id) : IRequest<LayoutDto?>;
