using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        private const int ReferenceYear = 2015;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<Author> GetAuthor()
        {
            return await _context.Authors
                .Where(author => author.Books.Any())
                .Select(author => new
                {
                    Author = author,
                    MaxTitleLength = author.Books.Max(book => book.Title.Length)
                })
                .OrderByDescending(authorData => authorData.MaxTitleLength)
                .ThenBy(authorData => authorData.Author.Id)
                .Select(authorData => authorData.Author)
                .FirstOrDefaultAsync();
        }

       
        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Authors
                .Where(author =>
                    author.Books.Count(book => book.PublishDate.Year > ReferenceYear) % 2 == 0
                )
                .ToListAsync();
        }
    }
}
