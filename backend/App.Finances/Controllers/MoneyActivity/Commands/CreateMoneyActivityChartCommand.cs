using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.MoneyActivity.Commands
{
    public class CreateMoneyActivityChartCommand : ICommand
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BusinessId { get; set; }
    }
}
