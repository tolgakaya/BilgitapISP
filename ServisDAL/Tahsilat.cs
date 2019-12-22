using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class Tahsilat
    {


        radiusEntities dc;
        public Tahsilat(radiusEntities dc)
        {

            this.dc = dc;
        }

        public string KullaniciID { get; set; }

        //decimal? masraf,  DateTime? vade_tarih,
        public decimal OdemeMiktar { get; set; }
        public System.DateTime OdemeTarih { get; set; }
        public string Aciklama { get; set; }
        public int Musteri_ID { get; set; }
        public string kullanici { get; set; }
        public int? hesap_id { get; set; }
        public bool? mahsup { get; set; }

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
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "tahsilat";
            ode.tahsilat_turu = "Nakit";
            ode.inserted = kullanici;
            ode.taksit_no = -1;

            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);

        }
        public void Ayni(string kullanici)
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
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "tahsilat";
            ode.tahsilat_turu = "Ayni";
            ode.inserted = kullanici;
            ode.taksit_no = -1;

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
            ode.tahsilat_odeme = "tahsilat";
            ode.tahsilat_turu = "Mahsup";
            ode.taksit_no = -1;
         
            ode.islem_tarihi = DateTime.Now;
            ode.mahsup = this.mahsup;
            ode.mahsup_key = this.mahsup_key;
            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);

        }
        //mahsullaşmaiçin -1 kodlu bir kart lazım bunun otomatik olarak oluşturduğumuzu varsayıyorum.
        public void Kart( int pos_id,string kullanici)
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
           
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "tahsilat";
            ode.tahsilat_turu = "Kart";
            ode.taksit_no = -1;
         
            ode.pos_id = pos_id;
            
            ode.mahsup = this.mahsup;
            ode.mahsup_key = this.mahsup_key;
            ode.inserted = kullanici;
             

            dc.musteriodemelers.Add(ode);

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
           
            ode.OdemeTarih = this.OdemeTarih;
            ode.islem_tarihi = DateTime.Now;
            ode.tahsilat_odeme = "tahsilat";
            ode.tahsilat_turu = "Banka";
 
            ode.taksit_no = -1;
            ode.banka_id = banka_id;
            ode.inserted = kullanici;
            dc.musteriodemelers.Add(ode);
            KaydetmeIslemleri.kaydetR(dc);
        }
     

    }
}
