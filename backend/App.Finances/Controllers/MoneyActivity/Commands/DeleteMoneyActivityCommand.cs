using Domain.Finances.DDD.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyFinances.Controllers.MoneyActivity.Commands
{
    public class DeleteMoneyActivityCommand : ICommand
    {
        [Required]
        public Guid Id { get; set; }
    }
}
