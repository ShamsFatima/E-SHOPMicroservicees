using System;

namespace Ordering.Domain.ValueObjects
{
    // Value Object for OrderId
    public record OrderId
    {
        public Guid Value { get; }

        // Private constructor ensures controlled creation
        private OrderId(Guid value)
        {
            Value = value;
        }

        // Static factory method (used by EF Core and domain logic)
        public static OrderId Of(Guid value)
        {
            // Optional validation: prevent empty GUIDs
            if (value == Guid.Empty)
                throw new ArgumentException("OrderId cannot be empty.", nameof(value));

            return new OrderId(value);
        }

        // Factory method to generate a new ID
        public static OrderId NewId() => new OrderId(Guid.NewGuid());

        // Implicit conversions to make usage seamless
        public static implicit operator Guid(OrderId id) => id.Value;
        public static implicit operator OrderId(Guid value) => new OrderId(value);

        public override string ToString() => Value.ToString();
    }
}
