using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public DbSet<Author> Author { get; set; }
    public DbSet<Book> Book { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<LibraryBranch> LibraryBranch { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
    // }
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }
}
