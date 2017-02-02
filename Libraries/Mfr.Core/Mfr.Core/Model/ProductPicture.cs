using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mfr.Core.Model
{
    public class ProductPicture
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string PictureUrl { get; set; }

        public string OriginalFileName { get; set; }

        public virtual Product Product { get; set; }
    }
}
