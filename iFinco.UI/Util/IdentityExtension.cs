using iFinco.UI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace iFinco.UI.Util
{
    public static class IdentityExtension
    {
        public static long GetCustomId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UID");
            // Test for null to avoid issues during local testing
            return (claim != null) ? Convert.ToInt64(claim.Value) : 0;
        }

        public static Nullable<long> GetCompanyId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CompanyId");
            // Test for null to avoid issues during local testing
            if (claim != null)
            {
                return Convert.ToInt64(claim.Value);
            }
            return null;
        }

        public static string GetCompanyName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CompanyName");
            if (claim == null)
            {
                return string.Empty;
            }
            return claim.Value.ToString();
        }

        public static string GetImage(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("image");
            if (claim != null && claim.Value != null)
                return claim.Value.ToString();
            else
                return "/PanalAssets/img/noImage.png";
        }

        public static string GetName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("name");
            return claim.Value.ToString();
        }

        public static string GetBranchName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BranchName");
            if (claim != null && claim.Value != null)
                return claim.Value.ToString();
            else
                return "Defualt";
        }

        public static long? GetBranchId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BranchId");
            if (claim != null && claim.Value != null)
                return Convert.ToInt64(claim.Value);
            else
                return null;
        }

        public static string GetUserRole(this IIdentity identity, string userId)
        {
            ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            List<string> roles = _userManager.GetRolesAsync(userId).Result.ToList();
            return String.Join(string.Empty, roles.ToArray());
        }

        public static bool IsOwner(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("IsOwner");
            if (claim != null && claim.Value != null)
                return Convert.ToBoolean(claim.Value);
            else
                return false;
        }
    }
}