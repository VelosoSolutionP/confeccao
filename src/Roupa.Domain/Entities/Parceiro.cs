using Roupa.Domain.Common;

namespace Roupa.Domain.Entities;

public class Parceiro : BaseEntity
{
    public string Nome { get; private set; } = default!;
    public string? CpfCnpj { get; private set; }
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }
    public string? Especialidade { get; private set; }
    public string? Cidade { get; private set; }
    public string? Observacoes { get; private set; }
    public bool Ativo { get; private set; } = true;

    private Parceiro() { }

    public static Parceiro Criar(string nome, string? cpfCnpj, string? telefone,
        string? email, string? especialidade, string? cidade, string? observacoes = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do parceiro é obrigatório.");

        return new Parceiro
        {
            Nome = nome.Trim(),
            CpfCnpj = cpfCnpj?.Trim(),
            Telefone = telefone?.Trim(),
            Email = email?.Trim(),
            Especialidade = especialidade?.Trim(),
            Cidade = cidade?.Trim(),
            Observacoes = observacoes?.Trim(),
            Ativo = true
        };
    }

    public void Atualizar(string nome, string? cpfCnpj, string? telefone,
        string? email, string? especialidade, string? cidade, string? observacoes)
    {
        Nome = nome.Trim();
        CpfCnpj = cpfCnpj?.Trim();
        Telefone = telefone?.Trim();
        Email = email?.Trim();
        Especialidade = especialidade?.Trim();
        Cidade = cidade?.Trim();
        Observacoes = observacoes?.Trim();
        SetAtualizado();
    }

    public void Desativar() { Ativo = false; SetAtualizado(); }
    public void Ativar()    { Ativo = true;  SetAtualizado(); }
}
