using Clean.Domain.Models;

namespace Clean.Domain.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetMovies();
        Task AddMovie(Movie movie);
        Task UpdateMovie(int id, Movie movie);
        Task<Movie> GetMovieByIdAsync(int id);
        Task DeleteMovie(Movie movie);
    }
}
