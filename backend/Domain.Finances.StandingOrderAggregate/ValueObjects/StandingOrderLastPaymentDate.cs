using Domain.Finances.DDD;
using System;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderLastPaymentDate : ValueObject<StandingOrderLastPaymentDate>
    {
        public DateTime Value { get; private set; }

        private StandingOrderLastPaymentDate(DateTime value)
        {
            Value = value;
        }

        public static StandingOrderLastPaymentDate Create(DateTime value)
        {
            return new StandingOrderLastPaymentDate(value);
        }
    }
}
