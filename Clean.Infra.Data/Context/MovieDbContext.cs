using Clean.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Clean.Infra.Data.Context
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        
    }
}