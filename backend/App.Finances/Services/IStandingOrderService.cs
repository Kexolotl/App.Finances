using System.Threading.Tasks;

namespace MyFinances.Services
{
    public interface IStandingOrderService
    {
        public Task ExcecuteStandingOrders();
    }
}
