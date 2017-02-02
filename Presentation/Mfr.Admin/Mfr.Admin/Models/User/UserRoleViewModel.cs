using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mfr.Admin.Models.User
{
    public class UserRoleViewModel
    {
        
        public bool ApplyPrem { get; set; }
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public bool IsActive { get; set; }

        public bool BaseCoding { get; set; }

        public bool Show { get; set; }
    }
}