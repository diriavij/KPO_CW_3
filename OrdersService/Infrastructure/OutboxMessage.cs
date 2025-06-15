namespace Infrastructure {
    public class OutboxMessage
    {
        public Guid Id { get; set; } 
        public string EventType { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty; 
        public DateTime OccurredAt { get; set; } 
        public bool Processed { get; set; } = false;
    }
}