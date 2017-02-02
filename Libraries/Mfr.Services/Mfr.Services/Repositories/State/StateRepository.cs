using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;

namespace Mfr.Services.Repositories.State
{
    public class StateRepository : Repository<Core.Model.State>, IStateRepository
    {
        private readonly ApplicationDbContext _context;
        public StateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IList<Core.Model.State> GetStateByCountryId(int id, bool showHidden)
        {
            var res = Find(x => x.CountryId == id && x.Show==true) as IList<Core.Model.State>;
            if (res == null) throw new ArgumentNullException();
            return res;

        }
        public IEnumerable<Core.Model.State> GetByCountryId(int id)
        {
            if (id == 0)
                throw new ArgumentException("bad request");

            return _context.State.Where(p => p.CountryId == id).ToList();
        }
    }
}
