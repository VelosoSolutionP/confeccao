using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roupa.Application.Parceiros.Commands;
using Roupa.Application.Parceiros.DTOs;
using Roupa.Application.Parceiros.Queries;

namespace Roupa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParceirosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParceirosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool apenasAtivos = true) =>
        Ok(await _mediator.Send(new ListarParceirosQuery(apenasAtivos)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var parceiros = await _mediator.Send(new ListarParceirosQuery(false));
        var p = parceiros.FirstOrDefault(x => x.Id == id);
        if (p is null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Criar([FromBody] CriarParceiroDto dto)
    {
        var result = await _mediator.Send(new CriarParceiroCommand(
            dto.Nome, dto.CpfCnpj, dto.Telefone, dto.Email,
            dto.Especialidade, dto.Cidade, dto.Observacoes));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Dados!.Id }, result.Dados);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarParceiroDto dto)
    {
        var result = await _mediator.Send(new AtualizarParceiroCommand(
            id, dto.Nome, dto.CpfCnpj, dto.Telefone, dto.Email,
            dto.Especialidade, dto.Cidade, dto.Observacoes));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return Ok(result.Dados);
    }

    [HttpPatch("{id:guid}/toggle-ativo")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> ToggleAtivo(Guid id)
    {
        var result = await _mediator.Send(new ToggleAtivoParceiroCommand(id));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return NoContent();
    }
}
