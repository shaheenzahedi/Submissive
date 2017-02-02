using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.Account
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Field {0} is empty")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$", ErrorMessage = "Password at least must have one lower-case character one upper case character and one symbol")]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must be the same")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat new password")]
        public string ConfirmPassword { get; set; }



    }
}
//        ^                  // the start of the string
//(?=.*[a-z])        // use positive look ahead to see if at least one lower case letter exists
//(?=.*[A-Z])        // use positive look ahead to see if at least one upper case letter exists
//(?=.*\d)           // use positive look ahead to see if at least one digit exists
//(?=.*[_\W])        // use positive look ahead to see if at least one underscore or non-word character exists
//.+                 // gobble up the entire string
//$                  // the end of the string