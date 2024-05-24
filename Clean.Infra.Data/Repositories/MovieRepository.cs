using Clean.Infra.Data.Context;
using Clean.Domain.Interfaces;
using Clean.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Clean.Infra.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetGenresAsync()
        {
            return await _context.Movies.Select(m => m.Genre).Distinct().ToListAsync();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie != null)
            {
                // Update existing movie properties
                existingMovie.Title = movie.Title;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.Genre = movie.Genre;
                existingMovie.Price = movie.Price;
                existingMovie.Rating = movie.Rating;
                existingMovie.ImageUrl = movie.ImageUrl;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
