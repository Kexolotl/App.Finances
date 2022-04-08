using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.Business.Commands
{
    public class DeleteBusinessCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
