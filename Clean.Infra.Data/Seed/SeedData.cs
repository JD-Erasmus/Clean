using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Infra.Data.Context;
using Clean.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Infra.Data.Seed
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Seed users
            UserSeedingService.SeedAdminUserAsync(serviceProvider).Wait();
            // Seed Movies
            using (var context = new MovieDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<MovieDbContext>>()))
            {
                // Look for any movies.
                if (context.Movies.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movies.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "R"
                    },
                    new Movie
                    {
                        Title = "Ghostbusters",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "PG"
                    }
                    // Add more movies if needed
                );
                context.SaveChanges();
            }
            
        }
    }
}

