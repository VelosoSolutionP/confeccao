namespace Roupa.Web.Models;

public class ParceiroDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string? CpfCnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Especialidade { get; set; }
    public string? Cidade { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class CriarParceiroModel
{
    public string Nome { get; set; } = "";
    public string? CpfCnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Especialidade { get; set; }
    public string? Cidade { get; set; }
    public string? Observacoes { get; set; }
}

public class AtualizarParceiroModel
{
    public string Nome { get; set; } = "";
    public string? CpfCnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Especialidade { get; set; }
    public string? Cidade { get; set; }
    public string? Observacoes { get; set; }
}
