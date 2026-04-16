using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Parceiros.DTOs;

namespace Roupa.Application.Parceiros.Commands;

public record CriarParceiroCommand(
    string Nome,
    string? CpfCnpj,
    string? Telefone,
    string? Email,
    string? Especialidade,
    string? Cidade,
    string? Observacoes) : IRequest<Result<ParceiroDto>>;
