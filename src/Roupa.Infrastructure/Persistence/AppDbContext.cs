using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Roupa.Domain.Entities;
using Roupa.Domain.Interfaces;

namespace Roupa.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Layout> Layouts => Set<Layout>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public async Task<int> SalvarAsync(CancellationToken ct = default) =>
        await SaveChangesAsync(ct);
}
