using ServisDAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class GelirGider
    {

        radiusEntities dc;
        public GelirGider(radiusEntities dc)
        {

            this.dc = dc;
        }

        public DateTime baslangic { get; set; }
        public DateTime son { get; set; }
        private List<GGR> odeme_raporu()
        {
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.masraf_id != null && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                                                        orderby o.OdemeTarih ascending
                                                        select new rapor_kalemi
                                                      {
                                                          MusteriID = o.Musteri_ID,
                                                          aciklama = o.Aciklama,
                                                          musteriAdi = o.customer.Ad,
                                                          tutarr = o.OdemeMiktar,
                                                          tarih = o.OdemeTarih,
                                                          islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                         (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                          islem_turu = o.tahsilat_turu,
                                                          islem_adres = o.pos_id == null ?
                                                          (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                          cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                          //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                          masraf_tipi = o.masraf_tipi,
                                                          masraf_id = (int)o.masraf_id
                                                      });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_id into g
                                select new GGR
                                {
                                    grup_id = g.Key,
                                    islem_adet = g.Count(),
                                    grup_adi = g.FirstOrDefault(x => x.masraf_id == g.Key).masraf_tipi,
                                    kalemler = g.Where(x => x.masraf_id == g.Key).ToList(),
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }
        private List<GGR> odeme_raporu_genel()
        {
            //burada yalnızca genel masraflar var- normal satınalma ödemeleri yok
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.masraf_id != null && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                                                        // orderby o.OdemeTarih descending
                                                        select new rapor_kalemi
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            tutarr = o.OdemeMiktar,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                           (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                            islem_turu = o.tahsilat_turu,
                                                            islem_adres = o.pos_id == null ?
                                                            (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                            cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                            //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                            masraf_tipi = o.masraf_tipi,
                                                            masraf_id = (int)o.masraf_id
                                                        });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_id into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    islem_adet = g.Count(),
                                    grup_adi = g.FirstOrDefault(x => x.masraf_id == g.Key).masraf_tipi,
                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }

        private List<GGR> odeme_raporu_genel(DateTime bas, DateTime sonu)
        {
            //burada yalnızca genel masraflar var- normal satınalma ödemeleri yok
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.masraf_id != null && o.OdemeTarih >= bas && o.OdemeTarih <= sonu
                                                        // orderby o.OdemeTarih descending
                                                        select new rapor_kalemi
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            tutarr = o.OdemeMiktar,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                           (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                            islem_turu = o.tahsilat_turu,
                                                            islem_adres = o.pos_id == null ?
                                                            (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                            cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                            //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                            masraf_tipi = o.masraf_tipi,
                                                            masraf_id = (int)o.masraf_id
                                                        });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_id into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    islem_adet = g.Count(),
                                    grup_adi = g.FirstOrDefault(x => x.masraf_id == g.Key).masraf_tipi,
                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }

        public wrapper gonder(string tip)
        {
            wrapper w = new wrapper();
            //w.firma = this.firma;
            w.baslangic = this.baslangic;
            w.son = this.son;

            if (tip.Equals("odeme"))
            {
                w.liste = odeme_raporu();
                w.tip = "ÖDEME RAPORU";

            }
            else if (tip.Equals("tahsilat"))
            {
                w.liste = tahsilat_raporu();
                w.tip = "TAHSİLAT RAPORU";
            }
            else if (tip.Equals("satis"))
            {
                w.liste = satis_raporu();
                w.tip = "SATIŞ RAPORU";
            }
            return w;
        }
        //burası
        public wrapper_genel_gruplu gonder_gruplu(string tip)
        {
            wrapper_genel_gruplu w = new wrapper_genel_gruplu();
            //w.firma = this.firma;

            if (tip.Equals("odeme_tahsilat_gruplu"))
            {
                List<GGR> odemeler = odeme_raporu_genel();
                List<GGR> tahsilatlar = tahsilat_raporu_genel();
                List<GGR2> listeler = new List<GGR2>();

                int odeme_adet = odemeler.Count;
                decimal odeme_toplam = 0;
                decimal tahsilat_toplam = 0;
                int tahsilat_adet = tahsilatlar.Count;
                if (odeme_adet > 0)
                {
                    odeme_toplam = odemeler.Sum(x => x.grup_toplam);
                    odeme_adet = odemeler.Sum(x => x.islem_adet);
                }
                if (tahsilat_adet > 0)
                {
                    tahsilat_toplam = tahsilatlar.Sum(x => x.grup_toplam);
                    tahsilat_adet = tahsilatlar.Sum(x => x.islem_adet);
                }
                GGR2 list = new GGR2();
                list.grup_adii = "Ödemeler";
                list.grup_toplamm = odeme_toplam;
                list.islem_adett = odeme_adet;
                list.listeler = odemeler;
                listeler.Add(list);

                GGR2 list2 = new GGR2();
                list2.grup_adii = "Tahsilatlar";
                list2.grup_toplamm = tahsilat_toplam;
                list2.islem_adett = tahsilat_adet;
                list2.listeler = tahsilatlar;
                listeler.Add(list2);

                w.baslama = baslangic;
                w.liste = listeler;
                w.odeme_adet = odeme_adet;
                w.odeme_toplam = odeme_toplam;
                w.fark = tahsilat_toplam - odeme_toplam;
                w.son = son;
                w.tahsilat_adet = tahsilat_adet;
                w.tahsilat_toplam = tahsilat_toplam;


            }



            return w;
        }
        public wrapper_genel_gruplu gonder_gruplu_satisli(string tip)
        {
            wrapper_genel_gruplu w = new wrapper_genel_gruplu();
            //w.firma = this.firma;

            if (tip.Equals("odeme_tahsilat_satis"))
            {
                List<GGR> odemeler = odeme_raporu_genel();
                List<GGR> tahsilatlar = tahsilat_raporu_genel();
                List<GGR> satislar = satis_raporu_genel();
                List<GGR2> listeler = new List<GGR2>();

                int odeme_adet = odemeler.Count;
                int satis_adet = satislar.Count;
                int tahsilat_adet = tahsilatlar.Count;

                decimal odeme_toplam = 0;
                decimal tahsilat_toplam = 0;
                decimal satis_toplam = 0;

                if (odeme_adet > 0)
                {
                    odeme_toplam = odemeler.Sum(x => x.grup_toplam);
                    odeme_adet = odemeler.Sum(x => x.islem_adet);
                }
                if (tahsilat_adet > 0)
                {
                    tahsilat_toplam = tahsilatlar.Sum(x => x.grup_toplam);
                    tahsilat_adet = tahsilatlar.Sum(x => x.islem_adet);
                }
                if (satis_adet > 0)
                {
                    satis_toplam = satislar.Sum(x => x.grup_toplam);
                    satis_adet = satislar.Sum(x => x.islem_adet);
                }
                GGR2 list = new GGR2();
                list.grup_adii = "Ödemeler";
                list.grup_toplamm = odeme_toplam;
                list.islem_adett = odeme_adet;
                list.listeler = odemeler;
                listeler.Add(list);

                GGR2 list2 = new GGR2();
                list2.grup_adii = "Tahsilatlar";
                list2.grup_toplamm = tahsilat_toplam;
                list2.islem_adett = tahsilat_adet;
                list2.listeler = tahsilatlar;
                listeler.Add(list2);

                GGR2 list3 = new GGR2();
                list3.grup_adii = "Satışlar";
                list3.grup_toplamm = satis_toplam;
                list3.islem_adett = satis_adet;
                list3.listeler = satislar;
                listeler.Add(list3);

                w.baslama = baslangic;
                w.satis_adet = satis_adet;
                w.satis_toplam = satis_toplam;
                w.liste = listeler;
                w.odeme_adet = odeme_adet;
                w.odeme_toplam = odeme_toplam;
                w.fark = tahsilat_toplam - odeme_toplam;
                w.son = son;
                w.tahsilat_adet = tahsilat_adet;
                w.tahsilat_toplam = tahsilat_toplam;


            }



            return w;
        }
        public wrapper_genel_gruplu gonder_gruplu_ayli(DateTime bas, DateTime sonu)
        {
            wrapper_genel_gruplu w = new wrapper_genel_gruplu();
            //w.firma = this.firma;

            List<GGR> odemeler = odeme_raporu_genel(bas, sonu);
            List<GGR> tahsilatlar = tahsilat_raporu_genel(bas, sonu);
            List<GGR> satislar = satis_raporu_genel(bas, sonu);
            List<GGR2> listeler = new List<GGR2>();

            int odeme_adet = odemeler.Count;
            int satis_adet = satislar.Count;
            int tahsilat_adet = tahsilatlar.Count;

            decimal odeme_toplam = 0;
            decimal tahsilat_toplam = 0;
            decimal satis_toplam = 0;

            if (odeme_adet > 0)
            {
                odeme_toplam = odemeler.Sum(x => x.grup_toplam);
                odeme_adet = odemeler.Sum(x => x.islem_adet);
            }
            if (tahsilat_adet > 0)
            {
                tahsilat_toplam = tahsilatlar.Sum(x => x.grup_toplam);
                tahsilat_adet = tahsilatlar.Sum(x => x.islem_adet);
            }
            if (satis_adet > 0)
            {
                satis_toplam = satislar.Sum(x => x.grup_toplam);
                satis_adet = satislar.Sum(x => x.islem_adet);
            }
            GGR2 list = new GGR2();
            list.grup_adii = "Ödemeler";
            list.grup_toplamm = odeme_toplam;
            list.islem_adett = odeme_adet;
            list.listeler = odemeler;
            listeler.Add(list);

            GGR2 list2 = new GGR2();
            list2.grup_adii = "Tahsilatlar";
            list2.grup_toplamm = tahsilat_toplam;
            list2.islem_adett = tahsilat_adet;
            list2.listeler = tahsilatlar;
            listeler.Add(list2);

            GGR2 list3 = new GGR2();
            list3.grup_adii = "Satışlar";
            list3.grup_toplamm = satis_toplam;
            list3.islem_adett = satis_adet;
            list3.listeler = satislar;
            listeler.Add(list3);

            w.baslama = baslangic;
            w.satis_adet = satis_adet;
            w.satis_toplam = satis_toplam;
            w.liste = listeler;
            w.odeme_adet = odeme_adet;
            w.odeme_toplam = odeme_toplam;
            w.fark = tahsilat_toplam - odeme_toplam;
            w.son = son;
            w.tahsilat_adet = tahsilat_adet;
            w.tahsilat_toplam = tahsilat_toplam;

            return w;
        }
        public wrapper_genel gonder_genel(string tip)
        {
            wrapper_genel w = new wrapper_genel();
            //w.firma = this.firma;

            w.baslama = this.baslangic;
            w.son = this.son;
            if (tip.Equals("odeme_tahsilat"))
            {
                List<GGR> odemeler = odeme_raporu_genel();

                w.odeme_adet = odemeler.Sum(x => x.islem_adet);
                w.odeme_toplam = odemeler.Sum(x => x.grup_toplam);

                List<GGR> tahsilatlar = tahsilat_raporu_genel();

                w.tahsilat_adet = tahsilatlar.Sum(x => x.islem_adet);
                w.tahsilat_toplam = tahsilatlar.Sum(x => x.grup_toplam);
                w.liste = odemeler.Union(tahsilatlar).ToList();
            }


            return w;
        }

        private List<GGR> tahsilat_raporu()
        {
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.tahsilat_odeme == "tahsilat" && o.tahsilat_turu != "cari" && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                                                        orderby o.OdemeTarih ascending
                                                        select new rapor_kalemi
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            tutarr = o.OdemeMiktar,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                           (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                            islem_turu = o.tahsilat_turu,
                                                            islem_adres = o.pos_id == null ?
                                                            (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                            cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                            //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                            masraf_tipi = o.masraf_tipi,
                                                            masraf_id = (int)o.masraf_id
                                                        });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),

                                    kalemler = g.Where(x => x.masraf_tipi == g.Key).ToList(),
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }
        private List<GGR> tahsilat_raporu_genel()
        {
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.tahsilat_odeme == "tahsilat" && o.tahsilat_turu != "cari" && o.OdemeTarih >= baslangic && o.OdemeTarih <= son
                                                        orderby o.OdemeTarih ascending
                                                        select new rapor_kalemi
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            tutarr = o.OdemeMiktar,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                           (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                            islem_turu = o.tahsilat_turu,
                                                            islem_adres = o.pos_id == null ?
                                                            (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                            cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                            //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                            masraf_tipi = o.masraf_tipi == null ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : "Borç") : o.masraf_tipi,
                                                            masraf_id = (int)o.masraf_id
                                                        });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),

                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }
        private List<GGR> tahsilat_raporu_genel(DateTime bas, DateTime sonu)
        {
            IEnumerable<rapor_kalemi> odeme_tahsilat = (from o in dc.musteriodemelers
                                                        where o.iptal == false && o.tahsilat_odeme == "tahsilat" && o.tahsilat_turu != "cari" && o.OdemeTarih >= bas && o.OdemeTarih <= sonu
                                                        orderby o.OdemeTarih ascending
                                                        select new rapor_kalemi
                                                        {
                                                            MusteriID = o.Musteri_ID,
                                                            aciklama = o.Aciklama,
                                                            musteriAdi = o.customer.Ad,
                                                            tutarr = o.OdemeMiktar,
                                                            tarih = o.OdemeTarih,
                                                            islem = o.tahsilat_odeme == "tahsilat" ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Alındı" : o.tahsilat_odeme)) :
                                                           (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : (o.tahsilat_turu == "borc" ? "Borç Verildi" : o.tahsilat_odeme)),
                                                            islem_turu = o.tahsilat_turu,
                                                            islem_adres = o.pos_id == null ?
                                                            (o.banka_id == null ? (o.kart_id == null ? "-" : o.kart_tanims.kart_adi) : o.banka.banka_adi) : o.pos_tanims.pos_adi,
                                                            cesit = o.tahsilat_turu == "iade" ? "iade" : o.tahsilat_odeme,
                                                            //masraf_tipi = o.masraf_id == null ? "Standart" : o.masraf_tipi,
                                                            masraf_tipi = o.masraf_tipi == null ? (o.tahsilat_turu == "iade" ? "Cihaz İadesi" : "Borç") : o.masraf_tipi,

                                                            masraf_id = (int)o.masraf_id
                                                        });

            List<GGR> gruplu = (from u in odeme_tahsilat
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),

                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();

            return gruplu;
        }

        private List<GGR> satis_raporu_genel()
        {


            IEnumerable<CariDetayx> internet_fatura = (from o in dc.faturas
                                                       where o.iptal == false && (o.tur == "Fatura") && o.sattis_tarih >= baslangic && o.sattis_tarih <= son
                                                       orderby o.sattis_tarih ascending
                                                       select new CariDetayx
                                                       {
                                                           MusteriID = 1,
                                                           aciklama = o.adres,
                                                           musteriAdi = o.ad,
                                                           tutar = o.tutar,
                                                           tarih = DateTime.Now,
                                                           islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
                                                           islem_turu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
                                                           islem_adres = "",
                                                           cesit = "fatura",
                                                           masraf_tipi = "internet"

                                                       });

            IEnumerable<CariDetayx> hesaplar = from s in dc.servicehesaps
                                               where s.iptal == false && s.Onay_tarih >= baslangic && s.Onay_tarih <= son && s.Yekun > 0
                                               select new CariDetayx
                                               {
                                                   MusteriID = 1,
                                                   aciklama = s.Aciklama,
                                                   musteriAdi = s.customer.Ad,
                                                   tutar = s.Yekun,
                                                   tarih = DateTime.Now,
                                                   islem = s.IslemParca,
                                                   islem_turu = s.cihaz_adi,
                                                   islem_adres = s.adet.ToString() + "Adet",
                                                   cesit = "karar",
                                                   masraf_tipi = s.cihaz_id == null ? "Servis" : "Cihaz"

                                               };
            IEnumerable<CariDetayx> satis = from s in dc.satislars
                                            where s.iptal == false && s.tarih >= baslangic && s.tarih <= son && s.yekun > 0
                                            select new CariDetayx
                                            {
                                                MusteriID = 1,
                                                aciklama = "Peşin satış",
                                                musteriAdi = s.customer.Ad,
                                                tutar = s.yekun,
                                                tarih = s.tarih,
                                                islem = "Peşin Satış",
                                                islem_turu = s.cihaz.cihaz_adi,
                                                islem_adres = s.adet.ToString() + "Adet",
                                                cesit = "pesin",
                                                masraf_tipi = s.cihaz_id == null ? "Servis" : "Ürün/Parça"

                                            };
            IEnumerable<CariDetayx> ggg = internet_fatura.Union(hesaplar).Union(satis);

            List<GGR> gruplu = (from u in ggg
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),
                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutar)
                                }).ToList();

            return gruplu;
        }

        private List<GGR> satis_raporu()
        {

            //IEnumerable<rapor_kalemi> internet_fatura = (from o in dc.faturas
            //                                             where o.iptal == false && (o.tur == "Fatura") && o.sattis_tarih >= baslangic && o.sattis_tarih <= son
            //                                             orderby o.sattis_tarih ascending
            //                                             select new rapor_kalemi
            //                                           {
            //                                               MusteriID = 1,
            //                                               aciklama = "",
            //                                               musteriAdi = o.ad,
            //                                               tutarr = o.tutar,
            //                                               tarih =(DateTime)o.sattis_tarih,
            //                                               //tarih = o.islem_tarihi == null ? (DateTime)o.sattis_tarih : (DateTime)o.islem_tarihi,
            //                                               islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
            //                                               islem_turu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
            //                                               islem_adres = "",
            //                                               cesit = "fatura",
            //                                               masraf_tipi = "internet"

            //                                           });

            IEnumerable<rapor_kalemi> hesaplar = from s in dc.servicehesaps
                                                 where s.iptal == false && s.Onay_tarih >= baslangic && s.Onay_tarih <= son && s.Yekun > 0
                                                 select new rapor_kalemi
                                               {
                                                   MusteriID = 1,
                                                   aciklama = s.Aciklama,
                                                   musteriAdi = s.customer.Ad,
                                                   tutarr = s.Yekun,
                                                   tarih = s.TarihZaman,
                                                   islem = s.IslemParca,
                                                   islem_turu = s.cihaz_adi,
                                                   islem_adres = s.adet.ToString() + "Adet",
                                                   cesit = "karar",
                                                   masraf_tipi = s.cihaz_id == null ? "Servis" : "Cihaz"

                                               };
            IEnumerable<rapor_kalemi> satis = from s in dc.satislars
                                              where s.iptal == false && s.tarih >= baslangic && s.tarih <= son && s.yekun > 0
                                              select new rapor_kalemi
                                            {
                                                MusteriID = 1,
                                                aciklama = "Peşin satış",
                                                musteriAdi = s.customer.Ad,
                                                tutarr = s.yekun,
                                                tarih = s.tarih,
                                                islem = "Peşin Satış",
                                                islem_turu = s.cihaz.cihaz_adi,
                                                islem_adres = s.adet.ToString() + "Adet",
                                                cesit = "pesin",
                                                masraf_tipi = s.cihaz_id == null ? "Servis" : "Ürün/Parça"

                                            };

            IEnumerable<rapor_kalemi> ggg = hesaplar.Union(satis).OrderBy(x => x.tarih);

            List<GGR> gruplu = (from u in ggg
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),
                                    kalemler = g.Where(x => x.masraf_tipi == g.Key).ToList(),
                                    grup_toplam = g.Sum(x => x.tutarr)
                                }).ToList();




            return gruplu;
        }

        private List<GGR> satis_raporu_genel(DateTime bas, DateTime sonu)
        {


            //IEnumerable<CariDetayx> internet_fatura = (from o in dc.faturas
            //                                           where o.iptal == false && (o.tur == "Fatura") && o.sattis_tarih >= bas && o.sattis_tarih <= sonu
            //                                           orderby o.sattis_tarih ascending
            //                                           select new CariDetayx
            //                                           {
            //                                               MusteriID = 1,
            //                                               aciklama = o.adres,
            //                                               musteriAdi = o.ad,
            //                                               tutar = o.tutar,
            //                                               tarih = DateTime.Now,
            //                                               islem = o.tur == "Fatura" ? "Kredi Yükleme" : "Devir",
            //                                               islem_turu = o.tur == "Fatura" ? "İnternet Abonelik" : "Devreden Cari",
            //                                               islem_adres = "",
            //                                               cesit = "fatura",
            //                                               masraf_tipi = "internet"

            //                                           });

            IEnumerable<CariDetayx> hesaplar = from s in dc.servicehesaps
                                               where s.iptal == false && s.Onay_tarih >= bas && s.Onay_tarih <= sonu && s.Yekun > 0
                                               select new CariDetayx
                                               {
                                                   MusteriID = 1,
                                                   aciklama = s.Aciklama,
                                                   musteriAdi = s.customer.Ad,
                                                   tutar = s.Yekun,
                                                   tarih = DateTime.Now,
                                                   islem = s.IslemParca,
                                                   islem_turu = s.cihaz_adi,
                                                   islem_adres = s.adet.ToString() + "Adet",
                                                   cesit = "karar",
                                                   masraf_tipi = s.cihaz_id == null ? "Servis" : "Cihaz"

                                               };
            IEnumerable<CariDetayx> satis = from s in dc.satislars
                                            where s.iptal == false && s.tarih >= baslangic && s.tarih <= son && s.yekun > 0
                                            select new CariDetayx
                                            {
                                                MusteriID = 1,
                                                aciklama = "Peşin satış",
                                                musteriAdi = s.customer.Ad,
                                                tutar = s.yekun,
                                                tarih = s.tarih,
                                                islem = "Peşin Satış",
                                                islem_turu = s.cihaz.cihaz_adi,
                                                islem_adres = s.adet.ToString() + "Adet",
                                                cesit = "pesin",
                                                masraf_tipi = s.cihaz_id == null ? "Servis" : "Ürün/Parça"

                                            };
            IEnumerable<CariDetayx> ggg = hesaplar.Union(satis).OrderBy(x => x.tarih);

            List<GGR> gruplu = (from u in ggg
                                group u by u.masraf_tipi into g
                                select new GGR
                                {
                                    //grup_id = g.Key,
                                    grup_adi = g.Key,
                                    islem_adet = g.Count(),

                                    kalemler = null,
                                    grup_toplam = g.Sum(x => x.tutar)
                                }).ToList();

            return gruplu;
        }
        public wrapper_genel_periyodik periyodik_rapor(int kac_gun, DateTime baslama, DateTime sonuc)
        {
            wrapper_genel_periyodik gruplu = new wrapper_genel_periyodik();

            List<GGR3> seri = new List<GGR3>();

            List<rapor_araligi> araliklar = new List<rapor_araligi>();
            //bakalaım kaç parça varmış
            int toplam_gun = (sonuc - baslama).Days;
            int adet = (int)Math.Ceiling((double)toplam_gun / kac_gun);
            DateTime count_date = baslama;
            for (int i = 1; i < adet + 1; i++)
            {
                rapor_araligi ar = new rapor_araligi();
                ar.bas = count_date;
                if (count_date.AddDays(kac_gun) < sonuc)
                {
                    ar.son = count_date.AddDays(kac_gun);
                }
                else
                {
                    ar.son = sonuc;
                }
                ar.aralik_adi = ar.bas.ToShortDateString() + "-" + ar.son.ToShortDateString();
                araliklar.Add(ar);
                count_date = ar.son;
            }

            foreach (rapor_araligi ra in araliklar)
            {

                GGR3 g3 = new GGR3();
                g3.ay = ra.aralik_adi;
                wrapper_genel_gruplu gr = gonder_gruplu_ayli(ra.bas, ra.son);
                g3.listeler = gr.liste;
                seri.Add(g3);
            }
            //gruplu.firma = firma;

            gruplu.liste = seri;
            gruplu.tarih_araligi = baslama.ToShortDateString() + "-" + sonuc.ToShortDateString();
            return gruplu;
        }



    }


    public class rapor_araligi
    {
        public string aralik_adi { get; set; }
        public DateTime bas { get; set; }
        public DateTime son { get; set; }
    }
    public class wrapper
    {
        public string firma { get; set; }
        public DateTime baslangic { get; set; }
        public DateTime son { get; set; }
        public string tip { get; set; }
        public List<GGR> liste { get; set; }
    }
    public class wrapper_genel
    {
        public string firma { get; set; }

        public DateTime baslama { get; set; }
        public DateTime son { get; set; }
        public List<GGR> liste { get; set; }
        public int odeme_adet { get; set; }
        public decimal odeme_toplam { get; set; }

        public int tahsilat_adet { get; set; }
        public decimal tahsilat_toplam { get; set; }
    }
    public class wrapper_genel_gruplu
    {
        public string firma { get; set; }
        public string tip { get; set; }
        public DateTime baslama { get; set; }
        public DateTime son { get; set; }
        public List<GGR2> liste { get; set; }

        public int odeme_adet { get; set; }
        public decimal odeme_toplam { get; set; }

        public decimal fark { get; set; }
        public int tahsilat_adet { get; set; }
        public decimal tahsilat_toplam { get; set; }
        public int satis_adet { get; set; }
        public decimal satis_toplam { get; set; }
    }
    public class wrapper_genel_periyodik
    {
        public string firma { get; set; }

        public string tarih_araligi { get; set; }
        public List<GGR3> liste { get; set; }

    }

    public class GGR
    {
        public int? grup_id { get; set; }
        public string grup_adi { get; set; }
        public int islem_adet { get; set; }
        public decimal grup_toplam { get; set; }
        public List<rapor_kalemi> kalemler { get; set; }


    }
    public class GGR2
    {
        //public int? grup_idd { get; set; }
        public string grup_adii { get; set; }
        public int islem_adett { get; set; }
        public decimal grup_toplamm { get; set; }
        public List<GGR> listeler { get; set; }


    }
    public class GGR3
    {

        public string ay { get; set; }

        public List<GGR2> listeler { get; set; }

    }
    public class rapor_kalemi
    {
        public int? masraf_id { get; set; }
        public string musteriAdi { get; set; }
        public decimal tutarr { get; set; }
        public DateTime tarih { get; set; }
        public string aciklama { get; set; }
        public string islem { get; set; }
        public string islem_turu { get; set; }
        public string islem_adres { get; set; }
        public int MusteriID { get; set; }
        public string cesit { get; set; }
        public string masraf_tipi { get; set; }

    }

}
