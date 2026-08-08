using MediatR;

namespace BookReview.Application.Books.Commands.CreateBook;

public sealed record CreateBookCommand(
    string Title,
    string Author,
    int PublishedYear
) : IRequest<Guid>;