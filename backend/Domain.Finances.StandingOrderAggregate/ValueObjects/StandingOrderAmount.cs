using Domain.Finances.DDD;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderAmount : ValueObject<StandingOrderAmount>
    {
        public decimal Value { get; private set; }

        private StandingOrderAmount(decimal value)
        {
            Value = value;
        }

        public static StandingOrderAmount Create(decimal value)
        {
            return new StandingOrderAmount(value);
        }
    }
}
