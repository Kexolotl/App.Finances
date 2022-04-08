using Domain.Finances.DDD;
using Domain.Finances.Utilities;

namespace Domain.Finances.CategoryAggregate.ValueObjects
{
    public class CategoryColorKey : ValueObject<CategoryColorKey>
    {
        public string Value { get; }

        private CategoryColorKey(string value)
        {
            Value = value;
        }

        public static CategoryColorKey Create(string value)
        {
            ArgumentValidator.NullOrNotStartsWith<CategoryColorKey>(nameof(Create), nameof(value), value, "#");
            ArgumentValidator.NullOrNotExactLength<CategoryColorKey>(nameof(Create), nameof(value), value, 7);

            return new CategoryColorKey(value);
        }
    }
}