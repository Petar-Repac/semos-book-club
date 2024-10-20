using BookClub.Models;
using Microsoft.EntityFrameworkCore;
namespace BookClub.Database;

public class BookContext : DbContext
{
    public BookContext(DbContextOptions<BookContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
}