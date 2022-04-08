using Domain.Finances.BusinessAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinances.Controllers.Business.Commands;
using MyFinances.Controllers.Business.Responses;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Controllers.Business
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessController : Controller
    {
        private readonly IDatabaseObjectServiceFactory _databaseObjectServiceFactory;
        private readonly IDatabaseObjectService<Domain.Finances.BusinessAggregate.Business> _businessService;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<BusinessController> logger)
        {
            _databaseObjectServiceFactory = databaseObjectServiceFactory;
            _businessService = _databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.BusinessAggregate.Business>();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _logger.LogInformation("List Businesses.");

            var businesses = await _businessService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            var response = new List<ListBusinessResponse>();
            foreach (var business in businesses)
            {
                var businessResponse = new ListBusinessResponse()
                {
                    Id = business.Id,
                    Name = business.Name.Value
                };
                response.Add(businessResponse);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation($"Get Business with id {id}.");

            var business = await _businessService.GetDatabaseObjectAsync(id).ConfigureAwait(false);

            var businessResponse = new CreateOrEditBusinessResponse
            {
                Id = business.Id,
                Name = business.Name.Value
            };

            return Ok(businessResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBusinessCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateBusinessCommand)}.");

            var business = Domain.Finances.BusinessAggregate.Business.Create(BusinessName.Create(command.Name));

            await _businessService.StoreDatabaseObjectAsync(business);
            return Ok(business.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateBusinessCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(UpdateBusinessCommand)}.");

            var business = await _businessService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);
            business.UpdateName(BusinessName.Create(command.Name));

            await _businessService.StoreDatabaseObjectAsync(business);
            return Ok(business.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteBusinessCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(DeleteBusinessCommand)}.");

            var business = await _businessService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);
            if (business == null)
            {
                throw new NullReferenceException($"Business not found for id {command.Id}.");
            }
            _businessService.DeleteDatabaseObject(command.Id);

            return Ok(command.Id);
        }
    }
}