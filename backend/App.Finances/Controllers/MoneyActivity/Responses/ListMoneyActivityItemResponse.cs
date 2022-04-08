using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using System;

namespace MyFinances.Controllers.MoneyActivity.Responses
{
    public class ListMoneyActivityItemResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string BusinessName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CashActivityDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public MoneyActivityType ActivityType { get; set; }
    }
}
