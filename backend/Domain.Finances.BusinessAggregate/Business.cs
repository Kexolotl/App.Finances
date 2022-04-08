using Domain.Finances.BusinessAggregate.ValueObjects;
using Domain.Finances.Utilities;
using Common.JsonDatabase.DatabaseObject;

namespace Domain.Finances.BusinessAggregate
{
    public class Business : DatabaseObjectBase
    {
        public BusinessName Name { get; private set; }

        private Business(BusinessName name)
        {
            Name = name;
        }

        public static Business Create(BusinessName name)
        {
            ArgumentValidator.ArgumentNullOrDefault<Business, BusinessName>(nameof(Create), nameof(name), name);

            return new Business(name);
        }

        public void UpdateName(BusinessName name)
        {
            ArgumentValidator.ArgumentNullOrDefault<Business, BusinessName>(nameof(UpdateName), nameof(name), name);

            Name = name;
        }
    }
}
