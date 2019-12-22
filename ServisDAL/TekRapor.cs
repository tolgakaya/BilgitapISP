using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekRapor
    {
        radiusEntities dc;

        public string baslama_tarihi { get; set; }
        public string bitis_tarihi { get; set; }
        public TekRapor(radiusEntities dc)
        {
            this.dc = dc;
        }
        private DateTime bas
        {
            get
            {

                if (!String.IsNullOrEmpty(this.baslama_tarihi))
                {
                    return DateTime.Parse(this.baslama_tarihi);
                }
                else
                {
                    return DateTime.Now.AddDays(-1);
                }
            }

        }
        private DateTime son
        {

            get
            {

                if (!String.IsNullOrEmpty(this.bitis_tarihi))
                {
                    return DateTime.Parse(this.bitis_tarihi);
                }
                else
                {
                    return DateTime.Now.AddDays(1);
                }
            }
        }

        public gunlukrapor gunluk()
        {
            gunlukrapor gunluk = new gunlukrapor();
            #region para durumu
            var kasa = dc.kasahesaps.FirstOrDefault(x => x.KasaTur == "Nakit").ToplamBakiye;
            gunluk.kasabakiye = kasa;

            var bankalar = dc.bankas.Sum(x => x.bakiye);
            gunluk.bankabakiye = bankalar;

            List<kart_hesaps> hesaplar = (from h in dc.kart_hesaps
                                          where h.iptal == false && h.cekildi == false && h.extre_tarih <= h.kart_tanims.extre_tarih
                                          select h).ToList();
            decimal extre = hesaplar.Sum(x => x.tutar);
            gunluk.kartbakiye = extre;

            List<poshesap> birikmis = (from h in dc.poshesaps
                                       where h.iptal == false && h.cekildi == false
                                       select h).ToList();

            decimal pos = birikmis.Sum(x => x.net_tutar);
            gunluk.posbakiye = pos;

            var cari = dc.panels.FirstOrDefault();

            gunluk.carialacak = cari.NetAlacak;
            gunluk.cariborc = cari.NetBorc;
            gunluk.caribakiye = cari.ToplamBakiye;

            #endregion

            #region açılan kapanan servisler
            var acilan_servisler = dc.services.Where(x => x.iptal == false && x.AcilmaZamani > bas && x.AcilmaZamani < son);
            var kapanan_servisler = dc.services.Where(x => x.iptal == false && x.KapanmaZamani > bas && x.KapanmaZamani < son);

            List<ServisRepo> acilanlar = (from s in acilan_servisler
                                          select new ServisRepo
                                          {
                                              serviceID = s.ServiceID,
                                              custID = (int)s.CustID,
                                              musteriAdi = s.customer.Ad,
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

            List<ServisRepo> kapananlar = (from s in kapanan_servisler
                                           select new ServisRepo
                                           {
                                               serviceID = s.ServiceID,
                                               custID = (int)s.CustID,
                                               musteriAdi = s.customer.Ad,
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
                                               kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated),
                                               maliyet = s.service_faturas.toplam_maliyet == null ? 0 : (decimal)s.service_faturas.toplam_maliyet,
                                               yekun = s.service_faturas.Yekun == null ? 0 : (decimal)s.service_faturas.Yekun

                                           }).ToList<ServisRepo>();

            int acilan_servis_sayisi = acilanlar.Count;
            int kapanan_servis_sayisi = kapananlar.Count;
            decimal kapanan_servis_maliyeti = 0;
            decimal kapanan_servis_tutari = 0;

            if (kapanan_servis_sayisi > 0)
            {
                kapanan_servis_maliyeti = kapananlar.Sum(x => x.maliyet);
                kapanan_servis_tutari = kapananlar.Sum(x => x.yekun);
            }
            gunluk.acilan_servisler = acilanlar;
            gunluk.kapanan_servisler = kapananlar;

            gunluk.acilan_servis_sayisi = acilan_servis_sayisi;
            gunluk.kapanan_servis_sayisi = kapanan_servis_sayisi;
            gunluk.kapanan_servis_maliyeti = kapanan_servis_maliyeti;
            gunluk.kapanan_servis_tutari = kapanan_servis_tutari;

            #endregion

            #region dış servisler
            var acilan_dis = dc.servicehesaps.Where(s => s.tamirci_id != null && s.iptal == false && s.TarihZaman > bas);
            var kapanan_dis = dc.servicehesaps.Where(s => s.tamirci_id != null && s.iptal == false && s.onay == true && s.Onay_tarih > bas && s.Onay_tarih < son);

            List<ServisHesapRepo> acilan_dislar = (from s in acilan_dis

                                                   select new ServisHesapRepo
                                                   {
                                                       hesapID = s.HesapID,
                                                       aciklama = s.Aciklama,
                                                       islemParca = s.IslemParca,
                                                       kdv = s.KDV,
                                                       musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                                                       disServis = s.tamirci_id == null ? "" : s.customer1.Ad,
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

            List<ServisHesapRepo> kapanan_dislar = (from s in kapanan_dis

                                                    select new ServisHesapRepo
                                                    {
                                                        hesapID = s.HesapID,
                                                        aciklama = s.Aciklama,
                                                        islemParca = s.IslemParca,
                                                        kdv = s.KDV,
                                                        musteriAdi = s.MusteriID == null ? "" : s.customer.Ad,
                                                        disServis = s.tamirci_id == null ? "" : s.customer1.Ad,
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

            int acilan_dis_servis_sayisi = acilan_dislar.Count;
            decimal acilan_dis_servis_maliyeti = 0;
            int kapanan_dis_servis_sayisi = kapanan_dislar.Count;
            decimal kapanan_dis_servis_maliyeti = 0;

            if (acilan_dis_servis_sayisi > 0)
            {
                acilan_dis_servis_maliyeti = acilan_dislar.Sum(x => x.yekun);
            }

            if (kapanan_dis_servis_sayisi > 0)
            {
                kapanan_dis_servis_maliyeti = kapanan_dislar.Sum(x => x.yekun);
            }

            gunluk.acilan_dis_servis_sayisi = acilan_dis_servis_sayisi;
            gunluk.acilan_dis_servis_maliyeti = acilan_dis_servis_maliyeti;
            gunluk.tamamlanan_dis_servis_sayisi = kapanan_dis_servis_sayisi;
            gunluk.tamamlanan_dis_servis_maliyeti = kapanan_dis_servis_maliyeti;
            gunluk.acilan_dis_servisler = acilan_dislar;
            gunluk.tamamlanan_dis_servisler = kapanan_dislar;

            #endregion

            #region ödeme ve tahsilatlar

            List<musteriOdemeRepo> odeme_tahsilatlar = (from o in dc.musteriodemelers
                                                        where (o.iptal == null || o.iptal == false) && o.OdemeTarih > bas && o.OdemeTarih < son
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

            decimal odeme_toplami = 0;
            decimal tahsilat_toplami = 0;
            decimal tahsilat_sayisi = 0;
            decimal odeme_sayisi = 0;

            if (odeme_tahsilatlar.Count > 0)
            {
                odeme_toplami = odeme_tahsilatlar.Where(x => x.tahsilat_odeme == "odeme").Sum(x => x.odemeMiktari);
                tahsilat_toplami = odeme_tahsilatlar.Where(x => x.tahsilat_odeme == "tahsilat").Sum(x => x.odemeMiktari);
            }
            gunluk.odemeler_toplami = odeme_toplami;
            gunluk.tahsilat_toplami = tahsilat_toplami;
            gunluk.odeme_tahsilatlar = odeme_tahsilatlar;
            #endregion

            #region iadeler

            decimal iade_tutari = 0;
            var iadeler = odeme_tahsilatlar.Where(x => x.tahsilatOdeme_turu == "iade").ToList();
            int iade_sayisi = iadeler.Count;

            if (iade_sayisi > 0)
            {
                iade_tutari = iadeler.Sum(x => x.odemeMiktari);
            }
            

            gunluk.iade_sayisi = iade_sayisi;
            gunluk.iade_toplami = iade_tutari;

            #endregion

            #region satinalmalar

            List<AlimRepo> alimlar = (from h in dc.alims
                                      where h.iptal == false && h.alim_tarih < son && h.alim_tarih > bas
                                      select new AlimRepo
                                      {
                                          alim_id = h.alim_id,
                                          aciklama = h.aciklama,
                                          alim_tarih = (DateTime)h.alim_tarih,
                                          belge_no = h.belge_no,
                                          konu = h.konu,
                                          toplam_kdv = h.toplam_kdv,
                                          toplam_tutar = h.toplam_tutar,
                                          toplam_yekun = h.toplam_yekun,
                                          musteri_adi = h.customer.Ad,
                                          CustID = h.CustID,
                                          kullanici = "Kayıt: " + h.inserted
                                      }).ToList();

            decimal satinalma_toplami = 0;
            int satinalma_sayisi = alimlar.Count;

            if (satinalma_sayisi > 0)
            {
                satinalma_toplami = alimlar.Sum(x => x.toplam_yekun);
            }
            gunluk.satinalma_toplami = satinalma_toplami;
            gunluk.satinalma_sayisi = satinalma_sayisi;
            gunluk.satinalimlar = alimlar;
            #endregion

            #region peşin satışlar
            List<satis_helper> satislar = (from s in dc.satislars
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
                                               kullanici = s.inserted
                                           }).ToList();

            int pesin_sayisi = satislar.Count;
            decimal pesin_toplami = 0;
            if (pesin_sayisi > 0)
            {
                pesin_toplami = satislar.Sum(x => x.yekun);
            }

            gunluk.pesin_satis_sayisi = pesin_sayisi;
            gunluk.pesin_satis_toplami = pesin_toplami;
            gunluk.pesin_satislar = satislar;
            #endregion

            #region emanetler

            List<yedekUrunRepo> yedek_verilen = (from u in dc.yedek_uruns
                                                 where u.tarih_verilme > bas && u.tarih_verilme < son
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

            List<yedekUrunRepo> yedek_donenler = (from u in dc.yedek_uruns
                                                  where u.tarih_donus != null && u.tarih_donus > bas && u.tarih_donus < son
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

            gunluk.emanettekiler_toplami = yedek_verilen.Count;
            gunluk.emanetten_donenler_toplami = yedek_donenler.Count;
            gunluk.emanet_donenler = yedek_donenler;
            gunluk.emanettekiler = yedek_verilen;
            #endregion

            gunluk.tarih = this.bas;
            return gunluk;
        }
    }

    public class gunlukrapor
    {
        //anlık durumlar
        public decimal kasabakiye { get; set; }
        public decimal bankabakiye { get; set; }
        public decimal posbakiye { get; set; }
        public decimal kartbakiye { get; set; }
        public decimal carialacak { get; set; }
        public decimal cariborc { get; set; }
        public decimal caribakiye { get; set; }
        //public decimal aktif_servis_sayisi { get; set; }
        //public decimal aktif_servis_tutari { get; set; }
        //public decimal aktif_dis_servis_maliyeti { get; set; }
        //public decimal aktif_dis_servis_sayisi { get; set; }

        public int acilan_servis_sayisi { get; set; }
        public List<ServisRepo> acilan_servisler { get; set; }
        public int kapanan_servis_sayisi { get; set; }
        public List<ServisRepo> kapanan_servisler { get; set; }
        public decimal kapanan_servis_tutari { get; set; }
        public decimal kapanan_servis_maliyeti { get; set; }

        public int acilan_dis_servis_sayisi { get; set; }
        public List<ServisHesapRepo> acilan_dis_servisler { get; set; }
        public decimal acilan_dis_servis_maliyeti { get; set; }
        public int tamamlanan_dis_servis_sayisi { get; set; }
        public List<ServisHesapRepo> tamamlanan_dis_servisler { get; set; }
        public decimal tamamlanan_dis_servis_maliyeti { get; set; }

        //public decimal stok_maliyeti { get; set; }
        //public decimal stok_satis_degeri { get; set; }

        public decimal odemeler_toplami { get; set; }
        public List<musteriOdemeRepo> odeme_tahsilatlar { get; set; }
        public decimal tahsilat_toplami { get; set; }
        public int odemeler_sayisi { get; set; }
        public int tahsilat_sayisi { get; set; }

        public int iade_sayisi { get; set; }
        //public List<musteriOdemeRepo> iadeler { get; set; }
        public decimal iade_toplami { get; set; }

        public int satinalma_sayisi { get; set; }
        public decimal satinalma_toplami { get; set; }
        public List<AlimRepo> satinalimlar { get; set; }

        public decimal pesin_satis_sayisi { get; set; }
        public List<satis_helper> pesin_satislar { get; set; }
        public decimal pesin_satis_toplami { get; set; }

        public decimal emanettekiler_toplami { get; set; }
        public List<yedekUrunRepo> emanettekiler { get; set; }
        public decimal emanetten_donenler_toplami { get; set; }
        public List<yedekUrunRepo> emanet_donenler { get; set; }
        public DateTime tarih { get; set; }

    }
}
