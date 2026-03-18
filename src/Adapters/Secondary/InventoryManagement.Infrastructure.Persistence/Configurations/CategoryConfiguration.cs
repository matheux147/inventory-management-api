using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Shortcode)
            .HasColumnName("shortcode")
            .HasMaxLength(50)
            .HasConversion(
                shortcode => shortcode.Value,
                value => CategoryShortcode.Create(value))
            .IsRequired();

        builder.HasIndex(x => x.Shortcode)
            .IsUnique();

        builder.Property(x => x.ParentCategoryId)
            .HasColumnName("parent_category_id");

        builder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(Category.Children))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}