using Domain.Finances.CategoryAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using Domain.Finances.Utilities;
using Common.JsonDatabase.DatabaseObject;

namespace Domain.Finances.CategoryAggregate
{
    public class Category : DatabaseObjectBase
    {
        public CategoryName Name { get; internal set; }
        public CategoryId ParentId { get; internal set; }
        public CategoryColorKey ColorKey { get; internal set; }

        private Category(CategoryName name, CategoryId parentId, CategoryColorKey color)
        {
            Name = name;
            ParentId = parentId;
            ColorKey = color;
        }

        public static Category Create(CategoryName name, CategoryColorKey colorKey, CategoryId parentId)
        {
            ArgumentValidator.ArgumentNullOrDefault<Category, CategoryName>(nameof(Create), nameof(name), name);
            ArgumentValidator.ArgumentNullOrDefault<Category, CategoryColorKey>(nameof(Create), nameof(name), colorKey);

            return new Category(name, parentId, colorKey);
        }

        public void UpdateName(CategoryName name)
        {
            ArgumentValidator.ArgumentNullOrDefault<Category, CategoryName>(nameof(UpdateName), nameof(name), name);

            Name = name;
        }

        public void UpdateColorKey(CategoryColorKey colorKey)
        {
            ArgumentValidator.ArgumentNullOrDefault<Category, CategoryColorKey>(nameof(UpdateColorKey), nameof(colorKey), colorKey);

            ColorKey = colorKey;
        }

        public void UpdateParentId(CategoryId parentId)
        {
            ParentId = parentId;
        }
    }
}
