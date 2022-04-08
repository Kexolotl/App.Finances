using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.StandingOrder.Commands
{
    public class UpdateStandingOrderCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? FinalPaymentDate { get; set; }
    }
}
