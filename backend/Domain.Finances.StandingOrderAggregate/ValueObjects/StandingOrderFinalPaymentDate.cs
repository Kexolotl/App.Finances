using Domain.Finances.DDD;
using System;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderFinalPaymentDate : ValueObject<StandingOrderFinalPaymentDate>
    {
        public DateTime Value { get; private set; }

        private StandingOrderFinalPaymentDate(DateTime value)
        {
            Value = value;
        }

        public static StandingOrderFinalPaymentDate Create(DateTime value)
        {
            return new StandingOrderFinalPaymentDate(value);
        }
    }
}
