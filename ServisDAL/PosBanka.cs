using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class PosBanka
    {


        radiusEntities dc;
        public PosBanka(radiusEntities dc)
        {

            this.dc = dc;
        }

        //günü gelsin gelmesin bütün hesaplar


        //bütün birikenler
        //aktif extre
        //çekilenler
        public posozet Hesaplar(int? pos_id, bool? aktif)
        {
            posozet ozet = new posozet();
            List<posrepo> hesaplar = new List<posrepo>();
            if (pos_id == null)
            {
                //bütün poslar

                if (aktif == null)
                {
                    //bütün poslar
                    // bütün birikenler
                    hesaplar = (from h in dc.poshesaps
                                where h.iptal == false && h.cekildi == false
                                orderby h.extre_tarihi
                                select new posrepo
                                {
                                    pos_id = h.pos_id,
                                    posadi = h.pos_tanims.pos_adi,
                                    extre_tarihi = h.extre_tarihi,
                                    komisyon_tutar = h.komisyon_tutar,
                                    komsiyon_oran = h.komsiyon_oran,
                                    net_tutar = h.net_tutar,
                                    tahsilat_tutar = h.tahsilat_tutar,
                                    tahsilat_tarih = h.tahsilat_tarih,
                                    Aciklama = h.islem,
                                    Musteri_ID = h.customer.CustID,
                                    Musteri_Adi = h.customer.Ad,
                                    kullanici = h.inserted

                                }).ToList();
                }
                else
                {
                    //bütün poslar
                    //günü gelenler
                    hesaplar = (from h in dc.poshesaps
                                where h.iptal == false && h.cekildi == false && h.extre_tarihi <= DateTime.Now
                                orderby h.extre_tarihi
                                select new posrepo
                                {
                                    pos_id = h.pos_id,
                                    posadi = h.pos_tanims.pos_adi,
                                    extre_tarihi = h.extre_tarihi,
                                    komisyon_tutar = h.komisyon_tutar,
                                    komsiyon_oran = h.komsiyon_oran,
                                    net_tutar = h.net_tutar,
                                    tahsilat_tutar = h.tahsilat_tutar,
                                    tahsilat_tarih = h.tahsilat_tarih,
                                    Aciklama = h.islem,
                                    Musteri_ID = h.customer.CustID,
                                    Musteri_Adi = h.customer.Ad,
                                    kullanici = h.inserted

                                }).ToList();
                }
            }
            else
            {
                //pos_id
                if (aktif == null)
                {
                    //pos_id
                    // bütün birikenler
                    hesaplar = (from h in dc.poshesaps
                                where h.iptal == false && h.cekildi == false && h.pos_id == pos_id
                                orderby h.extre_tarihi
                                select new posrepo
                                {
                                    pos_id = h.pos_id,
                                    posadi = h.pos_tanims.pos_adi,
                                    extre_tarihi = h.extre_tarihi,
                                    komisyon_tutar = h.komisyon_tutar,
                                    komsiyon_oran = h.komsiyon_oran,
                                    net_tutar = h.net_tutar,
                                    tahsilat_tutar = h.tahsilat_tutar,
                                    tahsilat_tarih = h.tahsilat_tarih,
                                    Aciklama = h.islem,
                                    Musteri_ID = h.customer.CustID,
                                    Musteri_Adi = h.customer.Ad,
                                    kullanici = h.inserted

                                }).ToList();
                }
                else
                {
                    //pos_id
                    //günü gelenler

                    hesaplar = (from h in dc.poshesaps
                                where h.iptal == false && h.cekildi == false && h.pos_id == pos_id && h.extre_tarihi <= DateTime.Now
                                orderby h.extre_tarihi
                                select new posrepo
                                {
                                    pos_id = h.pos_id,
                                    posadi = h.pos_tanims.pos_adi,
                                    extre_tarihi = h.extre_tarihi,
                                    komisyon_tutar = h.komisyon_tutar,
                                    komsiyon_oran = h.komsiyon_oran,
                                    net_tutar = h.net_tutar,
                                    tahsilat_tutar = h.tahsilat_tutar,
                                    tahsilat_tarih = h.tahsilat_tarih,
                                    Aciklama = h.islem,
                                    Musteri_ID = h.customer.CustID,
                                    Musteri_Adi = h.customer.Ad,
                                    kullanici = h.inserted

                                }).ToList();
                }
            }
            int adet = hesaplar.Count;
            decimal tahsilat = 0;
            decimal net = 0;
            decimal kom = 0;
            if (adet > 0)
            {
                tahsilat = hesaplar.Sum(x => x.tahsilat_tutar);
                net = hesaplar.Sum(x => x.net_tutar);
                kom = hesaplar.Sum(x => x.komisyon_tutar);
            }
            ozet.poshesaplar = hesaplar;
            ozet.adet = adet;
            ozet.komisyon_tutar = kom;
            ozet.net_tutar = net;
            ozet.tahsilat_tutar = tahsilat;

            return ozet;
        }

        public posozet HesaplarCekilen(int? pos_id)
        {
            posozet ozet = new posozet();
            List<posrepo> hesaplar = new List<posrepo>();
            if (pos_id == null)
            {
                //çekilenlerin hepsi

                hesaplar = (from h in dc.poshesaps
                            where h.iptal == false && h.cekildi == true
                            orderby h.extre_tarihi
                            select new posrepo
                            {
                                pos_id = h.pos_id,
                                posadi = h.pos_tanims.pos_adi,
                                extre_tarihi = h.extre_tarihi,
                                komisyon_tutar = h.komisyon_tutar,
                                komsiyon_oran = h.komsiyon_oran,
                                net_tutar = h.net_tutar,
                                tahsilat_tutar = h.tahsilat_tutar,
                                tahsilat_tarih = h.tahsilat_tarih,
                                Aciklama = h.islem,
                                Musteri_ID = h.customer.CustID,
                                Musteri_Adi = h.customer.Ad,
                                kullanici = h.inserted

                            }).ToList();
            }
            else
            {
                //pos_id çekilen
                hesaplar = (from h in dc.poshesaps
                            where h.iptal == false && h.cekildi == true && h.pos_id == pos_id
                            orderby h.extre_tarihi
                            select new posrepo
                            {
                                pos_id = h.pos_id,
                                posadi = h.pos_tanims.pos_adi,
                                extre_tarihi = h.extre_tarihi,
                                komisyon_tutar = h.komisyon_tutar,
                                komsiyon_oran = h.komsiyon_oran,
                                net_tutar = h.net_tutar,
                                tahsilat_tutar = h.tahsilat_tutar,
                                tahsilat_tarih = h.tahsilat_tarih,
                                Aciklama = h.islem,
                                Musteri_ID = h.customer.CustID,
                                Musteri_Adi = h.customer.Ad,
                                kullanici = h.inserted

                            }).ToList();
            }
            int adet = hesaplar.Count;
            decimal tahsilat = 0;
            decimal net = 0;
            decimal kom = 0;
            if (adet > 0)
            {
                tahsilat = hesaplar.Sum(x => x.tahsilat_tutar);
                net = hesaplar.Sum(x => x.net_tutar);
                kom = hesaplar.Sum(x => x.komisyon_tutar);
            }
            ozet.poshesaplar = hesaplar;
            ozet.adet = adet;
            ozet.komisyon_tutar = kom;
            ozet.net_tutar = net;
            ozet.tahsilat_tutar = tahsilat;
            return ozet;
        }
        public List<pos_tanims> PosListe()
        {
            return dc.pos_tanims.ToList();
        }
        public void Transfer(int pos_id, string kullanici)
        {
            //birikenlerden günü gelenler
            List<poshesap> birikmis = (from h in dc.poshesaps
                                       where h.iptal == false && h.cekildi == false && h.pos_id == pos_id && h.extre_tarihi <= DateTime.Now
                                       select h).ToList();
            foreach (poshesap hesap in birikmis)
            {
                hesap.cekildi = true;
                hesap.updated = kullanici;
            }
            KaydetmeIslemleri.kaydetR(dc);

        }

        public List<banka> bankalar()
        {
            return dc.bankas.ToList();
        }
        public void YeniBanka(string ad, string aciklama, decimal bakiye)
        {
            banka yeni = new banka();
            yeni.cikis = 0;
            yeni.aciklama = aciklama;
            yeni.bakiye = bakiye;
            yeni.banka_adi = ad;
            yeni.Firma = "firma";
            yeni.giris = bakiye;

            dc.bankas.Add(yeni);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void YeniPos(string ad, int sure, decimal komisyon, int banka_id)
        {
            pos_tanims pos = new pos_tanims();
            pos.banka_id = banka_id;
            pos.Firma = "firma";

            pos.pos_adi = ad;
            pos.ikinci_komisyon = komisyon;
            pos.ikinci_sure = sure;
            dc.pos_tanims.Add(pos);
            KaydetmeIslemleri.kaydetR(dc);

        }
        public void kasa_transfer(decimal miktar, int banka_id)
        {
            banka b = dc.bankas.FirstOrDefault(x => x.banka_id == banka_id);
            if (b != null)
            {
                //aktif kasamız
                kasahesap h = dc.kasahesaps.FirstOrDefault(x => x.KasaTur == "Nakit");
                if (h != null)
                {
                    kasahareket k = new kasahareket();
                    k.aktif_bakiye = h.ToplamBakiye + miktar;
                    k.cikis = 0;
                    k.Firma = "firma";
                    k.giris = miktar;
                    k.iptal = false;
                    k.islem = "Bankadan çekildi";
                    k.KasaTur = "Nakit";

                    k.Musteri_ID = -1;
                    k.Odeme_ID = null;

                    k.tarih = DateTime.Now;
                    dc.kasaharekets.Add(k);


                    b.cikis += miktar;
                    b.bakiye -= miktar;

                    h.ToplamGiris += miktar;
                    h.ToplamBakiye += miktar;

                    KaydetmeIslemleri.kaydetR(dc);
                }
            }
        }


    }
    public class posrepo
    {
        public int pos_id { get; set; }
        public string posadi { get; set; }
        public decimal tahsilat_tutar { get; set; }
        public decimal komsiyon_oran { get; set; }
        public decimal komisyon_tutar { get; set; }
        public decimal net_tutar { get; set; }
        public System.DateTime extre_tarihi { get; set; }
        public bool cekildi { get; set; }
        public System.DateTime tahsilat_tarih { get; set; }
        public int Musteri_ID { get; set; }
        public string Aciklama { get; set; }
        public string Musteri_Adi { get; set; }
        public string kullanici { get; set; }
    }
    public class posozet
    {
        public int adet { get; set; }
        public decimal tahsilat_tutar { get; set; }
        public decimal net_tutar { get; set; }
        public decimal komisyon_tutar { get; set; }
        public List<posrepo> poshesaplar { get; set; }
    }
}
