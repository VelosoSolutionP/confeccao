using Roupa.Domain.Enums;

namespace Roupa.Application.Layouts.DTOs;

public record LayoutDto(
    Guid Id,
    Guid ClienteId,
    string NomeCliente,
    string Modelo,
    string Descricao,
    TipoProduto TipoProduto,
    string Tecido,
    string Cores,
    TipoLogomarca TipoLogomarca,
    string PosicaoLogomarca,
    string TamanhoLogomarca,
    string CorLogomarca,
    string? Outros,
    string? UrlImagemFrente,
    string? UrlImagemCostas,
    DateTime CriadoEm,
    bool Aprovado = false);

public record CriarLayoutDto(
    Guid ClienteId,
    string Modelo,
    string Descricao,
    TipoProduto TipoProduto,
    string Tecido,
    string Cores,
    TipoLogomarca TipoLogomarca,
    string PosicaoLogomarca,
    string TamanhoLogomarca,
    string CorLogomarca,
    string? Outros);
