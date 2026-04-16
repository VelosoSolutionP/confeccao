namespace Roupa.Application.Parceiros.DTOs;

public record ParceiroDto(
    Guid Id,
    string Nome,
    string? CpfCnpj,
    string? Telefone,
    string? Email,
    string? Especialidade,
    string? Cidade,
    string? Observacoes,
    bool Ativo,
    DateTime CriadoEm);

public record CriarParceiroDto(
    string Nome,
    string? CpfCnpj,
    string? Telefone,
    string? Email,
    string? Especialidade,
    string? Cidade,
    string? Observacoes);
