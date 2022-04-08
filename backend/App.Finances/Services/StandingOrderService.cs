using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using Domain.Finances.StandingOrderAggregate.ValueObjects;
using Microsoft.Extensions.Logging;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Services
{
    public class StandingOrderService : IStandingOrderService
    {
        private readonly IDatabaseObjectServiceFactory _databaseObjectServiceFactory;
        private readonly ILogger<StandingOrderService> _logger;

        public StandingOrderService(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<StandingOrderService> logger)
        {
            _databaseObjectServiceFactory = databaseObjectServiceFactory;
            _logger = logger;
        }

        public async Task ExcecuteStandingOrders()
        {
            _logger.LogInformation("Execute standing order service");

            var standingOrderService = _databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.StandingOrderAggregate.StandingOrder>();
            var moneyActivityService = _databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.MoneyActivityAggregate.MoneyActivity>();
            var standingOrders = await standingOrderService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            var now = DateTime.UtcNow;

            var dirtyStandingOrders = new List<Domain.Finances.StandingOrderAggregate.StandingOrder>();
            var newMoneyActivities = new List<Domain.Finances.MoneyActivityAggregate.MoneyActivity>();

            foreach (var standingOrder in standingOrders)
            {
                if (!standingOrder.IsActive.Value)
                {
                    _logger.LogInformation("Skip standing order because it is not active");
                    continue;
                }

                if (standingOrder.FinalPaymentDate != null && now.Date > standingOrder.FinalPaymentDate.Value.Date)
                {
                    _logger.LogInformation("Turn off standing order active state for {id}, because finalPaymentDate is reached.", standingOrder.Id, now);
                    continue;
                }

                var currentPaymentDate = standingOrder.FirstPaymentDate.Value;
                if (standingOrder.NextPaymentDate != null)
                {
                    currentPaymentDate = standingOrder.NextPaymentDate.Value;
                }

                if (now.Date < currentPaymentDate.Date)
                {
                    _logger.LogInformation("Skip standing order because current payment date is not now.");
                    continue;
                }

                _logger.LogInformation("Run scheduler for standing order with id {id} and time {now}", standingOrder.Id, now.ToString());

                switch (standingOrder.MoneyActivityType)
                {
                    case MoneyActivityType.Income:
                        throw new NotImplementedException();
                    case MoneyActivityType.Saving:
                        throw new NotImplementedException();
                    case MoneyActivityType.Expenditure:
                        _logger.LogInformation("Create expenditure with amount ", standingOrder.Amount.Value);
                        var expenditureMoneyActivity = Domain.Finances.MoneyActivityAggregate.MoneyActivity.CreateExpenditure(MoneyActivityAmount.Create(standingOrder.Amount.Value), standingOrder.BusinessId, standingOrder.CategoryId, MoneyActivityCashActivityDate.Create(DateTime.UtcNow), null, standingOrder.PaymentType, standingOrder.ImportantForTax, null);
                        newMoneyActivities.Add(expenditureMoneyActivity);
                        break;
                }

                DateTime nextPaymentDate;
                switch (standingOrder.Frequency)
                {
                    case StandingOrderFrequency.Daily:
                        nextPaymentDate = standingOrder.NextPaymentDate != null ? standingOrder.NextPaymentDate.Value.AddDays(1) : standingOrder.FirstPaymentDate.Value.AddDays(1);
                        break;
                    case StandingOrderFrequency.Monthly:
                        nextPaymentDate = standingOrder.NextPaymentDate != null ? standingOrder.NextPaymentDate.Value.AddMonths(1) : standingOrder.FirstPaymentDate.Value.AddMonths(1);
                        break;
                    case StandingOrderFrequency.Yearly:
                        nextPaymentDate = standingOrder.NextPaymentDate != null ? standingOrder.NextPaymentDate.Value.AddYears(1) : standingOrder.FirstPaymentDate.Value.AddYears(1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Frequency out of range.");
                }

                standingOrder.UpdateLastPaymentDate(StandingOrderLastPaymentDate.Create(DateTime.UtcNow));
                standingOrder.UpdateNextPaymentDate(StandingOrderNextPaymentDate.Create(nextPaymentDate));

                dirtyStandingOrders.Add(standingOrder);
            }

            foreach (var moneyActivity in newMoneyActivities)
            {
                await moneyActivityService.StoreDatabaseObjectAsync(moneyActivity).ConfigureAwait(false);
            }

            foreach (var dirtyStandingOrder in dirtyStandingOrders)
            {
                await standingOrderService.StoreDatabaseObjectAsync(dirtyStandingOrder).ConfigureAwait(false);
            }
        }
    }
}
