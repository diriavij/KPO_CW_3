namespace Domain
{
    public record Money : IComparable<Money>
    {
        public decimal Value { get; init; }

        public Money(decimal value)
        {
            if (value < 0) throw new ArgumentException("Money cannot be negative", nameof(value));
            Value = value;
        }

        public static Money Zero => new(0m);

        public static Money operator +(Money a, Money b) => new(a.Value + b.Value);
        public static Money operator -(Money a, Money b)
        {
            var result = a.Value - b.Value;
            if (result < 0) throw new InvalidOperationException("Insufficient funds");
            return new(result);
        }

        public int CompareTo(Money? other)
            => other is null ? 1 : Value.CompareTo(other.Value);

        public static bool operator < (Money a, Money b) => a.Value <  b.Value;
        public static bool operator > (Money a, Money b) => a.Value >  b.Value;
        public static bool operator <=(Money a, Money b) => a.Value <= b.Value;
        public static bool operator >=(Money a, Money b) => a.Value >= b.Value;
    }
}