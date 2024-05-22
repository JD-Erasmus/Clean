using Clean.Domain.Models;

namespace Clean.Application.Interfaces
{
    

    public interface IMovieService
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<string> GetGenres();
        Task AddMovie(Movie movie);
        Task<Movie> GetMovieByIdAsync(int id);
        Task UpdateMovie(int id, Movie movie);
        Task DeleteMovie(int id);
    }
}
