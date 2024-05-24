using Clean.Domain.Interfaces;
using Clean.Infra.Data.Context;
using Clean.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;

        public UnitOfWork(MovieDbContext context)
        {
            _context = context;
            Movies = new MovieRepository(_context);
        }

        public IMovieRepository Movies { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
