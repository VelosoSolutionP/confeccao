using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Layouts.DTOs;
using Roupa.Domain.Enums;

namespace Roupa.Application.Layouts.Commands;

public record CriarLayoutCommand(
    Guid ClienteId,
    string Modelo,
    string Descricao,
    TipoProduto TipoProduto,
    string Tecido,
    string Cores,
    TipoLogomarca TipoLogomarca,
    string PosicaoLogomarca,
    string TamanhoLogomarca,
    string CorLogomarca,
    string? Outros) : IRequest<Result<LayoutDto>>;
