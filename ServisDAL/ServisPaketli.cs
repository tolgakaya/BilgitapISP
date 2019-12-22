using System;
using System.Collections.Generic;
using System.Linq;
using ServisDAL.Repo;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class ServisPaketli
    {

        radiusEntities dc;
      
        public ServisPaketli(radiusEntities dc)
        {
            
            this.dc = dc;

        }
        public PaketRepo paket { get; set; }
        public List<Detay_Repo> detay { get; set; }

        public void PaketKaydet()
        {
            servis_pakets al = new servis_pakets();
            al.aciklama = paket.aciklama;
            al.paket_adi = paket.paket_adi;
            al.tutar = paket.tutar;
            al.Firma = "firma";
           
            al.iptal = false;

            if (detay.Count > 0)
            {
                foreach (Detay_Repo det in detay)
                {
                    servis_paket_detays d = new servis_paket_detays();
                    d.Aciklama = det.Aciklama;
                    d.adet = det.adet;
                    d.paket_id = al.paket_id;
                    d.cihaz_adi = det.cihaz_adi;
                    d.cihaz_gsure = det.cihaz_gsure;
                    d.cihaz_id = det.cihaz_id;
                    d.Firma = "firma";
                  
                    d.paket_adi = paket.paket_adi;
                    decimal tutar = (100 * det.Yekun) / (100 + (decimal)det.KDV);
                    d.Tutar = tutar;
                    d.KDV = det.KDV;
                    d.Yekun = det.Yekun;
                    d.IslemParca = det.IslemParca;
                    d.iptal = false;
                    al.servis_paket_detays.Add(d);

                }
            }
            dc.servis_pakets.Add(al);

             KaydetmeIslemleri.kaydetR(dc);
        }
       

        public List<PaketRepo> paketler()
        {
            return (from p in dc.servis_pakets
                    where p.iptal == false   
                    select new PaketRepo
                    {
                        paket_id = p.paket_id,
                        paket_adi = p.paket_adi,
                        tutar = p.tutar,
                        aciklama = p.aciklama
                    }).ToList();
        }

        public PaketOzet PaketGoster(int paket_id)
        {
            List<Detay_Repo> detaylar = (from p in dc.servis_paket_detays
                                         where p.iptal == false && p.paket_id == paket_id
                                         select new Detay_Repo
                                         {
                                             paket_id = p.paket_id,
                                             Aciklama = p.Aciklama,
                                             adet = p.adet,
                                             cihaz_adi = p.cihaz_adi,
                                             cihaz_gsure = p.cihaz_gsure,
                                             cihaz_id = p.cihaz_id,
                                             detay_id = p.id.ToString(),
                                             IslemParca = p.IslemParca,
                                             KDV =(decimal)p.KDV,
                                             Yekun = p.Yekun
                                         }).ToList();

            PaketRepo paket = (from p in dc.servis_pakets
                               where p.paket_id == paket_id
                               select new PaketRepo
                               {
                                   paket_id = p.paket_id,
                                   paket_adi = p.paket_adi,
                                   tutar = p.tutar,
                                   aciklama = p.aciklama
                               }).FirstOrDefault();
            return new PaketOzet { detaylar = detaylar, paket_bilgileri = paket };
        }
        public void paket_iptal(int paket_id)
        {
          



                //paketin kendisini iptal edelim
            //triggerla detayları da iptal edelim
                servis_pakets paketimiz = dc.servis_pakets.FirstOrDefault(x => x.paket_id == paket_id);
                if (paketimiz != null)
                {
                    paketimiz.iptal = true;

                }
                 KaydetmeIslemleri.kaydetR(dc);
            //}
        }

        public void paket_guncelle(int paket_id)
        {
            //önce paket detaylarını iptal edelim sonra yeni detayları ekleyelim
            List<servis_paket_detays> detaylar = dc.servis_paket_detays.Where(x => x.paket_id == paket_id).ToList();

            foreach (servis_paket_detays dett in detaylar)
            {
                dc.servis_paket_detays.Remove(dett);
            }
            servis_pakets al = dc.servis_pakets.FirstOrDefault(x => x.paket_id == paket_id);
            al.aciklama = paket.aciklama;
            al.paket_adi = paket.paket_adi;
            al.tutar = paket.tutar;
        

            if (detay.Count > 0)
            {
                foreach (Detay_Repo det in detay)
                {
                    servis_paket_detays d = new servis_paket_detays();
                    d.Aciklama = det.Aciklama;
                    d.adet = det.adet;
                    d.paket_id = al.paket_id;
                    d.cihaz_adi = det.cihaz_adi;
                    d.cihaz_gsure = det.cihaz_gsure;
                    d.cihaz_id = det.cihaz_id;
                    d.Firma = "firma";
                    d.KDV = det.KDV;
                    d.paket_adi = paket.paket_adi;
                     decimal tutar = det.Yekun;
                    if (det.KDV!=null)
                    {
                        tutar = (100 * det.Yekun) / (100 + (decimal)det.KDV);
                    }
                    
                    d.Tutar = tutar;
                    d.Yekun = det.Yekun;
                    d.IslemParca = det.IslemParca;
                    d.iptal = false;
                    al.servis_paket_detays.Add(d);

                }
            }
        

             KaydetmeIslemleri.kaydetR(dc);
        }

    }

    public class PaketOzet
    {
        public PaketRepo paket_bilgileri { get; set; }
        public List<Detay_Repo> detaylar { get; set; }
    }
    public class Detay_Repo
    {
        public string detay_id { get; set; }
        public int paket_id { get; set; }

        public string IslemParca { get; set; }

        public decimal KDV { get; set; }
        public decimal Yekun { get; set; }
        public string Aciklama { get; set; }
        public int adet { get; set; }
        public Nullable<int> cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public Nullable<int> cihaz_gsure { get; set; }


    }
    public class paket_secim_repo
    {
        public int paket_id { get; set; }
        public string paket_adi { get; set; }
    }
    public class PaketRepo
    {
        public int paket_id { get; set; }
        public string paket_adi { get; set; }
        public decimal tutar { get; set; }
        public string aciklama { get; set; }


    }


}
