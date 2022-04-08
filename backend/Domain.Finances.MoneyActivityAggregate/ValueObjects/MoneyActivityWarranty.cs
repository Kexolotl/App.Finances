using Domain.Finances.DDD;
using System;

namespace Domain.Finances.MoneyActivityAggregate.ValueObjects
{
    public class MoneyActivityWarranty : ValueObject<MoneyActivityWarranty>
    {
        public DateTime PurchaseDate { get; }
        public int? LengthInMonth { get; }

        private MoneyActivityWarranty(DateTime purchaseDate, int? lengthInMonth)
        {
            PurchaseDate = purchaseDate;
            LengthInMonth = lengthInMonth;
        }

        public static MoneyActivityWarranty Create(DateTime purchaseDate, int? length)
        {
            return new MoneyActivityWarranty(purchaseDate, length);
        }
    }
}