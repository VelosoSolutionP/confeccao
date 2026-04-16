using Microsoft.EntityFrameworkCore;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Infrastructure.Persistence.Repositories;

public class ParceiroRepository : IParceiroRepository
{
    private readonly AppDbContext _db;

    public ParceiroRepository(AppDbContext db) => _db = db;

    public async Task<Parceiro?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Parceiros.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<Parceiro>> ListarAsync(bool apenasAtivos = true, CancellationToken ct = default)
    {
        var query = _db.Parceiros.AsQueryable();
        if (apenasAtivos) query = query.Where(p => p.Ativo);
        return await query.OrderBy(p => p.Nome).ToListAsync(ct);
    }

    public async Task AdicionarAsync(Parceiro parceiro, CancellationToken ct = default) =>
        await _db.Parceiros.AddAsync(parceiro, ct);

    public void Atualizar(Parceiro parceiro) =>
        _db.Parceiros.Update(parceiro);
}
