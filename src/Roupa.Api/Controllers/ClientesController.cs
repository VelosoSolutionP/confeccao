using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roupa.Application.Clientes.Commands;
using Roupa.Application.Clientes.DTOs;
using Roupa.Application.Clientes.Queries;

namespace Roupa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool apenasAtivos = true)
    {
        var clientes = await _mediator.Send(new ListarClientesQuery(apenasAtivos));
        return Ok(clientes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var cliente = await _mediator.Send(new ObterClientePorIdQuery(id));
        if (cliente is null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        var result = await _mediator.Send(new CriarClienteCommand(dto.RazaoSocial, dto.NomeFantasia, dto.Cnpj, dto.Email, dto.Telefone));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Dados!.Id }, result.Dados);
    }
}
