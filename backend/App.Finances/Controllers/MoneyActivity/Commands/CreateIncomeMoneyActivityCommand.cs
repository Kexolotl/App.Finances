using Domain.Finances.DDD.Commands;
using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinances.Controllers.MoneyActivity.Commands
{
    public class CreateIncomeMoneyActivityCommand : ICommand
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
    }
}
