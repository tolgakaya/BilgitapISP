using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class FaturaIslemleri
    {
        radiusEntities dc;
      
        public FaturaIslemleri(radiusEntities dc)
        {
          
            this.dc = dc;
        }

 
       
        #region  Firma ödeme alma ve tahsilat işlemleri


        //artık tahsilat
      
        //cariden tahsilat
   
        public string FaturaOdeCariEntegre(int musteriID, DateTime odeme_tarihi, decimal cari,string kullanici)
        {
            string musterimiz = dc.customers.Where(x => x.CustID == musteriID).Select(x => x.TC).FirstOrDefault();
            decimal odenecek = cari;
            string sonuc = "";
            sonuc += "bakiye---" + odenecek.ToString();
            FaturaOzet ozet = new FaturaOzet();
            if (odenecek < 0)
            {
                odenecek = -odenecek;

                sonuc += "bakiye varmış";

                List<fatura> odenmemisFaturalar = (from f in dc.faturas
                                                   where (f.odendi == null || f.odendi == false) && f.tc == musterimiz
                                                   && (f.iptal == null || f.iptal == false) 
                                                   orderby f.son_odeme_tarihi
                                                   select f).ToList();




                #region fatura ödeme işlemi


                ozet.netOdenecek = odenecek;
                int odenmemisSayisi = odenmemisFaturalar.Count;
                decimal odenmemisBakiye = odenmemisFaturalar.Sum(x => x.bakiye);
                ozet.sayi = odenmemisSayisi;
                ozet.bakiye = odenmemisBakiye;

                decimal artik = odenecek - odenmemisBakiye;

                ozet.artik = artik;
                foreach (fatura fat in odenmemisFaturalar)
                {
                    if (fat.bakiye <= odenecek && odenecek > 0)
                    {
                        sonuc += "--tamödeme önce" + fat.bakiye.ToString();
                        odenecek -= fat.bakiye;
                        fat.odendi = true;
                        fat.odenen = fat.tutar;
                        fat.bakiye = 0;
                        fat.tahsilat_turu = "cari";
                        fat.hesap_id = null;
                        fat.tahsilat_aciklama = "Alacağına mahsup";
                        fat.masraf = null;
                        fat.belge_no = "-";
                        fat.vade_tarih = null;
                        fat.mahsup = null;
                        fat.mahsup_key = "-";
                        fat.iade_id = null;
                        fat.odeme_tarihi = odeme_tarihi;
                        fat.guncelleme_tarihi = DateTime.Now;
                        fat.islemci_user = "-";
                        fat.updated = kullanici;
                        sonuc += "--tamödeme sonra" + fat.bakiye.ToString();

                    }
                    if (fat.bakiye > odenecek && odenecek > 0)
                    {
                        sonuc += "yarımödeme öne--" + odenecek.ToString();
                        fat.odenen += odenecek;
                        fat.bakiye = fat.bakiye - odenecek;
                        odenecek = -1; //fat.bakiye; //zaten bakiye büyük olduğu için ödenecek kalmıyor
                        fat.tahsilat_turu = "cari";
                        fat.hesap_id = null;
                        fat.tahsilat_aciklama = "Alacağına mahsup";
                        fat.masraf = null;
                        fat.belge_no = "-";
                        fat.vade_tarih = null;
                        fat.mahsup = null;
                        fat.mahsup_key = "-";
                        fat.iade_id = null;
                        fat.odeme_tarihi = odeme_tarihi;
                        fat.guncelleme_tarihi = DateTime.Now;
                        fat.islemci_user = "-";
                        fat.updated = kullanici;
                        sonuc += "--yarımödeme sonra" + odenecek.ToString();
                    }

                }

                ozet.artik = odenecek;

                KaydetmeIslemleri.kaydetR(dc);
                //dc.SaveChanges();
                //Distance is not important for real lovers.  Because the uniqye feeling that explains the real love is longing
                #endregion
            }
            return sonuc;
        }

        #endregion
       
   

        public List<TeknikServis.Radius.customer> YaklasanGunuGecenler(DateTime yaklasmaParam)
        {
            //yaklaşma kriteri

            List<TeknikServis.Radius.fatura> faturalar = (from f in dc.faturas
                                                          where  (f.odendi == null || f.odendi == false)
                                                          && (f.iptal == null || f.iptal == false) && (f.son_odeme_tarihi <= yaklasmaParam)
                                                          select f).Distinct().ToList();

            List<TeknikServis.Radius.customer> yaklasanGunuGecenFaturalar = (from f in faturalar
                                                                             from m in dc.customers
                                                                             where m.CustID == f.MusteriID
                                                                             select m).Distinct().ToList();

            return yaklasanGunuGecenFaturalar;

        }
        public int yaklasanOdemeSayisi(DateTime yaklasmaParam)
        {
            //yaklaşma kriteri

            List<TeknikServis.Radius.fatura> faturalar = (from f in dc.faturas
                                                          where  (f.odendi == null || f.odendi == false)
                                                          && (f.iptal == null || f.iptal == false) && (f.son_odeme_tarihi <= yaklasmaParam)
                                                          select f).Distinct().ToList();

            return (from f in faturalar
                    from m in dc.customers
                    where m.CustID == f.MusteriID
                    select m).Distinct().Count();



        }
     


    }
 
    public class taksitimiz
    {
        public int taksitNo { get; set; }
        public decimal taksitTutari { get; set; }
        public DateTime taksitTarihi { get; set; }
    }
    public class fatura_taksit
    {
        public int ID { get; set; }
        public string no { get; set; }
        public string kullanici { get; set; }
        public string username { get; set; }
        public string tur { get; set; }
        public string tur_aciklama { get; set; }
        public System.DateTime son_odeme_tarihi { get; set; }
        public DateTime? islem_tarihi { get; set; }
        public DateTime? satis_tarihi { get; set; }
        public string tc { get; set; }
        public decimal tutar { get; set; }
        public decimal yekun { get; set; }
        public string telefon { get; set; }
        public decimal odenen { get; set; }
        public decimal bakiye { get; set; }
        public Nullable<bool> odendi { get; set; }
        public string hesap_tur { get; set; }
       
    }

   
   
    [Serializable]
    public class FaturaOzet
    {
        public string musteri_adi { get; set; }
        public decimal tutar { get; set; }
        public decimal odenen { get; set; }
        public decimal bakiye { get; set; }
        public int sayi { get; set; }
        public decimal masraf { get; set; }
        public decimal netOdenecek { get; set; }
        public decimal adetMasraf { get; set; }
        public decimal tutarMasraf { get; set; }
        public decimal artik { get; set; }
        //taksit bilgileri
        public decimal taksit_tutar { get; set; }
        public decimal taksit_odenen { get; set; }
        public decimal taksit_bakiye { get; set; }
        public int taksit_sayi { get; set; }
        public decimal taksit_masraf { get; set; }

        public decimal taksit_adetMasraf { get; set; }
        public decimal taksit_tutarMasraf { get; set; }

        public bool KrediYuklendi { get; set; }

        public DateTime? Gecerlilik { get; set; }

  
    }
 
}
