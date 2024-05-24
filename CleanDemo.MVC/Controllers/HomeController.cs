using Clean.Application.Interfaces;
using CleanDemo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Clean.MVC.ViewModels;
using AutoMapper;


namespace CleanDemo.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;

        public HomeController(IMovieService movieService, ILogger<HomeController> logger, IMapper mapper)
        {
            _movieService = movieService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all movies asynchronously
            var movies = await _movieService.GetMoviesAsync();

            // Map Movie entities to MovieViewModels
            var movieViewModels = _mapper.Map<IEnumerable<MovieViewModel>>(movies);

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
