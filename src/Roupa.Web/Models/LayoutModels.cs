namespace Roupa.Web.Models;

public enum TipoProduto { CamisaPolo = 1, CamisaMalha = 2, Jaleco = 3, Calca = 4, Bermuda = 5, Agasalho = 6, Colete = 7, Outro = 99 }
public enum TipoLogomarca { Bordado = 1, SilkScreen = 2, Sublimacao = 3, Transfer = 4 }

public class LayoutDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = "";
    public string Modelo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public TipoProduto TipoProduto { get; set; }
    public string Tecido { get; set; } = "";
    public string Cores { get; set; } = "";
    public TipoLogomarca TipoLogomarca { get; set; }
    public string PosicaoLogomarca { get; set; } = "";
    public string TamanhoLogomarca { get; set; } = "";
    public string CorLogomarca { get; set; } = "";
    public string? Outros { get; set; }
    public string? UrlImagemFrente { get; set; }
    public string? UrlImagemCostas { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class CriarLayoutModel
{
    public Guid ClienteId { get; set; }
    public string Modelo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public TipoProduto TipoProduto { get; set; } = TipoProduto.CamisaPolo;
    public string Tecido { get; set; } = "";
    public string Cores { get; set; } = "";
    public TipoLogomarca TipoLogomarca { get; set; } = TipoLogomarca.Bordado;
    public string PosicaoLogomarca { get; set; } = "";
    public string TamanhoLogomarca { get; set; } = "";
    public string CorLogomarca { get; set; } = "";
    public string? Outros { get; set; }
}
