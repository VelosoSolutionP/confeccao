using Microsoft.AspNetCore.Identity;
using Roupa.Application.Auth.DTOs;
using Roupa.Application.Common;
using Roupa.Application.Interfaces;
using Roupa.Infrastructure.Persistence;

namespace Roupa.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenDto>> LoginAsync(string email, string senha, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<TokenDto>.Falha("Email ou senha inválidos.");

        var senhaValida = await _userManager.CheckPasswordAsync(user, senha);
        if (!senhaValida)
            return Result<TokenDto>.Falha("Email ou senha inválidos.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GerarToken(user.Id, user.Email!, user.NomeCompleto, roles);
        return Result<TokenDto>.Ok(token);
    }

    public async Task<Result> RegistrarAsync(string nome, string email, string senha, string confirmacaoSenha, CancellationToken ct = default)
    {
        if (senha != confirmacaoSenha)
            return Result.Falha("As senhas não coincidem.");

        if (await _userManager.FindByEmailAsync(email) is not null)
            return Result.Falha("Este email já está cadastrado.");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            NomeCompleto = nome,
            EmailConfirmed = true
        };

        var resultado = await _userManager.CreateAsync(user, senha);
        if (!resultado.Succeeded)
            return Result.Falha(string.Join("; ", resultado.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "Operador");
        return Result.Ok();
    }
}
