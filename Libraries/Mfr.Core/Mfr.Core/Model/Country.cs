using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Mfr.Core.Model
{
    public class Country
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Show { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<State> States { get; set; } 
    }
}
