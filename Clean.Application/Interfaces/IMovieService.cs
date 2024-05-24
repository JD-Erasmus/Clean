using Clean.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clean.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<IEnumerable<string>> GetGenresAsync();
        Task AddMovieAsync(Movie movie);
        Task<Movie> GetMovieByIdAsync(int id);
        Task UpdateMovieAsync(int id, Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
