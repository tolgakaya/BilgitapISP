using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;


namespace ServisDAL
{
    public class CihazMalzeme
    {


        radiusEntities dc;
        public CihazMalzeme(radiusEntities dc)
        {

            this.dc = dc;
        }


        public List<cihaz_rp> CihazListesiComp()
        {
            return (from c in dc.cihazs

                    select new cihaz_rp
                    {
                        ID = c.ID,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,

                    }).Take(3).ToList();
        }

        public List<cihaz_rp> CihazListesiComp(string terim, bool stok)
        {
            if (stok == true)
            {

                return (from c in dc.cihazs
                        from s in dc.cihaz_stoks
                        where c.ID == s.cihaz_id && (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim))
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = s.bakiye,
                            cikis = s.cikis,
                            giris = s.giris,
                            fiyat = (decimal)s.son_alis_fiyati
                        }).Take(3).ToList();

            }
            else
            {
                return (from c in dc.cihazs

                        where (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim))
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = 0,
                            cikis = 0,
                            giris = 0,
                            fiyat = 0
                        }).Take(3).ToList();

            }

        }

        public List<cihaz_rp> CihazListesi2(string terim = "")
        {
            if (terim == "")
            {
                return (from c in dc.cihazs
                        from s in dc.cihaz_stoks
                        where c.ID == s.cihaz_id
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            grupid = c.grupid,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = s.bakiye,
                            cikis = s.cikis,
                            giris = s.giris,
                            fiyat = (Decimal)s.son_alis_fiyati,
                            satis = (Decimal)s.satis_fiyati,
                            barkod = c.barkod

                        }).Take(10).ToList();
            }
            else
            {
                return (from c in dc.cihazs
                        from s in dc.cihaz_stoks
                        where c.ID == s.cihaz_id && (c.cihaz_adi.Contains(terim) || c.aciklama.Contains(terim))
                        select new cihaz_rp
                        {
                            ID = c.ID,
                            grupid = c.grupid,
                            cihaz_adi = c.cihaz_adi,
                            aciklama = c.aciklama,
                            seri_no = c.seri_no,
                            garanti_suresi = c.garanti_suresi,
                            bakiye = s.bakiye,
                            cikis = s.cikis,
                            giris = s.giris,
                            fiyat = (Decimal)s.son_alis_fiyati,
                            satis = (Decimal)s.satis_fiyati,
                            barkod = c.barkod

                        }).Take(10).ToList();
            }

        }
        public cihaz_rp CihazBarkod(string barkod)
        {
            return (from c in dc.cihazs
                    from s in dc.cihaz_stoks
                    where c.ID == s.cihaz_id && c.barkod.Equals(barkod)
                    select new cihaz_rp
                    {
                        ID = c.ID,
                        grupid = c.grupid,
                        cihaz_adi = c.cihaz_adi,
                        aciklama = c.aciklama,
                        seri_no = c.seri_no,
                        garanti_suresi = c.garanti_suresi,
                        bakiye = s.bakiye,
                        cikis = s.cikis,
                        giris = s.giris,
                        fiyat = (Decimal)s.son_alis_fiyati,
                        satis = (Decimal)s.satis_fiyati

                    }).FirstOrDefault();
        }

        public void Yeni(string ad, string aciklama, int sure, int grupid, string barkod)
        {
            cihaz c = new cihaz();
            c.aciklama = aciklama;
            c.grupid = grupid;
            c.cihaz_adi = ad;
            c.Firma = "firma";
            c.garanti_suresi = sure;
            c.barkod = barkod;
            c.seri_no = "-";
            dc.cihazs.Add(c);


            KaydetmeIslemleri.kaydetR(dc);

        }
        public void CihazGuncelle(string ad, string aciklama, int sure, int cihazid, int grupid, string barkod)
        {
            cihaz c = dc.cihazs.FirstOrDefault(x => x.ID == cihazid);
            if (c != null)
            {
                c.aciklama = aciklama;
                c.grupid = grupid;
                c.cihaz_adi = ad;
                c.Firma = "firma";
                c.garanti_suresi = sure;
                c.barkod = barkod;
                c.seri_no = "-";
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void StokGuncelle(int stok, int cihazid, decimal birim_maliyet)
        {

            cihaz_stoks cs = dc.cihaz_stoks.FirstOrDefault(x => x.cihaz_id == cihazid);

            int simdikiStok = cs.bakiye;
            int simdikiGiris = cs.giris;
            int simdikiBakiye = cs.bakiye;

            int girilecek = 0;
            int cikilacak = 0;
            if (stok > simdikiBakiye)
            {
                //yeni giriş yapılacak
                int fark = stok - simdikiBakiye;
                girilecek = fark;
            }
            else if (stok < simdikiBakiye)
            {
                //çıkış yapılacak
                int fark = simdikiBakiye - stok;
                cikilacak = fark;
            }


            cs.bakiye = stok;
            cs.cikis += cikilacak;
            cs.giris += girilecek;
            cs.son_alis_fiyati = birim_maliyet;
            //cihaz fifonun da güncellenmesi gerek
            //bunun için varsa daha önceki bakiyesi olan bütün fifolar iptal edilir
            //ve yukarıdaki bakiye bilgisi fioya ogünün tarihi ile girilir
            var fifos = dc.cihaz_fifos.Where(x => x.cihaz_id == cihazid && x.bakiye > 0 && x.iptal == false);
            foreach (var f in fifos)
            {
                f.iptal = true;
            }

            cihaz_fifos cf = new cihaz_fifos();
            cf.cihaz_id = cihazid;
            cf.cikis = 0;
            cf.fiyat = birim_maliyet;
            cf.giris = stok;
            cf.bakiye = stok;
            cf.tarih = DateTime.Now;
            cf.iptal = false;
            dc.cihaz_fifos.Add(cf);
            KaydetmeIslemleri.kaydetR(dc);



        }

        public void FiyatGuncelle(int cihazid, decimal fiyat)
        {
            cihaz_stoks c = dc.cihaz_stoks.FirstOrDefault(x => x.cihaz_id == cihazid);
            if (c != null)
            {
                c.satis_fiyati = fiyat;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public List<CihazRepo> cihazListesi(int musID)
        {
            return (from u in dc.cihaz_garantis
                    where u.CustID == musID && u.iptal == false
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();
        }

        public List<CihazRepo> cihaz_listesi(string terim)
        {
            return (from u in dc.cihaz_garantis
                    where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim)
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        musteriAdi = u.CustID == null ? "Demirbaş" : u.customer.Ad,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = u.CustID == null ? -99 : (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();

        }
        public List<CihazRepo> cihaz_listesi(int id)
        {
            return (from u in dc.cihaz_garantis
                    where u.iptal == false && u.cihaz_id == id
                    select new CihazRepo
                    {
                        urunID = u.ID,
                        cihaz_id = u.cihaz_id,
                        musteriAdi = u.CustID == null ? "Demirbaş" : u.customer.Ad,
                        cinsi = u.cihaz.cihaz_adi,
                        garantiBaslangic = (DateTime)u.baslangic,
                        garantiBitis = u.bitis,
                        garantiSuresi = u.cihaz.garanti_suresi,
                        aciklama = u.cihaz.aciklama,
                        durum = u.durum,
                        musteriID = u.CustID == null ? -99 : (int)u.CustID,
                        iade_tutar = u.iade_tutar,
                        satis_tutar = u.satis_tutar
                    }).ToList();

        }
        public string urun_teller(string terim)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim) && !String.IsNullOrEmpty(u.customer.telefon)
                                       select u.customer.telefon).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ",";
            }
            return mailler;

        }
        public string urun_teller(int id)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz_id == id && !String.IsNullOrEmpty(u.customer.telefon)
                                       select u.customer.telefon).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ",";
            }
            return mailler;

        }
        public string urun_mailler(string terim)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz.cihaz_adi.Contains(terim) && !String.IsNullOrEmpty(u.customer.email)
                                       select u.customer.email).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ";";
            }
            return mailler;

        }
        public string urun_mailler(int id)
        {
            List<string> musteriler = (from u in dc.cihaz_garantis
                                       where u.iptal == false && u.cihaz_id == id && !String.IsNullOrEmpty(u.customer.email)
                                       select u.customer.email).Distinct().ToList();

            string mailler = "";
            foreach (string c in musteriler)
            {
                mailler += c + ";";
            }
            return mailler;

        }
        public void garanti_iptal(int garanti_id)
        {
            cihaz_garantis c = dc.cihaz_garantis.FirstOrDefault(x => x.ID == garanti_id);
            if (c != null)
            {
                c.iptal = true;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public void garanti_iade(int garanti_id, decimal iade_tutar, string aciklama, int CustID, string kullanici)
        {
            //iade alındığında yeni bir stok_fifos girer ve açıklama olarak da iade gireriz-
            cihaz_garantis c = dc.cihaz_garantis.FirstOrDefault(x => x.ID == garanti_id);
            if (c != null)
            {
                //faturaya iade_id'yi işaretle
                //fatura iptal edilirken cari güncelleniyor(triggerda)
                //önce iade alınıp sonra fatura iptal edildiğinde cari de çift kayıt oluyordu
                //bunun için faturaya service hesabına göre arayıp iade_id'yi yazdırıyorum
                //triggerda cari hesap güncellenirken iade_id' null ise cari güncelleme yapıyor.
                List<fatura> hesapFaturalari = (from f in dc.faturas
                                                where f.servicehesap_id == c.servicehesap_id && (f.iptal == null || f.iptal == false)
                                                select f).ToList();
                foreach (fatura fati in hesapFaturalari)
                {
                    fati.iade_id = garanti_id;
                    fati.updated = kullanici;
                }

                musteriodemeler mo = new musteriodemeler();
                mo.Aciklama = c.adet + " Adet " + c.cihaz.cihaz_adi + " " + "Cihaz iadesi " + aciklama;
                mo.belge_no = "";
                mo.iade_id = garanti_id;
                mo.iptal = false;
                mo.kullanici = kullanici;
                mo.KullaniciID = kullanici;
                mo.mahsup_key = "";
                mo.Musteri_ID = CustID;
                mo.no = "-";
                mo.OdemeMiktar = iade_tutar;
                mo.OdemeTarih = DateTime.Now;
                mo.islem_tarihi = DateTime.Now;
                mo.inserted = kullanici;
                mo.tahsilat_odeme = "tahsilat";
                mo.tahsilat_turu = "iade";
                mo.taksit_no = 0;
                mo.Firma = "firma";
                dc.musteriodemelers.Add(mo);

                c.durum = "iade";
                c.iade_tutar = iade_tutar;
                c.aciklama = aciklama;
                //iadeyi cihaz fifo girelim
                cihaz_fifos fifo = new cihaz_fifos();
                fifo.bakiye = 1;
                fifo.cihaz_id = c.cihaz_id;
                fifo.cikis = 0;
                fifo.fiyat = iade_tutar;
                fifo.giris = 1;
                fifo.iptal = false;
                fifo.tarih = DateTime.Now;
                dc.cihaz_fifos.Add(fifo);


                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        //cihaz grupları(vergiler için)
        public void GrupEkle(string grupadi, decimal kdv, decimal otv, decimal oiv)
        {
            cihaz_grups g = new cihaz_grups();
            g.grupadi = grupadi;
            g.kdv = kdv;
            g.oiv = oiv;
            g.otv = otv;
            dc.cihaz_grups.Add(g);
            KaydetmeIslemleri.kaydetR(dc);
        }
        public void GrupDuzenle(int grupid, string grupadi, decimal kdv, decimal otv, decimal oiv)
        {
            var grup = dc.cihaz_grups.FirstOrDefault(x => x.grupid == grupid);
            if (grup != null)
            {
                grup.grupadi = grupadi;
                grup.kdv = kdv;
                grup.otv = otv;
                grup.oiv = oiv;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
        public List<cihaz_grups> CihazGruplar()
        {
            return dc.cihaz_grups.ToList();
        }
        public List<cihaz_grups> gruplar(string terim = "")
        {
            if (terim == "")
            {
                return (dc.cihaz_grups).ToList();
            }
            else
            {
                return dc.cihaz_grups.Where(x => x.grupadi.Contains(terim)).Take(10).ToList();
            }

        }
    }
    public class CihazRepo
    {
        public int urunID { get; set; }
        public int musteriID { get; set; }
        public int cihaz_id { get; set; }
        public string musteriAdi { get; set; }
        public string cinsi { get; set; }
        public DateTime garantiBaslangic { get; set; }
        public DateTime garantiBitis { get; set; }
        public int garantiSuresi { get; set; }
        public string aciklama { get; set; }
        public string durum { get; set; }
        public decimal? satis_tutar { get; set; }
        public decimal? iade_tutar { get; set; }

    }
    public class cihaz_rp
    {
        public int ID { get; set; }
        public int grupid { get; set; }
        public string barkod { get; set; }
        public string cihaz_adi { get; set; }
        public string aciklama { get; set; }
        public string seri_no { get; set; }
        public int garanti_suresi { get; set; }
        public int? giris { get; set; }
        public int? cikis { get; set; }
        public int? bakiye { get; set; }
        public decimal fiyat { get; set; }
        public decimal satis { get; set; }
    }

}
