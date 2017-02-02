using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class ProductType
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool Show { get; set; }

        public virtual ICollection<Product> Products { get; set; } 
    }
}
