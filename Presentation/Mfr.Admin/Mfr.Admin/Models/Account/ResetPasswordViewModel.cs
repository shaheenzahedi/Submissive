using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Field {0} is empty")]
        [EmailAddress(ErrorMessage = "E-mail is not in a correct format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$", ErrorMessage = "Password at least must have one lower-case character one upper case character and one symbol")]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Compare("Password", ErrorMessage = "Passwords must be the same")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat new password")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}