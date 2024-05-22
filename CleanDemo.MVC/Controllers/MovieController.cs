using Clean.Application.Interfaces;
using Clean.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Clean.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanDemo.MVC.ViewModels;
namespace CleanDemo.MVC.Controllers
{
  

    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index(string searchString, string movieGenre)
        {
            // Get all movies
            var movies = _movieService.GetMovies();

            // Populate genres for the dropdown menu
            var genres = _movieService.GetGenres();

            // Filter movies based on search string and genre selection
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(m => m.Genre == movieGenre);
            }

            // Create the view model
            var viewModel = new MovieGenreViewModel
            {
                Movies = movies.ToList(),
                Genres = new SelectList(genres)
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel viewModel)
        {
           
            if (ModelState.IsValid)
            {
                // Check if an image file was uploaded
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    // Generate a unique filename for the uploaded image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ImageFile.FileName;

                    // Save the uploaded image to the wwwroot/Images/Uploads/Movies directory
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Uploads", "Movies", uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    // Set the ImageUrl property of the movie to the relative path of the uploaded image
                    viewModel.ImageUrl = "/Images/Uploads/Movies/" + uniqueFileName;
                }

                // Create a new Movie object using the ViewModel data
                var movie = new Movie
                {
                    Title = viewModel.Title,
                    ReleaseDate = viewModel.ReleaseDate,
                    Genre = viewModel.Genre,
                    Price = viewModel.Price,
                    Rating = viewModel.Rating,
                    ImageUrl = viewModel.ImageUrl
                };

                // Add the movie to the database using the MovieService
                await _movieService.AddMovie(movie);

                // Redirect to the Index action after successful addition
                return RedirectToAction(nameof(Index));
            }
            //PRINT ERRORS OF INVALID MODELSTATE
            /*foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    // Log or debug the error message
                    var errorMessage = error.ErrorMessage;
                    var propertyName = modelStateEntry.AttemptedValue;
                    // Log or debug the property name causing the error
                    Console.WriteLine($"Validation error for property '{propertyName}': {errorMessage}");
                }
            }*/

            // If ModelState is not valid, return the view with the ViewModel
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price,
                Rating = movie.Rating,
                ImageUrl = movie.ImageUrl
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            ModelState.Remove("ImageFile");
            if (ModelState.IsValid)
            {
                // Fetch the existing movie from the database
                var existingMovie = await _movieService.GetMovieByIdAsync(id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                // Update the existing movie's properties with the new values
                existingMovie.Title = viewModel.Title;
                existingMovie.ReleaseDate = viewModel.ReleaseDate;
                existingMovie.Genre = viewModel.Genre;
                existingMovie.Price = viewModel.Price;
                existingMovie.Rating = viewModel.Rating;
                

                // Check if an image file was uploaded
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    // Generate a unique filename for the uploaded image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ImageFile.FileName;

                    // Save the uploaded image to the wwwroot/Images/Uploads/Movies directory
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Uploads", "Movies", uniqueFileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    // Set the ImageUrl property of the movie to the relative path of the uploaded image
                    existingMovie.ImageUrl = "/Images/Uploads/Movies/" + uniqueFileName;
                }
               

                // Update the movie in the database using the MovieService
                await _movieService.UpdateMovie(id, existingMovie);

                // Redirect to the Index action after successful update
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return the view with the ViewModel
            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price,
                Rating = movie.Rating,
                ImageUrl = movie.ImageUrl
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Call the DeleteMovie method of the IMovieService interface
                await _movieService.DeleteMovie(id);
                // Redirect to the Index action after successful deletion
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during deletion
                // You can log the exception or display an error message to the user
                return RedirectToAction("Error", "Home"); // Example: Redirect to an error page
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieService.GetMovieByIdAsync(id.Value);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price,
                Rating = movie.Rating,
                ImageUrl = movie.ImageUrl
            };

            return View(viewModel);
        }

    }
}
