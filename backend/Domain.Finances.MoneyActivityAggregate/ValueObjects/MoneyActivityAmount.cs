using Domain.Finances.DDD;

namespace Domain.Finances.MoneyActivityAggregate.ValueObjects
{
    public class MoneyActivityAmount : ValueObject<MoneyActivityAmount>
    {
        public decimal Value { get; }

        private MoneyActivityAmount(decimal value)
        {
            Value = value;
        }

        public static MoneyActivityAmount Create(decimal value)
        {
            return new MoneyActivityAmount(value);
        }
    }
}
