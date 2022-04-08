using Domain.Finances.DDD;
using Domain.Finances.Utilities;
using System;

namespace Domain.Finances.SharedValueObjects
{
    public class CategoryId : ValueObject<CategoryId>
    {
        public Guid Value { get; private set; }

        private CategoryId(Guid value)
        {
            Value = value;
        }

        public static CategoryId Create(Guid value)
        {
            ArgumentValidator.ArgumentNullOrDefault<CategoryId, Guid>(nameof(Create), nameof(value), value);
            return new CategoryId(value);
        }
    }
}