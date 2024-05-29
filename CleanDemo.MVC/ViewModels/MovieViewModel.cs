using Clean.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Clean.MVC.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Rating { get; set; }
        [BindNever]
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        [BindNever]
        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();
    }
}
