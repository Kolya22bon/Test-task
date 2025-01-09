using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        private static readonly DateTime AlbumReleaseDate = new DateTime(2012, 5, 25);
        private const string Keyword = "Red";

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Book> GetBook()
        {
            return await _context.Books
                .OrderByDescending(book => book.Price * book.QuantityPublished)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Book>> GetBooks()
        {
            return await _context.Books
                .Where(book => book.Title.Contains(Keyword, StringComparison.OrdinalIgnoreCase)
                            && book.PublishDate > AlbumReleaseDate)
                .ToListAsync();
        }
    }
}
