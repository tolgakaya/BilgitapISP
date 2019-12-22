using System;
using System.Collections.Generic;
using System.Linq;
using ServisDAL.Repo;
using TeknikServis.Radius;

namespace ServisDAL
{

    public class KasaIslemleri
    {
        radiusEntities db;


        public KasaIslemleri(radiusEntities db)
        {

            this.db = db;
        }

        //kasalar her zaman adminin veya bayinin kullanıcı adına tanımlanıyor
        //bir admin bayisinin kasasını görecek olsa bile onun adıyla araması lazım

        public int AktifKasa()
        {
            return db.kasahesaps.FirstOrDefault().ID;
        }

        private KasaOzet Hesaplar(int gun, string kasa_turu)
        {
            KasaOzet oz = new KasaOzet();
            DateTime tarih = DateTime.Now.AddDays(-gun);
            List<Kasa> kasa_hareketler = (from k in db.kasaharekets
                                          where k.iptal == false && k.tarih >= tarih && k.KasaTur == kasa_turu
                                          orderby k.tarih descending
                                          select new Kasa
                                          {
                                              aktif_bakiye = k.aktif_bakiye,
                                              cikis = k.cikis,
                                              giris = k.giris,
                                              islem = k.islem,
                                              KasaTur = k.KasaTur,
                                              Odeme_ID = k.Odeme_ID,
                                              tarih = (DateTime)k.tarih
                                          }).ToList();
            decimal bakiye = 0;
            int adet = kasa_hareketler.Count;
            if (adet > 0)
            {
                bakiye = kasa_hareketler.FirstOrDefault().aktif_bakiye;

            }

            oz.islemeler = kasa_hareketler;
            oz.adet = adet;
            oz.aktif_bakiye = bakiye;

            return oz;
        }
        // giriş ve çıkışlarda rowdatabounda rengi değiştirelim.
        public KasaOzetOdemelerle HesaplarOdemeyle(DateTime baslangic, DateTime bitis, string kasa_turu)
        {
            KasaOzetOdemelerle oz = new KasaOzetOdemelerle();
            List<Kasa_Odeme> odemelerle = new List<Kasa_Odeme>();
            if (!String.IsNullOrEmpty(kasa_turu))
            {
                odemelerle = (from k in db.kasaharekets
                              //from o in db.musteriodemelers
                              where k.iptal == false && k.tarih >= baslangic && k.tarih <= bitis && k.KasaTur == kasa_turu
                              orderby k.tarih descending
                              select new Kasa_Odeme
                              {
                                  aktif_bakiye = k.aktif_bakiye,
                                  cikis = k.cikis,
                                  giris = k.giris,
                                  islem = k.islem,
                                  KasaTur = k.KasaTur,
                                  Odeme_ID = k.Odeme_ID,
                                  musteriAdi = k.customer.Ad,
                                  musteriID = k.Musteri_ID,
                                  odemeTarih = (DateTime)k.tarih,
                                  tahsilatOdeme_turu = k.musteriodemeler.tahsilat_turu,
                                  islem_adres = k.musteriodemeler.pos_id == null ?
                                  (k.musteriodemeler.banka_id == null ? (k.musteriodemeler.kart_id == null ? k.musteriodemeler.kart_tanims.kart_adi : "KASA") : k.musteriodemeler.banka.banka_adi) : k.musteriodemeler.pos_tanims.pos_adi,
                                  kullanici=k.inserted

                              }).ToList();
            }
            else
            {
                odemelerle = (from k in db.kasaharekets
                              //from o in db.musteriodemelers
                              where k.iptal == false && k.tarih >= baslangic && k.tarih <= bitis
                              orderby k.tarih descending
                              select new Kasa_Odeme
                              {
                                  aktif_bakiye = k.aktif_bakiye,
                                  cikis = k.cikis,
                                  giris = k.giris,
                                  islem = k.islem,
                                  KasaTur = k.KasaTur,
                                  Odeme_ID = k.Odeme_ID,
                                  musteriAdi = k.customer.Ad,
                                  musteriID = k.Musteri_ID,
                                  odemeTarih = (DateTime)k.tarih,
                                  tahsilatOdeme_turu = k.musteriodemeler.tahsilat_turu,
                                  islem_adres = k.musteriodemeler.pos_id == null ?
                                  (k.musteriodemeler.banka_id == null ? (k.musteriodemeler.kart_id == null ? k.musteriodemeler.kart_tanims.kart_adi : "KASA") : k.musteriodemeler.banka.banka_adi) : k.musteriodemeler.pos_tanims.pos_adi,
                                  kullanici=k.inserted

                              }).ToList();
            }

            decimal bakiye = 0;
            int adet = odemelerle.Count;
            if (adet > 0)
            {
                bakiye = odemelerle.FirstOrDefault().aktif_bakiye;
            }

            oz.islemeler = odemelerle;
            oz.adet = adet;
            oz.aktif_bakiye = bakiye;

            return oz;

        }
    }

    public class KasaOzetOdemelerle
    {
        public int adet { get; set; }
        public decimal aktif_bakiye { get; set; }
        public List<Kasa_Odeme> islemeler { get; set; }
    }
    public class Kasa_Odeme
    {
        //public int ID { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public int? Odeme_ID { get; set; }
        public decimal aktif_bakiye { get; set; }
        public string KasaTur { get; set; }
        public string islem { get; set; }


        public int musteriID { get; set; }
        public string musteriAdi { get; set; }


        public DateTime odemeTarih { get; set; }

        public string tahsilatOdeme_turu { get; set; }

        public string islem_adres { get; set; }
        public string kullanici { get; set; }

    }
    public class Kasa
    {
        //public int ID { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public int? Odeme_ID { get; set; }
        public decimal aktif_bakiye { get; set; }
        public string KasaTur { get; set; }
        public string islem { get; set; }
        public DateTime tarih { get; set; }


    }
    public class KasaOzet
    {
        public int adet { get; set; }
        public decimal aktif_bakiye { get; set; }
        public List<Kasa> islemeler { get; set; }
    }
}
