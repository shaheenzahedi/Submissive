using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "User Name")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "نام")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string FName { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Family Name")]
        [StringLength(100, ErrorMessage = "{0} Must be at least {2} characters", MinimumLength = 4)]
        public string LName { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Field {0} is empty")]
        //[RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "لطفا Mobile Phone را در فرم درست وارد کنید")]
        [Display(Name = "Mobile Phone")]
        public string Phonenumber { get; set; }

        [Display(Name = "Avatar")]
        [DataType(DataType.ImageUrl)]
        public string AvatarUrl { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }
    }
}