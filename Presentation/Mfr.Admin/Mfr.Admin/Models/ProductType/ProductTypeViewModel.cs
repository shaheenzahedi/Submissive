using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.ProductType
{
    public class ProductTypeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public bool Show { get; set; }
    }
}