using Domain.Finances.DDD;

namespace Domain.Finances.MoneyActivityAggregate.ValueObjects
{
    public class MoneyActivityDescription : ValueObject<MoneyActivityDescription>
    {
        public string Value { get; }

        private MoneyActivityDescription(string value)
        {
            Value = value;
        }

        public static MoneyActivityDescription Create(string value)
        {
            return new MoneyActivityDescription(value);
        }
    }
}
