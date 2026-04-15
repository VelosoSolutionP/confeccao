using Roupa.Application.Auth.DTOs;

namespace Roupa.Application.Interfaces;

public interface ITokenService
{
    TokenDto GerarToken(string userId, string email, string nome, IList<string> roles);
}
