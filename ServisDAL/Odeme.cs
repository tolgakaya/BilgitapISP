using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class Odeme
    {


        radiusEntities dc;
        public Odeme(radiusEntities dc)
        {

            this.dc = dc;
        }

        public string KullaniciID { get; set; }

        public decimal OdemeMiktar { get; set; }
        public System.DateTime OdemeTarih { get; set; }
        public string Aciklama { get; set; }

        public int Musteri_ID { get; set; }
        public string kullanici { get; set; }
        public DateTime? extre_tarih { get; set; }
        public bool? duzensiz { get; set; }
        public bool? mahsup { get; set; }
        public string masraf_tipi { get; set; }
        public int? masraf_id { get; set; }
        public string mahsup_key { get; set; }
        public void Nakit(string kullanici)
        {

            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";


            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.OdemeTarih = this.OdemeTarih;
            ode.masraf_tipi = this.masraf_tipi;
            ode.masraf_id = this.masraf_id;
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "odeme";
            ode.tahsilat_turu = "Nakit";
            ode.inserted = kullanici;
            ode.taksit_no = -1;
            ode.extre_tarih = extre_tarih;
            ode.standart = duzensiz;
            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);

        }
        public void Mahsup(string kullanici)
        {

            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";
            ode.inserted = kullanici;
            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.OdemeTarih = this.OdemeTarih;
            ode.tahsilat_odeme = "odeme";
            ode.tahsilat_turu = "Mahsup";
            ode.taksit_no = -1;
            ode.extre_tarih = extre_tarih;
            ode.islem_tarihi = DateTime.Now;
            ode.mahsup = this.mahsup;
            ode.mahsup_key = this.mahsup_key;
            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);

        }
        //mahsullaşmaiçin -1 kodlu bir kart lazım bunun otomatik olarak oluşturduğumuzu varsayıyorum.
        public void Kart(int taksit_sayisi, int kart_id, bool karta_ode, string kullanici)
        {
            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";

            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.OdemeTarih = this.OdemeTarih;
            ode.masraf_tipi = this.masraf_tipi;
            ode.masraf_id = this.masraf_id;
            ode.extre_tarih = extre_tarih;
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "odeme";
            ode.tahsilat_turu = "Kart";
            ode.taksit_no = -1;
            ode.taksit_sayisi = taksit_sayisi;
            ode.kart_id = kart_id;
            ode.standart = duzensiz;
            ode.mahsup = this.mahsup;
            ode.mahsup_key = this.mahsup_key;
            ode.inserted = kullanici;
            if (karta_ode == true)
            {
                KartaOdemeKasa(kart_id, OdemeMiktar, OdemeTarih, "Kart Extre Ödeme", kullanici);
            }

            dc.musteriodemelers.Add(ode);

            KaydetmeIslemleri.kaydetR(dc);
        }
        public void KartaOdemeKasa(int kart_id, decimal tutar, DateTime odeme_tarih, string aciklama, string kullanici)
        {
            Odeme o = new Odeme(dc);

            kart_odemes ko = new kart_odemes();
            string ad = dc.kart_tanims.FirstOrDefault(x => x.kart_id == kart_id).kart_adi;

            //o.masraf_id = masraf_tipi;
            o.masraf_tipi = "Kart Extre Ödeme";
            o.OdemeMiktar = tutar;
            o.OdemeTarih = odeme_tarih;
            o.Musteri_ID = -1;
            o.KullaniciID = "-";
            o.kullanici = "-";
            o.Aciklama = ad + " -- Kart Extre Ödemesi";
            o.mahsup = false;
            o.duzensiz = false;



            ko.aciklama = ad + " -- Kart Extre Ödemesi";
            ko.iptal = false;
            ko.kart_id = kart_id;
            ko.tarih = odeme_tarih;
            ko.tutar = tutar;
            ko.inserted = kullanici;
            dc.kart_odemes.Add(ko);

            o.Nakit(kullanici);

            KaydetmeIslemleri.kaydetR(dc);
        }
        public void Banka(int banka_id, string kullanici)
        {
            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";

            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.masraf_tipi = this.masraf_tipi;
            ode.masraf_id = this.masraf_id;
            ode.extre_tarih = extre_tarih;
            ode.OdemeTarih = this.OdemeTarih;
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "odeme";
            ode.tahsilat_turu = "Banka";
            ode.standart = duzensiz;
            ode.taksit_no = -1;
            ode.banka_id = banka_id;
            ode.inserted = kullanici;
            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void Cek(string belge_no, DateTime vade)
        {
            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";

            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.islem_tarihi = DateTime.Now;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.masraf_tipi = this.masraf_tipi;
            ode.masraf_id = this.masraf_id;
            ode.OdemeTarih = this.OdemeTarih;
            ode.tahsilat_odeme = "odeme";
            ode.tahsilat_turu = "Cek";
            ode.taksit_no = -1;
            ode.standart = duzensiz;
            ode.belge_no = belge_no;
            ode.vade_tarih = vade;
            ode.masraf = 0;
            ode.mahsup = this.mahsup;
            ode.mahsup_key = this.mahsup_key;

            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);
        }

        // borç her zaman kasaya alınıyor ve kasadan veriliyor
        // alınan borç yine yukarıdaki yöntemlerden birisiyle ödenebilir.
        //verilen borcun tahsil edilmesini de düşünecez. // verilen borçta faturas'a kayıt yapsa ve oradan normal tahsilat işlese.
        public void BorcAl()
        {
            musteriodemeler ode = new musteriodemeler();
            ode.Aciklama = this.Aciklama;
            ode.Firma = "firma";
            ode.no = "-";

            ode.iptal = false;
            ode.kullanici = this.kullanici;
            ode.KullaniciID = this.KullaniciID;
            ode.Musteri_ID = this.Musteri_ID;
            ode.OdemeMiktar = this.OdemeMiktar;
            ode.OdemeTarih = this.OdemeTarih;
            ode.masraf_tipi = this.masraf_tipi;
            ode.masraf_id = this.masraf_id;
            ode.tahsilat_odeme = "tahsilat";
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_turu = "Borc";
            ode.taksit_no = -1;

            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void BorcVer()
        {
            TeknikServis.Radius.customer mu = dc.customers.Where(x => x.CustID == this.Musteri_ID).FirstOrDefault();

            fatura fatik = new fatura();
            fatik.bakiye = this.OdemeMiktar;


            fatik.no = "bak";
            fatik.taksit_no = 0;
            fatik.odenen = 0;
            fatik.Firma = "firma";
            fatik.MusteriID = Musteri_ID;
            fatik.tahsilat_aciklama = this.Aciklama;
            fatik.islem_tarihi = DateTime.Now;
            fatik.tc = mu.TC;
            fatik.telefon = mu.telefon;
            fatik.tutar = this.OdemeMiktar;

            fatik.tur = "Borc";
            fatik.iptal = false;
            fatik.service_id = null;
            dc.faturas.Add(fatik);

            KaydetmeIslemleri.kaydetR(dc);
        }

    }
}
