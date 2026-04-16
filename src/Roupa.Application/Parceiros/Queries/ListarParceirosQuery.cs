using MediatR;
using Roupa.Application.Parceiros.DTOs;

namespace Roupa.Application.Parceiros.Queries;

public record ListarParceirosQuery(bool ApenasAtivos = true) : IRequest<IEnumerable<ParceiroDto>>;
