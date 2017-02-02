using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class Address
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Country Country { get; set; }

        public virtual State State { get; set; }

        public virtual City City { get; set; }
    }
}
