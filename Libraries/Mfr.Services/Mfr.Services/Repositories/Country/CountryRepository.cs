using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.Country
{
    public class CountryRepository : Repository<Core.Model.Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        public CountryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       

    }
}
