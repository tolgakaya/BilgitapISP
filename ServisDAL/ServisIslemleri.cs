using System;
using System.Collections.Generic;
using System.Linq;
using ServisDAL.Repo;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class sayfali
    {
        public List<ServisRepo> servis_listesi { get; set; }
        public int kayit_sayisi { get; set; }
    }
    public class maliyet
    {
        public List<ServisRepo> servis_listesi { get; set; }
        public int adet { get; set; }
        public decimal toplam_maliyet { get; set; }
        public decimal toplam_tutar { get; set; }
        public decimal toplam_fark { get; set; }
        public DateTime basTarih { get; set; }

        public string kisit { get; set; }
        public string firma { get; set; }
    }
    public class ServisIslemleri
    {

        radiusEntities db;

        public ServisIslemleri(radiusEntities db)
        {

            this.db = db;

        }

        #region servis arama ve listeleme işlemleri

        public TeknikServis.Radius.service servisTekR(int id)
        {
            return (from s in db.services
                    where s.ServiceID == id
                    select s).FirstOrDefault();

        }



        //tamirci carisine servis kapatıldığı zaman yansıma yapılıyor
        //tamirci kaydında prim oranı belirleniyor
        //servis kapatıldığı zaman service faturas kapandı olarak işaretleniyor
        //servis_faturas'dan triggerla usta_id nin carisine ustanın bilgilerine göre hesaplama yapılıp yazılıyor.

        public List<ServisRepo> servisTamirciRapor(int tamirci_id, bool? kapanma, DateTime? zaman = null)
        {


            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma != null)
            {
                return (from s in db.services
                        where s.usta_id == tamirci_id && (kapanma == false ? s.KapanmaZamani == null : s.KapanmaZamani != null) && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                            sonGorevli = s.SonAtananID,
                            usta = s.usta,
                            usta_id = s.usta_id,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            urunID = s.UrunID,
                            urunAdi = s.urun.Cinsi,
                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,
                            sonDurum = s.SonDurum,
                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            disServis = h.customer1.Ad,

                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }
            else
            {
                //hepsi
                return (from s in db.services
                        where s.usta_id == tamirci_id && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                            sonGorevli = s.SonAtananID,
                            usta_id = s.usta_id,
                            usta = s.usta,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            urunID = s.UrunID,
                            urunAdi = s.urun.Cinsi,
                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,
                            sonDurum = s.SonDurum,
                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            disServis = h.customer1.Ad,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                            kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }


        }
        public List<ServisRepo> servisRapor(bool? kapanma, DateTime? zaman = null)
        {


            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma != null)
            {
                return (from s in db.services
                        where (kapanma == false ? s.KapanmaZamani == null : s.KapanmaZamani != null) && s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            sonGorevli = s.SonAtananID,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            urunID = s.UrunID,
                            urunAdi = s.urun.Cinsi,
                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,
                            sonDurum = s.SonDurum,
                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }
            else
            {
                //hepsi
                return (from s in db.services
                        where s.iptal == false && s.AcilmaZamani > sinir
                        orderby s.AcilmaZamani descending
                        select new ServisRepo
                        {
                            serviceID = s.ServiceID,
                            custID = s.CustID == null ? -99 : (int)s.CustID,
                            musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                            adres = s.CustID == null ? "-" : s.customer.Adres,
                            telefon = s.CustID == null ? "-" : s.customer.telefon,
                            kullaniciID = s.olusturan_Kullanici,
                            sonGorevli = s.SonAtananID,
                            aciklama = s.Aciklama,
                            acilmaZamani = s.AcilmaZamani,
                            kapanmaZamani = s.KapanmaZamani,
                            urunID = s.UrunID,
                            urunAdi = s.urun.Cinsi,
                            kimlikNo = s.Servis_Kimlik_No,
                            tipID = s.tip_id,
                            css = s.service_tips.css,
                            servisTipi = s.service_tips.tip_ad,
                            sonDurum = s.SonDurum,
                            baslik = s.Baslik,
                            tutar = s.service_faturas.Tutar,
                            yekun = s.service_faturas.Yekun,
                            maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                            fark = 0,
                            hesaplar = (from h in db.servicehesaps
                                        where h.ServiceID == s.ServiceID && h.iptal == false
                                        orderby h.TarihZaman descending
                                        select new ServisHesapRepo
                                        {
                                            hesapID = h.HesapID,
                                            aciklama = h.Aciklama,
                                            islemParca = h.IslemParca,
                                            kdv = h.KDV,
                                            onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                            onaylimi = h.onay,
                                            onayTarih = h.Onay_tarih,
                                            tarihZaman = h.TarihZaman,
                                            servisID = s.ServiceID,
                                            tutar = h.Tutar,
                                            yekun = h.Yekun,
                                            cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                            birim_maliyet = h.birim_maliyet,
                                            toplam_maliyet = h.toplam_maliyet,
                                        }).ToList()

                        }).ToList<ServisRepo>();
            }


        }
        public void usta_ata(int tamirci_id, int servis_id, string kullanici)
        {
            service s = db.services.FirstOrDefault(x => x.ServiceID == servis_id);
            if (s != null)
            {
                customer usta = db.customers.FirstOrDefault(x => x.CustID == tamirci_id);
                if (usta != null)
                {
                    s.usta_id = tamirci_id;
                    s.usta = usta.Ad;
                    s.updated = kullanici;
                    KaydetmeIslemleri.kaydetR(db);
                }

            }
        }
        public void usta_fire(int servis_id, string kullanici)
        {
            service s = db.services.FirstOrDefault(x => x.ServiceID == servis_id);
            if (s != null)
            {
                s.usta_id = null;
                s.usta = "";
                s.updated = kullanici;
                KaydetmeIslemleri.kaydetR(db);
            }
        }


        public sayfali servisTamirciSayfali(int tamirci_id, bool kapanma, int sayfaNo, int perpage, DateTime? zaman = null)
        {

            sayfali say = new sayfali();
            DateTime sinir = zaman == null ? DateTime.Now.AddYears(-1) : (DateTime)zaman;
            if (kapanma == false)
            {
                List<service> liste = (from s in db.services
                                       where s.usta_id == tamirci_id && s.KapanmaZamani == null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;

                List<ServisRepo> servisler = (from s in liste
                                              select new ServisRepo
                                              {
                                                  serviceID = s.ServiceID,
                                                  custID = s.CustID == null ? -99 : (int)s.CustID,
                                                  musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                  adres = s.CustID == null ? "-" : s.customer.Adres,
                                                  telefon = s.CustID == null ? "-" : s.customer.telefon,
                                                  kullaniciID = s.olusturan_Kullanici,
                                                  kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                                  usta = s.usta,
                                                  usta_id = s.usta_id,
                                                  sonGorevli = s.SonAtananID,
                                                  aciklama = s.Aciklama,
                                                  acilmaZamani = s.AcilmaZamani,
                                                  kapanmaZamani = s.KapanmaZamani,
                                                  urunID = s.UrunID,
                                                  urunAdi = s.UrunID == null ? "" : s.urun.Cinsi,
                                                  kimlikNo = s.Servis_Kimlik_No,
                                                  tipID = s.tip_id,
                                                  css = s.service_tips.css,
                                                  servisTipi = s.service_tips.tip_ad,
                                                  sonDurum = s.SonDurum,
                                                  baslik = s.Baslik,
                                                  tutar = s.service_faturas.Tutar,
                                                  yekun = s.service_faturas.Yekun,
                                                  maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                                  fark = 0,
                                                  hesaplar = (from h in db.servicehesaps
                                                              where h.ServiceID == s.ServiceID && h.iptal == false
                                                              orderby h.TarihZaman descending
                                                              select new ServisHesapRepo
                                                              {
                                                                  hesapID = h.HesapID,
                                                                  aciklama = h.Aciklama,
                                                                  islemParca = h.IslemParca,
                                                                  kdv = h.KDV,
                                                                  onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                                  disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                                  onaylimi = h.onay,
                                                                  onayTarih = h.Onay_tarih,
                                                                  tarihZaman = h.TarihZaman,
                                                                  servisID = s.ServiceID,
                                                                  tutar = h.Tutar,
                                                                  yekun = h.Yekun,
                                                                  cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                                  birim_maliyet = h.birim_maliyet,
                                                                  toplam_maliyet = h.toplam_maliyet,
                                                                  kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                                              }).ToList()

                                              }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                say.servis_listesi = servisler;
                return say;
            }
            else
            {
                //hepsi
                List<service> liste = (from s in db.services
                                       where s.usta_id == tamirci_id && s.iptal == false && s.KapanmaZamani != null && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;

                List<ServisRepo> servisler = (from s in liste
                                              select new ServisRepo
                                              {
                                                  serviceID = s.ServiceID,
                                                  custID = s.CustID == null ? -99 : (int)s.CustID,
                                                  musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                  adres = s.CustID == null ? "-" : s.customer.Adres,
                                                  telefon = s.CustID == null ? "-" : s.customer.telefon,
                                                  kullaniciID = s.olusturan_Kullanici,
                                                  kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                                  sonGorevli = s.SonAtananID,
                                                  aciklama = s.Aciklama,
                                                  usta_id = s.usta_id,
                                                  usta = s.usta,
                                                  acilmaZamani = s.AcilmaZamani,
                                                  kapanmaZamani = s.KapanmaZamani,
                                                  urunID = s.UrunID,
                                                  urunAdi = s.UrunID == null ? "" : s.urun.Cinsi,
                                                  kimlikNo = s.Servis_Kimlik_No,
                                                  tipID = s.tip_id,
                                                  css = s.service_tips.css,
                                                  servisTipi = s.service_tips.tip_ad,
                                                  sonDurum = s.SonDurum,
                                                  baslik = s.Baslik,
                                                  tutar = s.service_faturas.Tutar,
                                                  yekun = s.service_faturas.Yekun,
                                                  maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                                  fark = 0,
                                                  hesaplar = (from h in db.servicehesaps
                                                              where h.ServiceID == s.ServiceID && h.iptal == false
                                                              orderby h.TarihZaman descending
                                                              select new ServisHesapRepo
                                                              {
                                                                  hesapID = h.HesapID,
                                                                  aciklama = h.Aciklama,
                                                                  islemParca = h.IslemParca,
                                                                  kdv = h.KDV,
                                                                  onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                                  onaylimi = h.onay,
                                                                  disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                                  onayTarih = h.Onay_tarih,
                                                                  tarihZaman = h.TarihZaman,
                                                                  servisID = s.ServiceID,
                                                                  tutar = h.Tutar,
                                                                  yekun = h.Yekun,
                                                                  cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                                  birim_maliyet = h.birim_maliyet,
                                                                  toplam_maliyet = h.toplam_maliyet,
                                                                  kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                                              }).ToList()

                                              }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                say.servis_listesi = servisler;
                return say;
            }


        }

        public sayfali servisSayfali(bool kapanma, int sayfaNo, int perpage, DateTime? zaman = null)
        {

            sayfali say = new sayfali();
            DateTime sinir = zaman == null ? DateTime.Now.AddDays(-7) : (DateTime)zaman;
            if (kapanma == false)
            {
                List<service> liste = (from s in db.services
                                       where s.KapanmaZamani == null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;
                List<ServisRepo> servisler = new List<ServisRepo>();
                if (liste.Count > 0)
                {
                    servisler = (from s in liste
                                 select new ServisRepo
                                 {
                                     serviceID = s.ServiceID,
                                     custID = s.CustID == null ? -99 : (int)s.CustID,
                                     musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                     adres = s.CustID == null ? "-" : s.customer.Adres,
                                     telefon = s.CustID == null ? "-" : s.customer.telefon,
                                     kullaniciID = s.olusturan_Kullanici,
                                     kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                     sonGorevli = s.SonAtananID,
                                     usta = s.usta,
                                     usta_id = s.usta_id,
                                     aciklama = s.Aciklama,
                                     acilmaZamani = s.AcilmaZamani,
                                     kapanmaZamani = s.KapanmaZamani,
                                     urunID = s.UrunID,
                                     urunAdi = s.UrunID == null ? "" : s.urun.Cinsi,
                                     kimlikNo = s.Servis_Kimlik_No,
                                     tipID = s.tip_id,
                                     css = s.service_tips.css,
                                     servisTipi = s.service_tips.tip_ad,
                                     sonDurum = s.SonDurum,
                                     baslik = s.Baslik,
                                     tutar = s.service_faturas.Tutar,
                                     yekun = s.service_faturas.Yekun,
                                     maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                     fark = 0,
                                     hesaplar = (from h in db.servicehesaps
                                                 where h.ServiceID == s.ServiceID && h.iptal == false
                                                 orderby h.TarihZaman descending
                                                 select new ServisHesapRepo
                                                 {
                                                     hesapID = h.HesapID,
                                                     aciklama = h.Aciklama,
                                                     islemParca = h.IslemParca,
                                                     kdv = h.KDV,
                                                     onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                     disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                     onaylimi = h.onay,
                                                     onayTarih = h.Onay_tarih,
                                                     tarihZaman = h.TarihZaman,
                                                     servisID = s.ServiceID,
                                                     tutar = h.Tutar,
                                                     yekun = h.Yekun,
                                                     cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                     birim_maliyet = h.birim_maliyet,
                                                     toplam_maliyet = h.toplam_maliyet,
                                                     kullanici = h.updated == null ? h.inserted : (h.inserted + "-" + h.updated)
                                                 }).ToList()

                                 }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                }

                say.servis_listesi = servisler;
                return say;
            }
            else
            {
                //hepsi
                List<service> liste = (from s in db.services
                                       where s.KapanmaZamani != null && s.iptal == false && s.AcilmaZamani > sinir
                                       orderby s.AcilmaZamani descending
                                       select s).ToList();
                say.kayit_sayisi = liste.Count;
                List<ServisRepo> servisler = new List<ServisRepo>();
                if (liste.Count >= 0)
                {
                    servisler = (from s in liste
                                 select new ServisRepo
                                 {
                                     serviceID = s.ServiceID,
                                     custID = s.CustID == null ? -99 : (int)s.CustID,
                                     musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                     adres = s.CustID == null ? "-" : s.customer.Adres,
                                     telefon = s.CustID == null ? "-" : s.customer.telefon,
                                     kullaniciID = s.olusturan_Kullanici,
                                     sonGorevli = s.SonAtananID,
                                     usta = s.usta,
                                     usta_id = s.usta_id,
                                     aciklama = s.Aciklama,
                                     acilmaZamani = s.AcilmaZamani,
                                     kapanmaZamani = s.KapanmaZamani,
                                     urunID = s.UrunID,
                                     urunAdi = s.UrunID == null ? "" : s.urun.Cinsi,
                                     kimlikNo = s.Servis_Kimlik_No,
                                     tipID = s.tip_id,
                                     css = s.service_tips.css,
                                     servisTipi = s.service_tips.tip_ad,
                                     sonDurum = s.SonDurum,
                                     baslik = s.Baslik,
                                     tutar = s.service_faturas.Tutar,
                                     yekun = s.service_faturas.Yekun,
                                     maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                     fark = 0,
                                     hesaplar = (from h in db.servicehesaps
                                                 where h.ServiceID == s.ServiceID && h.iptal == false
                                                 orderby h.TarihZaman descending
                                                 select new ServisHesapRepo
                                                 {
                                                     hesapID = h.HesapID,
                                                     aciklama = h.Aciklama,
                                                     islemParca = h.IslemParca,
                                                     kdv = h.KDV,
                                                     onayDurumu = (h.onay == true ? "EVET" : "HAYIR"),
                                                     onaylimi = h.onay,
                                                     onayTarih = h.Onay_tarih,
                                                     disServis = h.tamirci_id == null ? "" : h.customer1.Ad,
                                                     tarihZaman = h.TarihZaman,
                                                     servisID = s.ServiceID,
                                                     tutar = h.Tutar,
                                                     yekun = h.Yekun,
                                                     cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi,
                                                     birim_maliyet = h.birim_maliyet,
                                                     toplam_maliyet = h.toplam_maliyet,
                                                 }).ToList()

                                 }).Skip((sayfaNo - 1) * perpage).Take(perpage).ToList<ServisRepo>();
                }

                say.servis_listesi = servisler;
                return say;
            }


        }

    
        public List<ServisRepo> servisUrun(string urun_kimlik, string tipID)
        {
            int tip = Int32.Parse(tipID);
            IEnumerable<ServisRepo> direkt = (from s in db.services
                                              where (s.urun.BelgeYol == urun_kimlik && s.iptal == false)
                                              && ((!string.IsNullOrEmpty(tipID) ? s.tip_id == tip : s.iptal == false))

                                              select new ServisRepo
                                              {
                                                  serviceID = s.ServiceID,
                                                  custID = s.CustID == null ? -99 : (int)s.CustID,
                                                  musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                  adres = s.CustID == null ? "-" : s.customer.Adres,
                                                  telefon = s.CustID == null ? "-" : s.customer.telefon,
                                                  kullaniciID = s.olusturan_Kullanici,
                                                  sonGorevli = s.SonAtananID,
                                                  son_dis_servis = s.son_dis_servis,
                                                  usta = s.usta,
                                                  aciklama = s.Aciklama,
                                                  acilmaZamani = s.AcilmaZamani,
                                                  kapanmaZamani = s.KapanmaZamani,
                                                  urunID = s.UrunID,
                                                  urunAdi = s.urun.Cinsi,
                                                  kimlikNo = s.Servis_Kimlik_No,
                                                  tipID = s.tip_id,
                                                  css = s.service_tips.css,
                                                  servisTipi = s.service_tips.tip_ad,
                                                  sonDurum = s.SonDurum,
                                                  baslik = s.Baslik,
                                                  kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                                              });

            IEnumerable<ServisRepo> dolayli = (from s in db.services
                                               from h in db.servicehesaps
                                               where s.iptal == false && h.ServiceID == s.ServiceID && h.Urun_Kimlik == urun_kimlik && s.urun.BelgeYol != urun_kimlik
                                                && ((!string.IsNullOrEmpty(tipID) ? s.tip_id == tip : s.iptal == false))

                                               select new ServisRepo
                                               {
                                                   serviceID = s.ServiceID,
                                                   custID = s.CustID == null ? -99 : (int)s.CustID,
                                                   musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                                                   kullaniciID = s.olusturan_Kullanici,
                                                   sonGorevli = s.SonAtananID,
                                                   son_dis_servis = s.son_dis_servis,
                                                   usta = s.usta,
                                                   aciklama = s.Aciklama,
                                                   acilmaZamani = s.AcilmaZamani,
                                                   kapanmaZamani = s.KapanmaZamani,
                                                   urunID = s.UrunID,
                                                   urunAdi = s.urun.Cinsi,
                                                   kimlikNo = s.Servis_Kimlik_No,
                                                   tipID = s.tip_id,
                                                   css = s.service_tips.css,
                                                   servisTipi = s.service_tips.tip_ad,
                                                   sonDurum = s.SonDurum,
                                                   baslik = s.Baslik,
                                                   kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                                               });
            return direkt.Union(dolayli).ToList<ServisRepo>();
        }
        public List<ServisRepo> ServisListesi(string tipID, string custids, string gorevliID, string kelime)
        {
            int tipid = 0;
            if (!string.IsNullOrEmpty(tipID))
            {
                tipid = Int32.Parse(tipID);
            }
            int? custid = null;

            if (!String.IsNullOrEmpty(custids))
            {
                custid = Int32.Parse(custids);
            }
            //sadece müşteri seçilmişse kapanan servisler de görünüyor
            return (from s in db.services
                    where (string.IsNullOrEmpty(custids) ? s.KapanmaZamani == null : s.CustID == custid) && s.iptal == false
                    && (!string.IsNullOrEmpty(tipID) ? s.tip_id == tipid : s.iptal == false)
                    && ((!string.IsNullOrEmpty(gorevliID) ? (s.SonAtananID == gorevliID || s.SonAtananID.Equals("0")) : s.iptal == false))
                    && (!string.IsNullOrEmpty(kelime) ? (s.customer.Ad.Contains(kelime) || s.Baslik.Contains(kelime) || s.urun.Cinsi.Contains(kelime) || s.urun.imeino.Contains(kelime) || s.urun.serino.Contains(kelime) || s.urun.digerno.Contains(kelime) ||
                    s.Servis_Kimlik_No.Contains(kelime) || s.Aciklama.Contains(kelime) || s.customer.TC.Contains(kelime) || s.customer.tanimlayici.Contains(kelime) || s.customer.Adres.Contains(kelime)) : s.iptal == false)

                    orderby s.AcilmaZamani descending
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,
                        custID = s.CustID == null ? -99 : (int)s.CustID,
                        musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                        adres = s.CustID == null ? "-" : s.customer.Adres,
                        telefon = s.CustID == null ? "-" : s.customer.telefon,
                        son_dis_servis = s.son_dis_servis,
                        usta = s.usta,
                        kullaniciID = s.olusturan_Kullanici,

                        sonGorevli = s.SonAtananID,
                        aciklama = s.Aciklama,
                        acilmaZamani = s.AcilmaZamani,
                        kapanmaZamani = s.KapanmaZamani,
                        urunID = s.UrunID,
                        urunAdi = s.urun.Cinsi,
                        kimlikNo = s.Servis_Kimlik_No,
                        tipID = s.tip_id,
                        css = s.service_tips.css,
                        servisTipi = s.service_tips.tip_ad,
                        sonDurum = s.SonDurum,
                        baslik = s.Baslik,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList<ServisRepo>();

        }
    
        public ServisRepo servisAraKimlikDetayTekR(string servisKimlikNo)
        {
            return (from s in db.services
                    where s.Servis_Kimlik_No == servisKimlikNo && s.iptal == false
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,

                        musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                        adres = s.CustID == null ? "-" : s.customer.Adres,
                        telefon = s.CustID == null ? "-" : s.customer.telefon,
                        aciklama = s.Aciklama,
                        acilmaZamani = s.AcilmaZamani,
                        kapanmaZamani = s.KapanmaZamani,
                        custID = s.CustID == null ? -99 : (int)s.CustID,
                        urunAdi = s.urun.Cinsi,
                        baslik = s.Baslik,
                        sonDurum = s.SonDurum,
                        sonDurumID = s.durum_id,
                        css = s.service_tips.css,
                        sonGorevliID = s.SonAtananID,
                        son_dis_servis = s.son_dis_servis,
                        usta = s.usta,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).FirstOrDefault<ServisRepo>();

        }
        public ServisRepo ServisTek(int servisid)
        {
            return (from s in db.services
                    where s.ServiceID == servisid && s.iptal == false
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,

                        musteriAdi = s.CustID == null ? "-" : s.customer.Ad,
                        adres = s.CustID == null ? "-" : s.customer.Adres,
                        telefon = s.CustID == null ? "-" : s.customer.telefon,
                        aciklama = s.Aciklama,
                        acilmaZamani = s.AcilmaZamani,
                        kapanmaZamani = s.KapanmaZamani,
                        custID = s.CustID == null ? -99 : (int)s.CustID,
                        urunAdi = s.urun.Cinsi,
                        baslik = s.Baslik,
                        sonDurum = s.SonDurum,
                        sonDurumID = s.durum_id,
                        css = s.service_tips.css,
                        sonGorevliID = s.SonAtananID,
                        son_dis_servis = s.son_dis_servis,
                        usta = s.usta,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).FirstOrDefault<ServisRepo>();

        }
        public ServisRepo servisAraBarkod(string servisKimlikNo)
        {
            return (from s in db.services
                    where s.Servis_Kimlik_No == servisKimlikNo && s.iptal == false
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,


                        custID = s.CustID == null ? -99 : (int)s.CustID,

                    }).FirstOrDefault<ServisRepo>();

        }

        //manager için

        #endregion

        #region servis ekleme ve güncelleme işlemleri

        public int servisEkleGorevliR(int? musID, string kullaniciID, string aciklama, int? urunID, int tipID, string atananID, string kimlik, string baslik, DateTime acilma_zamani, string kullanici)
        {
            //burada baslangic durumu neymiş bakalım.
            AyarIslemleri ayar = new AyarIslemleri(db);
            TeknikServis.Radius.service_durums durum_ayar = ayar.tekDurumBaslangicR();

            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
            if (musID != null)
            {
                servis.CustID = (int)musID;
            }
            else
            {
                servis.CustID = null;
            }

            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.SonAtananID = atananID;
            servis.AcilmaZamani = acilma_zamani;
            //servis.KapanmaZamani = DateTime.Now.AddYears(-20);
            servis.iptal = false;
            servis.Aciklama = aciklama;
            servis.UrunID = urunID;
            servis.inserted = kullanici;
            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = tipID;
            servis.SonDurum = durum_ayar.Durum;
            servis.durum_id = durum_ayar.Durum_ID;

            //servis durumuna göre detay ekleyelim
            TeknikServis.Radius.servicedetay detay = new TeknikServis.Radius.servicedetay();
            detay.Aciklama = "Yeni servis kaydı yapıldı";
            detay.Baslik = "Yeni servis kaydı yapıldı";
            detay.BelgeYol = "";
            detay.Firma = "firma";
            detay.inserted = kullanici;
            detay.KullaniciID = kullaniciID;
            detay.ServiceID = servis.ServiceID;
            detay.TarihZaman = DateTime.Now;
            detay.Durum_ID = durum_ayar.Durum_ID;
            servis.servicedetays.Add(detay);
            db.services.Add(servis);
            KaydetmeIslemleri.kaydetR(db);

            return durum_ayar.Durum_ID;

        }


        public int servisEklePaketli(int paket_id, int? musID, string kullaniciID, string aciklama, int? urunID, int tipID, string atananID, string kimlik, string baslik, DateTime acilma_zamani, string kullanici)
        {
            List<servis_paket_detays> detaylar = db.servis_paket_detays.Where(x => x.paket_id == paket_id).ToList();

            //burada baslangic durumu neymiş bakalım.
            AyarIslemleri ayar = new AyarIslemleri(db);
            TeknikServis.Radius.service_durums durum_ayar = ayar.tekDurumBaslangicR();

            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
            servis.CustID = (int)musID;
            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.inserted = kullanici;
            servis.SonAtananID = atananID;
            servis.AcilmaZamani = acilma_zamani;
            //servis.KapanmaZamani = DateTime.Now.AddYears(-20);
            servis.iptal = false;
            servis.Aciklama = aciklama;
            servis.UrunID = urunID;

            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = tipID;
            servis.SonDurum = durum_ayar.Durum;
            servis.durum_id = durum_ayar.Durum_ID;

            //servis durumuna göre detay ekleyelim
            TeknikServis.Radius.servicedetay detay = new TeknikServis.Radius.servicedetay();
            detay.Aciklama = "Yeni servis kaydı yapıldı";
            detay.Baslik = "Yeni servis kaydı yapıldı";
            detay.BelgeYol = "";
            detay.Firma = "firma";
            detay.inserted = kullanici;
            detay.KullaniciID = kullaniciID;
            detay.ServiceID = servis.ServiceID;
            detay.TarihZaman = DateTime.Now;
            detay.Durum_ID = durum_ayar.Durum_ID;
            servis.servicedetays.Add(detay);
            if (detaylar.Count > 0)
            {
                foreach (servis_paket_detays p in detaylar)
                {
                    TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();

                    hesap.ServiceID = servis.ServiceID;
                    hesap.MusteriID = (int)musID;

                    hesap.IslemParca = p.IslemParca;
                    hesap.iptal = false;
                    hesap.inserted = kullanici;
                    hesap.Tutar = p.Tutar;
                    hesap.KDV = p.KDV;
                    hesap.Yekun = p.Yekun;
                    hesap.toplam_maliyet = 0;
                    hesap.birim_maliyet = 0;
                    hesap.Aciklama = p.Aciklama;
                    hesap.Firma = "firma";
                    hesap.kullanici = atananID;
                    hesap.TarihZaman = acilma_zamani;
                    hesap.cihaz_id = p.cihaz_id;
                    hesap.adet = p.adet;
                    hesap.cihaz_adi = p.cihaz_adi;
                    hesap.cihaz_gsure = (int)p.cihaz_gsure;

                    servis.servicehesaps.Add(hesap);


                }
            }
            db.services.Add(servis);
            KaydetmeIslemleri.kaydetR(db);

            return durum_ayar.Durum_ID;

        }

        public decimal? servisEkleKararli(int? paket_id, int musID, string kullaniciID, string aciklama, string kimlik, string baslik, DateTime acilma_zamani, karar_wrap karar, string kullanici)
        {

            //burada baslangic durumu neymiş bakalım.
            AyarIslemleri ayar = new AyarIslemleri(db);
            decimal cari = db.carihesaps.Find(musID).ToplamBakiye;

            TeknikServis.Radius.service servis = new TeknikServis.Radius.service();
            servis.CustID = (int)musID;
            servis.Baslik = baslik;
            servis.Firma = "firma";
            servis.olusturan_Kullanici = kullaniciID;
            servis.SonAtananID = "0";
            servis.AcilmaZamani = acilma_zamani;
            //servis.KapanmaZamani = DateTime.Now.AddYears(-20);
            servis.iptal = false;
            servis.Aciklama = aciklama;
            servis.UrunID = null;
            servis.KapanmaZamani = acilma_zamani;
            servis.Servis_Kimlik_No = kimlik;
            servis.tip_id = -1;
            servis.SonDurum = "Satış Yapıldı";
            servis.durum_id = -1;
            servis.inserted = kullanici;


            //yukarıda otomatik servisi tanımladık, seçime göre paket satışı yapılırsa şu paket uygulamasını ekliyoruz
            if (paket_id != null)
            {
                int stok = 1;
                List<servis_paket_detays> detaylar = db.servis_paket_detays.Where(x => x.paket_id == paket_id).ToList();
                if (detaylar.Count > 0)
                {
                    //paketin içindeki cihazların stoğunda bir problem varmı ona bakacaz.

                    foreach (servis_paket_detays p in detaylar)
                    {
                        if (p.cihaz_id != null)
                        {
                            //cihaz stoğuna bakalım
                            int bakiye = db.cihaz_stoks.Find(p.cihaz_id).bakiye;
                            if (bakiye <= 0)
                            {
                                stok *= 0;

                            }

                        }
                        TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();

                        hesap.ServiceID = servis.ServiceID;
                        hesap.MusteriID = (int)musID;

                        hesap.IslemParca = p.IslemParca;
                        hesap.iptal = false;
                        hesap.servis_kimlik = kimlik;
                        hesap.Tutar = p.Tutar;
                        hesap.KDV = p.KDV;
                        hesap.Yekun = p.Yekun;
                        hesap.birim_maliyet = 0;
                        hesap.toplam_maliyet = 0;
                        hesap.Aciklama = p.Aciklama;
                        hesap.Firma = "firma";
                        hesap.kullanici = kullaniciID;
                        hesap.TarihZaman = acilma_zamani;
                        hesap.cihaz_id = p.cihaz_id;
                        hesap.adet = p.adet;
                        hesap.cihaz_adi = p.cihaz_adi;
                        hesap.cihaz_gsure = (int)p.cihaz_gsure;
                        hesap.inserted = kullanici;
                        servis.servicehesaps.Add(hesap);




                    }

                }
                if (stok > 0)
                {
                    db.services.Add(servis);
                    KaydetmeIslemleri.kaydetR(db);
                }
                else
                {
                    return null;
                }


                //şimdi bu servis hesaplarının tamamını onaylayacağız

                List<servicehesap> kararlar = db.servicehesaps.Where(x => x.servis_kimlik == kimlik).ToList();
                if (kararlar.Count > 0)
                {
                    customer mu = db.customers.FirstOrDefault(x => x.CustID == musID);

                    foreach (servicehesap h in kararlar)
                    {
                        //cariye yansıyabilmesi için yukarıdaki kararları fatura oluşturman gerek

                        fatura fatik = new fatura();
                        fatik.bakiye = h.Yekun;


                        fatik.no = h.HesapID.ToString();
                        fatik.taksit_no = 0;
                        fatik.odenen = 0;
                        fatik.Firma = h.Firma;
                        fatik.tc = mu.TC;
                        fatik.telefon = mu.telefon;
                        fatik.tutar = h.Yekun;
                        fatik.sattis_tarih = DateTime.Now;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.service_id = h.ServiceID;
                        fatik.MusteriID = (int)musID;
                        fatik.islem_tarihi = DateTime.Now;
                        fatik.son_odeme_tarihi = acilma_zamani;

                        fatik.sattis_tarih = acilma_zamani;
                        fatik.tur = "Taksit";
                        fatik.iptal = false;
                        fatik.service_id = h.ServiceID;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.inserted = kullanici;
                        db.faturas.Add(fatik);

                        h.onay = true;
                        h.Onay_tarih = acilma_zamani;
                        if (h.cihaz_id != null)
                        {
                            var fifo = (from f in db.cihaz_fifos
                                        where f.cihaz_id == h.cihaz_id && f.bakiye > 0
                                        select f).FirstOrDefault();
                            if (fifo != null)
                            {
                                h.stok_id = fifo.id;
                                h.birim_maliyet = fifo.fiyat;
                                h.toplam_maliyet = h.adet * fifo.fiyat;
                            }


                        }

                    }
                }

                KaydetmeIslemleri.kaydetR(db);
                //sonradaservisi kapatacağız
            }
            else if (karar != null)
            {
                int stok = 1;
                //paket değilmiş
                if (karar.cihaz_id != null)
                {
                    //cihaz stoğuna bakalım
                    int bakiye = db.cihaz_stoks.Find(karar.cihaz_id).bakiye;
                    if (bakiye <= 0)
                    {
                        stok *= 0;

                    }
                }
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();

                hesap.ServiceID = servis.ServiceID;
                hesap.MusteriID = (int)musID;

                hesap.IslemParca = karar.islemParca;
                hesap.iptal = false;
                hesap.servis_kimlik = kimlik;
                hesap.Tutar = karar.tutar;
                hesap.kullanici = kullaniciID;
                decimal tutar = (100 * karar.yekun) / (100 + karar.kdv);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * karar.kdv) / 100;

                hesap.Yekun = karar.yekun;
                hesap.Aciklama = karar.aciklama;
                hesap.Firma = "firma";

                hesap.TarihZaman = acilma_zamani;
                hesap.cihaz_id = karar.cihaz_id;
                hesap.adet = karar.adet;
                hesap.cihaz_adi = karar.cihaz_adi;
                hesap.cihaz_gsure = (int)karar.cihaz_gsure;

                hesap.inserted = kullanici;

                if (stok > 0)
                {
                    servis.servicehesaps.Add(hesap);

                    db.services.Add(servis);


                    KaydetmeIslemleri.kaydetR(db);
                }
                else
                {
                    return null;
                }


                //şimdi bu servis hesaplarının tamamını onaylayacağız

                List<servicehesap> kararlar = db.servicehesaps.Where(x => x.servis_kimlik == kimlik).ToList();
                if (kararlar.Count > 0)
                {
                    customer mu = db.customers.FirstOrDefault(x => x.CustID == musID);
                    foreach (servicehesap h in kararlar)
                    {
                        fatura fatik = new fatura();
                        fatik.bakiye = h.Yekun;


                        fatik.no = h.HesapID.ToString();
                        fatik.taksit_no = 0;
                        fatik.odenen = 0;
                        fatik.Firma = h.Firma;
                        fatik.tc = mu.TC;
                        fatik.telefon = mu.telefon;
                        fatik.sattis_tarih = DateTime.Now;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.service_id = h.ServiceID;
                        fatik.MusteriID = (int)musID;
                        fatik.islem_tarihi = DateTime.Now;
                        fatik.son_odeme_tarihi = acilma_zamani;

                        fatik.sattis_tarih = acilma_zamani;
                        fatik.tutar = h.Yekun;

                        fatik.tur = "Taksit";
                        fatik.iptal = false;
                        fatik.service_id = h.ServiceID;
                        fatik.servicehesap_id = h.HesapID;
                        fatik.inserted = kullanici;
                        db.faturas.Add(fatik);

                        h.onay = true;
                        h.Onay_tarih = acilma_zamani;
                        if (h.cihaz_id != null)
                        {
                            var fifo = (from f in db.cihaz_fifos
                                        where f.cihaz_id == h.cihaz_id && f.bakiye > 0
                                        select f).FirstOrDefault();
                            if (fifo != null)
                            {
                                h.stok_id = fifo.id;
                                h.birim_maliyet = fifo.fiyat;
                                h.toplam_maliyet = h.adet * fifo.fiyat;
                            }


                        }

                    }
                }

                KaydetmeIslemleri.kaydetR(db);
            }


            return cari;

        }


        public void servisGuncelleR(int servisID, int musID, string aciklama, int tipID, string konu, string kullanici)
        {
            TeknikServis.Radius.service servis = tekServisR(servisID);
            if (servis != null)
            {
                servis.CustID = musID;

                servis.Aciklama = aciklama;
                servis.Baslik = konu;

                servis.tip_id = tipID;
                servis.updated = kullanici;

                KaydetmeIslemleri.kaydetR(db);
            }


        }



        private TeknikServis.Radius.service tekServisR(int serviceID)
        {
            return (from s in db.services
                    where s.ServiceID == serviceID
                    select s).FirstOrDefault();
        }

        private void servisIptalR(TeknikServis.Radius.service servis, string kullanici)
        {

            //servis toplamlarına göre
            if (servis.usta_id != null)
            {
                //usta prim oranları
                customer usta = db.customers.FirstOrDefault(x => x.CustID == servis.usta_id);
                carihesap cari = db.carihesaps.FirstOrDefault(x => x.MusteriID == servis.usta_id);
                service_faturas fat = db.service_faturas.FirstOrDefault(x => x.ServiceID == servis.ServiceID);
                decimal prim_yekun = usta.prim_yekun == null ? 0 : (decimal)usta.prim_yekun;
                decimal prim_kar = usta.prim_kar == null ? 0 : (decimal)usta.prim_kar;

                cari.ToplamAlacak = cari.ToplamAlacak - (fat.Yekun * prim_yekun / 100) - (fat.Yekun - (decimal)fat.toplam_maliyet) * prim_kar / 100;

            }
            servis.iptal = true;
            servis.deleted = kullanici;
            KaydetmeIslemleri.kaydetR(db);
        }

        public void servisIptalR(int serviceID, string kullanici)
        {
            TeknikServis.Radius.service s = tekServisR(serviceID);
            if (s != null)
            {
                servisIptalR(s, kullanici);
            }

        }
        #endregion
        #region servis detay ve hesap işlemleri
        public void servisDetayEkleR(int servisID, string belgeYol, string aciklama, int durumID, string kullanici, string baslik)
        {
            TeknikServis.Radius.servicedetay detay = new TeknikServis.Radius.servicedetay();
            detay.ServiceID = servisID;
            detay.Firma = "firma";
            detay.TarihZaman = DateTime.Now;
            detay.BelgeYol = belgeYol;
            detay.Aciklama = aciklama;
            detay.Durum_ID = durumID;
            detay.KullaniciID = kullanici;
            detay.inserted = kullanici;
            detay.Baslik = baslik;
            db.servicedetays.Add(detay);
            KaydetmeIslemleri.kaydetR(db);

        }


        public List<ServisDetayRepo> detayListesiDetayKimlikR(string kimlik)
        {
            return (from s in db.servicedetays

                    where s.service.Servis_Kimlik_No == kimlik
                    orderby s.DetayID descending
                    select new ServisDetayRepo
                    {
                        detayID = s.DetayID,
                        servisID = s.ServiceID,
                        tarihZaman = s.TarihZaman,
                        belgeYol = s.BelgeYol,
                        aciklama = s.Aciklama,
                        durumID = s.Durum_ID,
                        durumAdi = s.service_durums.Durum,
                        gorevliID = s.KullaniciID,
                        baslik = s.Baslik,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)


                    }).ToList();
        }

        public void serviceKararEkleRYeni(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int adet, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;

                }
                else
                {
                    hesap.MusteriID = null;
                }
                // hesap.MusteriID = ser.CustID; query stringden alınıyor gereksiz mi bakıver
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.birim_maliyet = 0;
                hesap.toplam_maliyet = 0;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.adet = adet;
                hesap.TarihZaman = karar_tarihi;

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleTamirci(int serviceID, int? musteriID, int tamirci_id, string islem, decimal kdvOran, decimal yekun, decimal maliyet, string aciklama, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;

                }
                else
                {
                    hesap.MusteriID = null;
                }

                //if (tamirci_id != null)
                //{
                //    customer tamirci = db.customers.FirstOrDefault(x => x.CustID == tamirci_id);
                //    ser.tamirci_id = tamirci_id;
                //    ser.son_dis_servis = tamirci.Ad;
                //}
                // hesap.MusteriID = ser.CustID; query stringden alınıyor gereksiz mi bakıver
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.birim_maliyet = maliyet;
                hesap.toplam_maliyet = maliyet;
                hesap.tamirci_id = tamirci_id;
                hesap.adet = 1;
                hesap.TarihZaman = karar_tarihi;
                hesap.inserted = kullanici;
                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public void serviceKararGuncelleTamirci(int hesapID, int tamirci_id, string islem, decimal kdvOran, decimal yekun, decimal maliyet, string aciklama, DateTime karar_tarihi, string kullanici)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            servicehesap hesap = db.servicehesaps.FirstOrDefault(x => x.HesapID == hesapID);
            if (hesap != null)
            {

                //customer tamirci = db.customers.FirstOrDefault(x => x.CustID == tamirci_id);
                //hesap.tamirci_id = tamirci_id;
                //hesap.son_dis_servis = tamirci.Ad;

                // hesap.MusteriID = ser.CustID; query stringden alınıyor gereksiz mi bakıver
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.birim_maliyet = maliyet;
                hesap.toplam_maliyet = maliyet;
                hesap.tamirci_id = tamirci_id;
                hesap.adet = 1;
                hesap.TarihZaman = karar_tarihi;
                hesap.updated = kullanici;

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public void serviceKararEkleRYeniCihazli(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama, int cihazID, int adet, string cihaz_adi, string garanti_sure, DateTime karar_tarihi, string kullanici)
        {
            int sure = Int32.Parse(garanti_sure);

            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";
                hesap.inserted = kullanici;
                hesap.TarihZaman = karar_tarihi;

                hesap.cihaz_id = cihazID;
                hesap.adet = adet;
                hesap.cihaz_adi = cihaz_adi;
                hesap.cihaz_gsure = sure;

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public List<paket_secim_repo> servis_paketleri()
        {
            return (from s in db.servis_pakets
                    where s.iptal == false
                    select new paket_secim_repo
                    {
                        paket_adi = s.paket_adi,
                        paket_id = s.paket_id

                    }).ToList();
        }
        public void servisKararPaketli(int serviceID, int paket_id, int? musteriID, string kullanici)
        {
            List<servis_paket_detays> detaylar = db.servis_paket_detays.Where(x => x.paket_id == paket_id).ToList();
            if (detaylar.Count > 0)
            {
                foreach (servis_paket_detays p in detaylar)
                {
                    TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();

                    hesap.ServiceID = serviceID;
                    if (musteriID != null)
                    {
                        hesap.MusteriID = (int)musteriID;
                    }
                    else
                    {
                        hesap.MusteriID = null;
                    }
                    hesap.IslemParca = p.IslemParca;
                    hesap.iptal = false;
                    hesap.Tutar = p.Tutar;
                    hesap.KDV = p.KDV;
                    hesap.Yekun = p.Yekun;
                    hesap.toplam_maliyet = 0;
                    hesap.birim_maliyet = 0;
                    hesap.Aciklama = p.Aciklama;
                    hesap.Firma = "firma";
                    hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                    hesap.TarihZaman = DateTime.Now;
                    hesap.cihaz_id = p.cihaz_id;
                    hesap.adet = p.adet;
                    hesap.cihaz_adi = p.cihaz_adi;
                    hesap.cihaz_gsure = (int)p.cihaz_gsure;
                    hesap.inserted = kullanici;
                    db.servicehesaps.Add(hesap);


                }
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        public void serviceKararEkleUrunlu(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama,
            string urun_Cinsi, string urun_aciklama, DateTime garanti_baslangic, int garanti_ay, DateTime garanti_bitis, string urunKimlik, DateTime karar_tarihi)
        {

            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }
                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";

                hesap.TarihZaman = karar_tarihi;
                hesap.Urun_GarantiBaslangic = garanti_baslangic;
                hesap.Urun_GarantiBitis = garanti_bitis;
                hesap.Urun_GarantiSuresi = garanti_ay;
                hesap.Urun_Cinsi = urun_Cinsi;
                hesap.Urun_Aciklama = urun_aciklama;
                hesap.Urun_Kimlik = urunKimlik;

                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public void serviceKararEkleUrunluCihazli(int serviceID, int? musteriID, string islem, decimal kdvOran, decimal yekun, string aciklama,
      string urun_Cinsi, string urun_aciklama, DateTime garanti_baslangic, int garanti_ay, DateTime garanti_bitis, string urunKimlik, int? cihazID, int adet, string cihaz_adi, string garanti_sure, DateTime karar_tarihi)
        {
            //Servis kararı eklendiğinde servis detaylarına triggerla kayıt yapılıyor
            service ser = db.services.FirstOrDefault(x => x.ServiceID == serviceID);
            if (ser != null)
            {
                int sure = Int32.Parse(garanti_sure);
                TeknikServis.Radius.servicehesap hesap = new TeknikServis.Radius.servicehesap();
                hesap.ServiceID = serviceID;
                if (musteriID != null)
                {
                    hesap.MusteriID = (int)musteriID;
                }
                else
                {
                    hesap.MusteriID = null;
                }

                hesap.kullanici = System.Web.HttpContext.Current.User.Identity.Name;
                hesap.IslemParca = islem;
                hesap.iptal = false;
                decimal tutar = (100 * yekun) / (100 + kdvOran);
                hesap.Tutar = tutar;
                hesap.KDV = (tutar * kdvOran) / 100;
                hesap.Yekun = yekun;//burası kdv dahil fiyat
                hesap.Aciklama = aciklama;
                hesap.Firma = "firma";

                hesap.TarihZaman = karar_tarihi;
                hesap.Urun_GarantiBaslangic = garanti_baslangic;
                hesap.Urun_GarantiBitis = garanti_bitis;
                hesap.Urun_GarantiSuresi = garanti_ay;
                hesap.Urun_Cinsi = urun_Cinsi;
                hesap.Urun_Aciklama = urun_aciklama;
                hesap.Urun_Kimlik = urunKimlik;
                hesap.cihaz_id = cihazID;
                hesap.adet = adet;
                hesap.cihaz_adi = cihaz_adi;
                hesap.cihaz_gsure = sure;
                db.servicehesaps.Add(hesap);

                KaydetmeIslemleri.kaydetR(db);
            }

        }

        public TeknikServis.Radius.servicehesap tekHesapR(int hesapID)
        {
            return db.servicehesaps.Where(s => s.HesapID == hesapID).FirstOrDefault();
        }

        public void serviceKararGuncelleR(int id, string islem, string aciklama, decimal kdvOran, decimal yekun, string kullanici)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);


            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;
            hesap.updated = kullanici;
            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);
        }
        public void serviceKararGuncelleCihazli(int id, string islem, string aciklama, decimal kdvOran, decimal yekun, string cihaz_adi, int cihaz_id, int adet, string kullanici)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);

            hesap.cihaz_adi = cihaz_adi;
            hesap.cihaz_id = cihaz_id;
            hesap.adet = adet;
            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;
            hesap.updated = kullanici;
            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);
        }

        public void serviceKararGuncelleUrunlu(int id, string islem, string aciklama, decimal kdvOran, decimal yekun,
          string urun_Cinsi, string urun_aciklama, DateTime garanti_baslangic, int garanti_ay, DateTime garanti_bitis)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);

            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;

            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            hesap.Urun_GarantiBaslangic = garanti_baslangic;
            hesap.Urun_GarantiBitis = garanti_bitis;
            hesap.Urun_GarantiSuresi = garanti_ay;
            hesap.Urun_Cinsi = urun_Cinsi;
            hesap.Urun_Aciklama = urun_aciklama;

            //ürün tablosunu güncellemiyorum çünkü onaylandığı zaman ürün ekleniyor. onaylanmış kararda güncelleme yapılamıyor


            KaydetmeIslemleri.kaydetR(db);
        }
        public void serviceKararGuncelleUrunluCihazli(int id, string islem, string aciklama, decimal kdvOran, decimal yekun,
        string urun_Cinsi, string urun_aciklama, DateTime garanti_baslangic, int garanti_ay, DateTime garanti_bitis, int cihaz_id, string cihaz_adi, int adet)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);

            hesap.IslemParca = islem;
            hesap.Aciklama = aciklama;
            hesap.Yekun = yekun;
            decimal tutar = (100 * yekun) / (100 + kdvOran);
            hesap.Tutar = tutar;

            hesap.KDV = (tutar * kdvOran) / 100;
            hesap.TarihZaman = DateTime.Now;
            hesap.Urun_GarantiBaslangic = garanti_baslangic;
            hesap.Urun_GarantiBitis = garanti_bitis;
            hesap.Urun_GarantiSuresi = garanti_ay;
            hesap.Urun_Cinsi = urun_Cinsi;
            hesap.Urun_Aciklama = urun_aciklama;
            hesap.cihaz_adi = cihaz_adi;
            hesap.cihaz_id = cihaz_id;
            hesap.adet = adet;
            //ürün tablosunu güncellemiyorum çünkü onaylandığı zaman ürün ekleniyor. onaylanmış kararda güncelleme yapılamıyor


            KaydetmeIslemleri.kaydetR(db);
        }


        public List<ServisHesapRepo> servisKararListesiDetayMusteriyeGoreHepsiR(int musteriID)
        {
            return (from s in db.servicehesaps
                    where s.MusteriID == musteriID && s.iptal == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                        disServis = s.tamirci_id == null ? "" : s.customer1.Ad,
                        ustaAdi = s.service.usta_id == null ? "" : s.service.customer1.Ad,
                        musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,

                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        kimlik = s.service.Servis_Kimlik_No,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).ToList();

        }
        public List<ServisHesapRepo> servisKararListesiTamirci(int musteriID)
        {
            return (from s in db.servicehesaps
                    where s.tamirci_id != null && s.tamirci_id == musteriID && s.iptal == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                        disServis = s.tamirci_id == null ? "" : s.customer1.Ad,
                        ustaAdi = s.service.usta_id == null ? "" : s.service.customer1.Ad,
                        musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,

                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        urun_cinsi = s.service.urun.Cinsi,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).ToList();

        }
        public List<ServisHesapRepo> servisKararListesiTamirci(int musteriID, DateTime baslama, bool kapanma)
        {
            return (from s in db.servicehesaps
                    where s.tamirci_id != null && s.tamirci_id == musteriID && s.iptal == false && s.TarihZaman >= baslama && s.onay == kapanma
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                        //ustaAdi = s.tamirci_id == null ? "" : s.customer1.Ad,
                        //disServis = s.service.usta_id == null ? "" : s.service.customer1.Ad,
                        musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        urun_cinsi = s.service.urun.Cinsi,
                        kimlik = s.service.Servis_Kimlik_No
                    }).ToList();

        }
        //onay bekleyen bütün servis kararları


        //Bayi ve kullanıcı
        public List<ServisHesapRepo> servisKararListesiDetayHepsiR(bool onaylanma)
        {
            return (from s in db.servicehesaps
                    where s.iptal == false && s.onay == onaylanma
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                        disServis = s.tamirci_id == null ? "" : s.customer1.Ad,
                        ustaAdi = s.service.usta_id == null ? "" : s.service.customer1.Ad,
                        musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        kimlik = s.service.Servis_Kimlik_No,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)


                    }).ToList();

        }

        public bool servis_kapalimi(int servisid)
        {
            if (db.services.FirstOrDefault(x => x.ServiceID == servisid).KapanmaZamani == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<ServisHesapRepo> servisKararListesiDetayR(int servisID)
        {
            return (from s in db.servicehesaps
                    where s.ServiceID == servisID && s.iptal == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.MusteriID == null ? "-" : s.customer.Ad,
                        ustaAdi = "",
                        disServis = s.tamirci_id == null ? "-" : s.customer1.Ad,
                        musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        cihaz = s.cihaz_adi,
                        birim_maliyet = s.birim_maliyet,
                        toplam_maliyet = s.toplam_maliyet,
                        kimlik = s.service.Servis_Kimlik_No,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList();

        }
        //durum güncelleme servis detayda yapılıyor her servis detay açıldığında duurm güncelleme yapılıyor
        //ve bu durum güncellemesi triggerla services tablosuna aktarılıyor

        //karar hesap penceresinden onaylandığı zaman triggerla cari hesaba yansıtılıyor
        //KARAR onaylandığı zaman aynı triggerla services tablosundaki durum güncelleniyor

        public List<taksitimiz> TaksitOlustur(DateTime baslamaTarihi, int periyot, int taksitSayisi, decimal tutar)
        {
            DateTime[] tarihler = new DateTime[(taksitSayisi)];
            List<taksitimiz> taksitler = new List<taksitimiz>();
            //son taksit hariç hepsi taksit miktarı olacak
            decimal taksitMiktari = Math.Ceiling(tutar / taksitSayisi);

            decimal sonTaksit = tutar - (taksitMiktari * (taksitSayisi - 1));

            for (int i = 0; i < taksitSayisi; i++)
            {
                tarihler[i] = baslamaTarihi;

                taksitimiz tak = new taksitimiz();
                tak.taksitTarihi = baslamaTarihi;
                tak.taksitNo = i;
                tak.taksitTutari = taksitMiktari;
                taksitler.Add(tak);

                baslamaTarihi = baslamaTarihi.AddDays(periyot);
            }
            taksitler[(taksitSayisi - 1)].taksitTutari = sonTaksit;
            return taksitler;
        }
        public List<taksitimiz> TaksitKaydet(int hesapID, DateTime baslamaTarihi, int periyot, int taksitSayisi, decimal tutar)
        {
            TeknikServis.Radius.servicehesap hesap = tekHesapR(hesapID);
            List<taksitimiz> taksitler = TaksitOlustur(baslamaTarihi, periyot, taksitSayisi, tutar);
            foreach (taksitimiz tak in taksitler)
            {

                tekTaksitKaydet(hesap, tak.taksitNo, tak.taksitTutari, tak.taksitTarihi, tak.taksitTutari);
            }
            hesap.onay = true;
            hesap.Onay_tarih = DateTime.Now;
            KaydetmeIslemleri.kaydetR(db);

            return taksitler;
        }
        private void tekTaksitKaydet(TeknikServis.Radius.servicehesap hesap, int taksitNo, decimal bakiye, DateTime tarih, decimal taksitMiktar)
        {

            TeknikServis.Radius.customer mu = db.customers.Where(x => x.CustID == hesap.MusteriID).FirstOrDefault();

            fatura fat = new fatura();
            fat.bakiye = bakiye;
            fat.sattis_tarih = DateTime.Now;
            fat.service_id = hesap.ServiceID;
            fat.servicehesap_id = hesap.HesapID;
            fat.son_odeme_tarihi = tarih;
            fat.MusteriID = hesap.MusteriID;
            fat.no = hesap.HesapID.ToString();
            fat.taksit_no = taksitNo;
            fat.odenen = 0;
            fat.Firma = hesap.Firma;
            fat.tc = mu.TC;
            fat.telefon = mu.telefon;
            fat.tutar = taksitMiktar;

            fat.tur = "Taksit";
            fat.iptal = false;
            fat.islem_tarihi = DateTime.Now;
            db.faturas.Add(fat);

        }

        public musteri_bilgileri servisKararOnayR(int id, string kullanici)
        {
            //tamirci_idyi ekleyeceğiz
            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);
            int? tamirci_id = hesap.tamirci_id;

            musteri_bilgileri mu = new musteri_bilgileri();
            if (hesap.cihaz_id != null)
            {
                var fifo = (from f in db.cihaz_fifos

                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = hesap.adet * fifo.fiyat;

                    //tek taksit oluşturalım
                    mu = (from m in db.customers
                          where m.CustID == hesap.MusteriID
                          select new musteri_bilgileri
                          {
                              ad = m.Ad,
                              email = m.email,
                              tel = m.telefon,
                              tc = m.TC,
                              caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                          }).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = hesap.Yekun;
                    fatik.tamirci_id = tamirci_id;
                    fatik.no = hesap.HesapID.ToString();
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = hesap.Firma;
                    fatik.tc = mu.tc;
                    fatik.telefon = mu.tel;
                    fatik.tutar = hesap.Yekun;
                    fatik.tamirci_maliyet = hesap.toplam_maliyet;
                    fatik.MusteriID = hesap.MusteriID;
                    fatik.tur = "Taksit";
                    fatik.iptal = false;
                    fatik.service_id = hesap.ServiceID;
                    fatik.servicehesap_id = id;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.inserted = kullanici;

                    db.faturas.Add(fatik);

                    //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;
                    KaydetmeIslemleri.kaydetR(db);
                    return mu;
                }
                else
                {
                    //stok yokmuş
                    return mu;
                }


            }
            else
            {
                //cihazlı değil
                //tek taksit oluşturalım
                mu = (from m in db.customers
                      where m.CustID == hesap.MusteriID
                      select new musteri_bilgileri
                      {
                          ad = m.Ad,
                          email = m.email,
                          tel = m.telefon,
                          tc = m.TC,
                          caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                      }).FirstOrDefault();


                fatura fatik = new fatura();
                fatik.bakiye = hesap.Yekun;
                fatik.tamirci_id = tamirci_id;
                fatik.no = hesap.HesapID.ToString();
                fatik.taksit_no = 0;
                fatik.odenen = 0;
                fatik.Firma = hesap.Firma;
                fatik.tc = mu.tc;
                fatik.telefon = mu.tel;
                fatik.tutar = hesap.Yekun;
                fatik.tamirci_maliyet = hesap.toplam_maliyet;
                fatik.MusteriID = hesap.MusteriID;
                fatik.tur = "Taksit";
                fatik.iptal = false;
                fatik.service_id = hesap.ServiceID;
                fatik.servicehesap_id = id;
                fatik.sattis_tarih = DateTime.Now;
                fatik.son_odeme_tarihi = DateTime.Now;
                fatik.islem_tarihi = DateTime.Now;
                fatik.inserted = kullanici;
                db.faturas.Add(fatik);

                //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                hesap.onay = true;

                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
                KaydetmeIslemleri.kaydetR(db);
                return mu;
            }



        }

        public musteri_bilgileri servisKararOnayNoMusteri(int id, string kullanici)
        {

            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);

            musteri_bilgileri mu = new musteri_bilgileri();


            if (hesap.cihaz_id != null)
            {

                hesap.onay = true;
                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
                var fifo = (from f in db.cihaz_fifos

                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = hesap.adet * fifo.fiyat;

                    mu = (from m in db.customers
                          where m.CustID == hesap.MusteriID
                          select new musteri_bilgileri
                          {
                              ad = m.Ad,
                              email = m.email,
                              tel = m.telefon,
                              tc = m.TC,
                              caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                          }).FirstOrDefault();
                    KaydetmeIslemleri.kaydetR(db);
                }



            }
            else
            {
                //cihazssız
                hesap.onay = true;
                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;

                mu = (from m in db.customers
                      where m.CustID == hesap.MusteriID
                      select new musteri_bilgileri
                      {
                          ad = m.Ad,
                          email = m.email,
                          tel = m.telefon,
                          tc = m.TC,
                          caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                      }).FirstOrDefault();

                KaydetmeIslemleri.kaydetR(db);
            }

            return mu;

        }
        // hesap iptal edildiği zaman  cari hesap güncelleniyor

        private bool kararTekli(servicehesap hesap, Musteri_Mesaj mu, string kullanici)
        {

            bool stok = true;
            int? tamirci_id = hesap.tamirci_id;

            if (hesap.cihaz_id != null)
            {

                var fifo = (from f in db.cihaz_fifos
                            where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                            orderby f.tarih ascending
                            select f).FirstOrDefault();
                if (fifo != null)
                {
                    fatura fatik = new fatura();
                    fatik.bakiye = hesap.Yekun;
                    fatik.tamirci_id = tamirci_id;
                    fatik.no = hesap.HesapID.ToString();
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = hesap.Firma;
                    fatik.tc = mu.tc;
                    fatik.telefon = mu.telefon;
                    fatik.tutar = hesap.Yekun;
                    fatik.tamirci_maliyet = hesap.toplam_maliyet;
                    fatik.MusteriID = hesap.MusteriID;
                    fatik.tur = "Taksit";
                    fatik.iptal = false;
                    fatik.service_id = hesap.ServiceID;
                    fatik.servicehesap_id = hesap.HesapID;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.inserted = kullanici;
                    db.faturas.Add(fatik);

                    //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;

                    hesap.stok_id = fifo.id;
                    hesap.birim_maliyet = fifo.fiyat;
                    hesap.toplam_maliyet = hesap.adet * fifo.fiyat;

                }
                else
                {
                    stok = false;
                }


            }
            else
            {
                fatura fatik = new fatura();
                fatik.bakiye = hesap.Yekun;
                fatik.tamirci_id = tamirci_id;
                fatik.no = hesap.HesapID.ToString();
                fatik.taksit_no = 0;
                fatik.odenen = 0;
                fatik.Firma = hesap.Firma;
                fatik.tc = mu.tc;
                fatik.telefon = mu.telefon;
                fatik.tutar = hesap.Yekun;
                fatik.tamirci_maliyet = hesap.toplam_maliyet;
                fatik.MusteriID = hesap.MusteriID;
                fatik.tur = "Taksit";
                fatik.iptal = false;
                fatik.service_id = hesap.ServiceID;
                fatik.servicehesap_id = hesap.HesapID;
                fatik.sattis_tarih = DateTime.Now;
                fatik.son_odeme_tarihi = DateTime.Now;
                fatik.islem_tarihi = DateTime.Now;
                fatik.inserted = kullanici;
                db.faturas.Add(fatik);

                //buradaki servisid tahsilatta tahsilat türü için kullanıyorum, triggerla service hesapsa kart ise yazdırıyorum


                hesap.onay = true;
                hesap.updated = kullanici;
                hesap.Onay_tarih = DateTime.Now;
            }
            return stok;

        }

        public Musteri_Mesaj kararOnaytoplu(int servisid, int custid, string kullanici)
        {
            decimal tutar = 0;
            string islemler = "";
            Musteri_Mesaj m = (from c in db.customers
                               where c.CustID == custid
                               select new Musteri_Mesaj
                               {
                                   musteri_adi = c.Ad,
                                   email = c.email,
                                   telefon = c.telefon,
                                   islemler = islemler,
                                   tutar = tutar.ToString(),
                                   tc = c.TC,
                                   caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == custid).ToplamBakiye
                               }).FirstOrDefault();
            List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.ServiceID == servisid && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();
            if (hesaplar.Count > 0)
            {
                int stok_kontrol = 1;
                foreach (servicehesap hesap in hesaplar)
                {
                    tutar += hesap.Yekun;
                    islemler += hesap.IslemParca + "-";
                    bool kont = kararTekli(hesap, m, kullanici);
                    if (kont == false)
                    {
                        stok_kontrol = stok_kontrol * 0;
                    }
                }
                if (stok_kontrol != 0)
                {
                    KaydetmeIslemleri.kaydetR(db);

                }
                else
                {
                    m.musteri_adi = string.Empty;
                }

            }

            m.tutar = tutar.ToString();
            m.islemler = islemler;

            return m;
        }
        public musteri_bilgileri kararOnaytoplu(int servisid, string kullanici)
        {

            List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.ServiceID == servisid && (x.iptal == false || x.iptal == null) && (x.onay == null || x.onay == false)).ToList();
            musteri_bilgileri mu = (from m in db.customers
                                    where m.CustID == hesaplar.FirstOrDefault().MusteriID
                                    select new musteri_bilgileri
                                    {
                                        ad = m.Ad,
                                        email = m.email,
                                        tel = m.telefon,
                                        tc = m.TC,
                                        caribakiye = db.carihesaps.FirstOrDefault(x => x.MusteriID == m.CustID).ToplamBakiye
                                    }).FirstOrDefault();
            if (hesaplar.Count > 0)
            {
                int kont = 1;
                foreach (servicehesap hesap in hesaplar)
                {
                    if (hesap.cihaz_id != null)
                    {
                        var fifo = (from f in db.cihaz_fifos

                                    where f.cihaz_id == hesap.cihaz_id && f.bakiye > 0
                                    orderby f.tarih ascending
                                    select f).FirstOrDefault();
                        if (fifo != null)
                        {
                            hesap.stok_id = fifo.id;
                            hesap.birim_maliyet = fifo.fiyat;
                            hesap.toplam_maliyet = hesap.adet * fifo.fiyat;
                        }
                        else
                        {
                            kont *= 0;
                            mu.ad = string.Empty;
                        }


                    }

                    hesap.onay = true;
                    hesap.updated = kullanici;
                    hesap.Onay_tarih = DateTime.Now;
                }

                if (kont > 0)
                {
                    KaydetmeIslemleri.kaydetR(db);
                }

            }
            return mu;
        }

        public string servisKararIptalR(int id, string kullanici)
        {
            string sonuc = "";
            TeknikServis.Radius.servicehesap hesap = tekHesapR(id);
            hesap.iptal = true;
            hesap.deleted = kullanici;

            if (hesap.onay == true)
            {
                if (!String.IsNullOrEmpty(hesap.Urun_Cinsi))
                {
                    sonuc += "1-hesaptaki cihaz urun tablosundan siliniyor==>";
                    //bulduğumuz ilk ürünümüşteri hesaplarından silek
                    urun urunumuz = (from u in db.uruns
                                     where u.MusteriID == hesap.MusteriID && u.BelgeYol == hesap.Urun_Kimlik && u.Cinsi == hesap.Urun_Cinsi && u.GranatiBaslangic == hesap.Urun_GarantiBaslangic
                                     orderby u.urunID descending
                                     select u).FirstOrDefault();
                    urunumuz.iptal = true;
                    sonuc += "2-urun silindi==>";
                }

                sonuc += "3-başka cihazlı onaylanmış hesap varmı?==>";

                //değişiklik
                //service hesaplarına bakacaz eğer cihazlı onaylanmamış bir hesap yoksa
                //service_faturas türünü servise çevirecez
                List<servicehesap> hesaplar = db.servicehesaps.Where(x => x.ServiceID == hesap.ServiceID && (x.iptal == false || x.iptal == null) && !String.IsNullOrEmpty(x.Urun_Cinsi) && x.onay == true).ToList();
                if (hesaplar.Count > 0)
                {
                    sonuc += "4-cihazlı hesap yok service_faturasa bakiyor==>";
                    service_faturas faturaOzeti = db.service_faturas.FirstOrDefault(x => x.ServiceID == hesap.ServiceID);
                    if (faturaOzeti != null)
                    {
                        sonuc += "5-service_faturas değiştiirliyor==>";
                        faturaOzeti.service_tur = "Servis";
                    }
                }
                else
                {
                    sonuc += "4-cihazlı hesap var değişiklik yapılmadı==>";
                    sonuc += "hesap sayisi---" + hesaplar.Count.ToString();
                }
                //değişiklik
            }

            KaydetmeIslemleri.kaydetR(db);
            return sonuc;

        }

        public int servisKapatR(int servisID, string kullanici)
        {
            //asıl servis sonlandırma servis detayı ekleyerek yapılıyor
            //servis detayında kapatma durumu seçilirse servis otomatik kapatılıyor.
            //bu yüzden kapatma için buraya bir kısa yol ekliyoruz.
            AyarIslemleri ayarlar = new AyarIslemleri(db);
            TeknikServis.Radius.service_durums durum = ayarlar.tekDurumSonmuR();
            if (durum != null)
            {
                string aciklama = "Servis otomatikolarak kapatıldı";
                string baslik = "Servis kapandı";
                servisDetayEkleR(servisID, "-", aciklama, durum.Durum_ID, kullanici, baslik);
                return durum.Durum_ID;
            }
            else
            {
                return -1;
            }

        }
        #endregion

        #region iş emirleri
        public void Atama(string gorevli_id, string kimlik)
        {
            service s = db.services.FirstOrDefault(x => x.Servis_Kimlik_No == kimlik);
            if (s != null)
            {
                s.SonAtananID = gorevli_id;
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        #endregion

        #region cihaz işlemleri

        public List<cihaz> CihazGoster()
        {
            return db.cihazs.Take(4).ToList();
        }
        public List<cihaz> CihazGoster(string arama_terimi)
        {
            return db.cihazs.Where(x => (x.cihaz_adi.Contains(arama_terimi) || x.aciklama.Contains(arama_terimi))).Take(4).ToList();
        }
        public bool CihazEkle(string cihaz_adi, string aciklama, string garanti_suresi)
        {
            bool flag = false;
            //bakalım bu adda cihaz var mı?
            int sure = Int32.Parse(garanti_suresi);
            cihaz c = db.cihazs.FirstOrDefault(x => x.cihaz_adi == cihaz_adi);
            if (c == null)
            {
                cihaz yeni = new cihaz();
                yeni.cihaz_adi = cihaz_adi;
                yeni.aciklama = aciklama;
                yeni.garanti_suresi = sure;
                yeni.Firma = "firma";

                db.cihazs.Add(yeni);
                KaydetmeIslemleri.kaydetR(db);
                flag = true;
            }
            return flag;
        }
        #endregion
        #region ürün işlemleri

        public void urunEkleR(int musteriID, string cinsi, DateTime baslangicTarih, int sureAy, string aciklama, string belgeyol, string imei, string serino, string diger)
        {
            TeknikServis.Radius.urun yeni = new TeknikServis.Radius.urun();
            yeni.Aciklama = aciklama;
            yeni.BelgeYol = belgeyol;
            yeni.Cinsi = cinsi;
            yeni.GarantiBitis = baslangicTarih.AddMonths(sureAy);
            yeni.GarantiSuresi = sureAy;
            yeni.GranatiBaslangic = baslangicTarih;
            yeni.MusteriID = musteriID;
            yeni.Firma = "firma";
            yeni.imeino = imei;
            yeni.serino = serino;
            yeni.digerno = diger;

            db.uruns.Add(yeni);
            KaydetmeIslemleri.kaydetR(db);

        }


        public List<urunRepo> urunListesiCompactR(int musID)
        {
            return (from u in db.uruns
                    where u.MusteriID == musID && (u.iptal == null || u.iptal == false)
                    select new urunRepo
                    {
                        urunID = u.urunID,
                        cinsi = u.Cinsi,
                        garantiBaslangic = u.GranatiBaslangic,
                        garantiBitis = u.GarantiBitis,
                        garantiSuresi = u.GarantiSuresi,
                        aciklama = u.Aciklama,
                        belgeYol = u.BelgeYol,
                        musteriID = u.MusteriID,
                        digerno = u.digerno,
                        imei = u.imeino,
                        serino = u.serino

                    }).ToList();
            // cihazlarını da ekleyelim mi


        }


        public List<urunRepo> urun_ara(string arama_terimi)
        {
            return (from u in db.uruns
                    from m in db.customers
                    where u.MusteriID == m.CustID && (u.BelgeYol.Contains(arama_terimi) || u.Cinsi.Contains(arama_terimi) || u.imeino.Contains(arama_terimi) || u.serino.Contains(arama_terimi) || u.digerno.Contains(arama_terimi)) && (u.iptal == null || u.iptal == false)
                    select new urunRepo
                    {
                        urunID = u.urunID,
                        musteriAdi = m.Ad,
                        cinsi = u.Cinsi,
                        garantiBaslangic = u.GranatiBaslangic,
                        garantiBitis = u.GarantiBitis,
                        garantiSuresi = u.GarantiSuresi,
                        aciklama = u.Aciklama,
                        belgeYol = u.BelgeYol,
                        musteriID = u.MusteriID,
                        digerno = u.digerno,
                        imei = u.imeino,
                        serino = u.serino

                    }).ToList();

        }
        public string urun_mailler(string arama_terimi)
        {
            List<customer> musteriler = (from u in db.uruns
                                         from m in db.customers
                                         where u.MusteriID == m.CustID && (u.BelgeYol.Contains(arama_terimi) || u.Cinsi.Contains(arama_terimi)) && (u.iptal == null || u.iptal == false)
                                         select m).Distinct().ToList();

            string mailler = "";

            foreach (customer c in musteriler)
            {
                if (!String.IsNullOrEmpty(c.email))
                {
                    mailler += c.email + ";";
                }
            }

            return mailler;

        }
        public string urun_mailler()
        {
            List<customer> musteriler = (from u in db.uruns
                                         from m in db.customers
                                         where u.MusteriID == m.CustID && (u.iptal == null || u.iptal == false)
                                         select m).Distinct().ToList();

            string mailler = "";

            foreach (customer c in musteriler)
            {
                if (!String.IsNullOrEmpty(c.email))
                {
                    mailler += c.email + ";";
                }
            }

            return mailler;

        }
        public string urun_teller()
        {
            List<customer> musteriler = (from u in db.uruns
                                         from m in db.customers
                                         where u.MusteriID == m.CustID && (u.iptal == null || u.iptal == false)
                                         select m).Distinct().ToList();

            string mailler = "";

            foreach (customer c in musteriler)
            {
                if (!String.IsNullOrEmpty(c.telefon))
                {
                    mailler += c.telefon + ",";
                }
            }

            return mailler;

        }
        public string urun_teller(string arama_terimi)
        {
            List<customer> musteriler = (from u in db.uruns
                                         from m in db.customers
                                         where u.MusteriID == m.CustID && (u.BelgeYol.Contains(arama_terimi) || u.Cinsi.Contains(arama_terimi)) && (u.iptal == null || u.iptal == false)
                                         select m).Distinct().ToList();

            string mailler = "";

            foreach (customer c in musteriler)
            {
                if (!String.IsNullOrEmpty(c.telefon))
                {
                    mailler += c.telefon + ",";
                }
            }

            return mailler;

        }
        public List<urunRepo> urun_listesi()
        {
            return (from u in db.uruns
                    from m in db.customers
                    where u.MusteriID == m.CustID && (u.iptal == null || u.iptal == false)
                    select new urunRepo
                    {
                        urunID = u.urunID,
                        musteriAdi = m.Ad,
                        cinsi = u.Cinsi,
                        garantiBaslangic = u.GranatiBaslangic,
                        garantiBitis = u.GarantiBitis,
                        garantiSuresi = u.GarantiSuresi,
                        aciklama = u.Aciklama,
                        belgeYol = u.BelgeYol,
                        musteriID = u.MusteriID,
                        digerno = u.digerno,
                        imei = u.imeino,
                        serino = u.serino

                    }).ToList();

        }

        public void urunIptal(int urunID)
        {
            urun ur = db.uruns.FirstOrDefault(x => x.urunID == urunID);
            if (ur != null)
            {
                ur.iptal = true;
                KaydetmeIslemleri.kaydetR(db);
            }
        }

        public void emanetUrunVerR(int musteriID, string acilama, string kullanici)
        {
            TeknikServis.Radius.yedek_uruns yedek = new TeknikServis.Radius.yedek_uruns();
            yedek.musteri_id = musteriID;
            yedek.Firma = "firma";
            yedek.urun_aciklama = acilama;
            yedek.tarih_verilme = DateTime.Now;
            yedek.inserted = kullanici;
            db.yedek_uruns.Add(yedek);
            KaydetmeIslemleri.kaydetR(db);

        }

        public TeknikServis.Radius.yedek_uruns tekEmanetR(int emanetID)
        {
            return (from u in db.yedek_uruns
                    where u.yedek_id == emanetID
                    select u).FirstOrDefault();
        }

        public void emanetAlR(int id, string kullanici)
        {
            TeknikServis.Radius.yedek_uruns emanet = tekEmanetR(id);
            emanet.tarih_donus = DateTime.Now;
            emanet.updated = kullanici;
            KaydetmeIslemleri.kaydetR(db);

        }



        //bayi için
        public List<yedekUrunRepo> emanettekiUrunlerimizHepsiR()
        {
            return (from u in db.yedek_uruns
                    where u.tarih_donus == null
                    orderby u.tarih_verilme descending
                    select new yedekUrunRepo
                    {

                        musteriAdi = u.customer.Ad,
                        musteriID = u.musteri_id,
                        urunAciklama = u.urun_aciklama,
                        verilmeTarihi = u.tarih_verilme,
                        yedekID = u.yedek_id,
                        donusTarih = u.tarih_donus,
                        donmeDurumu = u.tarih_donus == null ? "Müşteride" : "Döndü",
                        kullanici = u.inserted
                    }).ToList();
        }

        public List<yedekUrunRepo> emanettekiUrunlerimizHepsiR(int cust_id)
        {
            return (from u in db.yedek_uruns
                    where u.tarih_donus == null && u.musteri_id == cust_id
                    orderby u.tarih_verilme descending
                    select new yedekUrunRepo
                    {
                        musteriAdi = u.customer.Ad,
                        musteriID = u.musteri_id,
                        urunAciklama = u.urun_aciklama,
                        verilmeTarihi = u.tarih_verilme,
                        yedekID = u.yedek_id,
                        donusTarih = u.tarih_donus,
                        donmeDurumu = u.tarih_donus == null ? "Müşteride" : "Döndü",
                        kullanici = u.inserted
                    }).ToList();
        }

        #endregion


        //manager ve kullanici için
        public List<cariHesapRepo> butunBorcuOlanHesaplarR(string sonMesaj, string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                //son mesaja göre  sırala
                if (String.IsNullOrEmpty(sonMesaj) || sonMesaj.Equals("hepsi"))
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
                else
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0
                            orderby h.son_mesaj ascending
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
            }
            else
            {
                //son mesaja göre  sırala
                if (String.IsNullOrEmpty(sonMesaj) || sonMesaj.Equals("hepsi"))
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0 && h.adi.Contains(s)
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
                else
                {
                    return (from h in db.carihesaps
                            where h.MusteriID > 0 && h.ToplamBakiye > 0 && h.adi.Contains(s)
                            orderby h.son_mesaj ascending
                            select new cariHesapRepo
                            {
                                musteriID = h.MusteriID,
                                musteriAdi = h.adi,
                                tel = h.telefon,
                                son_mesaj = h.son_mesaj,
                                netBakiye = h.ToplamBakiye,
                                netBorclanma = h.NetBorc,
                                netAlacak = h.NetAlacak

                            }
                           ).ToList();
                }
            }


        }
        //iş emirleri eklenecek
        // emirler servislere bağlı olacak
        //böylece servislerle ilgili ek emir düzenlenebilecek
        // kullanıcı emri yerine getirrise onaylayacak
        // kullanıcının işlemi servis detayı olarak otomatik eklenecek
    }
    public class Musteri_Mesaj
    {
        public string musteri_adi { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string islemler { get; set; }
        public string tutar { get; set; }
        public string tc { get; set; }
        public string username { get; set; }
        public decimal caribakiye { get; set; }
    }

    public class karar_wrap
    {
        public string islemParca { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal yekun { get; set; }
        public string aciklama { get; set; }
        public int? cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public int adet { get; set; }
        public int? cihaz_gsure { get; set; }


    }
    public class musteri_bilgileri
    {
        public string email { get; set; }
        public string ad { get; set; }
        public string tel { get; set; }
        public string tc { get; set; }
        public decimal caribakiye { get; set; }
        public string durum { get; set; }

    }
}
