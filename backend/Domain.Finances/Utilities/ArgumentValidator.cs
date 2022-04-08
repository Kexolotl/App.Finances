using System;
using System.Collections.Generic;

namespace Domain.Finances.Utilities
{
    public class ArgumentValidator
    {
        public static void NullOrWhitespace<TModel>(string caller, string argumentName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentValidationException<TModel, string>(caller, argumentName, value, "Argument must be set.");
            }
        }

        public static void NullOrNotStartsWith<TModel>(string caller, string argumentName, string value, string comparisonChar)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith(comparisonChar, StringComparison.CurrentCulture))
            {
                throw new ArgumentValidationException<TModel, string>(caller, argumentName, value, $"The Argument must be set and must start with '{comparisonChar}'.");
            }
        }

        public static void NullOrLongerThan<TModel>(string caller, string argumentName, string value, int length)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > length)
            {
                throw new ArgumentValidationException<TModel, string>(caller, argumentName, value, $"Argument is not set or longer than {length}.");
            }
        }

        public static void NullOrNotExactLength<TModel>(string caller, string argumentName, string value, int length)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > length || value.Length < length)
            {
                throw new ArgumentValidationException<TModel, string>(caller, argumentName, value, $"Argument is not exact length of {length}.");
            }
        }

        public static void ArgumentNullOrDefault<TModel, TArgument>(string caller, string argumentName, TArgument value)
        {
            if (EqualityComparer<TArgument>.Default.Equals(value, default))
            {
                throw new ArgumentValidationException<TModel, TArgument>(caller, argumentName, default, "Argument must be set.");
            }
        }
        
        public static void ArgumentOutOfRange<TModel, TArgument>(string caller, string argumentName, TArgument value)
        {
            if (!Enum.IsDefined(typeof(TArgument), value))
            {
                throw new ArgumentValidationException<TModel, TArgument>(caller, argumentName, default, "Argument is out of range.");
            }
        }

        public class ArgumentValidationException<TModel, TArgument> : Exception
        {
            public ArgumentValidationException(string caller, string argumentName, TArgument argument, string message) : base(message)
            {
                Console.WriteLine("{0} {1} {2} {3}", nameof(TModel), nameof(TArgument), caller, argumentName, argument);
            }
        }
    }
}
