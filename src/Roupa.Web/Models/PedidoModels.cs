namespace Roupa.Web.Models;

public enum StatusPedido { Rascunho = 1, Confirmado = 2, EmProducao = 3, Concluido = 4, Cancelado = 5 }

public class PedidoDto
{
    public Guid Id { get; set; }
    public string Numero { get; set; } = "";
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = "";
    public StatusPedido Status { get; set; }
    public string StatusNome { get; set; } = "";
    public DateTime? DataEntrega { get; set; }
    public string? Observacoes { get; set; }
    public decimal Total { get; set; }
    public List<PedidoItemDto> Itens { get; set; } = new();
    public DateTime CriadoEm { get; set; }
}

public class PedidoItemDto
{
    public Guid Id { get; set; }
    public Guid LayoutId { get; set; }
    public string ModeloLayout { get; set; } = "";
    public string Tamanho { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total { get; set; }
}

public class CriarPedidoModel
{
    public Guid ClienteId { get; set; }
    public DateTime? DataEntrega { get; set; }
    public string? Observacoes { get; set; }
}

public class AdicionarItemModel
{
    public Guid LayoutId { get; set; }
    public string Tamanho { get; set; } = "M";
    public int Quantidade { get; set; } = 1;
    public decimal PrecoUnitario { get; set; }
}
