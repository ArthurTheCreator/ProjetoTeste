﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoTeste.Infrastructure.Persistence.Entity;

namespace ProjetoTeste.Infrastructure.Persistence.Mapping;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("marca");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name).HasColumnName("nome");
        builder.Property(b => b.Name).IsRequired();
        builder.Property(b => b.Name).HasMaxLength(40);

        builder.Property(b => b.Code).HasColumnName("codigo");
        builder.Property(b => b.Code).IsRequired();
        builder.Property(b => b.Code).HasMaxLength(6);

        builder.Property(b => b.Description).HasColumnName("descricao");
        builder.Property(b => b.Description).HasMaxLength(100);
    }
}