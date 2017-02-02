using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.IO;
namespace Mfr.Admin.App.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserPictureAvatar(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("AvatarUrl");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}