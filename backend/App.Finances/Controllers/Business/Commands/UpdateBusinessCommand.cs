using Domain.Finances.DDD.Commands;
using System;

namespace MyFinances.Controllers.Business.Commands
{
    public class UpdateBusinessCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string ColorKey { get; set; }
    }
}
