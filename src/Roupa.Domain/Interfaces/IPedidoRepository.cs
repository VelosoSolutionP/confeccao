using Roupa.Domain.Entities;
using Roupa.Domain.Enums;

namespace Roupa.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Pedido?> ObterPorNumeroAsync(string numero, CancellationToken ct = default);
    Task<IEnumerable<Pedido>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default);
    Task<IEnumerable<Pedido>> ListarPorStatusAsync(StatusPedido status, CancellationToken ct = default);
    Task<IEnumerable<Pedido>> ListarAsync(CancellationToken ct = default);
    Task AdicionarAsync(Pedido pedido, CancellationToken ct = default);
    void Atualizar(Pedido pedido);
}
