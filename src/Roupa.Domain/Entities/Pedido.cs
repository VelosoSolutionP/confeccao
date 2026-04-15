using Roupa.Domain.Common;
using Roupa.Domain.Enums;

namespace Roupa.Domain.Entities;

public class Pedido : BaseEntity
{
    public string Numero { get; private set; } = default!;
    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; } = default!;

    public StatusPedido Status { get; private set; } = StatusPedido.Rascunho;
    public DateTime? DataEntrega { get; private set; }
    public string? Observacoes { get; private set; }

    public IReadOnlyCollection<PedidoItem> Itens => _itens.AsReadOnly();
    private readonly List<PedidoItem> _itens = new();

    private Pedido() { }

    public static Pedido Criar(Guid clienteId, DateTime? dataEntrega = null, string? observacoes = null)
    {
        return new Pedido
        {
            Numero = GerarNumero(),
            ClienteId = clienteId,
            DataEntrega = dataEntrega,
            Observacoes = observacoes
        };
    }

    public void AdicionarItem(Guid layoutId, string tamanho, int quantidade, decimal precoUnitario)
    {
        var item = PedidoItem.Criar(Id, layoutId, tamanho, quantidade, precoUnitario);
        _itens.Add(item);
        SetAtualizado();
    }

    public void ConfirmarPedido()
    {
        if (Status != StatusPedido.Rascunho)
            throw new InvalidOperationException("Apenas pedidos em rascunho podem ser confirmados.");
        Status = StatusPedido.Confirmado;
        SetAtualizado();
    }

    public void IniciarProducao()
    {
        if (Status != StatusPedido.Confirmado)
            throw new InvalidOperationException("Apenas pedidos confirmados podem entrar em produção.");
        Status = StatusPedido.EmProducao;
        SetAtualizado();
    }

    public void Concluir()
    {
        if (Status != StatusPedido.EmProducao)
            throw new InvalidOperationException("Apenas pedidos em produção podem ser concluídos.");
        Status = StatusPedido.Concluido;
        SetAtualizado();
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Concluido)
            throw new InvalidOperationException("Pedidos concluídos não podem ser cancelados.");
        Status = StatusPedido.Cancelado;
        SetAtualizado();
    }

    public decimal Total => _itens.Sum(i => i.Total);

    private static string GerarNumero() =>
        $"PED-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
}
