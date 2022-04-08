using Domain.Finances.DDD;

namespace Domain.Finances.BusinessAggregate.ValueObjects
{
    public class BusinessName : ValueObject<BusinessName>
    {
        public string Value { get; }

        private BusinessName(string value)
        {
            Value = value;
        }

        public static BusinessName Create(string value)
        {
            return new BusinessName(value);
        }
    }
}
