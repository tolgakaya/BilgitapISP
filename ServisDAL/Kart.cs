using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class Kart
    {
        radiusEntities dc;
        public Kart(radiusEntities dc)
        {
            this.dc = dc;
        }

        public kartozet2 ExtreGorunumu(string kartimiz, DateTime baslangic, DateTime son)
        {
            //buradaki tarih aralığı extre-yani son ödeme tarihi 
            //aynısı ödeme tarihine görede gösterilebilir

            kartozet2 ozet = new kartozet2();
            List<kartrepo> hesaplar = new List<kartrepo>();
            kart_cari_rapo cari = new kart_cari_rapo();

            int adet = 0;
            decimal taksit_tutar = 0;
            decimal odeme_tutar = 0;
            if (String.IsNullOrEmpty(kartimiz))
            {
                hesaplar = (from h in dc.kart_hesaps
                            where h.iptal == false && h.extre_tarih >= baslangic && h.extre_tarih <= son
                            select new kartrepo
                            {
                                kart_id = h.kart_id,
                                kart_adi = h.kart_tanims.kart_adi,
                                extre_tarih = h.extre_tarih,
                                tarih = (DateTime)h.tarih,
                                tutar = h.tutar,
                                toplam_tutar = h.toplam_tutar,
                                taksit_sayisi = h.aciklama,
                                Aciklama = h.islem,
                                Musteri_Adi = h.customer.Ad,
                                cekildi = h.cekildi,
                                kullanici = h.inserted
                            }).ToList();

                //burda cariye gerek yok
                adet = hesaplar.Count;
                if (adet > 0)
                {
                    taksit_tutar = hesaplar.Sum(x => x.tutar);
                    odeme_tutar = hesaplar.Sum(x => x.toplam_tutar);
                }
                cari.kart_adi = "BÜTÜN KARTLAR";
                cari.bakiye = 0;
            }
            else
            {
                int kart_id = Int32.Parse(kartimiz);

                hesaplar = (from h in dc.kart_hesaps
                            where h.iptal == false && h.kart_id == kart_id && h.extre_tarih >= baslangic && h.extre_tarih <= son
                            select new kartrepo
                            {
                                kart_id = h.kart_id,
                                kart_adi = h.kart_tanims.kart_adi,
                                extre_tarih = h.extre_tarih,
                                tarih = (DateTime)h.tarih,
                                tutar = h.tutar,
                                toplam_tutar = h.toplam_tutar,
                                taksit_sayisi = h.aciklama,
                                Aciklama = h.islem,
                                Musteri_Adi = h.customer.Ad,
                                cekildi = h.cekildi,
                                kullanici = h.inserted
                            }).ToList();

                adet = hesaplar.Count;
                if (adet > 0)
                {
                    taksit_tutar = hesaplar.Sum(x => x.tutar);
                    odeme_tutar = hesaplar.Sum(x => x.toplam_tutar);
                }

                cari = (from c in dc.kart_caris
                        where c.kart_id == kart_id
                        select new kart_cari_rapo
                        {
                            kart_adi = c.kart_adi,
                            borc = c.borc,
                            odenen = c.odenen,
                            bakiye = c.bakiye

                        }).FirstOrDefault();


            }
            ozet.adet = adet;
            ozet.hesaplar = hesaplar;
            ozet.odeme_tutar = odeme_tutar;
            ozet.cari = cari;
            ozet.tutar = taksit_tutar;

            return ozet;
        }


        public kartozet Birikenler(int? kart_id, bool? cekildi)
        {
            kartozet ozet = new kartozet();
            List<kartrepo> hesaplar = new List<kartrepo>();
            if (kart_id == null)
            {
                // kart önemli değil
                if (cekildi == null)
                {
                    //kart da çekilme de önemli değil
                    hesaplar = (from h in dc.kart_hesaps
                                where h.iptal == false && h.extre_tarih <= h.kart_tanims.extre_tarih
                                select new kartrepo
                                {
                                    kart_id = h.kart_id,
                                    kart_adi = h.kart_tanims.kart_adi,
                                    extre_tarih = h.extre_tarih,
                                    tarih = (DateTime)h.tarih,
                                    tutar = h.tutar,
                                    Aciklama = h.islem,
                                    Musteri_Adi = h.customer.Ad,
                                    cekildi = h.cekildi,
                                    kullanici = h.inserted
                                }).ToList();
                }
                else
                {
                    //sadece çekilmeye göre
                    hesaplar = (from h in dc.kart_hesaps
                                where h.iptal == false && h.cekildi == cekildi && h.extre_tarih <= h.kart_tanims.extre_tarih
                                select new kartrepo
                                {
                                    kart_id = h.kart_id,
                                    kart_adi = h.kart_tanims.kart_adi,
                                    extre_tarih = h.extre_tarih,
                                    tarih = (DateTime)h.tarih,
                                    tutar = h.tutar,
                                    Aciklama = h.islem,
                                    Musteri_Adi = h.customer.Ad,
                                    cekildi = h.cekildi,
                                    kullanici = h.inserted
                                }).ToList();
                }
            }
            else
            {

                if (cekildi == null)
                {
                    //sadece kart_idye göre
                    hesaplar = (from h in dc.kart_hesaps
                                where h.iptal == false && h.kart_id == kart_id && h.extre_tarih <= h.kart_tanims.extre_tarih
                                select new kartrepo
                                {
                                    kart_id = h.kart_id,
                                    kart_adi = h.kart_tanims.kart_adi,
                                    extre_tarih = h.extre_tarih,
                                    tarih = (DateTime)h.tarih,
                                    tutar = h.tutar,
                                    Aciklama = h.islem,
                                    Musteri_Adi = h.customer.Ad,
                                    cekildi = h.cekildi,
                                    kullanici = h.inserted
                                }).ToList();
                }
                else
                {
                    //kart_id ve çekilmeye göre
                    hesaplar = (from h in dc.kart_hesaps
                                where h.iptal == false && h.kart_id == kart_id && h.cekildi == cekildi && h.extre_tarih <= h.kart_tanims.extre_tarih
                                select new kartrepo
                                {
                                    kart_id = h.kart_id,
                                    kart_adi = h.kart_tanims.kart_adi,
                                    extre_tarih = h.extre_tarih,
                                    tarih = (DateTime)h.tarih,
                                    tutar = h.tutar,
                                    Aciklama = h.islem,
                                    Musteri_Adi = h.customer.Ad,
                                    cekildi = h.cekildi,
                                    kullanici = h.inserted
                                }).ToList();
                }
            }
            int adet = hesaplar.Count;
            decimal tutar = hesaplar.Sum(x => x.tutar);

            ozet.adet = adet;
            ozet.tutar = tutar;
            ozet.hesaplar = hesaplar;
            return ozet;
        }



        private ExtreRepo Extre(int kart_id)
        {
            // kart ödemesini yapsak bile ödeme iptalini nasıl yapacaz. kasa vb işlemler zaten otomatik yapılıyor ancak
            // kart hesaplarındaki ptalleri düzeltmek gerek.
            //odeme olarak kaydetcez bununiçin sahte bir müşteri oluşturalım ve -1 idsi olsun
            ExtreRepo repo = new ExtreRepo();

            List<kart_hesaps> hesaplar = (from h in dc.kart_hesaps
                                          where h.iptal == false && h.kart_id == kart_id && h.cekildi == false && h.extre_tarih <= h.kart_tanims.extre_tarih
                                          select h).ToList();

            //devreden bakiye var mı bakalım
            kart_tanims tanim = dc.kart_tanims.Find(kart_id);


            decimal tutar = hesaplar.Sum(x => x.tutar) + tanim.devreden_bakiye;
            repo.hesaplar = hesaplar;
            repo.extre_tarih = hesaplar.Select(x => x.extre_tarih).FirstOrDefault();
            repo.tutar = tutar;


            return repo;
        }

        // extre_tarihi güncellemesini kontrol et. her seferinde bir ay eklemesi doğru değil. iki aydır kullanılmıyorsa ne olacak.
        public void ExtreOde(int kart_id, string tur, int? taksit_sayi, int? yeni_kart_id, int? banka_id,string kullanici)
        {

            ExtreRepo repo = Extre(kart_id);
            if (repo.hesaplar.Count > 0)
            {
                //kart devreden bakiye varsa sıfırlayalım //triggerda yapıyoruz
                foreach (kart_hesaps hesap in repo.hesaplar)
                {
                    hesap.cekildi = true;

                }
                Odeme o = new Odeme(dc);
                o.OdemeMiktar = repo.tutar;
                o.OdemeTarih = DateTime.Now;
                o.Musteri_ID = -1;
                o.KullaniciID = "-";
                o.kullanici = "-";
                o.Aciklama = "Kart extre ödemesi";
                o.extre_tarih = (DateTime)repo.extre_tarih;

                if (tur.Equals("Nakit"))
                {
                    o.Nakit(kullanici);
                    KaydetmeIslemleri.kaydetR(dc);
                }
                else if (tur.Equals("Banka"))
                {
                    int id = (int)banka_id;
                    o.Banka(id,kullanici);
                    KaydetmeIslemleri.kaydetR(dc);
                }
                else if (tur.Equals("Kart"))
                {
                    int id = (int)yeni_kart_id;
                    int taksit = (int)taksit_sayi;
                    o.Kart(taksit, id, false,kullanici);
                    KaydetmeIslemleri.kaydetR(dc);
                }

            }

        }

        public void Yeni(string kart_adi, DateTime extre_tarih, string aciklama)
        {
            kart_tanims k = new kart_tanims();
            k.kart_adi = kart_adi;
            k.aciklama = aciklama;
            k.devreden_bakiye = 0;
            k.extre_tarih = extre_tarih;
            k.Firma = "firma";

            dc.kart_tanims.Add(k);
            KaydetmeIslemleri.kaydetR(dc);
        }

        public void Guncelle(string kart_adi, DateTime extre_tarih, string aciklama, int kart_id)
        {
            kart_tanims k = dc.kart_tanims.FirstOrDefault(x => x.kart_id == kart_id);
            if (k != null)
            {
                k.kart_adi = kart_adi;
                k.aciklama = aciklama;
                k.extre_tarih = extre_tarih;
                k.devreden_bakiye = 0;
                k.Firma = "firma";

                //kart carisini güncelleyelim
                //burada devreden bakiye yerine aktif kart cari bakiye kullanılıyor.
                //böylece yönetici elle kart bakiyesini değiştirebilir.

                //kart_caris c = dc.kart_caris.FirstOrDefault(x => x.kart_id == kart_id);

                //c.bakiye = devreden_bakiye;

                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        public void CariGuncelle(decimal bakiye, int kart_id)
        {

            kart_caris cs = dc.kart_caris.FirstOrDefault(x => x.kart_id == kart_id);

            decimal simdikiBakiye = cs.bakiye;
            decimal simdikiBorc = cs.borc;
            decimal simdikiOdenen = cs.odenen;

            decimal girilecek = 0;
            decimal cikilacak = 0;
            if (bakiye > simdikiBakiye)
            {
                //yeni giriş yapılacak
                decimal fark = bakiye - simdikiBakiye;
                girilecek = fark;
            }
            else if (bakiye < simdikiBakiye)
            {
                //çıkış yapılacak
                decimal fark = simdikiBakiye - bakiye;
                cikilacak = fark;
            }


            cs.bakiye = bakiye;
            cs.odenen += cikilacak;
            cs.borc += girilecek;

            KaydetmeIslemleri.kaydetR(dc);

        }
        public List<kart_list> KartListe()
        {
            //return dc.kart_tanims.Where(x => x.Firma == firma && x.owner == owner && x.kart_id > -1).ToList();
            return (from k in dc.kart_tanims
                    where k.kart_id != -1
                    select new kart_list
                    {
                        kart_id = k.kart_id,
                        kart_adi = k.kart_adi,
                        extre_tarih = k.extre_tarih,
                        aciklama = k.aciklama,
                        bakiye = k.kart_caris.bakiye
                    }).ToList();


        }

        //karta yapılan Ödemeler
        public kart_hesap_ozet KartaYapilanOdemeler(int kart_id, string baslangics, string sons)
        {
            kart_hesap_ozet ozet = new kart_hesap_ozet();

            DateTime son = DateTime.Now;
            DateTime baslangic = DateTime.Now.AddDays(-30);

            if (!String.IsNullOrEmpty(sons))
            {
                son = DateTime.Parse(sons);
            }

            if (!String.IsNullOrEmpty(baslangics))
            {
                baslangic = DateTime.Parse(baslangics);
            }

            List<karta_yapilan_repo> odemelerimiz = (from c in dc.kart_odemes
                                                     where c.kart_id == kart_id && c.iptal == false && c.tarih >= baslangic && c.tarih <= son
                                                     select new karta_yapilan_repo
                                                     {
                                                         id = c.id,
                                                         kart_adi = c.kart_tanims.kart_adi,
                                                         kart_id = c.kart_id,
                                                         tarih = c.tarih,
                                                         aciklama = c.aciklama,
                                                         tutar = c.tutar,
                                                         kullanici=c.inserted
                                                     }).ToList();

            int sayi = odemelerimiz.Count;
            decimal toplam = 0;
            if (sayi > 0)
            {
                toplam = odemelerimiz.Sum(x => x.tutar);
            }

            kart_caris cari = dc.kart_caris.FirstOrDefault(x => x.kart_id == kart_id);
            ozet.bakiye = cari.bakiye;
            ozet.borc = cari.borc;
            ozet.kart_adi = cari.kart_tanims.kart_adi;
            ozet.odenen = cari.odenen;
            ozet.adet = sayi;
            ozet.toplam = toplam;
            ozet.odemeler = odemelerimiz;

            return ozet;
        }


    }
    public class kart_hesap_ozet
    {
        public string kart_adi { get; set; }
        public decimal borc { get; set; }
        public decimal odenen { get; set; }
        public decimal bakiye { get; set; }
        public int adet { get; set; }
        public decimal toplam { get; set; }
        public List<karta_yapilan_repo> odemeler { get; set; }
    }
    public class karta_yapilan_repo
    {
        public int id { get; set; }
        public int kart_id { get; set; }
        public string kart_adi { get; set; }
        public decimal tutar { get; set; }
        public System.DateTime tarih { get; set; }
        public string aciklama { get; set; }
        public string kullanici { get; set; }
    }

    public class kart_list
    {
        public int kart_id { get; set; }
        public string kart_adi { get; set; }
        public string aciklama { get; set; }
        public DateTime extre_tarih { get; set; }
        public decimal bakiye { get; set; }

    }
    public class ExtreRepo
    {
        public DateTime extre_tarih { get; set; }
        public decimal tutar { get; set; }
        public List<kart_hesaps> hesaplar { get; set; }
    }

    public class kartrepo
    {
        public int kart_id { get; set; }
        public string kart_adi { get; set; }
        public System.DateTime tarih { get; set; }
        public System.DateTime extre_tarih { get; set; }
        public decimal tutar { get; set; }
        public string taksit_sayisi { get; set; }
        public decimal toplam_tutar { get; set; }
        public bool cekildi { get; set; }
        public string Musteri_Adi { get; set; }
        public string Aciklama { get; set; }
        public string kullanici { get; set; }

    }
    public class kartozet
    {
        public int adet { get; set; }
        public decimal tutar { get; set; }
        public decimal odeme_tutar { get; set; }
        public List<kartrepo> hesaplar { get; set; }

    }
    public class kart_cari_rapo
    {

        public string kart_adi { get; set; }
        public decimal borc { get; set; }
        public decimal odenen { get; set; }
        public decimal bakiye { get; set; }
    }
    public class kartozet2
    {
        public int adet { get; set; }
        public decimal tutar { get; set; }
        public decimal odeme_tutar { get; set; }
        public kart_cari_rapo cari { get; set; }
        public List<kartrepo> hesaplar { get; set; }

    }


}
