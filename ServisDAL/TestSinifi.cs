using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class TestSinifi
    {
        radiusEntities dc;
        public TestSinifi(radiusEntities dc)
        {
            this.dc = dc;
        }

        public void Stress()
        {
            //this.MusteriEkle();
            //this.UrunEkle();
            //this.CihazEkle();
            //this.AlimEkle();
            //this.ServisEkle();
            this.SatisEkle();
            this.TahsilatEkleNakit();
            this.TahsilatEklePos();
        }
        public void MusteriEkle()
        {
            MusteriIslemleri mu = new MusteriIslemleri(dc);
            for (int i = 1; i < 2000; i++)
            {
                string s = "Müşteri" + Araclar.yaziyaCevir(i);
                mu.musteriEkleR(s, s, s, s, s, s, s, s, s, "0", "0", true, false, false, false,null);
            }
            //mu.musteriEkleR(ad, soyad, ad, adres, tel, tel, email, kim, tc, prim_kar, prim_yekun, true, false, false, false);
        }
        public void UrunEkle()
        {
            List<customer> musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            ServisIslemleri s = new ServisIslemleri(dc);

            foreach (var c in musteriler)
            {
                for (int i = 0; i < 2; i++)
                {
                    string cins = "Cihaz" + (i + i).ToString();
                    string imei = (123 + c.CustID + i).ToString();
                    s.urunEkleR(c.CustID, cins, DateTime.Now.AddDays(-10), 12, cins, "", imei, imei, "");
                }

            }
        }

        public void CihazEkle()
        {
            CihazMalzeme ma = new CihazMalzeme(dc);
            int grupid = dc.cihaz_grups.FirstOrDefault(x=>x.grupid>0).grupid;

            for (int i = 0; i < 2000; i++)
            {
                string ad = "Ürün" + i.ToString();
                ma.Yeni(ad, ad, 12, grupid, ad);
            }
        }

        public void AlimEkle()
        {
            List<alim_detays> alim_detaylar = dc.alim_detays.ToList();
            foreach (var a in alim_detaylar)
            {
                dc.alim_detays.Remove(a);
            }
            List<alim> alimlar = dc.alims.ToList();
            foreach (var al in alimlar)
            {
                dc.alims.Remove(al);

            }
            KaydetmeIslemleri.kaydetR(dc);

            int tedarikci = dc.customers.FirstOrDefault(x => x.tedarikci == true).CustID;
            //ilk cihazidsi
            int cihazid = dc.cihazs.FirstOrDefault().ID;

            for (int i = 1; i < 1000; i++)
            {
                SatinAlim al = new SatinAlim(dc);
                AlimRepo hesap = new AlimRepo();
                hesap.aciklama = i.ToString();
                hesap.alim_tarih = DateTime.Now.AddDays(-i);
                hesap.belge_no = i.ToString() + i.ToString();
                hesap.CustID = tedarikci;
                hesap.konu = "Satınalma->" + i.ToString();
                hesap.kullanici = "admin";
                hesap.musteri_adi = "Tedarikçi Amca";
                hesap.toplam_kdv = 18;
                hesap.toplam_tutar = 100;
                hesap.toplam_yekun = 118;
                al.hesap = hesap;

                DetayRepo d = new DetayRepo();
                d.aciklama = "Detay->" + i.ToString();
                d.adet = 10;
                d.cihaz_adi = "-";
                d.cust_id = tedarikci;
                d.cihaz_id = cihazid;
                d.fiyat = 100 + i;
                d.kdv = (100 + i) * 18 / 100;
                d.kullanici = "admin";
                d.musteri_adi = "Tedarikçi Amca";
                d.tarih = DateTime.Now.AddDays(-i);
                d.tutar = 10 * (100 + i);
                d.yekun = 10 * (100 + i);

                List<DetayRepo> detaylar = new List<DetayRepo>();
                detaylar.Add(d);
                al.detay = detaylar;

                al.alim_kaydet("Admin");
                cihazid += i;

            }



        }

        //paketli servis ekleyelim canlı ve hesaplı olması için
        //burada servis paketlerinin hazırda var olduğunu varsayıyorum,stokları da yeterli olmalı
        public void ServisEkle()
        {
            List<customer> musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            ServisIslemleri s = new ServisIslemleri(dc);
            var paketler = dc.servis_pakets.ToList();
            int servicetipi = dc.service_tips.FirstOrDefault().tip_id;
            foreach (var c in musteriler)
            {
                //musteri urunleri
                int urunID = dc.uruns.Where(x => x.MusteriID == c.CustID).FirstOrDefault(x => x.iptal != true).urunID;
                foreach (var p in paketler)
                {
                    //her müşteriye bütün paketler için servis ekleyecez
                    s.servisEklePaketli(p.paket_id, c.CustID, "Admin", p.paket_adi, urunID, servicetipi, "0", Araclar.KimlikUret(10), p.paket_adi, DateTime.Now.AddDays(-5), "Admin");

                }

            }
        }
        public void SatisEkle()
        {
            List<customer> musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            ServisIslemleri s = new ServisIslemleri(dc);
            var paketler = dc.servis_pakets.ToList();

            foreach (var c in musteriler)
            {
                //musteri urunleri

                foreach (var p in paketler)
                {
                    //her müşteriye bütün paketler için servis ekleyecez
                    s.servisEkleKararli(p.paket_id, c.CustID, "firma", "Satış", Araclar.KimlikUret(10), "Satış", DateTime.Now.AddDays(-10), null, "Admin");
                }

            }
        }

        public void TahsilatEkleNakit()
        {
            List<customer> musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            Tahsilat t = new Tahsilat(dc);
            foreach (var c in musteriler)
            {
                //nakit 

                t.Aciklama = c.Ad;
                t.kullanici = "Admin";
                t.KullaniciID = "Admin";
                t.mahsup = false;
                t.Musteri_ID = c.CustID;
                t.OdemeMiktar = 20;
                t.OdemeTarih = DateTime.Now.AddDays(-2);
                t.Nakit("Admin");

            }
        }
        public void TahsilatEklePos()
        {
            List<customer> musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            Tahsilat t = new Tahsilat(dc);
            int pos_id = dc.pos_tanims.FirstOrDefault(x => x.pos_id > 0).pos_id;

            foreach (var c in musteriler)
            {

                t.Aciklama = c.Ad;
                t.kullanici = "Admin";
                t.KullaniciID = "Admin";
                t.mahsup = false;
                t.Musteri_ID = c.CustID;
                t.OdemeMiktar = 33;
                t.OdemeTarih = DateTime.Now.AddDays(-3);
                t.Kart(pos_id, "Admin");

            }
        }
    }
}
