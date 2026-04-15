using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roupa.Domain.Entities;

namespace Roupa.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.RazaoSocial).HasMaxLength(200).IsRequired();
        builder.Property(c => c.NomeFantasia).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Cnpj).HasMaxLength(18).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Telefone).HasMaxLength(20);
        builder.HasIndex(c => c.Cnpj).IsUnique();

        builder.HasMany(c => c.Layouts)
            .WithOne(l => l.Cliente)
            .HasForeignKey(l => l.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Pedidos)
            .WithOne(p => p.Cliente)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
