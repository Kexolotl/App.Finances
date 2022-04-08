using Domain.Finances.DDD;

namespace Domain.Finances.CategoryAggregate.ValueObjects
{
    public class CategoryName : ValueObject<CategoryName>
    {
        public string Value { get; }

        private CategoryName(string value)
        {
            Value = value;
        }

        public static CategoryName Create(string value)
        {
            return new CategoryName(value);
        }
    }
}