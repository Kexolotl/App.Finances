using Domain.Finances.DDD;
using System;

namespace Domain.Finances.StandingOrderAggregate.ValueObjects
{
    public class StandingOrderNextPaymentDate : ValueObject<StandingOrderNextPaymentDate>
    {
        public DateTime Value { get; private set; }

        private StandingOrderNextPaymentDate(DateTime value)
        {
            Value = value;
        }

        public static StandingOrderNextPaymentDate Create(DateTime value)
        {
            return new StandingOrderNextPaymentDate(value);
        }
    }
}
