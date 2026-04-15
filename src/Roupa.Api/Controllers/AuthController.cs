using MediatR;
using Microsoft.AspNetCore.Mvc;
using Roupa.Application.Auth.Commands;
using Roupa.Application.Auth.DTOs;

namespace Roupa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginCommand(dto.Email, dto.Senha));
        if (!result.Sucesso) return Unauthorized(new { erro = result.Erro });
        return Ok(result.Dados);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDto dto)
    {
        var result = await _mediator.Send(new RegistrarCommand(dto.Nome, dto.Email, dto.Senha, dto.ConfirmacaoSenha));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return Created("", new { mensagem = "Usuário criado com sucesso." });
    }
}
