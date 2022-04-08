using Domain.Finances.DDD;

namespace Domain.Finances.MoneyActivityAggregate.ValueObjects
{
    public class MoneyActivityReceiptStored : ValueObject<MoneyActivityReceiptStored>
    {
        public bool Value { get; }

        private MoneyActivityReceiptStored(bool value)
        {
            Value = value;
        }

        public static MoneyActivityReceiptStored Create(bool value)
        {
            return new MoneyActivityReceiptStored(value);
        }
    }
}