using MediatR;
using Roupa.Application.Common;
using Roupa.Application.Pedidos.DTOs;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Pedidos.Commands;

public class CriarPedidoCommandHandler : IRequestHandler<CriarPedidoCommand, Result<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;

    public CriarPedidoCommandHandler(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo, IUnitOfWork uow)
    {
        _pedidoRepo = pedidoRepo;
        _clienteRepo = clienteRepo;
        _uow = uow;
    }

    public async Task<Result<PedidoDto>> Handle(CriarPedidoCommand request, CancellationToken ct)
    {
        var cliente = await _clienteRepo.ObterPorIdAsync(request.ClienteId, ct);
        if (cliente is null) return Result<PedidoDto>.Falha("Cliente não encontrado.");

        var pedido = Pedido.Criar(request.ClienteId, request.DataEntrega, request.Observacoes);
        await _pedidoRepo.AdicionarAsync(pedido, ct);
        await _uow.SalvarAsync(ct);

        return Result<PedidoDto>.Ok(PedidoMapper.ToDto(pedido, cliente.NomeFantasia, []));
    }
}

public class AdicionarItemPedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, Result<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepo;
    private readonly IClienteRepository _clienteRepo;
    private readonly ILayoutRepository _layoutRepo;
    private readonly IUnitOfWork _uow;

    public AdicionarItemPedidoCommandHandler(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo, ILayoutRepository layoutRepo, IUnitOfWork uow)
    {
        _pedidoRepo = pedidoRepo;
        _clienteRepo = clienteRepo;
        _layoutRepo = layoutRepo;
        _uow = uow;
    }

    public async Task<Result<PedidoDto>> Handle(AdicionarItemPedidoCommand request, CancellationToken ct)
    {
        var pedido = await _pedidoRepo.ObterPorIdAsync(request.PedidoId, ct);
        if (pedido is null) return Result<PedidoDto>.Falha("Pedido não encontrado.");

        var layout = await _layoutRepo.ObterPorIdAsync(request.LayoutId, ct);
        if (layout is null) return Result<PedidoDto>.Falha("Layout não encontrado.");

        try { pedido.AdicionarItem(request.LayoutId, request.Tamanho, request.Quantidade, request.PrecoUnitario); }
        catch (Exception ex) { return Result<PedidoDto>.Falha(ex.Message); }

        _pedidoRepo.Atualizar(pedido);
        await _uow.SalvarAsync(ct);

        var cliente = await _clienteRepo.ObterPorIdAsync(pedido.ClienteId, ct);
        var layoutsMap = new Dictionary<Guid, string> { [layout.Id] = layout.Modelo };
        return Result<PedidoDto>.Ok(PedidoMapper.ToDto(pedido, cliente?.NomeFantasia ?? "", layoutsMap));
    }
}

public class StatusPedidoCommandHandler :
    IRequestHandler<ConfirmarPedidoCommand, Result>,
    IRequestHandler<IniciarProducaoCommand, Result>,
    IRequestHandler<ConcluirPedidoCommand, Result>,
    IRequestHandler<CancelarPedidoCommand, Result>
{
    private readonly IPedidoRepository _repo;
    private readonly IUnitOfWork _uow;

    public StatusPedidoCommandHandler(IPedidoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public Task<Result> Handle(ConfirmarPedidoCommand request, CancellationToken ct) =>
        AlterarStatus(request.PedidoId, p => p.ConfirmarPedido(), ct);

    public Task<Result> Handle(IniciarProducaoCommand request, CancellationToken ct) =>
        AlterarStatus(request.PedidoId, p => p.IniciarProducao(), ct);

    public Task<Result> Handle(ConcluirPedidoCommand request, CancellationToken ct) =>
        AlterarStatus(request.PedidoId, p => p.Concluir(), ct);

    public Task<Result> Handle(CancelarPedidoCommand request, CancellationToken ct) =>
        AlterarStatus(request.PedidoId, p => p.Cancelar(), ct);

    private async Task<Result> AlterarStatus(Guid id, Action<Pedido> acao, CancellationToken ct)
    {
        var pedido = await _repo.ObterPorIdAsync(id, ct);
        if (pedido is null) return Result.Falha("Pedido não encontrado.");
        try { acao(pedido); }
        catch (Exception ex) { return Result.Falha(ex.Message); }
        _repo.Atualizar(pedido);
        await _uow.SalvarAsync(ct);
        return Result.Ok();
    }
}
