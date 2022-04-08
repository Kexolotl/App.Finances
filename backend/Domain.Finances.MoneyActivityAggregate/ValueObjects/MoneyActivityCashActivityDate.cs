using Domain.Finances.DDD;
using System;

namespace Domain.Finances.MoneyActivityAggregate.ValueObjects
{
    public class MoneyActivityCashActivityDate : ValueObject<MoneyActivityCashActivityDate>
    {
        public DateTime Value { get; }

        private MoneyActivityCashActivityDate(DateTime value)
        {
            Value = value;
        }

        public static MoneyActivityCashActivityDate Create(DateTime value)
        {
            return new MoneyActivityCashActivityDate(value);
        }
    }
}
