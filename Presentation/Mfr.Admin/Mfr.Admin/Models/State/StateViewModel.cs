using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.State
{
    public class StateViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public bool Show { get; set; }
    }
}