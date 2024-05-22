/*using Clean.Application.Interfaces;
using Clean.Application.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanDemo.MVC.Controllers
{
    public class MovieControllerold : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<MovieControllerold> _logger;

        public MovieControllerold(IMovieService movieService, IWebHostEnvironment environment, ILogger<MovieControllerold> logger)
        {
            _movieService = movieService;
            _environment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = _movieService.GetMovies();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating, ImageFile")] MovieViewModel movieViewModel, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".webp", ".svg" };
                        var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("ImageFile", "Invalid file format. Please upload a JPG, WEBP, or SVG file.");
                            return View(movieViewModel);
                        }

                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "Uploads", "Movies");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        movieViewModel.ImageUrl = $"/images/Uploads/Movies/{uniqueFileName}";
                    }

                    await _movieService.AddMovie(movieViewModel);
                    TempData["SuccessMessage"] = "Added New Movie Successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding a movie.");
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            return View(movieViewModel);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
*/