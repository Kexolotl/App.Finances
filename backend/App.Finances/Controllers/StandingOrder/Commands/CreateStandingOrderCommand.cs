using Domain.Finances.DDD.Commands;
using Domain.Finances.SharedValueObjects;
using Domain.Finances.StandingOrderAggregate.ValueObjects;
using System;

namespace MyFinances.Controllers.StandingOrder.Commands
{
    public class CreateStandingOrderCommand : ICommand
    {
        public Guid CategoryId { get; set; }
        public Guid? BusinessId { get; set; }
        public string Amount { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public DateTime? FinalPaymentDate { get; set; }
        public StandingOrderFrequency Frequency { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
