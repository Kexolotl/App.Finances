using Domain.Finances.SharedValueObjects;
using MyFinances.Controllers.MoneyActivity.Responses;
using System.Collections.Generic;
using System.Linq;

namespace MyFinances.Helper
{
    public class CategoryHelper
    {
        public static string GetFullName(Domain.Finances.CategoryAggregate.Category category, IEnumerable<Domain.Finances.CategoryAggregate.Category> categories)
        {
            var fullName = new List<string>();
            fullName.Add(category.Name.Value);
            if (category.ParentId != null)
            {
                var parentCategory = categories.Single(x => x.Id == category.ParentId.Value);
                fullName.Add(GetFullName(parentCategory, categories));
            }

            fullName.Reverse();
            return string.Join(":", fullName);
        }

        public static void GetChildren(IEnumerable<Domain.Finances.CategoryAggregate.Category> currentChildren, List<MoneyActivityStatisticResponse.CategoryResponse> response)
        {
            foreach (var child in currentChildren)
            {
                var childResponse = new MoneyActivityStatisticResponse.CategoryResponse
                {
                    Id = child.Id,
                    Name = child.Name.Value,
                    ParentId = child.ParentId.Value
                };

                response.Add(childResponse);

                var childChildren = currentChildren.Where(x => x.ParentId != null && x.ParentId.Value == child.Id).ToList();
                GetChildren(childChildren, response);
            }
        }

        public static Domain.Finances.CategoryAggregate.Category FindRoot(IEnumerable<Domain.Finances.CategoryAggregate.Category> categories, CategoryId categoryId)
        {
            var category = categories.Single(x => x.Id == categoryId.Value);
            if (category.ParentId == null)
            {
                return category;
            }
            return FindRoot(categories, category.ParentId);
        }
    }
}
