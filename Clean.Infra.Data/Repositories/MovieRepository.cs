using Clean.Infra.Data.Context;
using Clean.Domain.Interfaces;
using Clean.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infra.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        public MovieDbContext _context;
        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetMovies()
        {
            return _context.Movies;
        }
        public async Task AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMovie(int id, Movie movie)
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
        public async Task<Movie> GetMovieByIdAsync(int id)  // Implement the method
        {
            return await _context.Movies.FindAsync(id);
        }
        public async Task DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
