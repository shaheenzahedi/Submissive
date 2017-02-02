using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.PhoneNumber
{
    public class PhoneNumberViewModel
    {
        public int Id { get; set; }

        public int UserApplicationId { get; set; }

        public string Number { get; set; }

        public int UserId { get; set; }
    }
}