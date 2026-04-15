using Microsoft.EntityFrameworkCore;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Infrastructure.Persistence.Repositories;

public class LayoutRepository : ILayoutRepository
{
    private readonly AppDbContext _db;

    public LayoutRepository(AppDbContext db) => _db = db;

    public async Task<Layout?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Layouts.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<Layout>> ListarPorClienteAsync(Guid clienteId, CancellationToken ct = default) =>
        await _db.Layouts.Where(l => l.ClienteId == clienteId).OrderByDescending(l => l.CriadoEm).ToListAsync(ct);

    public async Task<IEnumerable<Layout>> ListarAsync(CancellationToken ct = default) =>
        await _db.Layouts.OrderByDescending(l => l.CriadoEm).ToListAsync(ct);

    public async Task AdicionarAsync(Layout layout, CancellationToken ct = default) =>
        await _db.Layouts.AddAsync(layout, ct);

    public void Atualizar(Layout layout) =>
        _db.Layouts.Update(layout);

    public void Remover(Layout layout) =>
        _db.Layouts.Remove(layout);
}
