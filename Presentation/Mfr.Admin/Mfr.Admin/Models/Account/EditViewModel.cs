using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.Account
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "User Name")]
        // [Remote("DoesUserNameExist", "account", HttpMethod = "POST", ErrorMessage = "The user with the given name, is already given")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Family Name")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please put the mobile phone in a correct format")]
        [Display(Name = "Mobile")]
        public string Phonenumber { get; set; }
        //[Display(Name = "E-mail")]
        //[Required(ErrorMessage = "Field {0} is empty")]
        // [Remote("DoesEmailExist", "account", HttpMethod = "POST", ErrorMessage = "The user with the given E-mail, is already given")]
        //[EmailAddress(ErrorMessage = "Please put the E-mail in a correct format")]
        //public string Email { get; set; }
        [Display(Name = "Avatar Picture")]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }

    }
}