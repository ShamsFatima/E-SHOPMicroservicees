using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private const int DefaultLength = 50;
        public string Value { get; }
        private OrderName(string value)
        {
            Value = value;
        }   
        public static OrderName Of(string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("OrderName value cannot be empty.", nameof(value));
            }
            if (value.Length > DefaultLength)
            {
                throw new ArgumentException($"OrderName value cannot exceed {DefaultLength} characters.", nameof(value));
            }
            return new OrderName(value);
        }
    }
}
