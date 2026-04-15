using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roupa.Application.Layouts.Commands;
using Roupa.Application.Layouts.DTOs;
using Roupa.Application.Layouts.Queries;

namespace Roupa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LayoutsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LayoutsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var layouts = await _mediator.Send(new ListarLayoutsQuery());
        return Ok(layouts);
    }

    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<IActionResult> ListarPorCliente(Guid clienteId)
    {
        var layouts = await _mediator.Send(new ListarLayoutsPorClienteQuery(clienteId));
        return Ok(layouts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var layout = await _mediator.Send(new ObterLayoutPorIdQuery(id));
        if (layout is null) return NotFound();
        return Ok(layout);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Criar([FromBody] CriarLayoutDto dto)
    {
        var result = await _mediator.Send(new CriarLayoutCommand(
            dto.ClienteId, dto.Modelo, dto.Descricao, dto.TipoProduto,
            dto.Tecido, dto.Cores, dto.TipoLogomarca, dto.PosicaoLogomarca,
            dto.TamanhoLogomarca, dto.CorLogomarca, dto.Outros));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Dados!.Id }, result.Dados);
    }
}
