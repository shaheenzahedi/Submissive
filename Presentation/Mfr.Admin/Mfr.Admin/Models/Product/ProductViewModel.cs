using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Mfr.Admin.Models.Product
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            IsSaved = false;
            AvailableImageUrl = new List<string>(); 
            AvailableImageOriginalFileNames = new List<string>();

        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Category  ")]

        public int ProductTypeId { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Name  ")]

        public string Title { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Description ")]
        public string Description { get; set; }

        public string ProductTypeName { get; set; }

        public string ImageUrl { get; set; }

        public IList<string> AvailableImageUrl { get; set; }

        public IList<string> AvailableImageOriginalFileNames { get; set; }

        public bool IsSaved { get; set; }

    }
}