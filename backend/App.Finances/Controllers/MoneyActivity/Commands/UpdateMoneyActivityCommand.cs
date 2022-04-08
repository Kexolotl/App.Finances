using Domain.Finances.DDD.Commands;
using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinances.Controllers.MoneyActivity.Commands
{
    public class UpdateMoneyActivityCommand : ICommand
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Amount { get; set; }

        public Guid? BusinessId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public DateTime CashActivityDate { get; set; }

        public string Description { get; set; }
        public MoneyActivityType ActivityType { get; set; }
        public UpdateWarranty Warranty { get; set; }
        public bool? ImportantForTax { get; set; }
    }

    public class UpdateWarranty
    {
        [Required]
        public DateTime PurchaseDate { get; set; }
        public int? LengthInMonth { get; set; }
    }
}
