using Domain.Finances.SharedValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinances.Controllers.MoneyActivity.Responses;
using MyFinances.Helper;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Controllers.MoneyActivity
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoneyActivityChartController : ControllerBase
    {
        private readonly IDatabaseObjectService<Domain.Finances.MoneyActivityAggregate.MoneyActivity> _moneyActivityService;
        private readonly IDatabaseObjectService<Domain.Finances.CategoryAggregate.Category> _categoryService;
        private readonly IDatabaseObjectService<Domain.Finances.BusinessAggregate.Business> _businessService;
        private readonly ILogger<MoneyActivityChartController> _logger;

        public MoneyActivityChartController(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<MoneyActivityChartController> logger)
        {
            _moneyActivityService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.MoneyActivityAggregate.MoneyActivity>();
            _categoryService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.CategoryAggregate.Category>();
            _businessService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.BusinessAggregate.Business>();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoneyActivityPieChartData(DateTime from, DateTime to, Guid? businessId, Guid? categoryId)
        {
            var response = new MoneyActivityPieChartDataResponse();
            var availableCategories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            var allMoneyActivities = await _moneyActivityService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            var moneyActivitiesInRange = allMoneyActivities.Where(x => x.CashActivityDate.Value.Date >= from.Date && x.CashActivityDate.Value.Date <= to.Date && x.ActivityType == MoneyActivityType.Expenditure).ToList();
            if (businessId.HasValue)
            {
                moneyActivitiesInRange = moneyActivitiesInRange.Where(x => x.BusinessId != null && x.BusinessId.Value == businessId).ToList();
            }

            var moneyActivities = new Dictionary<Guid, List<decimal>>();
            if (categoryId.HasValue)
            {
                moneyActivities = BuildPieChartMoneyActivitiesBySingleCategory(moneyActivitiesInRange, availableCategories.ToArray(), categoryId.Value);
            }
            else
            {
                moneyActivities = BuildPieChartMoneyActivitiesByRoot(moneyActivitiesInRange, availableCategories.ToArray());
            }

            foreach (var (key, amounts) in moneyActivities)
            {
                if (amounts.Count == 0)
                {
                    continue;
                }

                var category = availableCategories.Single(x => x.Id == key);

                var amount = amounts.Sum();

                response.MoneyActivities.Add(new MoneyActivityPieChartDataResponse.MoneyActivityResponse
                {
                    Amount = decimal.Round(amounts.Sum(), 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture),
                    Category = category.Name.Value
                });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoneyActivityXyChartData(DateTime from, DateTime to, Guid? businessId, Guid? categoryId)
        {
            var response = new MoneyActivityXyChartDataResponse();
            var availableCategories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            var allMoneyActivities = await _moneyActivityService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            var moneyActivitiesInRange = allMoneyActivities.Where(x => x.CashActivityDate.Value.Date >= from.Date && x.CashActivityDate.Value.Date <= to.Date && x.ActivityType == MoneyActivityType.Expenditure).ToList();
            if (businessId.HasValue)
            {
                moneyActivitiesInRange = moneyActivitiesInRange.Where(x => x.BusinessId != null && x.BusinessId.Value == businessId).ToList();
            }

            var moneyActivities = new Dictionary<Guid, List<decimal>>();
            if (categoryId.HasValue)
            {
                moneyActivities = BuildPieChartMoneyActivitiesBySingleCategory(moneyActivitiesInRange, availableCategories.ToArray(), categoryId.Value);
            }
            else
            {
                moneyActivities = BuildPieChartMoneyActivitiesByRoot(moneyActivitiesInRange, availableCategories.ToArray());
            }

            foreach (var (key, amounts) in moneyActivities)
            {
                if (amounts.Count == 0)
                {
                    continue;
                }

                var category = availableCategories.Single(x => x.Id == key);

                var amount = amounts.Sum();

                response.MoneyActivities.Add(new MoneyActivityXyChartDataResponse.MoneyActivityResponse
                {
                    Amount = decimal.Round(amounts.Sum(), 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture),
                    Category = category.Name.Value
                });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoneyActivityChartOverview(DateTime from, DateTime to)
        {
            _logger.LogInformation($"Get money statisc for interval {from} - {to}.");

            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            var businesses = await _businessService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            var response = new MoneyActivityStatisticResponse();

            foreach (var category in categories.Where(x => x.ParentId == null))
            {
                var categoryResponse = new MoneyActivityStatisticResponse.CategoryResponse
                {
                    Id = category.Id,
                    ParentId = category.ParentId?.Value,
                    Name = category.Name.Value
                };

                response.Categories.Add(categoryResponse);
            }

            foreach (var businessPlace in businesses)
            {
                var businessPlaceResponse = new MoneyActivityStatisticResponse.BusinessResponse
                {
                    Id = businessPlace.Id,
                    Name = businessPlace.Name.Value
                };
                response.Businesses.Add(businessPlaceResponse);
            }

            return Ok(response);
        }

        private static Dictionary<Guid, List<decimal>> BuildPieChartMoneyActivitiesByRoot(IReadOnlyCollection<Domain.Finances.MoneyActivityAggregate.MoneyActivity> moneyActivitiesInRange, IReadOnlyCollection<Domain.Finances.CategoryAggregate.Category> availableCategories)
        {
            var moneyActivities = new Dictionary<Guid, List<decimal>>();
            foreach (var item in moneyActivitiesInRange)
            {
                var root = CategoryHelper.FindRoot(availableCategories, item.CategoryId);
                if (!moneyActivities.ContainsKey(root.Id))
                {
                    moneyActivities.Add(root.Id, new List<decimal> { item.Amount.Value });
                    continue;
                }
                moneyActivities[root.Id].Add(item.Amount.Value);
            }
            return moneyActivities;
        }

        private static Dictionary<Guid, List<decimal>> BuildPieChartMoneyActivitiesBySingleCategory(IReadOnlyCollection<Domain.Finances.MoneyActivityAggregate.MoneyActivity> moneyActivitiesInRange, IReadOnlyCollection<Domain.Finances.CategoryAggregate.Category> availableCategories, Guid categoryId)
        {
            var moneyActivities = new Dictionary<Guid, List<decimal>>();
            foreach (var item in moneyActivitiesInRange)
            {
                var root = CategoryHelper.FindRoot(availableCategories, item.CategoryId);
                if (root.Id != categoryId)
                {
                    continue;
                }

                if (!moneyActivities.ContainsKey(item.CategoryId.Value))
                {
                    moneyActivities.Add(item.CategoryId.Value, new List<decimal> { item.Amount.Value });
                    continue;
                }
                moneyActivities[item.CategoryId.Value].Add(item.Amount.Value);
            }
            return moneyActivities;
        }
    }
}
