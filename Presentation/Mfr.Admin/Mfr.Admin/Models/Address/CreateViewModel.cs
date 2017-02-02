using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.Address
{
    public class CreateViewModel
    {

        public int Id { get; set; }
        public int UserApplicationId { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name ="Country")]
        public int CountryId { get; set; }

        public string   CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name ="State")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name ="City")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name ="Address Detail")]
        public string Description { get; set; }

    }
}