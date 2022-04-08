using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinances.Controllers.MoneyActivity.Responses;
using MyFinances.Controllers.MoneyActivity.Commands;
using MyFinances.Helper;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;

namespace MyFinances.Controllers.MoneyActivity
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoneyActivityController : ControllerBase
    {
        private readonly IDatabaseObjectService<Domain.Finances.MoneyActivityAggregate.MoneyActivity> _moneyActivityService;
        private readonly IDatabaseObjectService<Domain.Finances.CategoryAggregate.Category> _categoryService;
        private readonly IDatabaseObjectService<Domain.Finances.BusinessAggregate.Business> _businessService;
        private readonly ILogger<MoneyActivityController> _logger;

        public MoneyActivityController(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<MoneyActivityController> logger)
        {
            _moneyActivityService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.MoneyActivityAggregate.MoneyActivity>();
            _categoryService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.CategoryAggregate.Category>();
            _businessService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.BusinessAggregate.Business>();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _logger.LogInformation("List money activities.");

            var response = new List<ListMoneyActivityItemResponse>();

            var moneyActivities = await _moneyActivityService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            var preparedCategories = new Dictionary<Guid, string>();
            foreach (var moneyActivity in moneyActivities)
            {
                var category = categories.Single(x => x.Id == moneyActivity.CategoryId.Value);
                var categoryName = string.Empty;
                if (!preparedCategories.ContainsKey(moneyActivity.CategoryId.Value))
                {
                    categoryName = CategoryHelper.GetFullName(category, categories);
                    preparedCategories.Add(moneyActivity.CategoryId.Value, categoryName);
                }
                else
                {
                    categoryName = preparedCategories[category.Id];
                }

                string businessName = null;
                if (moneyActivity.BusinessId != null)
                {
                    var business = await _businessService.GetDatabaseObjectAsync(moneyActivity.BusinessId.Value).ConfigureAwait(false);
                    businessName = business.Name.Value;
                }
                response.Add(new ListMoneyActivityItemResponse
                {
                    Id = moneyActivity.Id,
                    Amount = moneyActivity.Amount.Value,
                    CategoryName = CategoryHelper.GetFullName(category, categories),
                    BusinessName = businessName,
                    CashActivityDate = moneyActivity.CashActivityDate.Value,
                    ActivityType = moneyActivity.ActivityType,
                    PaymentType = moneyActivity.PaymentType
                });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            _logger.LogInformation($"Get money activity with id {id}.");
            var moneyActivityResponse = new CreateOrEditMoneyActivityResponse();

            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var item in categories)
            {
                moneyActivityResponse.AvailableCategories.Add(new CreateOrEditMoneyActivityResponse.CategoryResponse
                {
                    Id = item.Id,
                    Name = CategoryHelper.GetFullName(item, categories)
                });
            }

            var businesses = await _businessService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var item in businesses)
            {
                moneyActivityResponse.AvailableBusinesses.Add(new CreateOrEditMoneyActivityResponse.BusinessResponse
                {
                    Id = item.Id,
                    Name = item.Name.Value
                });
            }

            if (!id.HasValue)
            {
                return Ok(moneyActivityResponse);
            }

            var moneyActivity = await _moneyActivityService.GetDatabaseObjectAsync(id.Value).ConfigureAwait(false);
            moneyActivityResponse.Id = moneyActivity.Id;
            moneyActivityResponse.Description = moneyActivity.Description?.Value;
            moneyActivityResponse.Amount = moneyActivity.Amount.Value.ToString(CultureInfo.InvariantCulture);
            moneyActivityResponse.CategoryId = moneyActivity.CategoryId.Value;
            moneyActivityResponse.BusinessId = moneyActivity.BusinessId?.Value;
            moneyActivityResponse.CashActivityDate = moneyActivity.CashActivityDate.Value;
            moneyActivityResponse.ActivityType = moneyActivity.ActivityType;
            moneyActivityResponse.PaymentType = moneyActivity.PaymentType;

            if (moneyActivity.ActivityType == MoneyActivityType.Expenditure)
            {
                moneyActivityResponse.ImportantForTax = moneyActivity.ImportantForTax.Value;
                moneyActivityResponse.Warranty = moneyActivity.Warranty != null ? new CreateOrEditMoneyActivityResponse.WarrantyResponse
                {
                    LengthInMonth = moneyActivity.Warranty.LengthInMonth,
                    PurchaseDate = moneyActivity.Warranty.PurchaseDate
                } : null;
            }

            return Ok(moneyActivityResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpenditure([FromBody] CreateExpenditureMoneyActivityCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateExpenditureMoneyActivityCommand)}.");

            var amount = MoneyActivityAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var businessId = command.BusinessId.HasValue ? BusinessId.Create(command.BusinessId.Value) : null;
            var categoryId = CategoryId.Create(command.CategoryId);
            var cashActivityDate = MoneyActivityCashActivityDate.Create(command.CashActivityDate);
            var importantForTax = ImportantForTax.Create(command.ImportantForTax);
            var description = MoneyActivityDescription.Create(command.Description);
            var warranty = command.Warranty != null ? MoneyActivityWarranty.Create(command.Warranty.PurchaseDate, command.Warranty.LegthInMonth) : null;

            var moneyActivity = Domain.Finances.MoneyActivityAggregate.MoneyActivity.CreateExpenditure(
                amount,
                businessId,
                categoryId,
                cashActivityDate,
                description,
                command.PaymentType,
                importantForTax,
                warranty);

            await _moneyActivityService.StoreDatabaseObjectAsync(moneyActivity);
            return Ok(moneyActivity.Id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaving([FromBody] CreateSavingMoneyActivityCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateSavingMoneyActivityCommand)}.");

            var amount = MoneyActivityAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var businessId = command.BusinessId.HasValue ? BusinessId.Create(command.BusinessId.Value) : null;
            var categoryId = CategoryId.Create(command.CategoryId);
            var cashActivityDate = MoneyActivityCashActivityDate.Create(command.CashActivityDate);
            var description = MoneyActivityDescription.Create(command.Description);

            var moneyActivity = Domain.Finances.MoneyActivityAggregate.MoneyActivity.CreateSaving(
                amount,
                businessId,
                categoryId,
                cashActivityDate,
                description,
                command.PaymentType);

            await _moneyActivityService.StoreDatabaseObjectAsync(moneyActivity);
            return Ok(moneyActivity.Id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeMoneyActivityCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateIncomeMoneyActivityCommand)}.");

            var amount = MoneyActivityAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var businessId = command.BusinessId.HasValue ? BusinessId.Create(command.BusinessId.Value) : null;
            var categoryId = CategoryId.Create(command.CategoryId);
            var cashActivityDate = MoneyActivityCashActivityDate.Create(command.CashActivityDate);
            var description = MoneyActivityDescription.Create(command.Description);

            var moneyActivity = Domain.Finances.MoneyActivityAggregate.MoneyActivity.CreateIncome(
                amount,
                businessId,
                categoryId,
                cashActivityDate,
                description,
                command.PaymentType);

            await _moneyActivityService.StoreDatabaseObjectAsync(moneyActivity);
            return Ok(moneyActivity.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteMoneyActivityCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(DeleteMoneyActivityCommand)}.");
            // fetch to ensure
            var moneyActivity = await _moneyActivityService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);
            _moneyActivityService.DeleteDatabaseObject(command.Id);
            return Ok(command.Id);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMoneyActivity([FromBody] UpdateMoneyActivityCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(UpdateMoneyActivityCommand)}.");

            var moneyActivity = await _moneyActivityService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);

            var amount = MoneyActivityAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var businessId = command.BusinessId.HasValue ? BusinessId.Create(command.BusinessId.Value) : null;
            var categoryId = CategoryId.Create(command.CategoryId);
            var cashActivityDate = MoneyActivityCashActivityDate.Create(command.CashActivityDate);
            var description = command.Description == null ? null : MoneyActivityDescription.Create(command.Description);

            moneyActivity.UpdateAmount(amount);
            moneyActivity.UpdateBusinessId(businessId);
            moneyActivity.UpdateCategoryId(categoryId);
            moneyActivity.UpdateCashActivityDate(cashActivityDate);
            moneyActivity.UpdateDescription(description);

            if (command.ImportantForTax.HasValue && (command.ActivityType == MoneyActivityType.Expenditure || command.ActivityType == MoneyActivityType.Income))
            {
                moneyActivity.UpdateImportantForTax(ImportantForTax.Create(command.ImportantForTax.Value));
            }

            if (command.ActivityType == MoneyActivityType.Expenditure)
            {
                var warranty = command.Warranty == null ? null : MoneyActivityWarranty.Create(command.Warranty.PurchaseDate, command.Warranty.LengthInMonth);
                moneyActivity.UpdateWarranty(warranty);
            }

            await _moneyActivityService.StoreDatabaseObjectAsync(moneyActivity).ConfigureAwait(false);
            return Ok(moneyActivity.Id);
        }
    }
}
