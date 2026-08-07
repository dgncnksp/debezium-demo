namespace BookReview.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}