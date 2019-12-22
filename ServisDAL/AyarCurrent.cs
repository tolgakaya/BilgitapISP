using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;
using System.Web;

namespace ServisDAL
{
    public class AyarCurrent
    {
        //confige firma ekleyeceksin
        radiusEntities dc;
        public AyarCurrent(radiusEntities dc)
        {
            this.dc = dc;
      

        }
        public ayargenel get()
        {
            ayargenel ay;
            object o = HttpContext.Current.Cache["config"];
            if (o != null)
            {
                ay = (ayargenel)o;
            }
            else
            {

                ayargenel a = dc.ayargenels.FirstOrDefault();
                if (a != null)
                {
                    //HttpContext.Current.Cache.Add("config", a, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);
                    HttpContext.Current.Cache.Add("config", a, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

                }
                ay = a;

            }
            return ay;
        }

        public bool lisansKontrol()
        {
            DateTime? expire = this.get().lisanstarih;
            bool gecerli = false;
            if (expire != null && ((DateTime)expire).Date > DateTime.Now.Date)
            {
                gecerli = true;
            }
            return gecerli;
        }
        public void set(DateTime faturaTarihi)
        {
            DateTime? sonfatura = this.get().sonfatura;
            if (sonfatura != null)
            {
                //karşılaştır ve büyükse yaz ve cachele
                //büyük değilse bir şey yapma
                if (faturaTarihi.Date > sonfatura)
                {

                    ayargenel ay = dc.ayargenels.FirstOrDefault();
                    ay.sonfatura = faturaTarihi;
                    KaydetmeIslemleri.kaydetR(dc);

                    HttpContext.Current.Cache.Remove("config");
                    //HttpContext.Current.Cache.Add("config", ay, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);
                    HttpContext.Current.Cache.Add("config", ay, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

                }
            }
            else
            {
                //son fatura yokmuş
                //yaz ve cachele

                ayargenel ay = dc.ayargenels.FirstOrDefault();
                ay.sonfatura = faturaTarihi;
                KaydetmeIslemleri.kaydetR(dc);

                HttpContext.Current.Cache.Remove("config");
                //HttpContext.Current.Cache.Add("config", ay, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);
                HttpContext.Current.Cache.Add("config", ay, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

            }
        }
        public void lisanla(int ay)
        {
            //eski expire bugünden küçükse bugüne ekle
            //bugünden büyükse expirın üstüne ekle

            ayargenel ag = this.get();
            DateTime? eskiExp = ag.lisanstarih;
            DateTime yeniExp = DateTime.Now.AddMonths(ay);
            if (eskiExp != null && ((DateTime)eskiExp).Date > DateTime.Now.Date)
            {
                yeniExp = ((DateTime)eskiExp).AddMonths(ay);
               
            }
            ag.lisanstarih = yeniExp;
                
            KaydetmeIslemleri.kaydetR(dc);
            //bu session işe yaramaz çünkü lisanslamayı süper admin yapacak
             HttpContext.Current.Cache.Remove("config");
             HttpContext.Current.Cache.Add("config", ag, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

           

        }
        public void guncelle(DateTime faturaTarihi)
        {
            DateTime? sonfatura = this.get().sonfatura;
            if (sonfatura != null)
            {
                //karşılaştır ve büyükse yaz ve cachele
                //büyük değilse bir şey yapma
                if (faturaTarihi.Date > sonfatura)
                {

                    ayargenel ay = dc.ayargenels.FirstOrDefault();
                    ay.sonfatura = faturaTarihi;
                    KaydetmeIslemleri.kaydetR(dc);

                    HttpContext.Current.Cache.Remove("config");
                    //HttpContext.Current.Cache.Add("config", ay, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);
                    HttpContext.Current.Cache.Add("config", ay, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

                }
            }
            else
            {
                //son fatura yokmuş
                //yaz ve cachele

                ayargenel ay = dc.ayargenels.FirstOrDefault();
                ay.sonfatura = faturaTarihi;
                KaydetmeIslemleri.kaydetR(dc);

                HttpContext.Current.Cache.Remove("config");
                //HttpContext.Current.Cache.Add("config", ay, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);
                HttpContext.Current.Cache.Add("config", ay, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);

            }
        }

    }
}
