namespace Roupa.Web.Models;

public class ClienteDto
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public string Cnpj { get; set; } = "";
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class CriarClienteModel
{
    public string RazaoSocial { get; set; } = "";
    public string NomeFantasia { get; set; } = "";
    public string Cnpj { get; set; } = "";
    public string? Email { get; set; }
    public string? Telefone { get; set; }
}
