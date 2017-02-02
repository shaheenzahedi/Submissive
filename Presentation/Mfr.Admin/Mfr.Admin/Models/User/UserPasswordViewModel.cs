using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.User
{
    public class UserPasswordViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$", ErrorMessage = "رمز ورود باید حداقل یک علامت و یک عدد داشته باشد،و حداقل باید دارای یک حروف بزرگ و یک حروف کوچک باشد('A' - 'Z')")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز جدید")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Field {0} is empty")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "رمز های عبور باید یکسان باشند.")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز جدید")]
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