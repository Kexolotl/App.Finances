using System;
using System.Collections.Generic;

namespace MyFinances.Controllers.MoneyActivity.Responses
{
    public class MoneyActivityStatisticResponse
    {
        public List<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();
        public List<BusinessResponse> Businesses { get; set; } = new List<BusinessResponse>();

        public class BusinessResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CategoryResponse
        {
            public Guid Id { get; set; }
            public Guid? ParentId { get; set; }
            public string Name { get; set; }
        }
    }

}
