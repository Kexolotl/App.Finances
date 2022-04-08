using Domain.Finances.DDD.Commands;
using Domain.Finances.SharedValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinances.Controllers.MoneyActivity.Commands
{
    public class CreateExpenditureMoneyActivityCommand : ICommand
    {
        [Required]
        public string Amount { get; set; }

        public string Description { get; set; }

        public Guid? BusinessId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public DateTime CashActivityDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool ImportantForTax { get; set; }
        public CreateWarranty Warranty { get; set; }

        public class CreateWarranty
        {
            public DateTime PurchaseDate { get; set; }
            public int LegthInMonth { get; set; }
        }
    }
}
