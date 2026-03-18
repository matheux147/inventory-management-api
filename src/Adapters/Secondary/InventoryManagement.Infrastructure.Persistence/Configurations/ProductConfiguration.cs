using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.SupplierId)
            .HasColumnName("supplier_id")
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.AcquisitionCost)
            .HasColumnName("acquisition_cost")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.AcquisitionCostUsd)
            .HasColumnName("acquisition_cost_usd")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.AcquireDate)
            .HasColumnName("acquire_date")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.SoldDate)
            .HasColumnName("sold_date")
            .HasColumnType("datetime2");

        builder.Property(x => x.CancelDate)
            .HasColumnName("cancel_date")
            .HasColumnType("datetime2");

        builder.Property(x => x.ReturnDate)
            .HasColumnName("return_date")
            .HasColumnType("datetime2");

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SupplierId);
        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.Status);
    }
}