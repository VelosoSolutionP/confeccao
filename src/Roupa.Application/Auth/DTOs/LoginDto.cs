namespace Roupa.Application.Auth.DTOs;

public record LoginDto(string Email, string Senha);

public record TokenDto(string AccessToken, string RefreshToken, DateTime Expiracao, string Nome, string Email);

public record RegistrarDto(string Nome, string Email, string Senha, string ConfirmacaoSenha);
