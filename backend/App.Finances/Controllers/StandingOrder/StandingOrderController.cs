using Domain.Finances.SharedValueObjects;
using Domain.Finances.StandingOrderAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinances.Controllers.StandingOrder.Commands;
using MyFinances.Controllers.StandingOrder.Responses;
using MyFinances.Helper;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Controllers.StandingOrder
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StandingOrderController : ControllerBase
    {
        private readonly IDatabaseObjectService<Domain.Finances.CategoryAggregate.Category> _categoryService;
        private readonly IDatabaseObjectService<Domain.Finances.BusinessAggregate.Business> _businessService;
        private readonly IDatabaseObjectService<Domain.Finances.StandingOrderAggregate.StandingOrder> _standingOrderService;
        private readonly ILogger<StandingOrderController> _logger;

        public StandingOrderController(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<StandingOrderController> logger)
        {
            _categoryService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.CategoryAggregate.Category>();
            _businessService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.BusinessAggregate.Business>();
            _standingOrderService = databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.StandingOrderAggregate.StandingOrder>();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _logger.LogInformation("List standing orders.");

            var response = new List<ListStandingOrderResponse>();

            var standingOrders = await _standingOrderService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var standingOrder in standingOrders)
            {
                var category = await _categoryService.GetDatabaseObjectAsync(standingOrder.CategoryId.Value).ConfigureAwait(false);
                string businessName = null;
                if (standingOrder.BusinessId != null)
                {
                    var business = await _businessService.GetDatabaseObjectAsync(standingOrder.BusinessId.Value).ConfigureAwait(false);
                    businessName = business.Name.Value;
                }

                response.Add(new ListStandingOrderResponse
                {
                    Id = standingOrder.Id,
                    CategoryName = category.Name.Value,
                    BusinessName = businessName,
                    Amount = standingOrder.Amount.Value.ToString(CultureInfo.InvariantCulture),
                    FirstPaymentDate = standingOrder.FirstPaymentDate.Value,
                    FinalPaymentDate = standingOrder.FinalPaymentDate?.Value,
                    NextPaymentDate = standingOrder.NextPaymentDate == null ? standingOrder.FirstPaymentDate.Value : standingOrder.NextPaymentDate.Value,
                    IsActive = standingOrder.IsActive.Value
                });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            _logger.LogInformation($"Get standing order with id {id}.");
            var response = new CreateOrEditStandingOrderResponse();

            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var item in categories)
            {
                response.Categories.Add(new CreateOrEditStandingOrderResponse.CategoryResponse
                {
                    Id = item.Id,
                    Name = CategoryHelper.GetFullName(item, categories)
                });
            }

            var businesses = await _businessService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var item in businesses)
            {
                response.Businesses.Add(new CreateOrEditStandingOrderResponse.BusinessResponse
                {
                    Id = item.Id,
                    Name = item.Name.Value
                });
            }

            if (!id.HasValue)
            {
                return Ok(response);
            }

            var standingOrder = await _standingOrderService.GetDatabaseObjectAsync(id.Value).ConfigureAwait(false);
            var category = categories.Single(x => x.Id == standingOrder.CategoryId.Value);

            response.Id = standingOrder.Id;
            response.Amount = standingOrder.Amount.Value.ToString(CultureInfo.InvariantCulture);
            response.Category = new CreateOrEditStandingOrderResponse.CategoryResponse
            {
                Id = category.Id,
                Name = CategoryHelper.GetFullName(category, categories)
            };

            if (standingOrder.BusinessId != null)
            {
                var business = await _businessService.GetDatabaseObjectAsync(standingOrder.BusinessId.Value).ConfigureAwait(false);
                response.Business = new CreateOrEditStandingOrderResponse.BusinessResponse
                {
                    Id = business.Id,
                    Name = business.Name.Value
                };
            }

            response.PaymentType = standingOrder.PaymentType;
            response.ActivityType = standingOrder.MoneyActivityType;
            response.Frequency = standingOrder.Frequency;

            response.FirstPaymentDate = standingOrder.FirstPaymentDate.Value;
            response.NextPaymentDate = standingOrder.NextPaymentDate?.Value;
            response.LastPaymentDate = standingOrder.LastPaymentDate?.Value;
            response.FinalPaymentDate = standingOrder.FinalPaymentDate?.Value;
            response.IsActive = standingOrder.IsActive.Value;

            response.ImportantForTax = standingOrder.ImportantForTax.Value;

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStandingOrderCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateStandingOrderCommand)}.");

            var amount = StandingOrderAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var categoryId = CategoryId.Create(command.CategoryId);
            var firstPaymentDate = StandingOrderFirstPaymentDate.Create(command.FirstPaymentDate);

            var standingOrder = Domain.Finances.StandingOrderAggregate.StandingOrder.Create(categoryId, amount, command.Frequency, command.PaymentType, firstPaymentDate);
            if (command.BusinessId.HasValue)
            {
                standingOrder.UpdateBusinessId(BusinessId.Create(command.BusinessId.Value));
            }

            if (command.FinalPaymentDate.HasValue)
            {
                standingOrder.UpdateFinalPaymentDate(StandingOrderFinalPaymentDate.Create(command.FinalPaymentDate.Value));
            }

            await _standingOrderService.StoreDatabaseObjectAsync(standingOrder);
            return Ok(standingOrder.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteStandingOrderCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(DeleteStandingOrderCommand)}.");

            var standingOrder = await _standingOrderService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);
            _standingOrderService.DeleteDatabaseObject(command.Id);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStandingOrder([FromBody] UpdateStandingOrderCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(UpdateStandingOrderCommand)}.");

            var standingOrder = await _standingOrderService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);

            var amount = StandingOrderAmount.Create(decimal.Parse(command.Amount, CultureInfo.InvariantCulture));
            var isActive = StandingOrderIsActive.Create(command.IsActive);
            var finalPaymentDate = command.FinalPaymentDate.HasValue ? StandingOrderFinalPaymentDate.Create(command.FinalPaymentDate.Value) : null;

            standingOrder.UpdateAmount(amount);
            standingOrder.UpdateIsActive(isActive);
            standingOrder.UpdateFinalPaymentDate(finalPaymentDate);

            await _standingOrderService.StoreDatabaseObjectAsync(standingOrder).ConfigureAwait(false);

            return Ok();
        }
    }
}
