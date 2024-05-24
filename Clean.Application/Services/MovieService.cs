using Clean.Domain.Models;
using Clean.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clean.Application.Interfaces;

namespace Clean.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _unitOfWork.Movies.GetMoviesAsync();
        }

        public async Task<IEnumerable<string>> GetGenresAsync()
        {
            // Implement GetGenresAsync in the repository
            return await _unitOfWork.Movies.GetGenresAsync();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _unitOfWork.Movies.AddMovieAsync(movie);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _unitOfWork.Movies.GetMovieByIdAsync(id);
        }

        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            await _unitOfWork.Movies.UpdateMovieAsync(id, movie);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(id);
            if (movie != null)
            {
                await _unitOfWork.Movies.DeleteMovieAsync(movie);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
