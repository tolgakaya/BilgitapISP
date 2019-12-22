using ServisDAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class Hareket
    {

        radiusEntities db;
        public Hareket(radiusEntities db)
        {

            this.db = db;
        }

        private TeknikServis.Radius.musteriodemeler tekOdemeR(int id)
        {
            return db.musteriodemelers.Where(p => p.Odeme_ID == id).FirstOrDefault();
        }

        public void odemeIptalR(int id, string kullanici)
        {
            // mahsupta iki türlü silinebilir, bir tahsilatı yapılan iki ödemesi yapılan 
            // BURADA eğer borç ise silinmeyecek. sadece fatura iptal edilecek. 
            TeknikServis.Radius.musteriodemeler odeme = tekOdemeR(id);
            odeme.iptal = true;
            odeme.deleted = kullanici;
            //mahsuben tahsilatı yapılmış bir işlemse
            if (odeme.mahsup == true)
            {

                //burada mahsubun diğer kaydıyla birlikte iptal edelim
                List<musteriodemeler> mahsuplar = db.musteriodemelers.Where(x => x.mahsup_key == odeme.mahsup_key).ToList();
                foreach (musteriodemeler o in mahsuplar)
                {
                    o.iptal = true;
                    o.deleted = kullanici;
                }


            }
            else
            {
                odeme.iptal = true;
                odeme.deleted = kullanici;
            }

            KaydetmeIslemleri.kaydetR(db);

        }




        public List<musteriOdemeRepo> musterininOdemeleriR(int musteriID, int gun)
        {
            DateTime tarih = DateTime.Now.AddDays(-gun);

            return (from o in db.musteriodemelers
                    where o.iptal == false && o.Musteri_ID == musteriID && o.OdemeTarih >= tarih
                    orderby o.OdemeTarih descending
                    select new musteriOdemeRepo
                    {
                        aciklama = o.Aciklama,
                        kullaniciID = o.KullaniciID,
                        musteriAdi = o.customer.Ad,
                        musteriID = o.Musteri_ID,
                        odemeID = o.Odeme_ID,
                        odemeMiktari = o.OdemeMiktar,
                        kullanici = o.inserted,
                        odemeTarih = o.OdemeTarih,
                        tahsilat_odeme = o.tahsilat_odeme,
                        tahsilatOdeme_turu = o.tahsilat_turu,
                        islem_adres = o.pos_id == null ?
                        (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                        masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                        islem_tarihi = o.islem_tarihi
                    }).ToList();
        }

        public List<musteriOdemeRepo> musterininOdemeleriR(int musteriID, DateTime baslangic, DateTime son)
        {

            return (from o in db.musteriodemelers
                    where o.iptal == false && o.Musteri_ID == musteriID && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                    orderby o.OdemeTarih descending
                    select new musteriOdemeRepo
                    {
                        aciklama = o.Aciklama,
                        kullaniciID = o.KullaniciID,
                        musteriAdi = o.customer.Ad,
                        musteriID = o.Musteri_ID,
                        odemeID = o.Odeme_ID,
                        odemeMiktari = o.OdemeMiktar,
                        kullanici = o.inserted,
                        odemeTarih = o.OdemeTarih,
                        tahsilat_odeme = o.tahsilat_odeme,
                        tahsilatOdeme_turu = o.tahsilat_turu,
                        islem_adres = o.pos_id == null ?
                        (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                        masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                        islem_tarihi = o.islem_tarihi
                    }).ToList();
        }
        //ödeme repo gösterim
        public List<musteriOdemeRepo> musterininOdemeleriR(int gun)
        {
            DateTime tarih = DateTime.Now.AddDays(-gun);

            return (from o in db.musteriodemelers
                    where (o.iptal == null || o.iptal == false) && o.OdemeTarih >= tarih
                    orderby o.OdemeTarih descending
                    select new musteriOdemeRepo
                    {
                        aciklama = o.Aciklama,
                        kullaniciID = o.KullaniciID,
                        musteriAdi = o.customer.Ad,
                        musteriID = o.Musteri_ID,
                        odemeID = o.Odeme_ID,
                        kullanici = o.inserted,
                        odemeMiktari = o.OdemeMiktar,
                        odemeTarih = o.OdemeTarih,
                        tahsilat_odeme = o.tahsilat_odeme,
                        tahsilatOdeme_turu = o.tahsilat_turu,
                        islem_adres = o.pos_id == null ?
                        (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                        masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                        islem_tarihi = o.islem_tarihi

                    }).ToList();

        }

        public List<musteriOdemeRepo> musterininOdemeleriR(DateTime baslangic, DateTime son)
        {

            return (from o in db.musteriodemelers
                    where (o.iptal == null || o.iptal == false) && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                    orderby o.OdemeTarih descending
                    select new musteriOdemeRepo
                    {
                        aciklama = o.Aciklama,
                        kullaniciID = o.KullaniciID,
                        musteriAdi = o.customer.Ad,
                        musteriID = o.Musteri_ID,
                        kullanici = o.inserted,
                        odemeID = o.Odeme_ID,
                        odemeMiktari = o.OdemeMiktar,
                        odemeTarih = o.OdemeTarih,
                        tahsilat_odeme = o.tahsilat_odeme,
                        tahsilatOdeme_turu = o.tahsilat_turu,
                        islem_adres = o.pos_id == null ?
                        (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                        masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                        islem_tarihi = o.islem_tarihi

                    }).ToList();

        }
        public List<CariDetayYeni> CariDetayREski(int musteriID, int gun)
        {
            // cari detay için
            // service_faturas -onaylanmış servis kararları- cariye işlenmiş zaten
            // faturas'ta sadece turu fatura olanlar
            // sonra da müşteri ödeme ve tahsilat hareketleri
            DateTime tarih = DateTime.Now.AddDays(-gun);


            IEnumerable<CariDetayYeni> hesaplar = from s in db.servicehesaps
                                                  where s.iptal == false && (s.MusteriID == musteriID || s.tamirci_id == musteriID) && s.Onay_tarih >= tarih && s.Yekun > 0
                                                  select new CariDetayYeni
                                                  {
                                                      //müşteri hesabı için servis toplamlarını kullanacağım
                                                      //o yüzden burada hesaplara sıfır yazdım
                                                      MusteriID = (int)s.MusteriID,
                                                      aciklama = s.Aciklama,
                                                      musteriAdi = s.customer.Ad,
                                                      borc = s.tamirci_id == musteriID ? s.toplam_maliyet : null,
                                                      //alacak = s.tamirci_id == musteriID ? 0 : s.Yekun,
                                                      alacak = null,
                                                      tarih = (DateTime)s.Onay_tarih,
                                                      islem = s.IslemParca,
                                                      konu = s.adet + " Adet" + s.cihaz_adi,
                                                      kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                                                  };

            IEnumerable<CariDetayYeni> servis = from s in db.services
                                                where s.iptal == false && (s.CustID == musteriID || s.usta_id == musteriID) && s.AcilmaZamani >= tarih && s.KapanmaZamani != null && s.service_faturas.Yekun > 0
                                                select new CariDetayYeni
                                                {
                                                    //hakedişin prim oranlarına göre hesaplanması gerek
                                                    //service faturasta triggerla yapılıyor
                                                    MusteriID = (int)s.CustID,
                                                    aciklama = s.Aciklama,
                                                    musteriAdi = s.customer.Ad,
                                                    borc = s.usta_id == musteriID ? (decimal)(s.service_faturas.toplam_fark) : 0,
                                                    alacak = s.usta_id == musteriID ? 0 : s.service_faturas.Yekun,
                                                    tarih = (DateTime)s.AcilmaZamani,
                                                    islem = s.Baslik,
                                                    konu = s.urun.Cinsi,
                                                    kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                                                };
            IEnumerable<CariDetayYeni> odeme_tahsilat = from o in db.musteriodemelers
                                                        where o.iptal == false && o.Musteri_ID == musteriID && o.OdemeTarih >= tarih
                                                        // orderby o.OdemeTarih descending
                                                        select new CariDetayYeni
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            borc = o.tahsilat_odeme == "tahsilat" ? o.OdemeMiktar : 0,
                                                            alacak = o.tahsilat_odeme == "odeme" ? o.OdemeMiktar : 0,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_turu,
                                                            konu = o.tahsilat_turu == "iade" ? "Ürün iadesi" : (o.tahsilat_turu == "Nakit" ? "Kasa" : (o.pos_id == null ?
                                                           (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi)),
                                                            kullanici = o.updated == null ? o.inserted : (o.inserted + "-" + o.updated)

                                                        };



            IEnumerable<CariDetayYeni> internet_fatura = (from o in db.faturas
                                                          where o.iptal == false && o.MusteriID == musteriID && (o.tur == "Fatura" || o.tur == "Devir") && o.sattis_tarih >= tarih
                                                          //orderby o.sattis_tarih descending
                                                          select new CariDetayYeni
                                                          {
                                                              MusteriID = (int)o.MusteriID,
                                                              aciklama = "Geçerlilik-" + o.son_odeme_tarihi.ToString(),
                                                              musteriAdi = o.ad,
                                                              borc = o.tutar,
                                                              alacak = 0,
                                                              tarih = (DateTime)o.sattis_tarih,
                                                              islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
                                                              konu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
                                                              kullanici = o.updated == null ? o.inserted : (o.inserted + "-" + o.updated)

                                                          });






            return odeme_tahsilat.Union(servis).Union(internet_fatura).Union(hesaplar).OrderByDescending(x => x.tarih).ToList();
        }
        public List<CariDetayx> CariDetayR(int musteriID, int gun)
        {
            // cari detay için
            // service_faturas -onaylanmış servis kararları- cariye işlenmiş zaten
            // faturas'ta sadece turu fatura olanlar
            // sonra da müşteri ödeme ve tahsilat hareketleri
            //tamircinin ve ustanın hesaplarını da ekliyoruz

            DateTime tarih = DateTime.Now.AddDays(-gun);

            IEnumerable<CariDetayx> odeme_tahsilat = (from o in db.musteriodemelers
                                                      where o.iptal == false && o.Musteri_ID == musteriID && o.OdemeTarih >= tarih
                                                      // orderby o.OdemeTarih descending
                                                      select new CariDetayx
                                                      {
                                                          MusteriID = o.Musteri_ID,
                                                          aciklama = o.Aciklama,
                                                          musteriAdi = o.customer.Ad,
                                                          tutar = o.OdemeMiktar,
                                                          maliyet = null,
                                                          tarih = o.OdemeTarih,
                                                          islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                         (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                          islem_turu = o.tahsilat_turu,

                                                          islem_adres = o.pos_id == null ?
                                                          (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                          cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                          masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                          islem_tarihi = o.islem_tarihi
                                                      });


            IEnumerable<CariDetayx> internet_fatura = (from o in db.faturas
                                                       where o.iptal == false && o.MusteriID == musteriID && (o.tur == "Fatura" || o.tur == "Devir") && o.sattis_tarih >= tarih
                                                       //orderby o.sattis_tarih descending
                                                       select new CariDetayx
                                                       {
                                                           MusteriID = (int)o.MusteriID,
                                                           aciklama = "Geçerlilik-" + o.bakiye,
                                                           musteriAdi = o.ad,
                                                           tutar = o.tutar,
                                                           maliyet = null,
                                                           tarih = (DateTime)o.sattis_tarih,
                                                           islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
                                                           islem_turu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
                                                           islem_adres = "",
                                                           cesit = "fatura",
                                                           masraf_tipi = "internet fatura",
                                                           islem_tarihi = o.islem_tarihi

                                                       });

            IEnumerable<CariDetayx> servis = from s in db.services
                                             where s.iptal == false && (s.CustID == musteriID || s.usta_id == musteriID) && s.AcilmaZamani >= tarih && s.service_faturas.Yekun > 0
                                             select new CariDetayx
                                             {
                                                 MusteriID = (int)s.CustID,
                                                 aciklama = s.Baslik,
                                                 musteriAdi = s.customer.Ad,
                                                 tutar = s.service_faturas.Yekun,
                                                 maliyet = s.service_faturas.toplam_maliyet,
                                                 tarih = s.AcilmaZamani,
                                                 islem = String.IsNullOrEmpty(s.service_faturas.service_tur) ? "Servis" : "Servis-Cihaz",
                                                 islem_turu = s.service_tips.tip_ad,
                                                 islem_adres = s.usta_id == null ? "Servis Toplamı" : " Servis Toplamı Usta",
                                                 cesit = "servis",
                                                 masraf_tipi = "cihaz-servis",
                                                 islem_tarihi = s.AcilmaZamani
                                             };


            IEnumerable<CariDetayx> hesaplar = from s in db.servicehesaps
                                               where s.iptal == false && (s.MusteriID == musteriID || s.tamirci_id == musteriID) && s.Onay_tarih >= tarih && s.Yekun > 0
                                               select new CariDetayx
                                               {
                                                   MusteriID = (int)s.MusteriID,
                                                   aciklama = s.Aciklama,
                                                   musteriAdi = s.customer.Ad,
                                                   tutar = s.Yekun,
                                                   maliyet = s.toplam_maliyet,
                                                   tarih = (DateTime)s.Onay_tarih,
                                                   islem = s.IslemParca,
                                                   islem_turu = s.cihaz_adi,
                                                   islem_adres = s.tamirci_id == null ? s.adet.ToString() + "Adet" : "Dış servis",
                                                   cesit = "karar",
                                                   masraf_tipi = "servis kararı",
                                                   islem_tarihi = s.Onay_tarih
                                               };

            return odeme_tahsilat.Union(servis).Union(internet_fatura).Union(hesaplar).OrderByDescending(x => x.tarih).ToList();
        }
        public TeknikServis.Radius.carihesap tekCariR(int custID)
        {
            return (from h in db.carihesaps
                    where h.MusteriID == custID
                    select h).FirstOrDefault();

        }
        public GenelGorunum Gosterge(int gun)
        {
            DateTime tarih = DateTime.Now.AddDays(-gun);
            GenelGorunum oz = new GenelGorunum();

            List<musteriOdemeRepo> liste = (from o in db.musteriodemelers
                                            where (o.iptal == null || o.iptal == false) && o.OdemeTarih >= tarih
                                            orderby o.OdemeTarih descending
                                            select new musteriOdemeRepo
                                            {
                                                aciklama = o.Aciklama,
                                                kullaniciID = o.KullaniciID,
                                                musteriAdi = o.customer.Ad,
                                                kullanici = o.inserted,
                                                musteriID = o.Musteri_ID,
                                                odemeID = o.Odeme_ID,
                                                odemeMiktari = o.OdemeMiktar,
                                                odemeTarih = o.OdemeTarih,
                                                tahsilat_odeme = o.tahsilat_odeme,
                                                tahsilatOdeme_turu = o.tahsilat_turu,
                                                islem_adres = o.pos_id == null ?
                                                (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                islem_tarihi = o.islem_tarihi
                                            }).ToList();

            oz.islemler = liste;

            //kasa bakiyemize bakalım
            decimal bakiye = db.kasahesaps.FirstOrDefault().ToplamBakiye;
            oz.kasa_bakiye = bakiye;

            // verilen ve alınan çeklere bakalım
            List<cekhesap> cekler = db.cekhesaps.Where(x => x.iptal == false && x.cekildi == false).ToList();
            decimal verilen_cekler = cekler.Where(x => x.alinan == false).Sum(x => x.nettutar);
            decimal alinan_cekler = cekler.Where(x => x.alinan == true).Sum(x => x.nettutar);
            oz.verilen_cek_toplami = verilen_cekler;
            oz.alinan_cek_toplami = alinan_cekler;

            // extre toplamı

            List<kart_hesaps> hesaplar = (from h in db.kart_hesaps
                                          where h.iptal == false && h.cekildi == false && h.extre_tarih <= h.kart_tanims.extre_tarih
                                          select h).ToList();
            decimal extre = hesaplar.Sum(x => x.tutar);
            oz.kart_extre_toplami = extre;

            List<poshesap> birikmis = (from h in db.poshesaps
                                       where h.iptal == false && h.cekildi == false
                                       select h).ToList();

            decimal pos = birikmis.Sum(x => x.net_tutar);
            oz.poslarda_birikenler = pos;
            decimal bankadakiler = db.bankas.ToList().Sum(x => x.bakiye);
            oz.banka_hesaplar_toplami = bankadakiler;

            return oz;

        }
    }

    public class GenelGorunum
    {
        public decimal kasa_bakiye { get; set; }
        public decimal verilen_cek_toplami { get; set; }
        public decimal alinan_cek_toplami { get; set; }
        //public decimal verilen_borc_toplami { get; set; }
        public decimal kart_extre_toplami { get; set; } //günü gelen
        public decimal poslarda_birikenler { get; set; }
        public decimal banka_hesaplar_toplami { get; set; }

        public List<musteriOdemeRepo> islemler { get; set; }
    }
    public class CariDetayx
    {
        public string musteriAdi { get; set; }
        public decimal tutar { get; set; }
        public DateTime tarih { get; set; }
        public decimal? maliyet { get; set; }
        public DateTime? islem_tarihi { get; set; }
        public string aciklama { get; set; }
        public string islem { get; set; }
        public string islem_turu { get; set; }
        public string islem_adres { get; set; }
        public int MusteriID { get; set; }
        public string cesit { get; set; }
        public string masraf_tipi { get; set; }

    }
    public class CariDetayYeni
    {
        public string musteriAdi { get; set; }
        public decimal? borc { get; set; }
        public decimal? alacak { get; set; }
        public DateTime tarih { get; set; }
        public string aciklama { get; set; }
        public string islem { get; set; }
        public string konu { get; set; }
        public int MusteriID { get; set; }
        public string kullanici { get; set; }


    }
}
