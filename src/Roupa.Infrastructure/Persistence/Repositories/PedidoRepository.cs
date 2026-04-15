using Microsoft.EntityFrameworkCore;
using Roupa.Domain.Entities;
using Roupa.Domain.Enums;
using Roupa.Domain.Interfaces;

namespace Roupa.Infrastructure.Persistence.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _db;

    public PedidoRepository(AppDbContext db) => _db = db;

    public async Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Pedido?> ObterPorNumeroAsync(string numero, CancellationToken ct = default) =>
        await _db.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Numero == numero, ct);

    public async Task<IEnumerable<Pedido>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default) =>
        await _db.Pedidos.Where(p => p.ClienteId == clienteId).OrderByDescending(p => p.CriadoEm).ToListAsync(ct);

    public async Task<IEnumerable<Pedido>> ListarPorStatusAsync(StatusPedido status, CancellationToken ct = default) =>
        await _db.Pedidos.Where(p => p.Status == status).OrderByDescending(p => p.CriadoEm).ToListAsync(ct);

    public async Task<IEnumerable<Pedido>> ListarAsync(CancellationToken ct = default) =>
        await _db.Pedidos.Include(p => p.Itens).OrderByDescending(p => p.CriadoEm).ToListAsync(ct);

    public async Task AdicionarAsync(Pedido pedido, CancellationToken ct = default) =>
        await _db.Pedidos.AddAsync(pedido, ct);

    public void Atualizar(Pedido pedido) =>
        _db.Pedidos.Update(pedido);
}
