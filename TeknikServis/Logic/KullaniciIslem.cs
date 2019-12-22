using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TeknikServis.Logic
{


    public static class KullaniciIslem
    {
        public static List<IdentityRole> Roller()
        {
            Models.ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);
            return roleMgr.Roles.Where(x => x.Name != "canEdit").ToList();

        }
        public static List<KullaniciRepo> KulaniciRoller()
        {
            HttpContext hcontext = HttpContext.Current;
            var manager = hcontext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser kullanici = manager.FindById(hcontext.User.Identity.GetUserId());
            string firma = kullanici.Firma;

            Models.ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            //userid ve roleid

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            List<KullaniciRepo> deneme = (from k in userMgr.Users
                                          from r in roleMgr.Roles
                                          where r.Users.Any(x => x.UserId == k.Id) && k.Id != kullanici.Id && k.Firma == firma
                                          select new KullaniciRepo
                                          {
                                              id = k.Id,
                                              email = k.Email,
                                              userName = k.UserName,
                                              rol = r.Name,
                                              rolid = r.Id
                                          }).ToList();


            return deneme;
        }
        public static List<KullaniciRepo> KulaniciRoller(string terim)
        {
            HttpContext hcontext = HttpContext.Current;
            var manager = hcontext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser kullanici = manager.FindById(hcontext.User.Identity.GetUserId());
            string firma = kullanici.Firma;

            Models.ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            //userid ve roleid

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            List<KullaniciRepo> deneme = (from k in userMgr.Users
                                          from r in roleMgr.Roles
                                          where r.Users.Any(x => x.UserId == k.Id) && k.UserName.Contains(terim) && k.Id != kullanici.Id && k.Firma == firma
                                          select new KullaniciRepo
                                          {
                                              id = k.Id,
                                              email = k.Email,
                                              userName = k.UserName,
                                              rol = r.Name,
                                              rolid = r.Id
                                          }).ToList();


            return deneme;
        }
        public static List<KullaniciRepo> FirmaMudurleri()
        {
            HttpContext hcontext = HttpContext.Current;
            var manager = hcontext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser kullanici = manager.FindById(hcontext.User.Identity.GetUserId());
            string firma = kullanici.Firma;
            string owner = kullanici.UserName;
            Models.ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);
            List<IdentityUserRole> bayiler = roleMgr.FindByName("mudur").Users.ToList();
            //userid ve roleid

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            List<KullaniciRepo> liste = (from r in bayiler
                                         from u in userMgr.Users
                                         where r.UserId == u.Id && u.Firma == firma
                                         select new KullaniciRepo
                                         {
                                             id = u.Id,
                                             email = u.Email,
                                             userName = u.UserName,

                                         }).ToList();

            return liste;
        }
        public static List<KullaniciRepo> FirmaMudurleriAra(string terim)
        {
            HttpContext hcontext = HttpContext.Current;
            var manager = hcontext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser kullanici = manager.FindById(hcontext.User.Identity.GetUserId());
            string firma = kullanici.Firma;
            string owner = kullanici.UserName;
            Models.ApplicationDbContext context = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);
            List<IdentityUserRole> bayiler = roleMgr.FindByName("mudur").Users.ToList();
            //userid ve roleid

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            List<KullaniciRepo> liste = (from r in bayiler
                                         from u in userMgr.Users
                                         where r.UserId == u.Id && u.Firma == firma && u.UserName.Contains(terim)
                                         select new KullaniciRepo
                                         {
                                             id = u.Id,
                                             email = u.Email,
                                             userName = u.UserName,

                                         }).ToList();

            return liste;
        }

        public static List<KullaniciRepo> firmaKullanicilari()
        {
            List<KullaniciRepo> liste = new List<KullaniciRepo>();
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser kullanicimiz = manager.FindById(context.User.Identity.GetUserId());
            string firma = kullanicimiz.Firma;
            string id = kullanicimiz.Id;
            //if (manager.IsInRole(id, "Admin"))
            //{
            //    liste = (from u in manager.Users
            //             where u.Firma == firma
            //             select new KullaniciRepo
            //             {
            //                 id = u.Id,
            //                 email = u.Email,
            //                 userName = u.UserName,

            //             }).ToList();
            //}
            //else if (manager.IsInRole(id, "manager"))
            //{
            //manager sadece kendi kullanıclarını görecek


            //vazgeçtim kullanıcı işlemlerini sadece Admin yapabilir
            //burada sadece kendi blgilerini listede görmesin

            liste = (from u in manager.Users
                     where u.Firma == firma && u.Id != id
                     select new KullaniciRepo
                     {
                         id = u.Id,
                         email = u.Email,
                         userName = u.UserName


                     }).ToList();
            //}



            return liste;
        }

        public static string currentKullaniciID()
        {
            HttpContext context = HttpContext.Current;
            return context.User.Identity.GetUserId();
        }


        public static List<KullaniciRepo> firmaKullanicilari(string id)
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            List<KullaniciRepo> liste = (from u in manager.Users
                                         where u.Id == id
                                         select new KullaniciRepo
                                         {
                                             id = u.Id,
                                             email = u.Email,
                                             userName = u.UserName,

                                         }).ToList();


            return liste;
        }
        


        public static string firma()
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return manager.FindById(context.User.Identity.GetUserId()).Firma;
        }
        public static kullanici_repo currentKullanici()
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser use = manager.FindById(context.User.Identity.GetUserId());

            kullanici_repo rep = new kullanici_repo { Adres = use.Adres, Eposta = use.Email, Firma = use.Firma, FirmaTam = use.TamFirma, id = use.Id, Telefon = use.Tel, UserName = use.UserName, Web = use.Web };
            return rep;
        }


        public static IdentityResult updateKullanici(string id, string ad, string mail)
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser u = manager.FindById(id);
            u.UserName = ad;
            u.Email = mail;
            return manager.Update(u);

        }

        public static IdentityResult updateRole(string id, string yeniRol, string eskiRol)
        {
            HttpContext context = HttpContext.Current;
            var userMgr = context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            IdentityResult result = new IdentityResult();

            if (userMgr.IsInRole(userMgr.FindById(id).Id, eskiRol))
            {
                result = userMgr.RemoveFromRoles(userMgr.FindById(id).Id, eskiRol);

                if (!userMgr.IsInRole(userMgr.FindById(id).Id, yeniRol))
                {
                    result = userMgr.AddToRole(userMgr.FindById(id).Id, yeniRol);
                }
            }

            return result;

        }
        public static IdentityResult updateKullanici(string id, string ad, string mail, string sifre)
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            String hashedNewPassword = manager.PasswordHasher.HashPassword(sifre);
            ApplicationUser u = manager.FindById(id);
            u.UserName = ad;
            u.Email = mail;
            u.PasswordHash = hashedNewPassword;
            return manager.Update(u);

        }
        public static void updateKullanici(string id, string resimYol)
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser u = manager.FindById(id);
            u.resimYol = resimYol;
            manager.Update(u);
        }
        public static void delKullanici(string id)
        {
            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            ApplicationUser u = manager.FindById(id);
            manager.Delete(u);

        }
        public static string resimYol()
        {

            HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string yol = manager.FindById(context.User.Identity.GetUserId()).resimYol;
            if (String.IsNullOrEmpty(yol))
            {
                yol = "~/Uploads/anten2.png";
            }

            return yol;
        }
    }

    public class KullaniciRepo
    {
        public string id { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string rol { get; set; }
        public string rolid { get; set; }
    }
    public class kullanici_repo
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public string Eposta { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string Telefon { get; set; }
        public string Firma { get; set; }
        public string FirmaTam { get; set; }

    }

    public class firma_owner
    {
        public string id { get; set; }

        public string UserName { get; set; }
        public string Firma { get; set; }

        public string rol { get; set; }

        public string Eposta { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string Telefon { get; set; }
        public string FirmaTam { get; set; }
    }
}
