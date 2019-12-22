using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class Kurulum
    {

        //string owner;
        radiusEntities dc;
        public Kurulum(radiusEntities dc)
        {

            this.dc = dc;
        }



        //cari hesap güncelleme yapalım
        public void ClearDB()
        {
            var alimlar = dc.alims.ToList();
            var aldetay = dc.alim_detays.ToList();
            foreach (var a in aldetay)
            {
                dc.alim_detays.Remove(a);

            }
            foreach (var al in alimlar)
            {
                dc.alims.Remove(al);
            }
            var bankalar = dc.bankas.ToList();
            foreach (var b in bankalar)
            {
                b.bakiye = 0;
                b.cikis = 0;
                b.giris = 0;

            }
            var cariler = dc.carihesaps.Where(x => x.MusteriID > 0).ToList();
            foreach (var c in cariler)
            {
                dc.carihesaps.Remove(c);
            }
            var cekler = dc.cekhesaps.ToList();
            foreach (var cek in cekler)
            {
                dc.cekhesaps.Remove(cek);
            }




            var kartcariler = dc.kart_caris.ToList();
            foreach (var kc in kartcariler)
            {
                dc.kart_caris.Remove(kc);
            }

            //kart tanim ve müşteriödemelere bağlı
            var karthesap = dc.kart_hesaps.ToList();
            foreach (var kh in karthesap)
            {
                dc.kart_hesaps.Remove(kh);
            }

            var kartodemeler = dc.kart_odemes.ToList();
            foreach (var ko in kartodemeler)
            {
                dc.kart_odemes.Remove(ko);
            }

            var kartlar = dc.kart_tanims.Where(x => x.kart_id > 0).ToList();
            foreach (var kart in kartlar)
            {
                dc.kart_tanims.Remove(kart);
            }


            //satışlar ve servis hesaplara bağlı
            var faturalar = dc.faturas.ToList();
            foreach (var fat in faturalar)
            {
                dc.faturas.Remove(fat);
            }

            //musteri ödemelerine bağlı
            var kasaislemleri = dc.kasaharekets.ToList();
            foreach (var ki in kasaislemleri)
            {
                dc.kasaharekets.Remove(ki);
            }

            var kasa = dc.kasahesaps.ToList();
            foreach (var kas in kasa)
            {
                kas.ToplamBakiye = 0;
                kas.ToplamCikis = 0;
                kas.ToplamGiris = 0;
            }


            //cihaz, müşteri//ödeme,
            var satis = dc.satislars.ToList();
            foreach (var sat in satis)
            {
                dc.satislars.Remove(sat);
            }

            //cihaz_garantis, masraftips, customers,postanims, karttanims,bankasa bağlı
            var musteriodeme = dc.musteriodemelers.ToList();
            foreach (var mo in musteriodeme)
            {
                dc.musteriodemelers.Remove(mo);
            }

            var fifos = dc.cihaz_fifos.ToList();
            foreach (var f in fifos)
            {
                dc.cihaz_fifos.Remove(f);
            }

            var garantis = dc.cihaz_garantis.ToList();
            foreach (var g in garantis)
            {
                dc.cihaz_garantis.Remove(g);
            }

            var stoklar = dc.cihaz_stoks.ToList();
            foreach (var st in stoklar)
            {
                dc.cihaz_stoks.Remove(st);
            }

            //customer, servis, cihaz
            var servishesap = dc.servicehesaps.ToList();
            foreach (var sh in servishesap)
            {
                dc.servicehesaps.Remove(sh);
            }
            var paketdetaylar = dc.servis_paket_detays.ToList();
            foreach (var p in paketdetaylar)
            {
                dc.servis_paket_detays.Remove(p);
            }

            var paketler = dc.servis_pakets.ToList();
            foreach (var pp in paketler)
            {
                dc.servis_pakets.Remove(pp);
            }

            var gruplar = dc.cihaz_grups.Where(x => x.grupid > 0).ToList();
            foreach (var gr in gruplar)
            {
                dc.cihaz_grups.Remove(gr);
            }

            var cihazlar = dc.cihazs.ToList();
            foreach (var cih in cihazlar)
            {
                dc.cihazs.Remove(cih);
            }


            var poshesap = dc.poshesaps.ToList();
            foreach (var ph in poshesap)
            {
                dc.poshesaps.Remove(ph);
            }
            var poslar = dc.pos_tanims.ToList();
            foreach (var p in poslar)
            {
                dc.pos_tanims.Remove(p);
            }

            //service,durum,

            var durumlar = dc.service_durums.ToList();
            foreach (var dr in durumlar)
            {
                dc.service_durums.Remove(dr);
            }


            var detaylar = dc.servicedetays.ToList();
            foreach (var sd in detaylar)
            {
                dc.servicedetays.Remove(sd);
            }

            var servisfaturalar = dc.service_faturas.ToList();
            foreach (var sf in servisfaturalar)
            {
                dc.service_faturas.Remove(sf);
            }

            //customer, servis tipi//urun_id
            var servisler = dc.services.ToList();
            foreach (var se in servisler)
            {
                dc.services.Remove(se);
            }

            // var durum = dc.service_durums.ToList();

            var urunler = dc.uruns.ToList();
            foreach (var u in urunler)
            {
                dc.uruns.Remove(u);
            }

            var yedekler = dc.yedek_uruns.ToList();
            foreach (var yu in yedekler)
            {
                dc.yedek_uruns.Remove(yu);
            }

            var musteriler = dc.customers.Where(x => x.CustID > 0).ToList();
            foreach (var m in musteriler)
            {
                dc.customers.Remove(m);
            }
            KaydetmeIslemleri.kaydetR(dc);
            //default değerleri ekleyelim

            string ad = "Tolga";
            string soyad = "Kaya";
            string adres = "Bozyazı/Mersin";
            string email = "bilgitap@hotmail.com";
            string tel = "05069468693";
            string tc = "50275488965";

            string kim = "Programcı";
            string prim_kar = "0";
            string prim_yekun = "0";

            string ad2 = "Tamirci";
            string soyad2 = "Tamirci";
            string adres2 = "Bozyazı/Mersin";
            string email2 = "bilgitap@hotmail.com";
            string tel2 = "05069468693";
            string tc2 = "50275488969";

            string kim2 = "Programcı";
            string prim_kar2 = "0";
            string prim_yekun2 = "0";


            string ad3 = "Usta";
            string soyad3 = "Usta";
            string adres3 = "Bozyazı/Mersin";
            string email3 = "bilgitap@hotmail.com";
            string tel3 = "05069468693";
            string tc3 = "50275488969";

            string kim3 = "Programcı";
            string prim_kar3 = "50";
            string prim_yekun3 = "0";

            MusteriIslemleri mu = new MusteriIslemleri(dc); ;
            mu.musteriEkleR(ad, soyad, ad, adres, tel, tel, email, kim, tc, prim_kar, prim_yekun, true, false, false, false,null);
            mu.musteriEkleR(ad2, soyad2, ad2, adres2, tel2, tel2, email2, kim2, tc2, prim_kar2, prim_yekun2, false, false, false, true,null);
            mu.musteriEkleR(ad3, soyad3, ad3, adres3, tel3, tel3, email3, kim3, tc3, prim_kar3, prim_yekun3, false, false, true, false,null);
            mu.musteriEkleR("Tedarikçi", " Amca", "tedarikçi ltd", adres3, tel3, tel3, email3, kim3, tc3, "0", prim_yekun3, false, true, false, false,null);


            //kendi servisimiz -99
            // peşin satış -13
            // Genel Masraf -2
            // kart extre ödemesi için kart hesabı -1



            string adc = "T50 Kasa";
            int sure = 12;

            string adc2 = "Power Tuşu T50";


            CihazMalzeme ma = new CihazMalzeme(dc);
            ma.GrupEkle("Genel %18", 18, 0, 0);
            ma.GrupEkle("Hizmet %18-22", 18, 0, 22);
            int grupid = dc.cihaz_grups.FirstOrDefault(x=>x.grupid>0).grupid;

            ma.Yeni(adc, adc, sure, grupid, "123456789");
            ma.Yeni(adc2, adc2, sure, grupid, "321654987");


            AyarIslemleri ay = new AyarIslemleri(dc);
            ay.servisDurumEkleR("Servise alındı", true, true, false, true, false, false, false);
            ay.servisDurumEkleR("Servise tamamlandı", true, true, false, false, true, false, false);
            ay.servisDurumEkleR("Müşteri onayı bekleniyor", true, true, false, false, false, true, false);
            ay.servisDurumEkleR("Müşteri onayladı", true, true, false, true, false, false, true);








        }
        public void CariGuncelle(int custid, decimal netBakiye, bool borclu, string kullanici)
        {

            //önce cari hesabı güncelleyelim
            carihesap c = dc.carihesaps.FirstOrDefault(x => x.MusteriID == custid);



            if (c != null)
            {
                c.NetAlacak = 0;
                c.NetBorc = 0;
                c.ToplamAlacak = 0;
                c.ToplamBakiye = 0;
                c.ToplamBorc = 0;
                c.ToplamOdedigimiz = 0;
                c.ToplamOdenen = 0;

                if (borclu == false)
                {
                    //alacaklıymış o yüzden fatura oluşturmayacaz sadece carihesap kaydı yapacaz

                    c.ToplamOdedigimiz = 0;
                    c.ToplamAlacak = netBakiye;

                }
                else
                {


                    TeknikServis.Radius.customer mu = dc.customers.Where(x => x.CustID == custid).FirstOrDefault();


                    fatura fatik = new fatura();
                    fatik.bakiye = netBakiye;
                    fatik.son_odeme_tarihi = DateTime.Now;
                    fatik.sattis_tarih = DateTime.Now;
                    fatik.no = "-1";
                    fatik.taksit_no = 0;
                    fatik.odenen = 0;
                    fatik.Firma = "firma";
                    fatik.tc = mu.TC;
                    fatik.MusteriID = mu.CustID;
                    fatik.islem_tarihi = DateTime.Now;
                    fatik.telefon = mu.telefon;
                    fatik.tutar = netBakiye;
                    fatik.Firma = mu.Firma;
                    fatik.tur = "Devir";
                    fatik.iptal = false;
                    fatik.inserted = kullanici;
                    dc.faturas.Add(fatik);

                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void CihazGuncelle()
        {
            //List<adminliste> adminler = dc.adminlistes.ToList();
            //List<cihaz> cihazlar = dc.cihazs.ToList();
            //foreach (adminliste ad in adminler)
            //{
            //    foreach (cihaz c in cihazlar)
            //    {
            //        cihaz_stoks stok = new cihaz_stoks();
            //        stok.aciklama = c.aciklama;
            //        stok.bakiye = 0;
            //        stok.cihaz_adi = c.cihaz_adi;
            //        stok.cihaz_id = c.ID;
            //        stok.cikis = 0;
            //        stok.Firma = c.Firma;
            //        stok.garanti_suresi = c.garanti_suresi;
            //        stok.giris = 0;
            //        stok.owner = ad.username;
            //        dc.cihaz_stoks.Add(stok);
            //    }
            //}
            //KaydetmeIslemleri.kaydetR(dc);
        }
    }
}
