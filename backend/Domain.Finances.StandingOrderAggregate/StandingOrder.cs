using Domain.Finances.SharedValueObjects;
using Domain.Finances.StandingOrderAggregate.ValueObjects;
using Domain.Finances.Utilities;
using Common.JsonDatabase.DatabaseObject;
using System;

namespace Domain.Finances.StandingOrderAggregate
{
    public class StandingOrder : DatabaseObjectBase
    {
        public CategoryId CategoryId { get; internal set; }
        public BusinessId BusinessId { get; internal set; }
        public StandingOrderAmount Amount { get; internal set; }
        public StandingOrderIsActive IsActive { get; internal set; }

        public StandingOrderFirstPaymentDate FirstPaymentDate { get; private set; }
        public StandingOrderNextPaymentDate NextPaymentDate { get; internal set; }
        public StandingOrderLastPaymentDate LastPaymentDate { get; internal set; }
        public StandingOrderFinalPaymentDate FinalPaymentDate { get; internal set; }
        public ImportantForTax ImportantForTax { get; internal set; }

        public StandingOrderFrequency Frequency { get; internal set; }
        public PaymentType PaymentType { get; internal set; }
        public MoneyActivityType MoneyActivityType { get; internal set; }


        public bool IsInfinite()
        {
            return FinalPaymentDate == null;
        }

        private StandingOrder(CategoryId categoryId, StandingOrderAmount amount, StandingOrderFrequency frequency, PaymentType paymentType, StandingOrderFirstPaymentDate firstPaymentDate)
        {
            CategoryId = categoryId;
            Amount = amount;
            Frequency = frequency;
            PaymentType = paymentType;
            FirstPaymentDate = firstPaymentDate;
            ImportantForTax = ImportantForTax.Create(false);
            MoneyActivityType = MoneyActivityType.Expenditure;
            IsActive = StandingOrderIsActive.Create(true);
        }

        public static StandingOrder Create(CategoryId categoryId, StandingOrderAmount amount, StandingOrderFrequency frequency, PaymentType paymentType, StandingOrderFirstPaymentDate firstPaymentDate)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, CategoryId>(nameof(Create), nameof(categoryId), categoryId);
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderAmount>(nameof(Create), nameof(amount), amount);
            ArgumentValidator.ArgumentOutOfRange<StandingOrder, StandingOrderFrequency>(nameof(Create), nameof(frequency), frequency);
            ArgumentValidator.ArgumentOutOfRange<StandingOrder, PaymentType>(nameof(Create), nameof(paymentType), paymentType);
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderFirstPaymentDate>(nameof(Create), nameof(firstPaymentDate), firstPaymentDate);

            return new StandingOrder(categoryId, amount, frequency, paymentType, firstPaymentDate);
        }

        public void UpdateCategoryId(CategoryId categoryId)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, CategoryId>(nameof(UpdateCategoryId), nameof(categoryId), categoryId);
            CategoryId = categoryId;
        }

        public void UpdateBusinessId(BusinessId businessId)
        {
            BusinessId = businessId;
        }

        public void UpdateAmount(StandingOrderAmount amount)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderAmount>(nameof(UpdateAmount), nameof(amount), amount);
            Amount = amount;
        }

        public void UpdateNextPaymentDate(StandingOrderNextPaymentDate nextPaymentDate)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderNextPaymentDate>(nameof(UpdateNextPaymentDate), nameof(nextPaymentDate), nextPaymentDate);
            NextPaymentDate = nextPaymentDate;
        }

        public void UpdateLastPaymentDate(StandingOrderLastPaymentDate lastPaymentDate)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderLastPaymentDate>(nameof(UpdateFinalPaymentDate), nameof(lastPaymentDate), lastPaymentDate);
            LastPaymentDate = lastPaymentDate;
        }

        public void UpdateFinalPaymentDate(StandingOrderFinalPaymentDate finalPaymentDate)
        {
            FinalPaymentDate = finalPaymentDate;
        }

        public void UpdatePaymentType(PaymentType paymentType)
        {
            PaymentType = paymentType;
        }

        public void UpdateIsActive(StandingOrderIsActive isActive)
        {
            ArgumentValidator.ArgumentNullOrDefault<StandingOrder, StandingOrderIsActive>(nameof(UpdateFinalPaymentDate), nameof(isActive), isActive);
            IsActive = isActive;
        }
    }
}
