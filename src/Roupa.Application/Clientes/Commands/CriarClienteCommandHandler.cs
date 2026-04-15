using MediatR;
using Roupa.Application.Clientes.DTOs;
using Roupa.Application.Common;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Application.Clientes.Commands;

public class CriarClienteCommandHandler : IRequestHandler<CriarClienteCommand, Result<ClienteDto>>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _uow;

    public CriarClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork uow)
    {
        _clienteRepository = clienteRepository;
        _uow = uow;
    }

    public async Task<Result<ClienteDto>> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var existente = await _clienteRepository.ObterPorCnpjAsync(request.Cnpj, cancellationToken);
        if (existente is not null)
            return Result<ClienteDto>.Falha("Já existe um cliente com este CNPJ.");

        var cliente = Cliente.Criar(request.RazaoSocial, request.NomeFantasia, request.Cnpj, request.Email, request.Telefone);
        await _clienteRepository.AdicionarAsync(cliente, cancellationToken);
        await _uow.SalvarAsync(cancellationToken);

        return Result<ClienteDto>.Ok(MapToDto(cliente));
    }

    private static ClienteDto MapToDto(Cliente c) =>
        new(c.Id, c.RazaoSocial, c.NomeFantasia, c.Cnpj, c.Email, c.Telefone, c.Ativo, c.CriadoEm);
}
