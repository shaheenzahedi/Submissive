using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Field {0} is empty")]
        [EmailAddress(ErrorMessage = "E-mail is not in a correct format")]
        public string Email { get; set; }
    }
}