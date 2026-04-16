namespace Roupa.Web.Models;

public enum TipoProduto { CamisaPolo = 1, CamisaMalha = 2, Jaleco = 3, Calca = 4, Bermuda = 5, Agasalho = 6, Colete = 7, Outro = 99 }
public enum TipoLogomarca { Bordado = 1, SilkScreen = 2, Sublimacao = 3, Transfer = 4 }

public record LayoutDto
{
    public Guid Id { get; init; }
    public Guid ClienteId { get; init; }
    public string NomeCliente { get; init; } = "";
    public string Modelo { get; init; } = "";
    public string Descricao { get; init; } = "";
    public TipoProduto TipoProduto { get; init; }
    public string Tecido { get; init; } = "";
    public string Cores { get; init; } = "";
    public TipoLogomarca TipoLogomarca { get; init; }
    public string PosicaoLogomarca { get; init; } = "";
    public string TamanhoLogomarca { get; init; } = "";
    public string CorLogomarca { get; init; } = "";
    public string? Outros { get; init; }
    public string? UrlImagemFrente { get; init; }
    public string? UrlImagemCostas { get; init; }
    public DateTime CriadoEm { get; init; }
    // Frontend-only state (not persisted to API yet)
    public bool Aprovado { get; init; }
    public int Versao { get; init; }
    public Dictionary<string, string>? Medidas { get; init; }
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
