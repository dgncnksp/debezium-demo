namespace BookReview.Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; set; }

    public string AggregateType { get; set; } = null!;

    public string AggregateId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}