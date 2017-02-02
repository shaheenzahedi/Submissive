using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.City
{
    public class CityCreateOrUpdateViewModel
    {
        public CityCreateOrUpdateViewModel()
        {
            AvailableCountry = new List<SelectListItem>();
            AvailableState = new List<SelectListItem>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Name City :")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "State :")]
        public int StateId { get; set; }

        public string StateTitle { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Country :")]
        public int CountryId { get; set; }

        public string CountryTitle { get; set; }

        [Display(Name = "Showable :")]
        public bool Show { get; set; }

        [Display(Name = "Country")]
        public int SelectedCountryId { get; set; }

        [Display(Name = "State")]
        public int SelectedStateId { get; set; }

        public IList<SelectListItem> AvailableCountry { get; set; }

        public IList<SelectListItem> AvailableState { get; set; } 
    }
}