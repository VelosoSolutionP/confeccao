using Roupa.Application.Auth.DTOs;
using Roupa.Application.Common;

namespace Roupa.Application.Interfaces;

public interface IAuthService
{
    Task<Result<TokenDto>> LoginAsync(string email, string senha, CancellationToken ct = default);
    Task<Result> RegistrarAsync(string nome, string email, string senha, string confirmacaoSenha, CancellationToken ct = default);
}
