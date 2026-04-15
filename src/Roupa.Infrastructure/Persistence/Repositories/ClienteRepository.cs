using Microsoft.EntityFrameworkCore;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Infrastructure.Persistence.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _db;

    public ClienteRepository(AppDbContext db) => _db = db;

    public async Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Clientes.FindAsync(new object[] { id }, ct);

    public async Task<Cliente?> ObterPorCnpjAsync(string cnpj, CancellationToken ct = default) =>
        await _db.Clientes.FirstOrDefaultAsync(c => c.Cnpj == cnpj, ct);

    public async Task<IEnumerable<Cliente>> ListarAsync(bool apenasAtivos = true, CancellationToken ct = default)
    {
        var query = _db.Clientes.AsQueryable();
        if (apenasAtivos) query = query.Where(c => c.Ativo);
        return await query.OrderBy(c => c.NomeFantasia).ToListAsync(ct);
    }

    public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default) =>
        await _db.Clientes.AddAsync(cliente, ct);

    public void Atualizar(Cliente cliente) =>
        _db.Clientes.Update(cliente);
}
