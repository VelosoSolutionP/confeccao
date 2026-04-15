using MediatR;
using Roupa.Application.Clientes.DTOs;
using Roupa.Application.Common;

namespace Roupa.Application.Clientes.Commands;

public record CriarClienteCommand(
    string RazaoSocial,
    string NomeFantasia,
    string Cnpj,
    string? Email,
    string? Telefone) : IRequest<Result<ClienteDto>>;
