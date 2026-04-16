using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roupa.Domain.Entities;

namespace Roupa.Infrastructure.Persistence.Configurations;

public class ParceiroConfiguration : IEntityTypeConfiguration<Parceiro>
{
    public void Configure(EntityTypeBuilder<Parceiro> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nome).HasMaxLength(200).IsRequired();
        builder.Property(p => p.CpfCnpj).HasMaxLength(20);
        builder.Property(p => p.Telefone).HasMaxLength(20);
        builder.Property(p => p.Email).HasMaxLength(200);
        builder.Property(p => p.Especialidade).HasMaxLength(100);
        builder.Property(p => p.Cidade).HasMaxLength(100);
        builder.Property(p => p.Observacoes).HasMaxLength(1000);
        builder.HasIndex(p => p.Nome);
    }
}
