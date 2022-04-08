using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.Category.Commands
{
    public class CreateCategoryCommand : ICommand
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string ColorKey { get; set; }
    }
}
