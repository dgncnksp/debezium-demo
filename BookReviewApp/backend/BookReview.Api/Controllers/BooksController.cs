using BookReview.Application.Books.Commands.CreateBook;
using BookReview.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Elastic.Clients.Elasticsearch;
using BookReview.Infrastructure.Search.Models;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MediatR;

namespace BookReview.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ElasticsearchClient _elasticClient;
    private readonly ISender _sender;

    public BooksController(
        ApplicationDbContext context,
        ElasticsearchClient elasticClient,
        ISender sender)
    {
        _context = context;
        _elasticClient = elasticClient;
        _sender = sender;
    }

    #region Queries

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var books = await _context.Books.ToListAsync();

        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        var response = await _elasticClient.InfoAsync();

        return Ok(new
        {
            Version = response.Version.Number,
            Cluster = response.ClusterName,
            Tagline = response.Tagline
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int? publishedAfter)
    {
        var boolQuery = new BoolQuery
        {
            Must =
            [
                new MultiMatchQuery
                {
                    Query = q,

                    Fields = Infer.Fields(
                        "title^3",
                        "author^2",
                        "description"),

                    Fuzziness = new Fuzziness("AUTO")
                }
            ]
        };

        if (publishedAfter.HasValue)
        {
            boolQuery.Filter =
            [
                new NumberRangeQuery("publishedYear")
                {
                    Gte = publishedAfter.Value
                }
            ];
        }

        var request = new SearchRequest<BookIndex>("books")
        {
            Size = 20,
            Query = boolQuery
        };

        var response =
            await _elasticClient.SearchAsync<BookIndex>(request);

        return Ok(response.Hits.Select(hit => new
        {
            Score = hit.Score,
            Document = hit.Source
        }));
    }

    #endregion

    #region Commands

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookCommand command,
        CancellationToken cancellationToken)
    {
        var bookId = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = bookId },
            new { id = bookId });
    }

    [HttpPost("reindex")]
    public async Task<IActionResult> ReIndex()
    {
        var books = await _context.Books.ToListAsync();

        var documents = books.Select(book => new BookIndex
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            PublishedYear = book.PublishedYear
        });

        var response = await _elasticClient.BulkAsync(b => b
            .Index("books")
            .IndexMany(documents));

        if (!response.IsValidResponse)
            return BadRequest(response.DebugInformation);

        return Ok(new
        {
            Count = documents.Count(),
            Message = "Books indexed successfully."
        });
    }

    #endregion
}