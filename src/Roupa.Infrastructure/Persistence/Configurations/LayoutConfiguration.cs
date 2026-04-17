using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roupa.Domain.Entities;

namespace Roupa.Infrastructure.Persistence.Configurations;

public class LayoutConfiguration : IEntityTypeConfiguration<Layout>
{
    public void Configure(EntityTypeBuilder<Layout> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Modelo).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Descricao).HasMaxLength(1000).IsRequired();
        builder.Property(l => l.Tecido).HasMaxLength(300).IsRequired();
        builder.Property(l => l.Cores).HasMaxLength(300).IsRequired();
        builder.Property(l => l.PosicaoLogomarca).HasMaxLength(200).IsRequired();
        builder.Property(l => l.TamanhoLogomarca).HasMaxLength(100).IsRequired();
        builder.Property(l => l.CorLogomarca).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Outros).HasMaxLength(500);
        builder.Property(l => l.Opcoes).HasMaxLength(500);
        builder.Property(l => l.UrlImagemFrente).HasMaxLength(500);
        builder.Property(l => l.UrlImagemCostas).HasMaxLength(500);
        builder.Property(l => l.TipoProduto).HasConversion<int>();
        builder.Property(l => l.TipoLogomarca).HasConversion<int>();
    }
}
