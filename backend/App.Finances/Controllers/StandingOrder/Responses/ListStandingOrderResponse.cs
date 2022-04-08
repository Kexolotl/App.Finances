using System;

namespace MyFinances.Controllers.StandingOrder.Responses
{
    public class ListStandingOrderResponse
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string BusinessName { get; set; }
        public string Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public DateTime? FinalPaymentDate { get; set; }
        public DateTime NextPaymentDate { get; set; }
    }
}
