using iFinco.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iFinco.UI.Util
{
    public class StartUpConfiguration
    {
        public static void RegisterRoles()
        {
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                role.IsForAdmin = true;
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Owner"))
            {
                var role = new ApplicationRole();
                role.Name = "Owner";
                role.IsForAdmin = false;
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Sales"))
            {
                var role = new ApplicationRole();
                role.Name = "Sales";
                role.IsForAdmin = false;
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Order Taker"))
            {
                var role = new ApplicationRole();
                role.Name = "Order Taker";
                role.IsForAdmin = false;
                roleManager.Create(role);
            }
        }
    }
}