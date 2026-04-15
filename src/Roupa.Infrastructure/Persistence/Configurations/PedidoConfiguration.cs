using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roupa.Domain.Entities;

namespace Roupa.Infrastructure.Persistence.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Numero).HasMaxLength(50).IsRequired();
        builder.HasIndex(p => p.Numero).IsUnique();
        builder.Property(p => p.Status).HasConversion<int>();
        builder.Property(p => p.Observacoes).HasMaxLength(1000);
        builder.Ignore(p => p.Total);

        builder.HasMany(p => p.Itens)
            .WithOne(i => i.Pedido)
            .HasForeignKey(i => i.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Tamanho).HasMaxLength(10).IsRequired();
        builder.Property(i => i.PrecoUnitario).HasPrecision(10, 2);
        builder.Ignore(i => i.Total);
    }
}
