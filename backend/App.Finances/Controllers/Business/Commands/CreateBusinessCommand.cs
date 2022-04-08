using Domain.Finances.DDD.Commands;

namespace MyFinances.Controllers.Business.Commands
{
    public class CreateBusinessCommand : ICommand
    {
        public string Name { get; set; }
    }
}
