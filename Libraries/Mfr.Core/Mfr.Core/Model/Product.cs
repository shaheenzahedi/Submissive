using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class Product
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<ProductPicture> ProductPictures { get; set; } 

    }
}
