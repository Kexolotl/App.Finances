using Domain.Finances.DDD;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderIsActive : ValueObject<StandingOrderIsActive>
    {
        public bool Value { get; private set; }

        private StandingOrderIsActive(bool value)
        {
            Value = value;
        }

        public static StandingOrderIsActive Create(bool value)
        {
            return new StandingOrderIsActive(value);
        }
    }
}
