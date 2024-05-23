
using Microsoft.Extensions.DependencyInjection;
using Clean.Application.Interfaces;
using Clean.Application.Services;
using Clean.Domain.Interfaces;
using Clean.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace Clean.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
          
            //CleanArchitecture.Application
            services.AddScoped<IMovieService, MovieService>();

            //CleanArchitecture.Domain.Interfaces | CleanArchitecture.Infra.Data.Repositories
            services.AddScoped<IMovieRepository, MovieRepository>();

      
        }
    }
}
