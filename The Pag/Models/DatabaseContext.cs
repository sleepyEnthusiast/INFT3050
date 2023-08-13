using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Book_genre> BookSubgenre { get; set; } = null!;
        public DbSet<Movie_genre> MovieSubgenre { get; set; } = null!;
        public DbSet<Game_genre> GameSubgenre { get; set; } = null!;
        //Book_genre NEW as well?
        public DbSet<Orders> Orders { get; set; } = null!;
        public DbSet<ProductsInOrders> ProductsInOrders { get; set; } = null!;
        public DbSet<TO> TO { get; set; } = null!;
        public DbSet<User> User { get; set; } = null!;
        public DbSet<Source> Source { get; set; } = null!;
        public DbSet<Stocktake> Stocktake { get; set; } = null!;
        public DbSet<Patrons> Patrons { get; set; } = null!;
    }
}
