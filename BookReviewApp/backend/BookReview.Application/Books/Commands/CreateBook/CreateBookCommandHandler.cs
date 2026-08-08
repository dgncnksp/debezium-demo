using System.Text.Json;
using BookReview.Application.Common.Interfaces;
using BookReview.Domain.Entities;
using MediatR;

namespace BookReview.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommandHandler
    : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateBookCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateBookCommand request,
        CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            PublishedYear = request.PublishedYear
        };
        _context.Books.Add(book);

        var payload = JsonSerializer.Serialize(new
        {
            id = book.Id,
            title = book.Title,
            author = book.Author,
            publishedYear = book.PublishedYear
        });

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            AggregateType = "Book",
            AggregateId = book.Id.ToString(),
            Type = "BookCreated",
            Payload = payload,
            CreatedAt = DateTime.UtcNow
        };


        _context.OutboxMessages.Add(outboxMessage);

        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}