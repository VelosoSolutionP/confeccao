using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roupa.Application.Pedidos.Commands;
using Roupa.Application.Pedidos.DTOs;
using Roupa.Application.Pedidos.Queries;
using Roupa.Domain.Enums;

namespace Roupa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PedidosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Listar() =>
        Ok(await _mediator.Send(new ListarPedidosQuery()));

    [HttpGet("cliente/{clienteId:guid}")]
    public async Task<IActionResult> ListarPorCliente(Guid clienteId) =>
        Ok(await _mediator.Send(new ListarPedidosPorClienteQuery(clienteId)));

    [HttpGet("status/{status}")]
    public async Task<IActionResult> ListarPorStatus(StatusPedido status) =>
        Ok(await _mediator.Send(new ListarPedidosPorStatusQuery(status)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var pedido = await _mediator.Send(new ObterPedidoPorIdQuery(id));
        if (pedido is null) return NotFound();
        return Ok(pedido);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
    {
        var result = await _mediator.Send(new CriarPedidoCommand(dto.ClienteId, dto.DataEntrega, dto.Observacoes));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Dados!.Id }, result.Dados);
    }

    [HttpPost("{id:guid}/itens")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> AdicionarItem(Guid id, [FromBody] AdicionarItemDto dto)
    {
        var result = await _mediator.Send(new AdicionarItemPedidoCommand(id, dto.LayoutId, dto.Tamanho, dto.Quantidade, dto.PrecoUnitario));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return Ok(result.Dados);
    }

    [HttpPatch("{id:guid}/confirmar")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Confirmar(Guid id)
    {
        var result = await _mediator.Send(new ConfirmarPedidoCommand(id));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return NoContent();
    }

    [HttpPatch("{id:guid}/producao")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> IniciarProducao(Guid id)
    {
        var result = await _mediator.Send(new IniciarProducaoCommand(id));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return NoContent();
    }

    [HttpPatch("{id:guid}/concluir")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Concluir(Guid id)
    {
        var result = await _mediator.Send(new ConcluirPedidoCommand(id));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancelar")]
    [Authorize(Roles = "Admin,Operador")]
    public async Task<IActionResult> Cancelar(Guid id)
    {
        var result = await _mediator.Send(new CancelarPedidoCommand(id));
        if (!result.Sucesso) return BadRequest(new { erro = result.Erro });
        return NoContent();
    }
}
