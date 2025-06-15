namespace Domain.Events
{
    public class OutboxEvent
    {
        public Guid      Id         { get; set; }
        public string    EventType  { get; set; } = null!;
        public string    Content    { get; set; } = null!;
        public DateTime  OccurredAt { get; set; }
    }
}