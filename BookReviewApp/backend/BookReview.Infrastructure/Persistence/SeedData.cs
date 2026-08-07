using BookReview.Domain.Entities;

namespace BookReview.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (context.Books.Any())
            return;

        context.Books.AddRange(
            new Book
            {
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Description = "A Handbook of Agile Software Craftsmanship",
                PublishedYear = 2008,
                ImageUrl = "https://picsum.photos/200/300?1"
            },
            new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt",
                Description = "Journey to Mastery",
                PublishedYear = 1999,
                ImageUrl = "https://picsum.photos/200/300?2"
            },
            new Book
            {
                Title = "Design Patterns",
                Author = "Gang of Four",
                Description = "Elements of Reusable Object-Oriented Software",
                PublishedYear = 1994,
                ImageUrl = "https://picsum.photos/200/300?3"
            },

            // Yeni kitaplar
            new Book
            {
                Title = "Clean Architecture",
                Author = "Robert C. Martin",
                Description = "A guide to software architecture and clean architecture principles.",
                PublishedYear = 2017,
                ImageUrl = "https://picsum.photos/200/300?4"
            },
            new Book
            {
                Title = "Refactoring",
                Author = "Martin Fowler",
                Description = "Improving the design of existing code.",
                PublishedYear = 2018,
                ImageUrl = "https://picsum.photos/200/300?5"
            },
            new Book
            {
                Title = "Domain-Driven Design",
                Author = "Eric Evans",
                Description = "Tackling complexity in the heart of software.",
                PublishedYear = 2003,
                ImageUrl = "https://picsum.photos/200/300?6"
            },
            new Book
            {
                Title = "Patterns of Enterprise Application Architecture",
                Author = "Martin Fowler",
                Description = "Enterprise application architecture patterns and best practices.",
                PublishedYear = 2002,
                ImageUrl = "https://picsum.photos/200/300?7"
            },
            new Book
            {
                Title = "Working Effectively with Legacy Code",
                Author = "Michael Feathers",
                Description = "Techniques for safely modifying legacy systems.",
                PublishedYear = 2004,
                ImageUrl = "https://picsum.photos/200/300?8"
            },
            new Book
            {
                Title = "Head First Design Patterns",
                Author = "Eric Freeman",
                Description = "A practical and visual introduction to design patterns.",
                PublishedYear = 2004,
                ImageUrl = "https://picsum.photos/200/300?9"
            },
            new Book
            {
                Title = "Building Microservices",
                Author = "Sam Newman",
                Description = "Designing fine-grained systems using microservice architecture.",
                PublishedYear = 2021,
                ImageUrl = "https://picsum.photos/200/300?10"
            },
            new Book
            {
                Title = "Implementing Domain-Driven Design",
                Author = "Vaughn Vernon",
                Description = "Practical implementation techniques for Domain-Driven Design.",
                PublishedYear = 2013,
                ImageUrl = "https://picsum.photos/200/300?11"
            }
        );

        await context.SaveChangesAsync();
    }
}