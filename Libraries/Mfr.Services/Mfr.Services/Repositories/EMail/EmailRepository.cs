using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.EMail
{
    public class EmailRepository : Repository<Core.Model.EMail>, IEmailRepository
    {
        private readonly ApplicationDbContext _context;
        public EmailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
    }
}
