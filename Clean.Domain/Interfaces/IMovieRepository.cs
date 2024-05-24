using Clean.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clean.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<IEnumerable<string>> GetGenresAsync();
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(int id, Movie movie);
        Task<Movie> GetMovieByIdAsync(int id);
        Task DeleteMovieAsync(Movie movie);
    }
}
