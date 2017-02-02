using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mfr.Admin.Models.User
{
    public class UserMainViewModel
    {
        public UserViewModel UserViewModel { get; set; }

        public UserPasswordViewModel UserPasswordViewModel { get; set; }

        public IList<UserRoleViewModel> UserRoleViewModel { get; set; }
    }
}