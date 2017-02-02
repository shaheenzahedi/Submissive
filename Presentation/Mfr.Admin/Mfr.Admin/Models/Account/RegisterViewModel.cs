using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "User Name")]
        [Remote("DoesUserNameExist", "account", HttpMethod = "POST", ErrorMessage = "A  user with the given user name has been already decleared")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "نام")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Family Name")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string FamilyName { get; set; }

        [Display(Name = "Avatar")]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$", ErrorMessage = "Password at least must have one lower-case character one upper case character and one symbol")]
        [Display(Name = "Mobile Phone")]
        public string Phonenumber { get; set; }
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Field {0} is empty")]
        [Remote("DoesEmailExist", "account", HttpMethod = "POST", ErrorMessage = "A user with the given E-Mail is already defined")]
        [EmailAddress(ErrorMessage = "E-Mail is not in a correct format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$", ErrorMessage = "Password at least must have one lower-case character one upper case character and one symbol")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords are not the same")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
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