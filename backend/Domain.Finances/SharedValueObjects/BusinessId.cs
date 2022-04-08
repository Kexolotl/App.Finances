using Domain.Finances.DDD;
using Domain.Finances.Utilities;
using System;

namespace Domain.Finances.SharedValueObjects
{
    public class BusinessId : ValueObject<BusinessId>
    {
        public Guid Value { get; private set; }

        private BusinessId(Guid value)
        {
            Value = value;
        }

        public static BusinessId Create(Guid value)
        {
            ArgumentValidator.ArgumentNullOrDefault<BusinessId, Guid>(nameof(Create), nameof(value), value);
            return new BusinessId(value);
        }
    }
}