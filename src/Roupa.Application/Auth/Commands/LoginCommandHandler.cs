using MediatR;
using Roupa.Application.Auth.DTOs;
using Roupa.Application.Common;
using Roupa.Application.Interfaces;

namespace Roupa.Application.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService) => _authService = authService;

    public Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken ct) =>
        _authService.LoginAsync(request.Email, request.Senha, ct);
}
