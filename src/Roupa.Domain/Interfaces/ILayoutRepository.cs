using Roupa.Domain.Entities;

namespace Roupa.Domain.Interfaces;

public interface ILayoutRepository
{
    Task<Layout?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Layout>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default);
    Task<IEnumerable<Layout>> ListarAsync(CancellationToken ct = default);
    Task AdicionarAsync(Layout layout, CancellationToken ct = default);
    void Atualizar(Layout layout);
    void Remover(Layout layout);
}
