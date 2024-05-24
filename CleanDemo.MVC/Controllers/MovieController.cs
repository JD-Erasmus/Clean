using Clean.Application.Interfaces;
using Clean.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Clean.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CleanDemo.MVC.ViewModels;
using AutoMapper;
using Clean.MVC.MappingProfiles;
namespace CleanDemo.MVC.Controllers
{
  

    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        public MovieController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchString, string movieGenre)
        {// Get all movies and genres
            var movies = await _movieService.GetMoviesAsync();
            var genres = await _movieService.GetGenresAsync();

            // Filter movies based on search string and genre selection
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
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
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".webp", ".svg", ".png" };
                    var fileExtension = Path.GetExtension(viewModel.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file format. Please upload a JPG, WEBP, or SVG file.");
                        return View(viewModel);
                    }
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
                /*change to use auto mapper*/
                var movie = _mapper.Map<Movie>(viewModel);

                // Add the movie to the database using the MovieService
                await _movieService.AddMovieAsync(movie);
                TempData["SuccessMessage"] = "Added New Movie Successfully.";
                
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
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".webp", ".svg", ".png" };
                    var fileExtension = Path.GetExtension(viewModel.ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file format. Please upload a JPG, WEBP, or SVG file.");
                        return View(viewModel);
                    }
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
                await _movieService.UpdateMovieAsync(id, existingMovie);
                TempData["SuccessMessage"] = "Edited Movie Successfully.";
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
                await _movieService.DeleteMovieAsync(id);
                TempData["SuccessMessage"] = "Movie deleted successfully.";
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home"); 
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
