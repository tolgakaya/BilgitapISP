using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class FaturaBas
    {
        radiusEntities dc;

        public FaturaBas(radiusEntities dc)
        {
            this.dc = dc;
        }

        public Fatura_No NoOlustur()
        {
            //bu fatura no işini yalnızca firma olarak çevir
            fatura f = dc.faturas.ToList().Where(x => x.basim_tarih != null).OrderBy(x => x.basim_tarih).LastOrDefault();

            string seri = "0";
            int no = -1;
            if (f != null)
            {

                seri = f.fat_seri;
                no = (int)f.fat_no + 1;
            }
            return new Fatura_No { seri = seri, no = no };

        }

        //eski internet faturası yeni peşin satış faturası olmuş
        public InternetFaturasi FaturaBilgileriInternet(int fatID)
        {
            fatura i = dc.faturas.FirstOrDefault(x => x.ID == fatID);
            InternetFaturasi internet = new InternetFaturasi();

            if (i != null)
            {
                List<Kalem> kalemler = new List<Kalem>();

                Fatura_No no = NoOlustur();
                Baski_Gorunum baski = new Baski_Gorunum
                {
                    ID = i.ID,
                    isim = i.customer.unvan == null ? i.customer.Ad : i.customer.unvan,
                    KDV = i.KDV,
                    OIV = i.OIV,
                    tarih = (DateTime)i.sattis_tarih, //tarihin ne olacağını bilmiyorum
                    TC = i.customer.TC,
                    VD = i.customer.vd,
                    adres = i.customer.Adres,
                    yaziIle = "YALNIZ " + Araclar.yaziyaCevir(i.tutar),
                    Tutar = i.vergisiz_tutar,
                    Yekun = i.tutar,
                    fat_no = no.no,
                    fat_seri = no.seri,
                    firma = "firma"


                };

                Kalem kalem = new Kalem
                                        {
                                            cinsi = "İnternet Abonelik",
                                            fiyat = i.tutar,
                                            mik = 1,
                                            tutar = i.tutar
                                        };
                kalemler.Add(kalem);
                internet.Bilgiler = baski;
                internet.Kalemler = kalemler;

                if (baski != null)
                {

                    i.fat_no = no.no;
                    i.fat_seri = no.seri;
                    i.basim_tarih = i.sattis_tarih;
                    KaydetmeIslemleri.kaydetR(dc);
                }


            }

            return internet;
        }

        public InternetFaturasi FaturaBilgileriPesin(int odeme_id, string unvan, string tc, string vd, string adres)
        {
            InternetFaturasi i = new InternetFaturasi();

            List<satislar> pesinler = dc.satislars.Where(x => x.odeme_id == odeme_id && x.iptal == false).ToList();
            Baski_Gorunum baski = (from s in pesinler
                                   group s by s.odeme_id into g
                                   select new Baski_Gorunum
                                   {
                                       ID = (int)g.Key,
                                       isim = unvan,
                                       KDV = g.Sum(x => x.kdv),
                                       OIV = g.Sum(x => x.oiv),
                                       tarih = g.FirstOrDefault().tarih,
                                       TC = tc,
                                       VD = vd,
                                       adres = adres,
                                       yaziIle = "YALNIZ " + Araclar.yaziyaCevir(g.Sum(x => x.yekun)),
                                       Tutar = g.Sum(x => x.tutar),
                                       Yekun = g.Sum(x => x.yekun),
                                       fat_seri = "",
                                       fat_no = 0,

                                   }).FirstOrDefault();

            if (baski != null)
            {
                AyarCurrent ay = new AyarCurrent(dc);
                ay.set(pesinler.FirstOrDefault().tarih);
                foreach (satislar sat in pesinler)
                {
                    sat.basim_tarih = sat.tarih;
                    sat.tc = tc;
                    sat.vd = vd;
                    sat.unvan = unvan;


                }
                KaydetmeIslemleri.kaydetR(dc);
            }
            List<Kalem> kalemler = (from k in pesinler
                                    select new Kalem
                                    {
                                        cinsi = k.cihaz.cihaz_adi,
                                        fiyat = k.yekun / k.adet,
                                        mik = k.adet,
                                        tutar = k.yekun
                                    }).ToList();
            i.Bilgiler = baski;
            i.Kalemler = kalemler;
            return i;
        }

        public Baski_Gorunum FaturaBilgileriServis(int servisID)
        {

            Baski_Gorunum baski = new Baski_Gorunum();

            //servis kaydının bayisi üzerinden işlem yapacaz
            TeknikServis.Radius.service servisimiz = dc.services.FirstOrDefault(x => x.ServiceID == servisID);
            if (servisimiz != null)
            {
                AyarCurrent ay = new AyarCurrent(dc);
                ay.set((DateTime)servisimiz.KapanmaZamani);

                Fatura_No no = NoOlustur();
                baski = new Baski_Gorunum
                                      {
                                          ID = servisimiz.ServiceID,
                                          isim = servisimiz.customer.unvan == null ? servisimiz.customer.Ad : servisimiz.customer.unvan,
                                          KDV = (decimal)servisimiz.service_faturas.KDV,
                                          OIV = 0,
                                          tarih = (DateTime)servisimiz.KapanmaZamani, //tarihin ne olacağını bilmiyorum
                                          TC = servisimiz.customer.TC,
                                          VD = servisimiz.customer.vd,
                                          adres = servisimiz.customer.Adres,
                                          yaziIle = "YALNIZ " + Araclar.yaziyaCevir((decimal)servisimiz.service_faturas.Yekun),
                                          Tutar = (decimal)servisimiz.service_faturas.Tutar,
                                          Yekun = (decimal)servisimiz.service_faturas.Yekun,
                                          fat_no = no.no,
                                          fat_seri = no.seri

                                      };

                if (baski != null)
                {

                    servisimiz.service_faturas.fat_no = no.no;
                    servisimiz.service_faturas.fat_seri = no.seri;
                    servisimiz.service_faturas.basim_tarih = servisimiz.KapanmaZamani;
                    KaydetmeIslemleri.kaydetR(dc);
                }

            }


            return baski;
        }
        public List<Kalem> FaturaKalemleriServis(int servisID)
        {

            return (from k in dc.servicehesaps
                    where k.ServiceID == servisID && (k.iptal == null || k.iptal == false) && k.onay == true
                    select new Kalem
                    {
                        cinsi = k.Urun_Cinsi == null ? k.IslemParca : k.Urun_Cinsi,
                        fiyat = (decimal)k.Tutar,
                        mik = 1,
                        tutar = (decimal)k.Tutar
                    }).ToList();
        }

        public List<Baski_Gorunum> BasimListesi(string firma,DateTime baslangicTarihi, DateTime bitisTarihi, string fatura_tipi, DateTime? sonfatura)
        {
            //tarih olarak satış tarihine göre alıyorum
            //eğer kullanıcı ise bu yukarıdaki bayiParam kul'un ownerı olacak, eğer bayi yada managersa username olacak
            if (sonfatura != null)
            {
                if (((DateTime)sonfatura).Date > baslangicTarihi.Date)
                {
                    baslangicTarihi = (DateTime)sonfatura;
                }
            }
            IEnumerable<Baski_Gorunum> servisler = from s in dc.services
                                                   where (s.iptal == null || s.iptal == false) && s.KapanmaZamani != null && (DateTime)s.KapanmaZamani >= baslangicTarihi && (DateTime)s.KapanmaZamani < bitisTarihi
                                                   && s.service_faturas.basim_tarih == null && s.service_faturas.Yekun > 0 && (fatura_tipi == "Hepsi" || fatura_tipi == "Servis" || fatura_tipi == "cihaz") &&
                                                   (fatura_tipi == "Hepsi" || (fatura_tipi == "Servis" ? (s.service_faturas.service_tur == "Servis" || String.IsNullOrEmpty(s.service_faturas.service_tur)) : s.service_faturas.service_tur == "cihaz"))

                                                   select new Baski_Gorunum
                                                   {
                                                       ID = s.ServiceID,
                                                       isim = s.customer.unvan == null ? s.customer.Ad : s.customer.unvan,
                                                       KDV = s.service_faturas.KDV,
                                                       OIV = 0,
                                                       tarih = s.KapanmaZamani == null ? DateTime.Now : (DateTime)s.KapanmaZamani, //tarihin ne olacağını bilmiyorum
                                                       Yekun = s.service_faturas.Yekun,
                                                       fat_seri = "",
                                                       fat_no = 0,
                                                       tur = s.service_faturas.service_tur == null ? "Servis" : s.service_faturas.service_tur,


                                                   };
            IEnumerable<Baski_Gorunum> pesinler = (from s in dc.satislars
                                                   where s.iptal == false && s.tarih >= baslangicTarihi && s.tarih < bitisTarihi && s.basim_tarih == null &&
                                                   (fatura_tipi == "Hepsi" || fatura_tipi == "Peşin")
                                                   group s by s.odeme_id into g
                                                   select new Baski_Gorunum
                                                   {
                                                       ID = (int)g.Key,
                                                       isim = "isimsiz",
                                                       KDV = g.Sum(x => x.kdv),
                                                       OIV = g.Sum(x => x.oiv),
                                                       tarih = g.FirstOrDefault().tarih,
                                                       Yekun = g.Sum(x => x.yekun),
                                                       fat_seri = "",
                                                       fat_no = 0,
                                                       tur = "pesin",


                                                   });

            IEnumerable<Baski_Gorunum> internet = from f in dc.faturas
                                                  where (f.iptal == null || f.iptal == false) && (DateTime)f.sattis_tarih >= baslangicTarihi && f.sattis_tarih < bitisTarihi
                                                  && f.basim_tarih == null &&
                                                  (fatura_tipi == "Hepsi" || fatura_tipi == "internet") && (f.tur == "Fatura")

                                                  select new Baski_Gorunum
                                                  {
                                                      ID = f.ID,
                                                      isim = f.customer1.unvan == null ? f.customer1.Ad : f.customer1.unvan,
                                                      KDV = f.KDV,
                                                      OIV = f.OIV,
                                                      tarih = (DateTime)f.sattis_tarih, //tarihin ne olacağını bilmiyorum
                                                      Yekun = f.tutar,
                                                      fat_seri = "",
                                                      fat_no = 0,
                                                      tur = "internet",

                                                  };
            return servisler.Union(internet).Union(pesinler).OrderBy(x => x.tarih).ToList();
        }

        public List<Baski_Gorunum> BasilmisFaturalar(string firma, DateTime baslama, DateTime bitis)
        {
            //eğer kullanıcı ise bu yukarıdaki bayiParam kul'un ownerı olacak, eğer bayi yada managersa username olacak
            IEnumerable<Baski_Gorunum> servisler = from s in dc.services
                                                   where (s.iptal == null || s.iptal == false)
                                                   && s.service_faturas.basim_tarih != null && (DateTime)s.service_faturas.basim_tarih >= baslama && (DateTime)s.service_faturas.basim_tarih <= bitis
                                                   select new Baski_Gorunum
                                                   {
                                                       ID = s.ServiceID,
                                                       isim = s.customer.Ad,
                                                       KDV = s.service_faturas.KDV,
                                                       OIV = 0,
                                                       tarih = (DateTime)s.service_faturas.basim_tarih,
                                                       Yekun = s.service_faturas.Yekun,
                                                       fat_seri = s.service_faturas.fat_seri,
                                                       fat_no = (int)s.service_faturas.fat_no,
                                                       tur = "Servis"

                                                   };

            IEnumerable<Baski_Gorunum> pesinler = (from s in dc.satislars
                                                   where s.iptal == false && s.basim_tarih != null && (DateTime)s.basim_tarih >= baslama && (DateTime)s.basim_tarih < bitis
                                                   group s by s.odeme_id into g
                                                   select new Baski_Gorunum
                                                   {
                                                       ID = (int)g.Key,
                                                       isim = g.FirstOrDefault().unvan,
                                                       KDV = g.Sum(x => x.kdv),
                                                       OIV = g.Sum(x => x.oiv),
                                                       tarih = (DateTime)g.FirstOrDefault().basim_tarih,
                                                       Yekun = g.Sum(x => x.yekun),
                                                       fat_seri = "",
                                                       fat_no = 0,
                                                       tur = "pesin",
                                                       tahsilat_tipi = g.FirstOrDefault().musteriodemeler.tahsilat_turu

                                                   });


            IEnumerable<Baski_Gorunum> manuel = from m in dc.manuels
                                                where m.iptal == false && m.tarih <= baslama && m.tarih <= bitis
                                                select new Baski_Gorunum
                                                {
                                                    ID = m.id,
                                                    isim = m.unvan,
                                                    OIV = m.oiv,
                                                    tarih = m.tarih,
                                                    Yekun = m.yekun,
                                                    fat_seri = "",
                                                    fat_no = 0,
                                                    tur = "manuel",
                                                };

            IEnumerable<Baski_Gorunum> internet = from f in dc.faturas
                                                  where (f.iptal == null || f.iptal == false)
                                                  && f.basim_tarih != null && f.tur == "fatura" && (DateTime)f.basim_tarih >= baslama && (DateTime)f.basim_tarih <= bitis
                                                  select new Baski_Gorunum
                                                  {
                                                      ID = f.ID,
                                                      isim = f.ad,
                                                      KDV = f.KDV,
                                                      OIV = f.OIV,
                                                      tarih = (DateTime)f.basim_tarih, //tarihin ne olacağını bilmiyorum
                                                      Yekun = f.tutar,
                                                      fat_seri = f.fat_seri,
                                                      fat_no = (int)f.fat_no,
                                                      tur = "internet"
                                                  };
            return servisler.Union(internet).Union(pesinler).Union(manuel).OrderByDescending(x => x.tarih).ToList();
        }
        public void FaturaIptal(int id, string tur)
        {
            if (tur.Equals("Servis"))
            {
                var servis = dc.service_faturas.FirstOrDefault(x => x.ServiceID == id);
                if (servis != null)
                {
                    servis.basim_tarih = null;
                    KaydetmeIslemleri.kaydetR(dc);
                }

            }
            else if (tur.Equals("pesin"))
            {
                var pesinler = dc.satislars.Where(x => x.odeme_id == id).ToList();
                if (pesinler != null)
                {
                    foreach (var p in pesinler)
                    {
                        p.basim_tarih = null;
                    }
                    KaydetmeIslemleri.kaydetR(dc);
                }

            }
            else if (tur.Equals("manuel"))
            {
                var man = dc.manuels.FirstOrDefault(x => x.id == id);
                if (man != null)
                {
                    man.iptal = true;
                    KaydetmeIslemleri.kaydetR(dc);
                }
            }
            else if (tur.Equals("internet"))
            {
                var i = dc.faturas.FirstOrDefault(x => x.ID == id);
                if (i != null)
                {
                    i.basim_tarih = null;
                    KaydetmeIslemleri.kaydetR(dc);
                }

            }
        }

        public InternetFaturasi FaturaManuel(string isim, decimal kdv, decimal oiv, DateTime tarih, string tc, string vd, decimal tutar, decimal yekun, List<Kalem> kalemler)
        {
            InternetFaturasi internet = new InternetFaturasi();
            if (kalemler != null)
            {
                Fatura_No no = NoOlustur();
                Baski_Gorunum baski = new Baski_Gorunum
                {
                    ID = 0,
                    isim = isim,
                    KDV = kdv,
                    OIV = oiv,
                    tarih = tarih, //tarihin ne olacağını bilmiyorum
                    TC = tc,
                    VD = vd,
                    yaziIle = "YALNIZ " + Araclar.yaziyaCevir(yekun),
                    Tutar = tutar,
                    Yekun = yekun,
                    fat_no = no.no,
                    fat_seri = no.seri

                };

                internet.Bilgiler = baski;
                internet.Kalemler = kalemler;

                if (baski != null)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    ay.set((DateTime)tarih);
                    //burada manuel fatura tablosuna kayıt yapılacak
                    manuel m = new manuel();
                    m.unvan = isim;
                    m.kdv = kdv;
                    m.oiv = oiv;
                    m.otv = 0;
                    m.tutar = tutar;
                    m.yekun = yekun;
                    m.iptal = false;
                    m.tarih = tarih;
                    dc.manuels.Add(m);
                    KaydetmeIslemleri.kaydetR(dc);
                }
            }

            return internet;
        }

        public Makbuz_Gorunum MakbuzBilgileri(int custID, string aciklama, ayargenel ayar, decimal tutar, string kullanici)
        {
            //string tarih = DateTime.Now.ToShortDateString();
            //string saat = DateTime.Now.ToShortTimeString();
            string yaz = "YALNIZ " + Araclar.yaziyaCevir(tutar);
            return (from c in dc.customers
                    where c.CustID == custID
                    select new Makbuz_Gorunum
                    {
                        Aciklama = aciklama,
                        Adres = ayar.adres,
                        FirmaTam = ayar.adi,
                        FirmaTelefon = ayar.tel,
                        Musteri = c.Ad,
                        musteriTel = c.telefon,
                        musteriAdres = c.Adres,
                        Saat = DateTime.Now,
                        Tarih = DateTime.Now,
                        Tutar = tutar.ToString(),
                        Web = ayar.web,
                        gecerlilik = DateTime.Now,
                        yaziile = yaz,
                        kullanici = kullanici

                    }).FirstOrDefault();
        }

        //buradaki firma bilgilerini tasarımdan kendileri yapsın biz burada 
        public Servis_Baslama ServisBilgileri(string aciklama, string adres, string firma, int tipid, string kimlik, string musteri, string tipad)
        {
            //servis tipinin genel şartlarını çekelim

            var sartlar = dc.service_tips.FirstOrDefault(x => x.tip_id == tipid).aciklama;
            List<Servis_Hesap> kararlar = new List<Servis_Hesap>();

            return new Servis_Baslama
            {
                Aciklama = aciklama,
                Adres = adres,

                firma = firma,
                FirmaTam = "",
                FirmaTelefon = "",
                kimlik = kimlik,
                Musteri = musteri,
                Saat = DateTime.Now,
                Tarih = DateTime.Now,
                tip = tipad,
                Web = "",
                sartlar = sartlar,
                kararlar = kararlar

            };
        }

        public Servis_Baslama ServisBilgileri(string kimlik, ayargenel ayar)
        {

            return (from s in dc.services
                    where s.Servis_Kimlik_No == kimlik && s.iptal == false
                    select new Servis_Baslama
                 {
                     Aciklama = s.Aciklama,
                     Konu = s.Baslik,
                     musteri_urunu = s.urun.Cinsi,
                     urun_kodu = s.urun.imeino != null ? s.urun.imeino : (s.urun.serino != null ? s.urun.serino : s.urun.digerno),
                     firma = s.Firma,
                     FirmaTam = ayar.adi,
                     FirmaTelefon = ayar.tel,
                     Web = ayar.web,
                     Adres = ayar.adres,
                     email = ayar.email,
                     barkod = s.Servis_Kimlik_No,
                     kimlik = s.Servis_Kimlik_No,
                     Musteri = s.customer.Ad,
                     MusteriAdres = s.customer.Adres,
                     MusteriTel = s.customer.telefon,
                     toplam_tutar = s.service_faturas.Yekun,
                     Saat = s.AcilmaZamani,
                     Tarih = s.AcilmaZamani,
                     tip = s.service_tips.tip_ad,

                     sartlar = "",
                     kararlar = (from h in dc.servicehesaps
                                 where h.iptal == false && h.ServiceID == s.ServiceID
                                 orderby h.TarihZaman descending
                                 select new Servis_Hesap
                                 {
                                     aciklama = h.Aciklama,
                                     islem = h.IslemParca,
                                     adet = h.adet,
                                     tutar = h.Yekun,
                                     cihaz = h.cihaz_id == null ? "-" : h.cihaz.cihaz_adi

                                 }).ToList()

                 }).FirstOrDefault();
        }

        public extre ExtreBilgileri(int custid, int gun, ayargenel ay)
        {
            extre e = new extre();
            carihesap h = dc.carihesaps.FirstOrDefault(x => x.MusteriID == custid);
            e.hesap = h;
            DateTime tarih = DateTime.Now.AddDays(-gun);

            string aralik = tarih.ToShortDateString() + "-" + DateTime.Now.ToShortDateString();
            e.aralik = aralik;
            customer musteriBilgileri = dc.customers.FirstOrDefault(x => x.CustID == custid);
            e.Ad = musteriBilgileri.Ad;
            e.tc = musteriBilgileri.TC;
            e.firma = musteriBilgileri.Firma;
            e.firmaAdi = ay.adi;
            e.firmaAdres = ay.adres;
            e.firmaTel = ay.tel;
            e.firmaWeb = ay.web;

            IEnumerable<CariDetayYeni> hesaplar = from s in dc.servicehesaps
                                                  where s.iptal == false && (s.MusteriID == custid || s.tamirci_id == custid) && s.Onay_tarih >= tarih && s.Yekun > 0
                                                  select new CariDetayYeni
                                                  {
                                                      //müşteri hesabı için servis toplamlarını kullanacağım
                                                      //o yüzden burada hesaplara sıfır yazdım
                                                      MusteriID = (int)s.MusteriID,
                                                      aciklama = s.Aciklama,
                                                      musteriAdi = s.customer.Ad,
                                                      borc = s.tamirci_id == custid ? s.toplam_maliyet : null,
                                                      //alacak = s.tamirci_id == musteriID ? 0 : s.Yekun,
                                                      alacak = null,
                                                      tarih = (DateTime)s.Onay_tarih,
                                                      islem = s.IslemParca,
                                                      konu = s.adet + " Adet" + s.cihaz_adi
                                                  };

            IEnumerable<CariDetayYeni> servis = from s in dc.services
                                                where s.iptal == false && (s.CustID == custid || s.usta_id == custid) && s.AcilmaZamani >= tarih && s.KapanmaZamani != null && s.service_faturas.Yekun > 0
                                                select new CariDetayYeni
                                                {
                                                    //hakedişin prim oranlarına göre hesaplanması gerek
                                                    //service faturasta triggerla yapılıyor
                                                    MusteriID = (int)s.CustID,
                                                    aciklama = s.Aciklama,
                                                    musteriAdi = s.customer.Ad,
                                                    borc = s.usta_id == custid ? (decimal)(s.service_faturas.toplam_fark) : 0,
                                                    alacak = s.usta_id == custid ? 0 : s.service_faturas.Yekun,
                                                    tarih = (DateTime)s.AcilmaZamani,
                                                    islem = s.Baslik,
                                                    konu = s.urun.Cinsi
                                                };
            IEnumerable<CariDetayYeni> odeme_tahsilat = from o in dc.musteriodemelers
                                                        where o.iptal == false && o.Musteri_ID == custid && o.OdemeTarih >= tarih
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

                                                        };



            IEnumerable<CariDetayYeni> internet_fatura = (from o in dc.faturas
                                                          where o.iptal == false && o.MusteriID == custid && (o.tur == "Fatura" || o.tur == "Devir") && o.sattis_tarih >= tarih
                                                          //orderby o.sattis_tarih descending
                                                          select new CariDetayYeni
                                                          {
                                                              MusteriID = (int)o.MusteriID,
                                                              aciklama = "Geçerlilik-" + o.bakiye,
                                                              musteriAdi = o.ad,
                                                              borc = 0,
                                                              alacak = o.tutar,
                                                              tarih = (DateTime)o.sattis_tarih,
                                                              islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
                                                              konu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",

                                                          });




            List<CariDetayYeni> detay = odeme_tahsilat.Union(servis).Union(internet_fatura).Union(hesaplar).OrderByDescending(x => x.tarih).ToList();
            e.detay = detay;

            return e;
        }
        //public extre ExtreBilgileri(int custid, int gun)
        //{
        //    extre e = new extre();
        //    carihesap h = dc.carihesaps.FirstOrDefault(x => x.MusteriID == custid);
        //    e.hesap = h;
        //    DateTime tarih = DateTime.Now.AddDays(-gun);

        //    string aralik = tarih.ToShortDateString() + "-" + DateTime.Now.ToShortDateString();
        //    e.aralik = aralik;
        //    customer musteriBilgileri = dc.customers.FirstOrDefault(x => x.CustID == custid);
        //    e.Ad = musteriBilgileri.Ad;
        //    e.tc = musteriBilgileri.TC;
        //    e.firma = musteriBilgileri.Firma;


        //    IEnumerable<CariDetayx> odeme_tahsilat = (from o in dc.musteriodemelers
        //                                              where o.iptal == false && o.Musteri_ID == custid && o.OdemeTarih >= tarih
        //                                              // orderby o.OdemeTarih descending
        //                                              select new CariDetayx
        //                                              {

        //                                                  aciklama = o.Aciklama,
        //                                                  tutar = o.OdemeMiktar,
        //                                                  maliyet = null,
        //                                                  tarih = o.OdemeTarih,
        //                                                  islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
        //                                                 (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
        //                                                  islem_turu = o.tahsilat_turu,
        //                                                  islem_adres = o.pos_id == null ?
        //                                                  (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
        //                                                  cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme
        //                                              });


        //    IEnumerable<CariDetayx> internet_fatura = (from o in dc.faturas

        //                                               where o.iptal == false && o.MusteriID == custid && (o.tur == "Fatura" || o.tur == "Devir") && o.sattis_tarih >= tarih
        //                                               //orderby o.sattis_tarih descending
        //                                               select new CariDetayx
        //                                               {

        //                                                   aciklama = o.adres,

        //                                                   tutar = o.tutar,
        //                                                   maliyet = null,
        //                                                   tarih = (DateTime)o.sattis_tarih,
        //                                                   islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
        //                                                   islem_turu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
        //                                                   islem_adres = "",
        //                                                   cesit = "fatura"
        //                                               });

        //    IEnumerable<CariDetayx> servis = from s in dc.services
        //                                     where s.iptal == false && (s.CustID == custid || s.usta_id == custid) && s.AcilmaZamani >= tarih && s.service_faturas.Yekun > 0
        //                                     select new CariDetayx
        //                                     {

        //                                         aciklama = s.Baslik,

        //                                         tutar = s.service_faturas.Yekun,
        //                                         maliyet = s.service_faturas.toplam_maliyet,
        //                                         tarih = s.AcilmaZamani,
        //                                         islem = String.IsNullOrEmpty(s.service_faturas.service_tur) ? "Servis": "Servis-Cihaz",
        //                                         islem_turu = s.service_tips.tip_ad,
        //                                         islem_adres = s.usta_id == null ? "Servis Toplamı" : " Servis Toplamı Usta" ,
        //                                         cesit = "servis"
        //                                     };


        //    IEnumerable<CariDetayx> hesaplar = from s in dc.servicehesaps
        //                                       where s.iptal == false && (s.MusteriID == custid || s.tamirci_id == custid) && s.Onay_tarih >= tarih && s.Yekun > 0
        //                                       select new CariDetayx
        //                                       {

        //                                           aciklama = s.Aciklama,

        //                                           tutar = s.Yekun,
        //                                           maliyet = s.toplam_maliyet,
        //                                           tarih = (DateTime)s.Onay_tarih,
        //                                           islem = s.IslemParca,
        //                                           islem_turu = s.cihaz_adi,
        //                                           islem_adres = s.tamirci_id == null ? s.adet.ToString() + "Adet" : "Dış servis",
        //                                           cesit = "karar"
        //                                       };

        //    List<CariDetayx> detay = odeme_tahsilat.Union(servis).Union(internet_fatura).Union(hesaplar).OrderByDescending(x => x.tarih).ToList();
        //    e.detay = detay;

        //    return e;
        //}
    }

    public class InternetFaturasi
    {
        public Baski_Gorunum Bilgiler { get; set; }
        public List<Kalem> Kalemler { get; set; }

    }

    public class extre
    {
        public List<CariDetayYeni> detay { get; set; }
        public carihesap hesap { get; set; }
        public string aralik { get; set; }
        public string Ad { get; set; }
        public string tc { get; set; }
        public string firma { get; set; }
        public string firmaAdi { get; set; }
        public string firmaAdres { get; set; }
        public string firmaTel { get; set; }
        public string firmaWeb { get; set; }

    }

    public class Fatura_No
    {
        public string seri { get; set; }
        public int no { get; set; }
        //public TeknikServis.Radius.adminliste lis { get; set; }
    }

    public class Baski_Gorunum
    {
        public int ID { get; set; }
        public decimal Tutar { get; set; }
        public decimal KDV { get; set; }
        public decimal OIV { get; set; }
        public decimal Yekun { get; set; }
        public string VD { get; set; }
        public string adres { get; set; }
        public string TC { get; set; }
        public string yaziIle { get; set; }
        public string isim { get; set; }
        public DateTime tarih { get; set; }
        public string fat_seri { get; set; }
        public int fat_no { get; set; }
        public string tur { get; set; }
        public string firma { get; set; }


        public string tahsilat_tipi { get; set; }
    }

    public class Pesin
    {
        public int id { get; set; }
        public string unvan { get; set; }
        public string vd { get; set; }
        public string tc { get; set; }
        public string adres { get; set; }
    }
    public class Makbuz_Gorunum
    {
        public string Musteri { get; set; }
        public string musteriTel { get; set; }
        public string musteriAdres { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string FirmaTelefon { get; set; }
        public string FirmaTam { get; set; }
        public string Tutar { get; set; }
        public string yaziile { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime Saat { get; set; }
        public string Aciklama { get; set; }
        public DateTime gecerlilik { get; set; }
        public string firma { get; set; }
        public string kullanici { get; set; }

    }
    public class Servis_Baslama
    {
        public string Musteri { get; set; }
        public string MusteriAdres { get; set; }
        public decimal toplam_tutar { get; set; }
        public string MusteriTel { get; set; }
        public string barkod { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string email { get; set; }
        public string FirmaTelefon { get; set; }
        public string FirmaTam { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime Saat { get; set; }
        public string Konu { get; set; }
        public string Aciklama { get; set; }
        public string firma { get; set; }
        public string musteri_urunu { get; set; }
        public string urun_kodu { get; set; }
        public string tip { get; set; }

        public string kimlik { get; set; }
        public string sartlar { get; set; }
        public List<Servis_Hesap> kararlar { get; set; }

    }

    public class Servis_Hesap
    {
        public string islem { get; set; }
        public string cihaz { get; set; }
        public int adet { get; set; }
        public decimal tutar { get; set; }
        public string aciklama { get; set; }
    }
    public class Kalem
    {
        public string cinsi { get; set; }
        public int mik { get; set; }
        public decimal fiyat { get; set; }
        public decimal tutar { get; set; }
    }
}
