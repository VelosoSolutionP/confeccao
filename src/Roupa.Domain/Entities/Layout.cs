using Roupa.Domain.Common;
using Roupa.Domain.Enums;

namespace Roupa.Domain.Entities;

public class Layout : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public Cliente Cliente { get; private set; } = default!;

    public string Modelo { get; private set; } = default!;
    public string Descricao { get; private set; } = default!;
    public TipoProduto TipoProduto { get; private set; }
    public string Tecido { get; private set; } = default!;
    public string Cores { get; private set; } = default!;

    public TipoLogomarca TipoLogomarca { get; private set; }
    public string PosicaoLogomarca { get; private set; } = default!;
    public string TamanhoLogomarca { get; private set; } = default!;
    public string CorLogomarca { get; private set; } = default!;
    public string? Outros { get; private set; }
    public string? Opcoes { get; private set; }

    public string? UrlImagemFrente { get; private set; }
    public string? UrlImagemCostas { get; private set; }

    private Layout() { }

    public static Layout Criar(
        Guid clienteId,
        string modelo,
        string descricao,
        TipoProduto tipoProduto,
        string tecido,
        string cores,
        TipoLogomarca tipoLogomarca,
        string posicaoLogomarca,
        string tamanhoLogomarca,
        string corLogomarca,
        string? outros = null,
        string? opcoes = null)
    {
        return new Layout
        {
            ClienteId = clienteId,
            Modelo = modelo,
            Descricao = descricao,
            TipoProduto = tipoProduto,
            Tecido = tecido,
            Cores = cores,
            TipoLogomarca = tipoLogomarca,
            PosicaoLogomarca = posicaoLogomarca,
            TamanhoLogomarca = tamanhoLogomarca,
            CorLogomarca = corLogomarca,
            Outros = outros,
            Opcoes = opcoes
        };
    }

    public void Atualizar(
        string modelo, string descricao, TipoProduto tipoProduto,
        string tecido, string cores, TipoLogomarca tipoLogomarca,
        string posicaoLogomarca, string tamanhoLogomarca, string corLogomarca,
        string? outros, string? opcoes = null)
    {
        Modelo = modelo;
        Descricao = descricao;
        TipoProduto = tipoProduto;
        Tecido = tecido;
        Cores = cores;
        TipoLogomarca = tipoLogomarca;
        PosicaoLogomarca = posicaoLogomarca;
        TamanhoLogomarca = tamanhoLogomarca;
        CorLogomarca = corLogomarca;
        Outros = outros;
        Opcoes = opcoes;
        SetAtualizado();
    }

    public void SetImagens(string? frente, string? costas)
    {
        UrlImagemFrente = frente;
        UrlImagemCostas = costas;
        SetAtualizado();
    }
}
