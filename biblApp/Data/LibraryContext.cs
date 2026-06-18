using Microsoft.EntityFrameworkCore;
using biblApp.Models;

namespace biblApp.Data
{
    public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Edition> Editions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edition>()
                .HasMany(e => e.Authors)
                .WithMany(a => a.Editions);
        }
    }
}