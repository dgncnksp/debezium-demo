using BookReview.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Elastic.Clients.Elasticsearch;
using BookReview.Infrastructure.Search.Models;
using BookReview.Application;
using BookReview.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VuePolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    var uri = configuration["Elasticsearch:Uri"]!;

    var settings = new ElasticsearchClientSettings(new Uri(uri));

    return new ElasticsearchClient(settings);
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();


app.UseCors("VuePolicy");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    await SeedData.InitializeAsync(context);
}


using (var scope = app.Services.CreateScope())
{
    var client = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

    const string indexName = "books";

    var exists = await client.Indices.ExistsAsync(indexName);

    if (!exists.Exists)
    {
        await client.Indices.CreateAsync<BookIndex>(
            indexName,
            c => c
                .Mappings(m => m
                    .Properties(p => p

                        .Keyword(x => x.Id)

                        .Text(x => x.Title)

                        .Text(x => x.Author)

                        .Text(x => x.Description)

                        .IntegerNumber(x => x.PublishedYear)

                    )
                )
        );
    }

    var countResponse = await client.CountAsync<BookIndex>(r => r.Indices(indexName));

    var documentCount = countResponse.Count;
    if (documentCount == 0)
    {
        // Bulk index yap
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var books = await db.Books.ToListAsync();
        var documents = books.Select(book => new BookIndex
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublishedYear = book.PublishedYear
        });

        var bulkResponse = await client.BulkAsync(b => b
            .Index(indexName)
            .IndexMany(documents)
        );

        if (!bulkResponse.IsValidResponse)
        {
            Console.WriteLine("=== ELASTICSEARCH BULK ERROR ===");
            Console.WriteLine(bulkResponse);
            Console.WriteLine("================================");


            throw new Exception("Elasticsearch bulk indexing failed.");
        }
    }
}
app.Run();