using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Services.Repositories.State
{
    public interface IStateRepository: IRepository<Core.Model.State>
    {
        IList<Core.Model.State> GetStateByCountryId(int id, bool showHidden);

        IEnumerable<Core.Model.State> GetByCountryId(int id);
    }
}
