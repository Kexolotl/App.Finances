using System;
using System.Collections.Generic;

namespace MyFinances.Api.Controllers.Category.Responses
{
    public class CreateOrEditCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string ColorKey { get; set; }
        public List<CreateOrEditCategoryResponse> AvailableCategories { get; set; } = new List<CreateOrEditCategoryResponse>();
    }
}
