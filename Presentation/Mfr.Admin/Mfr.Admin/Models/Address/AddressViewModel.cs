using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.Address
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            AvailableCountry = new List<SelectListItem>();
        }

        public int Id { get; set; }

        public int UserApplicationId { get; set; }
       // [Required]
        public int CountryId { get; set; }
       // [Required]
        public int StateId { get; set; }
       // [Required]
        public int CityId { get; set; }

        public string CityName { get; set; }

        [Display(Name = "Address Detail")]
        public string Description { get; set; }

        public IList<SelectListItem> AvailableCountry { get; set; }
    }
    }