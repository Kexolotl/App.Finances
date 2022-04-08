using System;

namespace MyFinances.Api.Controllers.Category.Responses
{
    public class ListCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ListCategoryResponse Parent { get; set; }
        public string ColorKey { get; set; }
    }
}
