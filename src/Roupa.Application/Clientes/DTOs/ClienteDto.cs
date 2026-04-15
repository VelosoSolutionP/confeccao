namespace Roupa.Application.Clientes.DTOs;

public record ClienteDto(
    Guid Id,
    string RazaoSocial,
    string NomeFantasia,
    string Cnpj,
    string? Email,
    string? Telefone,
    bool Ativo,
    DateTime CriadoEm);

public record CriarClienteDto(
    string RazaoSocial,
    string NomeFantasia,
    string Cnpj,
    string? Email,
    string? Telefone);

public record AtualizarClienteDto(
    string RazaoSocial,
    string NomeFantasia,
    string? Email,
    string? Telefone);
