using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekMusteri
    {
        radiusEntities dc;

        int custID;
        public TekMusteri(radiusEntities dc, int custIDParam)
        {
            this.custID = custIDParam;

            this.dc = dc;
        }

        //
        public MusteriDetay DetayGoster()
        {
            MusteriDetay detay = new MusteriDetay();
            detay.cari = cari();
            detay.servis = servis();
            MusteriBilgileri mus = musteri();
            string goster = "";
            if (String.IsNullOrEmpty(mus.tc))
            {
                goster += "TC numarası eksik-";

            }
            else
            {
                bool harfmi = Char.IsLetter(mus.tc[0]);
                if (harfmi == true)
                {
                    goster += "TC numarası eksik-";

                }

            }
            if (String.IsNullOrEmpty(mus.adres))
            {
                goster += " Adres eksik-";

            }
            if (String.IsNullOrEmpty(mus.tel))
            {
                goster += " Telefon eksik-";

            }
            detay.musteri = mus;
            detay.odemeler = odemeler();
            detay.kararlar = kararlar();
            detay.tamirler = tamirler();
            detay.yedekler = emanetler();

            detay.urunler = urunler();
            detay.krediler = krediler();
            detay.alimlar = Alimlar();
            detay.eksikler = goster;
            return detay;
        }
        public List<CihazRepo> urunler()
        {
            return (from u in dc.cihaz_garantis
                    where u.CustID == custID && u.iptal == false
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        musteriAdi = u.customer.Ad,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();

        }
        private List<ServisRepo> servis()
        {
            return (from s in dc.services
                    where s.iptal == false && (s.CustID == custID || s.usta_id == custID)
                    orderby s.AcilmaZamani descending
                    select new ServisRepo
                    {
                        serviceID = s.ServiceID,
                        custID = (int)s.CustID,
                        musteriAdi = s.customer.Ad,
                        kullaniciID = s.olusturan_Kullanici,
                        sonGorevli = s.SonAtananID,
                        yekun = s.service_faturas.Yekun,
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

                    }).Take(10).ToList<ServisRepo>();
        }

        private cariHesapRepo cari()
        {
            return (from h in dc.carihesaps
                    where h.MusteriID == custID
                    select new cariHesapRepo
                    {
                        musteriID = h.MusteriID,
                        musteriAdi = "",
                        netBakiye = h.ToplamBakiye,
                        netBorclanma = h.NetBorc,
                        netAlacak = h.NetAlacak
                    }
                      ).FirstOrDefault();
        }

        private MusteriBilgileri musteri()
        {
            return (from c in dc.customers
                    where c.CustID == custID
                    select new MusteriBilgileri
                    {
                        adi = c.Ad,
                        adres = c.Adres,
                        tc = c.TC,
                        tel = c.telefon,
                        istihbarat = c.istihbarat,
                        disservis = c.disservis,
                        musteri = c.musteri,
                        tedarikci = c.tedarikci,
                        usta = c.usta,
                        paket_id = c.paket_id

                    }).FirstOrDefault();

        }

        private List<musteriOdemeRepo> odemeler()
        {
            return (from o in dc.musteriodemelers
                    where o.iptal == false && o.Musteri_ID == custID
                    orderby o.OdemeTarih descending
                    select new musteriOdemeRepo
                    {
                        aciklama = o.Aciklama,
                        kullaniciID = o.KullaniciID,
                        musteriAdi = o.customer.Ad,
                        musteriID = o.Musteri_ID,
                        odemeID = o.Odeme_ID,
                        odemeMiktari = o.OdemeMiktar,
                        odemeTarih = o.OdemeTarih,
                        tahsilat_odeme = o.tahsilat_odeme,
                        tahsilatOdeme_turu = o.tahsilat_turu,
                        islem_adres = o.pos_id == null ?
                        (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                        masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                        islem_tarihi = o.islem_tarihi,
                        kullanici = o.updated == null ? o.inserted : (o.inserted + "-" + o.updated)
                    }).Take(10).ToList();
        }

        private List<ServisHesapRepo> kararlar()
        {
            return (from s in dc.servicehesaps
                    where s.MusteriID == custID && s.iptal == false && (s.onay == null || s.onay == false)
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.customer.Ad,
                        musteriID = (int)s.MusteriID,
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).Take(10).ToList();

        }
        private List<ServisHesapRepo> tamirler()
        {
            return (from s in dc.servicehesaps
                    where s.tamirci_id == custID && s.iptal == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        musteriAdi = s.customer.Ad,
                        musteriID = (int)s.MusteriID,
                        onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                        onaylimi = s.onay,
                        onayTarih = s.Onay_tarih,
                        tarihZaman = s.TarihZaman,
                        servisID = s.ServiceID,
                        tutar = s.Tutar,
                        yekun = s.Yekun,
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)
                    }).Take(10).ToList();

        }
        private List<yedekUrunRepo> emanetler()
        {
            return (from u in dc.yedek_uruns
                    where u.tarih_donus == null && u.musteri_id == custID
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

        private List<AlimRepo> Alimlar()
        {
            return (from h in dc.alims
                    where h.iptal == false && h.CustID == custID
                    orderby h.alim_tarih descending
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
                        kullanici = h.inserted
                    }).Take(10).ToList();
        }

        private List<KrediRepo> krediler()
        {
            return (from k in dc.faturas
                    where k.MusteriID == custID && k.tur == "Fatura" && k.iptal != true
                    orderby k.son_odeme_tarihi descending
                    select new KrediRepo
                    {
                        ID = k.ID,
                        islem_tarihi = k.islem_tarihi,
                        gecerlilik_tarihi = k.son_odeme_tarihi,
                        paketadi = k.abonelik_pakets.paket_adi,
                        tutar = k.tutar,
                        kullanici = k.inserted
                    }).ToList();
        }
    }
    public class MusteriDetay
    {
        public List<AlimRepo> alimlar { get; set; }
        public cariHesapRepo cari { get; set; }
        public List<ServisRepo> servis { get; set; }
        public MusteriBilgileri musteri { get; set; }
        public List<musteriOdemeRepo> odemeler { get; set; }
        public List<ServisHesapRepo> kararlar { get; set; }
        public List<ServisHesapRepo> tamirler { get; set; }
        public List<yedekUrunRepo> yedekler { get; set; }

        public List<CihazRepo> urunler { get; set; }
        public List<KrediRepo> krediler { get; set; }
        public GunuGelen GunuGelenler { get; set; }
        public string eksikler { get; set; }

    }
    public class KrediRepo
    {
        public int ID { get; set; }
        public decimal tutar { get; set; }
        public System.DateTime gecerlilik_tarihi { get; set; }
        public Nullable<System.DateTime> islem_tarihi { get; set; }
        public string kullanici { get; set; }
        public string paketadi { get; set; }
        public Nullable<int> paket_id { get; set; }

    }
    public class GunuGelen
    {
        public decimal tutar { get; set; }
        public string sonrakiTarih { get; set; }
        public decimal sonrakiTutar { get; set; }
    }
    public class MusteriBilgileri
    {
        public int CustID { get; set; }



        //müşteri bilgileri
        public string adi { get; set; }
        public string tc { get; set; }
        public string tel { get; set; }
        public string adres { get; set; }

        public string firma { get; set; }
        public string istihbarat { get; set; }
        public bool? disservis { get; set; }
        public bool? tedarikci { get; set; }
        public bool? musteri { get; set; }
        public bool? usta { get; set; }
        public int? paket_id { get; set; }


    }
}
