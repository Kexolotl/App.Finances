using Domain.Finances.DDD;

namespace Domain.Finances.SharedValueObjects
{
    public class ImportantForTax : ValueObject<ImportantForTax>
    {
        public bool Value { get; }

        private ImportantForTax(bool value)
        {
            Value = value;
        }

        public static ImportantForTax Create(bool value)
        {
            return new ImportantForTax(value);
        }
    }
}
