using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.Category.Commands
{
    public class DeleteCategoryCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
