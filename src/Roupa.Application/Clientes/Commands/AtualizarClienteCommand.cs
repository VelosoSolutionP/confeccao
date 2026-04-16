using MediatR;
using Roupa.Application.Clientes.DTOs;
using Roupa.Application.Common;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Clientes.Commands;

public record AtualizarClienteCommand(
    Guid Id,
    string RazaoSocial,
    string NomeFantasia,
    string? Email,
    string? Telefone) : IRequest<Result<ClienteDto>>;

public class AtualizarClienteCommandHandler : IRequestHandler<AtualizarClienteCommand, Result<ClienteDto>>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public AtualizarClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Result<ClienteDto>> Handle(AtualizarClienteCommand request, CancellationToken ct)
    {
        var c = await _repo.ObterPorIdAsync(request.Id, ct);
        if (c is null) return Result<ClienteDto>.Falha("Cliente não encontrado.");

        c.Atualizar(request.RazaoSocial, request.NomeFantasia, request.Email, request.Telefone);
        _repo.Atualizar(c);
        await _uow.SalvarAsync(ct);

        return Result<ClienteDto>.Ok(new ClienteDto(c.Id, c.RazaoSocial, c.NomeFantasia, c.Cnpj, c.Email, c.Telefone, c.Ativo, c.CriadoEm));
    }
}
