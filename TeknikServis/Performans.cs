using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeknikServis.Radius;

namespace TeknikServis
{
    public class Performans
    {
        private string firma;
        public Performans(string firma)
        {
            this.firma = firma;
        }
        public List<customer> liste()
        {
            using (radiusEntities dc=MyContext.Context("TOL"))
            {
                return dc.customers.Where(x => x.Firma == "TOL").ToList();
            }
        }
        public void ekle()
        {
            customer c = new customer();
            c.Firma = "TOL";
            c.Ad = "Test";
            c.Adres = "Test";
            c.email = "test";
            c.TC = "test";
            c.telefon = "test";
            c.telefon_cep = "test";
            using (radiusEntities dc=MyContext.Context("TOL"))
            {
                dc.customers.Add(c);
                dc.SaveChanges();
            }
        }
        public void sil()
        {
            using (radiusEntities dc=MyContext.Context("TOL"))
            {
                customer c = dc.customers.Where(x => x.Ad == "Test").FirstOrDefault();
                if (c!=null)
                {
                    dc.customers.Remove(c);
                    dc.SaveChanges();
                }
            }
        }
             

    }
    public class entegre
    {
        private radiusEntities dc;
         public entegre(radiusEntities dc)
        {
            this.dc = dc;
        }
        public List<customer> liste()
        {
           
                return dc.customers.Where(x => x.Firma == "TOL").ToList();
           
        }
        public void ekle()
        {
            customer c = new customer();
            c.Firma = "TOL";
            c.Ad = "Test";
            c.Adres = "Test";
            c.email = "test";
            c.TC = "test";
            c.telefon = "test";
            c.telefon_cep = "test";
          
                dc.customers.Add(c);
                dc.SaveChanges();
           
        }
        public void sil()
        {
               customer c = dc.customers.Where(x => x.Ad == "Test").FirstOrDefault();
                if (c!=null)
                {
                    dc.customers.Remove(c);
                    dc.SaveChanges();
                }
          
        }
    }
}