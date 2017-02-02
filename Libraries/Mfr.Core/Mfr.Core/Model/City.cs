using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class City
    {
        public int Id { get; set; }

        public int StateId { get; set; }

        public string Title { get; set; }

        public bool Show { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
