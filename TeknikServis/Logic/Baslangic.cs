using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServisDAL;
using TeknikServis.Radius;
using Microsoft.AspNet.Identity;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
namespace TeknikServis.Logic
{
    public class Baslangic
    {
        private string firma;
        radiusEntities dc;
        public Baslangic(string firmaParam)
        {
            this.firma = firmaParam;
            dc = MyContext.Context(firma);
        }
        public string Kur()
        {
            string sonuc = "";
            //Kurulum k = new Kurulum(firma);
            //sonuc += k.Musteri();
            //sonuc += bayi();
            //sonuc += k.defaultlar();
            return sonuc;
        }
        public string bayi()
        {
           /* string sonuc = "";
            int count = 0;
            List<rm_managers> yoneticiler = dc.rm_managers.ToList();
            if (yoneticiler.Count > 0)
            {
                foreach (rm_managers man in yoneticiler)
                {
                    if (man.managername != this.admin)
                    {
                        tekBayi(man);
                        count++;
                    }

                }
                KaydetmeIslemleri.kaydetR(dc);
            }
            sonuc = count.ToString() + " adet yönetici tanımlandı<br />";
            return sonuc;*/
            return "";
        }

        //başlangıçta bir admin ismi al
        // manager listesini al
        //managerın ismi Admin ise geç
        //değilse manager olarak kaydet

        private void tekBayi()
        {
           /* HttpContext context = HttpContext.Current;
            var manager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string mail = m.email;
            if (String.IsNullOrEmpty(m.email))
            {
                mail = m.managername + "@" + firma + ".com";
            }
            var user = new ApplicationUser() { UserName = m.managername, Email = mail, Firma = firma, Adres = adres, Tel = telefon, Web = web, TamFirma = TamFirma,  resimYol = resimyol };
            IdentityResult result = manager.Create(user, m.managername);
            if (result.Succeeded)
            {
                if (!manager.IsInRole(manager.FindByEmail(mail).Id, "manager"))
                {
                    result = manager.AddToRole(manager.FindByEmail(mail).Id, "manager");
                    Radius.adminliste list = new Radius.adminliste();
                    list.Firma = firma;
                    list.username = m.managername;
                    list.adres = adres;
                    list.email = mail;
                    list.FirmaTam = TamFirma;
                    list.tel = telefon;
                    dc.adminlistes.Add(list);
                }
            }
            */

        }
        public string TamFirma { get; set; }
        public string telefon { get; set; }
        public string web { get; set; }
        public string admin { get; set; }
        public string resimyol { get; set; }
        public string adres { get; set; }

    }
    [Serializable]
    public class wrap
    {
        public string admin { get; set; }
        public string resimyol { get; set; }
        public string TamFirma { get; set; }
        public string telefon { get; set; }
        public string web { get; set; }
        public string adres { get; set; }
        public string firma { get; set; }

    }
}