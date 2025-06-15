namespace Infrastructure.Messaging
{
    public class OutboxMessage
    {
        public Guid     Id         { get; set; }
        public string   EventType  { get; set; } = null!;
        public string   Content    { get; set; } = null!;
        public DateTime OccurredAt { get; set; }
        public bool     Processed  { get; set; }
    }
}