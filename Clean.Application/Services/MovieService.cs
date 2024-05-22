using Clean.Application.Interfaces;
using Clean.Domain.Interfaces;
using Clean.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Clean.Application.Services
{
    

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public IEnumerable<Movie> GetMovies()
        {
            return _movieRepository.GetMovies();
        }
        public IEnumerable<string> GetGenres()
        {
            return _movieRepository.GetMovies().Select(m => m.Genre).Distinct();
        }

        public async Task AddMovie(Movie movie)
        {
            await _movieRepository.AddMovie(movie);
        }
        public async Task UpdateMovie(int id, Movie movie)
        {
            await _movieRepository.UpdateMovie(id, movie);
        }
        public async Task<Movie> GetMovieByIdAsync(int id)  
        {
            return await _movieRepository.GetMovieByIdAsync(id);
        }
        public async Task DeleteMovie(int id)
        {
            // Retrieve the movie by its ID
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null)
            {
                // If the movie does not exist, throw an exception or handle accordingly
                throw new ApplicationException($"Movie with ID {id} not found.");
            }

            // Call the DeleteMovie method of the repository to delete the movie
            await _movieRepository.DeleteMovie(movie);
        }
    }
}
