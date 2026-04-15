using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Interfaces;

namespace Roupa.Application.Auth.Commands;

public class RegistrarCommandHandler : IRequestHandler<RegistrarCommand, Result>
{
    private readonly IAuthService _authService;

    public RegistrarCommandHandler(IAuthService authService) => _authService = authService;

    public Task<Result> Handle(RegistrarCommand request, CancellationToken ct) =>
        _authService.RegistrarAsync(request.Nome, request.Email, request.Senha, request.ConfirmacaoSenha, ct);
}
