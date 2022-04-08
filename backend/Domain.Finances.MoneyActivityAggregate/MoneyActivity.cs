using Domain.Finances.MoneyActivityAggregate.ValueObjects;
using Domain.Finances.SharedValueObjects;
using Domain.Finances.Utilities;
using Common.JsonDatabase.DatabaseObject;

namespace Domain.Finances.MoneyActivityAggregate
{
    public class MoneyActivity : DatabaseObjectBase
    {
        public MoneyActivityAmount Amount { get; internal set; }
        public BusinessId BusinessId { get; internal set; }
        public CategoryId CategoryId { get; internal set; }
        public PaymentType PaymentType { get; internal set; }
        public MoneyActivityType ActivityType { get; internal set; }
        public MoneyActivityCashActivityDate CashActivityDate { get; internal set; }
        public ImportantForTax ImportantForTax { get; internal set; }
        public MoneyActivityReceiptStored ReceiptStored { get; internal set; }
        public MoneyActivityWarranty Warranty { get; internal set; }
        public MoneyActivityDescription Description { get; internal set; }

        private MoneyActivity(MoneyActivityAmount amount,
            BusinessId businessId,
            CategoryId categoryId,
            PaymentType paymentType,
            MoneyActivityType activityType,
            MoneyActivityCashActivityDate cashActivityDate,
            ImportantForTax importantForTax,
            MoneyActivityWarranty warranty,
            MoneyActivityDescription description)
        {
            Amount = amount;
            BusinessId = businessId;
            CategoryId = categoryId;
            PaymentType = paymentType;
            ActivityType = activityType;
            CashActivityDate = cashActivityDate;
            ImportantForTax = importantForTax;
            Warranty = warranty;
            Description = description;
        }

        public static MoneyActivity CreateIncome(MoneyActivityAmount amount, BusinessId businessId, CategoryId categoryId, MoneyActivityCashActivityDate cashActivityDate, MoneyActivityDescription description, PaymentType paymentType)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityAmount>(nameof(CreateIncome), nameof(amount), amount);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, CategoryId>(nameof(CreateIncome), nameof(categoryId), categoryId);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityCashActivityDate>(nameof(CreateIncome), nameof(cashActivityDate), cashActivityDate);

            return new MoneyActivity(amount, businessId, categoryId, paymentType, MoneyActivityType.Income, cashActivityDate, null, null, description);
        }

        public static MoneyActivity CreateSaving(MoneyActivityAmount amount, BusinessId businessId, CategoryId categoryId, MoneyActivityCashActivityDate cashActivityDate, MoneyActivityDescription description, PaymentType paymentType)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityAmount>(nameof(CreateSaving), nameof(amount), amount);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, CategoryId>(nameof(CreateSaving), nameof(categoryId), categoryId);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityCashActivityDate>(nameof(CreateSaving), nameof(cashActivityDate), cashActivityDate);

            return new MoneyActivity(amount, businessId, categoryId, paymentType, MoneyActivityType.Saving, cashActivityDate, null, null, description);
        }

        public static MoneyActivity CreateExpenditure(MoneyActivityAmount amount, BusinessId businessId, CategoryId categoryId, MoneyActivityCashActivityDate cashActivityDate, MoneyActivityDescription description, PaymentType paymentType, ImportantForTax importantForTax, MoneyActivityWarranty warranty)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityAmount>(nameof(CreateExpenditure), nameof(amount), amount);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, CategoryId>(nameof(CreateExpenditure), nameof(categoryId), categoryId);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityCashActivityDate>(nameof(CreateExpenditure), nameof(cashActivityDate), cashActivityDate);
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, ImportantForTax>(nameof(CreateExpenditure), nameof(importantForTax), importantForTax);

            return new MoneyActivity(amount, businessId, categoryId, paymentType, MoneyActivityType.Expenditure, cashActivityDate, importantForTax, warranty, description);
        }

        public void UpdateAmount(MoneyActivityAmount amount)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityAmount>(nameof(UpdateAmount), nameof(amount), amount);
            Amount = amount;
        }

        public void UpdateBusinessId(BusinessId businessId)
        {
            BusinessId = businessId;
        }

        public void UpdateCategoryId(CategoryId categoryId)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, CategoryId>(nameof(UpdateCategoryId), nameof(categoryId), categoryId);
            CategoryId = categoryId;
        }

        public void UpdateImportantForTax(ImportantForTax importantForTax)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, ImportantForTax>(nameof(UpdateImportantForTax), nameof(importantForTax), importantForTax);
            ImportantForTax = importantForTax;
        }

        public void UpdateDescription(MoneyActivityDescription description)
        {
            Description = description;
        }

        public void UpdateCashActivityDate(MoneyActivityCashActivityDate cashActivityDate)
        {
            ArgumentValidator.ArgumentNullOrDefault<MoneyActivity, MoneyActivityCashActivityDate>(nameof(UpdateCashActivityDate), nameof(cashActivityDate), cashActivityDate);
            CashActivityDate = cashActivityDate;
        }

        public void UpdateWarranty(MoneyActivityWarranty warranty)
        {
            Warranty = warranty;
        }
    }
}
