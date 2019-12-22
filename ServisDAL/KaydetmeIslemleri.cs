using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Web;
using TeknikServis.Radius;
using System.Linq;

namespace ServisDAL
{
    public static class KaydetmeIslemleri
    {
      
        public static void kaydetR(radiusEntities db)
        {

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Dictionary<string, string> mesajlar = new Dictionary<string, string>();
                int count = 0;
                foreach (var errs in ex.EntityValidationErrors)
                {

                    foreach (var err in errs.ValidationErrors)
                    {
                        string propName = err.PropertyName + "-" + count.ToString();
                        count++;
                        string errMess = err.ErrorMessage;
                        mesajlar.Add(propName, errMess);
                    }
                }
                HttpContext.Current.Session["mesaj"] = mesajlar;
                HttpContext.Current.Response.Redirect("/Sonuc.aspx");
            }
            //catch (DbUpdateConcurrencyException exxx)
            //{

            //    //var entry = exxx.Entries.Single();
            //    exxx.Entries.Single().Reload();
            //    //entry.OriginalValues.SetValues(entry.GetDatabaseValues());
            //}
            //catch (Exception exx)
            //{
            //    HttpContext.Current.Session["mesele"] = exx.Message;
            //    HttpContext.Current.Response.Redirect("/Sonuc.aspx");
            //}
         

        }
    }
}
