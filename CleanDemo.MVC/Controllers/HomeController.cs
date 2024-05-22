using Clean.Application.Interfaces;
using CleanDemo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clean.MVC.ViewModels;


namespace CleanDemo.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMovieService movieService, ILogger<HomeController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetMovies();
            var movieViewModels = movies.Select(movie => new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price,
                Rating = movie.Rating,
                ImageUrl = movie.ImageUrl
            }).ToList();

            return View(movieViewModels);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
