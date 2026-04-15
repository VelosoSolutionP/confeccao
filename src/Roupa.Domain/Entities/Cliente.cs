using Roupa.Domain.Common;

namespace Roupa.Domain.Entities;

public class Cliente : BaseEntity
{
    public string RazaoSocial { get; private set; } = default!;
    public string NomeFantasia { get; private set; } = default!;
    public string Cnpj { get; private set; } = default!;
    public string? Email { get; private set; }
    public string? Telefone { get; private set; }
    public bool Ativo { get; private set; } = true;

    public IReadOnlyCollection<Layout> Layouts => _layouts.AsReadOnly();
    private readonly List<Layout> _layouts = new();

    public IReadOnlyCollection<Pedido> Pedidos => _pedidos.AsReadOnly();
    private readonly List<Pedido> _pedidos = new();

    private Cliente() { }

    public static Cliente Criar(string razaoSocial, string nomeFantasia, string cnpj, string? email = null, string? telefone = null)
    {
        return new Cliente
        {
            RazaoSocial = razaoSocial,
            NomeFantasia = nomeFantasia,
            Cnpj = cnpj,
            Email = email,
            Telefone = telefone
        };
    }

    public void Atualizar(string razaoSocial, string nomeFantasia, string? email, string? telefone)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Email = email;
        Telefone = telefone;
        SetAtualizado();
    }

    public void Desativar() { Ativo = false; SetAtualizado(); }
    public void Ativar() { Ativo = true; SetAtualizado(); }
}
