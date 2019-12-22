using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekServis
    {
        radiusEntities dc;

        int servisid;
        public TekServis(radiusEntities dc, int servisid)
        {
            this.servisid = servisid;

            this.dc = dc;
        }

        public ServisInfo servis()
        {
            ServisInfo s = new ServisInfo();
            s.kararlar = this.kararlar();
            s.genel = this.genel();
            s.detaylar = this.detaylar();
            return s;
        }
        private ServisRepo genel()
        {
            return (from s in dc.services
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
        private List<ServisHesapRepo> kararlar()
        {
            return (from s in dc.servicehesaps
                    where s.ServiceID == servisid && s.iptal == false
                    orderby s.TarihZaman descending
                    select new ServisHesapRepo
                    {
                        hesapID = s.HesapID,
                        aciklama = s.Aciklama,
                        islemParca = s.IslemParca,
                        kdv = s.KDV,
                        //musteriAdi = s.MusteriID == null ? "-" : s.customer.Ad,
                        ////ustaAdi = s.tamirci_id == null ? "-" : s.customer1.Ad,
                        disServis = s.tamirci_id == null ? "-" : s.customer1.Ad,
                        //musteriID = s.MusteriID == null ? -99 : (int)s.MusteriID,
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
                        kullanici = s.updated == null ? s.inserted : (s.inserted + "-" + s.updated)

                    }).ToList();

        }
        private List<ServisDetayRepo> detaylar()
        {
            return (from s in dc.servicedetays

                    where s.ServiceID == servisid
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

    }

    public class ServisInfo
    {
        public ServisRepo genel { get; set; }
        public List<ServisHesapRepo> kararlar { get; set; }
        public List<ServisDetayRepo> detaylar { get; set; }

    }
}
