using Roupa.Domain.Common;

namespace Roupa.Domain.Entities;

public class PedidoItem : BaseEntity
{
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; private set; } = default!;

    public Guid LayoutId { get; private set; }
    public Layout Layout { get; private set; } = default!;

    public string Tamanho { get; private set; } = default!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public decimal Total => Quantidade * PrecoUnitario;

    private PedidoItem() { }

    public static PedidoItem Criar(Guid pedidoId, Guid layoutId, string tamanho, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (precoUnitario < 0) throw new ArgumentException("Preço unitário não pode ser negativo.");

        return new PedidoItem
        {
            PedidoId = pedidoId,
            LayoutId = layoutId,
            Tamanho = tamanho,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitario
        };
    }
}
