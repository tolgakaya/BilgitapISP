using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class HizliSatis
    {
        radiusEntities dc;

        public HizliSatis(radiusEntities dc)
        {

            this.dc = dc;
        }

        public void Nakit(List<satis_helper> liste, decimal iskonto, string kullanici)
        {
            foreach (satis_helper ss in liste)
            {
                decimal net = ss.yekun - (ss.yekun * iskonto / 100);
                musteriodemeler a = new musteriodemeler();
                a.Aciklama = "peşin satış";
                a.belge_no = "belgeno";
                a.Firma = "firma";
                a.iptal = false;
                a.islem_tarihi = DateTime.Now;
                a.kullanici = "onemsiz";
                a.KullaniciID = "onemsiz";
                a.Musteri_ID = -13;
                a.no = "0";
                a.OdemeMiktar = net;
                a.OdemeTarih = DateTime.Now;
                a.tahsilat_odeme = "tahsilat";
                a.tahsilat_turu = "Nakit";
                a.taksit_no = -1;
                a.inserted = kullanici;
                dc.musteriodemelers.Add(a);

                cihaz_grups grup = dc.cihaz_grups.FirstOrDefault(x => x.grupid == ss.grupid);

                decimal kdv = 0;
                decimal oiv = 0;
                decimal otv = 0;

                if (grup.kdv != null)
                {
                    kdv = (decimal)grup.kdv;
                }
                if (grup.oiv != null)
                {
                    oiv = (decimal)grup.oiv;
                }
                if (grup.otv != null)
                {
                    otv = (decimal)grup.otv;
                }

                decimal tutar = (100 * net) / (kdv + oiv + otv + 100);

                satislar s = new satislar();
                s.adet = ss.adet;
                s.bandrol = 0;
                s.cihaz_id = ss.cihaz_id;
                s.musteri_id = -13;
                s.odeme_id = a.Odeme_ID;
                s.iptal = false;
                s.oiv = tutar * oiv / 100;
                s.otv = tutar * otv / 100;
                s.kdv = tutar * kdv / 100;
                s.tarih = DateTime.Now;
                s.tutar = tutar;
                s.vergi = 0;
                s.iskonto_oran = iskonto;
                s.yekun = net;
                s.Firma = "firma";
                s.inserted = kullanici;
                //fifoya göre çıkılacak stoğun hangi girişli stok olduğunu belirlemek için  satış kalemnine 
                //o stoğun kodunu girecez

                int stok_id = (from f in dc.cihaz_fifos
                               where f.cihaz_id == ss.cihaz_id && f.bakiye > 0
                               select f).FirstOrDefault().id;
                s.stok_id = stok_id;

                a.satislars.Add(s);
                dc.musteriodemelers.Add(a);

            }
            KaydetmeIslemleri.kaydetR(dc);



        }
        public void Kart(List<satis_helper> liste, int pos_id, decimal iskonto, string kullanici)
        {

            foreach (satis_helper ss in liste)
            {
                decimal net = ss.yekun - (ss.yekun * iskonto / 100);
                musteriodemeler a = new musteriodemeler();
                a.Aciklama = "peşin satış";
                a.belge_no = "belgeno";
                a.Firma = "firma";
                a.iptal = false;
                a.islem_tarihi = DateTime.Now;
                a.kullanici = "onemsiz";
                a.KullaniciID = "onemsiz";
                a.Musteri_ID = -13;
                a.no = "0";
                a.OdemeMiktar = net;
                a.mahsup = false;
                a.OdemeTarih = DateTime.Now;
                a.tahsilat_odeme = "tahsilat";
                a.tahsilat_turu = "Kart";
                a.pos_id = pos_id;
                a.inserted = kullanici;
                a.taksit_no = -1;

                decimal kdv = 0;
                decimal oiv = 0;
                decimal otv = 0;
                cihaz_grups grup = dc.cihaz_grups.FirstOrDefault(x => x.grupid == ss.grupid);

                if (grup.kdv != null)
                {
                    kdv = (decimal)grup.kdv;
                }
                if (grup.oiv != null)
                {
                    oiv = (decimal)grup.oiv;
                }
                if (grup.otv != null)
                {
                    otv = (decimal)grup.otv;
                }

                decimal tutar = (100 * net) / (kdv + oiv + otv + 100);

                dc.musteriodemelers.Add(a);
                satislar s = new satislar();
                s.adet = ss.adet;
                s.bandrol = 0;
                s.cihaz_id = ss.cihaz_id;
                s.musteri_id = -13;
                s.odeme_id = a.Odeme_ID;
                s.iptal = false;
                s.oiv = tutar * oiv / 100;
                s.otv = tutar * otv / 100;
                s.kdv = tutar * kdv / 100;
                s.tarih = DateTime.Now;
                s.tutar = ss.tutar;
                s.vergi = 0;
                s.iskonto_oran = iskonto;
                s.yekun = net;
                s.Firma = "firma";
                s.inserted = kullanici;
                a.satislars.Add(s);
                dc.musteriodemelers.Add(a);

            }
            KaydetmeIslemleri.kaydetR(dc);



        }
        public List<satis_helper> liste(DateTime bas, DateTime son)
        {

            List<satis_helper> satis = (from s in dc.satislars
                                        where s.iptal == false && s.tarih >= bas && s.tarih <= son
                                        select new satis_helper
                                        {
                                            satis_id = s.satis_id,
                                            cihaz_id = s.cihaz_id,
                                            cihaz_adi = s.cihaz.cihaz_adi,
                                            adet = s.adet,
                                            iskonto = s.iskonto_oran,
                                            yekun = s.yekun,
                                            tarih = s.tarih,
                                            kullanici=s.inserted
                                        }).ToList();
            return satis;

        }

        public void iptal(int satis_id, string kullanici)
        {

            satislar s = dc.satislars.FirstOrDefault(x => x.satis_id == satis_id);
            if (s != null)
            {
                s.iptal = true;
                s.deleted = kullanici;
                s.musteriodemeler.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }

        }
    }
    public class satis_helper
    {
        public int satis_id { get; set; }
        public int cihaz_id { get; set; }
        public int grupid { get; set; }
        public int adet { get; set; }
        public string cihaz_adi { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal oiv { get; set; }
        public decimal otv { get; set; }
        public decimal iskonto { get; set; }
        public decimal yekun { get; set; }
        public decimal vergi_toplami { get; set; }
        public DateTime tarih { get; set; }
        public string kullanici { get; set; }
    }

}
