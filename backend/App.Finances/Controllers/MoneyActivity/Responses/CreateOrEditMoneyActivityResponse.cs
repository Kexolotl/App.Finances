using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using System;
using System.Collections.Generic;

namespace MyFinances.Controllers.MoneyActivity.Responses
{
    public class CreateOrEditMoneyActivityResponse
    {
        public Guid Id { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public Guid? BusinessId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime CashActivityDate { get; set; }
        public bool? ImportantForTax { get; set; }
        public WarrantyResponse Warranty { get; set; }
        public PaymentType PaymentType { get; set; }
        public MoneyActivityType ActivityType { get; set; }

        public List<BusinessResponse> AvailableBusinesses { get; set; } = new List<BusinessResponse>();
        public List<CategoryResponse> AvailableCategories { get; set; } = new List<CategoryResponse>();

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

        public class WarrantyResponse
        {
            public int? LengthInMonth { get; set; }
            public DateTime PurchaseDate { get; set; }
        }
    }
}
