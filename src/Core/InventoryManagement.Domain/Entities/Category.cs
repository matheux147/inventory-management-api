using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

public class Category : Entity
{
    private readonly List<Category> _children = [];

    public string Name { get; private set; } = null!;
    public CategoryShortcode Shortcode { get; private set; } = null!;

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    public IReadOnlyCollection<Category> Children => _children.AsReadOnly();

    private Category()
    {
    }

    public Category(string name, CategoryShortcode shortcode, Category? parentCategory = null)
    {
        SetName(name);
        SetShortcode(shortcode);
        SetParent(parentCategory);
    }

    public void Rename(string name)
    {
        SetName(name);
    }

    public void ChangeShortcode(CategoryShortcode shortcode)
    {
        SetShortcode(shortcode);
    }

    public void SetParent(Category? parentCategory)
    {
        if (parentCategory is not null && parentCategory.Id == Id)
            throw new DomainException("A category cannot be its own parent.");

        if (ParentCategory is not null)
            ParentCategory.RemoveChildInternal(this);

        ParentCategory = parentCategory;
        ParentCategoryId = parentCategory?.Id;

        if (parentCategory is not null)
            parentCategory.AddChildInternal(this);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");

        Name = name.Trim();
    }

    private void SetShortcode(CategoryShortcode shortcode)
    {
        Shortcode = shortcode ?? throw new DomainException("Category shortcode is required.");
    }

    private void AddChildInternal(Category child)
    {
        if (_children.Any(x => x.Id == child.Id))
            return;

        _children.Add(child);
    }

    private void RemoveChildInternal(Category child)
    {
        var existing = _children.FirstOrDefault(x => x.Id == child.Id);
        if (existing is not null)
            _children.Remove(existing);
    }
}
