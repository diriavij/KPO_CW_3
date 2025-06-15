namespace Domain {
    public record Money(decimal Value) {
        public static Money FromDecimal(decimal amount) {
            if (amount < 0) throw new ArgumentException("Money cannot be negative");
            return new Money(amount);
        }
    }
}