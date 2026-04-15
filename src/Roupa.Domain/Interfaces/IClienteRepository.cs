using Roupa.Domain.Entities;

namespace Roupa.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Cliente?> ObterPorCnpjAsync(string cnpj, CancellationToken ct = default);
    Task<IEnumerable<Cliente>> ListarAsync(bool apenasAtivos = true, CancellationToken ct = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken ct = default);
    void Atualizar(Cliente cliente);
}
