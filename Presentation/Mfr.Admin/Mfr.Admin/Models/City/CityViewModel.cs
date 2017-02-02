using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.City
{
    public class CityViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int StateId { get; set; }

        public string StateTitle { get; set; }

        public int CountryId { get; set; }

        public string CountryTitle { get; set; }

        public bool Show { get; set; }

    }
}