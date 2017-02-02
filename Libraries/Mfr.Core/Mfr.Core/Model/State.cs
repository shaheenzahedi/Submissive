using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class State
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string Title { get; set; }

        public bool Show { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<Address> States { get; set; } 

    }
}
