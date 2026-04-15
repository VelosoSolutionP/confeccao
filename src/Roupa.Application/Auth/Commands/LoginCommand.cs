using MediatR;
using Roupa.Application.Auth.DTOs;
using Roupa.Application.Common;

namespace Roupa.Application.Auth.Commands;

public record LoginCommand(string Email, string Senha) : IRequest<Result<TokenDto>>;
