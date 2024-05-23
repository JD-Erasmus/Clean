using AutoMapper;
using Clean.Domain.Models;
using Clean.MVC.ViewModels;
namespace Clean.MVC.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();

        }
    }
}