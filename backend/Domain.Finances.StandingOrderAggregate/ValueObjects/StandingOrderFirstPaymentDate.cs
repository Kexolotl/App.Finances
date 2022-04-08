using Domain.Finances.DDD;
using System;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderFirstPaymentDate : ValueObject<StandingOrderFirstPaymentDate>
    {
        public DateTime Value { get; private set; }

        private StandingOrderFirstPaymentDate(DateTime value)
        {
            Value = value;
        }

        public static StandingOrderFirstPaymentDate Create(DateTime value)
        {
            return new StandingOrderFirstPaymentDate(value);
        }
    }
}
