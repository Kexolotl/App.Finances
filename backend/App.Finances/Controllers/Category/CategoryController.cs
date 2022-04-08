using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFinances.Api.Controllers.Category.Responses;
using MyFinances.Controllers.Category.Commands;
using MyFinances.Helper;
using Common.JsonDatabase.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Api.Controllers.Category
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IDatabaseObjectServiceFactory _databaseObjectServiceFactory;
        private readonly IDatabaseObjectService<Domain.Finances.CategoryAggregate.Category> _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IDatabaseObjectServiceFactory databaseObjectServiceFactory, ILogger<CategoryController> logger)
        {
            _databaseObjectServiceFactory = databaseObjectServiceFactory;
            _categoryService = _databaseObjectServiceFactory.GetDatabaseObjectService<Domain.Finances.CategoryAggregate.Category>();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            _logger.LogInformation("List Categories.");

            var response = new List<ListCategoryResponse>();

            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);
            foreach (var category in categories)
            {
                response.Add(new ListCategoryResponse
                {
                    Id = category.Id,
                    Name = CategoryHelper.GetFullName(category, categories),
                    Parent = null,
                    ColorKey = category.ColorKey?.Value,
                });
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? id)
        {
            _logger.LogInformation($"Get Category with id {id}.");

            var categoryResponse = new CreateOrEditCategoryResponse();
            var categories = await _categoryService.GetDatabaseObjectsAsync().ConfigureAwait(false);

            foreach (var item in categories)
            {
                if (id.HasValue && item.Id == id.Value)
                {
                    continue;
                }

                var availableCategory = new CreateOrEditCategoryResponse
                {
                    Id = item.Id,
                    Name = CategoryHelper.GetFullName(item, categories),
                    ParentId = item.ParentId?.Value,
                    ColorKey = item.ColorKey.Value
                };
                categoryResponse.AvailableCategories.Add(availableCategory);
            }

            if (!id.HasValue)
            {
                return Ok(categoryResponse);
            }

            var category = await _categoryService.GetDatabaseObjectAsync(id.Value).ConfigureAwait(false);
            categoryResponse.Id = category.Id;
            categoryResponse.Name = category.Name.Value;
            categoryResponse.ParentId = category.ParentId?.Value;
            categoryResponse.ColorKey = category.ColorKey.Value;

            return Ok(categoryResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCategoryCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(CreateCategoryCommand)}.");

            var name = Domain.Finances.CategoryAggregate.ValueObjects.CategoryName.Create(command.Name);
            var parentId = command.ParentId.HasValue ? Domain.Finances.SharedValueObjects.CategoryId.Create(command.ParentId.Value) : null;
            var colorKey = Domain.Finances.CategoryAggregate.ValueObjects.CategoryColorKey.Create(command.ColorKey);

            var category = Domain.Finances.CategoryAggregate.Category.Create(name, colorKey, parentId);
            await _categoryService.StoreDatabaseObjectAsync(category).ConfigureAwait(false);

            return Ok(category.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]UpdateCategoryCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(UpdateCategoryCommand)}.");

            var category = await _categoryService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);

            var name = Domain.Finances.CategoryAggregate.ValueObjects.CategoryName.Create(command.Name);
            var parentId = command.ParentId.HasValue ? Domain.Finances.SharedValueObjects.CategoryId.Create(command.ParentId.Value) : null;
            var colorKey = Domain.Finances.CategoryAggregate.ValueObjects.CategoryColorKey.Create(command.ColorKey);

            category.UpdateName(name);
            category.UpdateColorKey(colorKey);
            category.UpdateParentId(parentId);

            await _categoryService.StoreDatabaseObjectAsync(category).ConfigureAwait(false);

            return Ok(category.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]DeleteCategoryCommand command)
        {
            _logger.LogInformation($"Command was triggert {nameof(DeleteCategoryCommand)}.");

            var category = await _categoryService.GetDatabaseObjectAsync(command.Id).ConfigureAwait(false);
            if (category == null)
            {
                throw new NullReferenceException($"Category not found for id {command.Id}.");
            }
            _categoryService.DeleteDatabaseObject(command.Id);

            return Ok(command.Id);
        }
    }
}
