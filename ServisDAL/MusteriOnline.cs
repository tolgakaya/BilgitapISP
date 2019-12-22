using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;


namespace ServisDAL
{

    public class MusteriOnline
    {
        radiusEntities dc;
 
        string tc;
        customer cust;
        public MusteriOnline(radiusEntities dc,  string tcParam)
        {
            this.tc = tcParam;
            this.dc = dc;
            cust = (from c in dc.customers
                    where c.TC == tc
                    select c).FirstOrDefault();
        }

        public MusteriDetay DetayGoster()
        {
            MusteriDetay detay = new MusteriDetay();
            //detay.cari = cari();
            detay.musteri = musteri();

            detay.GunuGelenler = GunuGelenler();
            return detay;
        }



        //private cariHesapRepo cari()
        //{
        //    return (from h in dc.carihesaps
        //            where h.MusteriID == cust.CustID
        //            select new cariHesapRepo
        //            {
        //                musteriID = h.MusteriID,
        //                musteriAdi = "",
        //                netBakiye = h.ToplamBakiye,
        //                netBorclanma = h.NetBorc,
        //                netAlacak = h.NetAlacak
        //            }
        //              ).FirstOrDefault();
        //}

        
        private MusteriBilgileri musteri()
        {
            return (from c in dc.customers
                    where c.CustID == cust.CustID
                    select new MusteriBilgileri
                    {
                        CustID = c.CustID,
                        adi = c.Ad,
                        adres = c.Adres,
                        tc = c.TC,
                        tel = c.telefon,
        
                        istihbarat = c.istihbarat,
                       
                        firma = c.Firma,
 
                    }).FirstOrDefault();

        }



        //private List<ServisHesapRepo> kararlar()
        //{
        //    return (from s in dc.servicehesaps
        //            where s.MusteriID == cust.CustID && s.iptal == false && (s.onay == null || s.onay == false)
        //            orderby s.TarihZaman descending
        //            select new ServisHesapRepo
        //            {
        //                hesapID = s.HesapID,
        //                aciklama = s.Aciklama,
        //                islemParca = s.IslemParca,
        //                kdv = s.KDV,
        //                musteriAdi = s.customer.Ad,
        //                musteriID = (int)s.MusteriID,
        //                onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
        //                onaylimi = s.onay,
        //                onayTarih = s.Onay_tarih,
        //                tarihZaman = s.TarihZaman,
        //                servisID = s.ServiceID,
        //                tutar = s.Tutar,
        //                yekun = s.Yekun
        //            }).Take(10).ToList();

        //}

        private List<fatura_taksit> faturalar()
        {
            IEnumerable<fatura_taksit> faturalarimiz = (from f in dc.faturas
                                                        where (f.odendi == null || f.odendi == false) && f.MusteriID == cust.CustID
                                                         && (f.iptal == null || f.iptal == false) && f.iade_id == null
                                                        orderby f.sattis_tarih ascending
                                                        select new fatura_taksit
                                                        {
                                                            bakiye = f.bakiye,
                                                            
                                                            hesap_tur = f.tur,
                                                            ID = f.ID,
                                                           
                                                            no = f.no,
                                                            odendi = f.odendi,
                                                            odenen = f.odenen,
                                                            tc = f.tc,
                                                            telefon = f.telefon,
                                                            tutar = f.tutar,
                                                            
                                                            yekun = f.bakiye,
                                                            son_odeme_tarihi = f.son_odeme_tarihi,
                                                            islem_tarihi = f.islem_tarihi
                                                        });


            return faturalarimiz.ToList();
        }

        private GunuGelen GunuGelenler()
        {
            GunuGelen gelen = new GunuGelen();
            decimal gelenTutar = 0;
            decimal sonrakiTutar = 0;
            string sonrakiTarih = "Bakiye Yok";

            List<fatura_taksit> fat = faturalar();
            if (fat != null)
            {
                List<fatura_taksit> gunuGelenlerimiz = fat.Where(x => x.son_odeme_tarihi <= DateTime.Now).ToList();
                if (gunuGelenlerimiz.Count > 0)
                {
                    gelenTutar = gunuGelenlerimiz.Sum(x => x.bakiye);
                }
                fatura_taksit sonraki = fat.FirstOrDefault(x => x.son_odeme_tarihi > DateTime.Now);
                if (sonraki != null)
                {
                    sonrakiTutar = sonraki.bakiye;
                    sonrakiTarih = sonraki.son_odeme_tarihi.ToShortDateString();
                }
            }

            gelen.sonrakiTarih = sonrakiTarih;
            gelen.sonrakiTutar = sonrakiTutar;
            gelen.tutar = gelenTutar;
            return gelen;
        }
 
    }
 
}
