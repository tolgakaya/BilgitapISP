using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;
using System.Web;

namespace ServisDAL.Repo
{
    public  class MusteriRepoC
    {
        radiusEntities dc;
        private string firma;
   
        public MusteriRepoC(string firmaParam)
        {
          
            this.firma = firmaParam;
            dc = MyContext.Context(firma);
        }

        //public List<customer> MusterilerBayi()
        //{
        //    string key="musteriler"+firma+bayi;
        //    if (HttpContext.Current.Cache[key]!=null)
        //    {
        //        return (List<customer>)HttpContext.Current.Cache[key];
        //    }
        //    else
        //    {
        //        List<customer> liste= dc.customers.Where(x => x.owner == bayi && x.Firma == firma).OrderByDescending(x => x.CustID).ToList();
        //        HttpContext.Current.Cache.Insert(key, liste, null, DateTime.Now.AddDays(20), TimeSpan.Zero);
        //        return liste;
        //    }
        //}
        public List<customer> Musteriler()
        {
            string key = "musteriler" + firma;
            if (HttpContext.Current.Cache[key] != null)
            {
                return (List<customer>)HttpContext.Current.Cache[key];
            }
            else
            {
                List<customer> liste = dc.customers.Where(x => x.Firma == firma).OrderByDescending(x => x.CustID).ToList();
                HttpContext.Current.Cache.Insert(key, liste, null, DateTime.Now.AddDays(20), TimeSpan.Zero);
                return liste;
            }
        }


    }
}
