using Domain.Finances.SharedValueObjects;
using Domain.Finances.StandingOrderAggregate.ValueObjects;
using System;
using System.Collections.Generic;

namespace MyFinances.Controllers.StandingOrder.Responses
{
    public class CreateOrEditStandingOrderResponse
    {
        public Guid Id { get; set; }
        public string Amount { get; set; }
        public CategoryResponse Category { get; set; }

        public List<BusinessResponse> Businesses { get; set; } = new List<BusinessResponse>();
        public List<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public PaymentType PaymentType { get; set; }
        public BusinessResponse Business { get; set; }
        public StandingOrderFrequency Frequency { get; set; }
        public DateTime FirstPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? FinalPaymentDate { get; set; }
        public bool ImportantForTax { get; set; }
        public bool IsActive { get; set; }
        public MoneyActivityType ActivityType { get; set; }

        public class BusinessResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CategoryResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
