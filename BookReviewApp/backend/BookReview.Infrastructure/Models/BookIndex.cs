namespace BookReview.Infrastructure.Search.Models;

public class BookIndex
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int PublishedYear { get; set; }
}