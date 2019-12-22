using TeknikServis.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TeknikServis.Logic
{
    internal class RoleActions
    {
        internal void AddUserAndRole()
        {
            // Access the application context and create result variables.
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object. 
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            // Then, you create the "canEdit" role if it doesn't already exist.
            if (!roleMgr.RoleExists("canEdit"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "canEdit" });
            }

            if (!roleMgr.RoleExists("Admin"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "Admin" });
            }

            if (!roleMgr.RoleExists("mudur"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "mudur" });
            }

            if (!roleMgr.RoleExists("servis"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "servis" });
            }

            if (!roleMgr.RoleExists("dukkan"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "dukkan" });
            }

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext  
            // object. Note that you can create new objects and use them as parameters in
            // a single line of code, rather than using multiple lines of code, as you did
            // for the RoleManager object.
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = "superadmin@cepservis.com",
                Email = "superadmin@cepservis.com",
                Firma = "BILGITAP"
            };


            //var appUser3 = new ApplicationUser
            //{
            //    UserName = "admin@tolbilisim.com",
            //    Email = "admin@tolbilisim.com",
            //    Firma = "TOL"
            //};
            IdUserResult = userMgr.Create(appUser, "T0m122-");
            //IdUserResult = userMgr.Create(appUser2, "T0m122-");
            //IdUserResult = userMgr.Create(appUser3, "T0m122-");
            // If the new "canEdit" user was successfully created, 
            // add the "canEdit" user to the "canEdit" role. 
            if (!userMgr.IsInRole(userMgr.FindByEmail("superadmin@cepservis.com").Id, "canEdit"))
            {
                IdUserResult = userMgr.AddToRole(userMgr.FindByEmail("superadmin@cepservis.com").Id, "canEdit");
            }

            //if (!userMgr.IsInRole(userMgr.FindByEmail("ilkadmin@wingtiptoys.com").Id, "Admin"))
            //{
            //    IdUserResult = userMgr.AddToRole(userMgr.FindByEmail("ilkadmin@wingtiptoys.com").Id, "Admin");
            //}
            //if (!userMgr.IsInRole(userMgr.FindByEmail("admin@tolbilisim.com").Id, "Admin"))
            //{
            //    IdUserResult = userMgr.AddToRole(userMgr.FindByEmail("admin@tolbilisim.com").Id, "Admin");
            //}
        }
    }
}