using BookReview.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Book> Books { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}