using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.PhoneNumber
{
    public class PhoneNumberRepository : Repository<Core.Model.PhoneNumber>, IPhoneNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public PhoneNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
}
