using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class CekSenet
    {
         
        private radiusEntities dc;
        
        public CekSenet(radiusEntities dc )
        {
            
            this.dc = dc;
        }

       
        public cekozet Cekler(int kritikGun, bool? cekildi, bool? alinan)
        {
          

            cekozet oz = new cekozet();
            DateTime kritikTarih = DateTime.Now.AddDays(kritikGun);
            List<cekrepo> sorgu = new List<cekrepo>();

            if (cekildi == null)
            {
                // çekilme durumu önemli değil
                if (alinan == null)
                {
                    //çekilme/alinma önemli değil
                    sorgu = (from c in dc.cekhesaps
                             where c.iptal == false 
                             && c.vade_tarih < kritikTarih
                             select new cekrepo
                             {
                                 cek_id = c.cek_id,
                                 Musteri_ID = c.Musteri_ID,
                                 belge_no = c.belge_no,
                                 tutar = c.tutar,
                                 masraf = c.masraf,
                                 nettutar = c.nettutar,
                                 musteri_adi = c.customer.Ad,
                                 tarih = c.tarih,
                                 vade_tarih = c.vade_tarih,
                                 tahsilat_tarih = c.tahsilat_tarih,
                                 alinan = c.alinan == true ? "Alınan Çek" : "Verilen Çek",
                                 tur = c.alinan,
                                 Aciklama = c.islem,
                                 cekildi = c.cekildi

                             }).ToList();
                }
                else
                {
                    //çekilme önemli değil alınma önemli
                    sorgu = (from c in dc.cekhesaps
                             where c.iptal == false && c.alinan == alinan  
                             && c.vade_tarih < kritikTarih
                             select new cekrepo
                             {
                                 cek_id = c.cek_id,
                                 Musteri_ID = c.Musteri_ID,
                                 belge_no = c.belge_no,
                                 tutar = c.tutar,
                                 masraf = c.masraf,
                                 nettutar = c.nettutar,
                                 musteri_adi = c.customer.Ad,
                                 tarih = c.tarih,
                                 vade_tarih = c.vade_tarih,
                                 tahsilat_tarih = c.tahsilat_tarih,
                                 alinan = c.alinan == true ? "Alınan Çek" : "Verilen Çek",
                                 tur = c.alinan,
                                 Aciklama = c.islem,
                                 cekildi = c.cekildi

                             }).ToList();
                }
            }
            else
            {
                //çekilme durumu önemli

                if (alinan == null)
                {
                    // çekilme önemli değil/ alınan verilen hepsi
                    sorgu = (from c in dc.cekhesaps
                             where c.iptal == false && c.cekildi == cekildi  
                             && c.vade_tarih < kritikTarih
                             select new cekrepo
                             {
                                 cek_id = c.cek_id,
                                 Musteri_ID = c.Musteri_ID,
                                 belge_no = c.belge_no,
                                 tutar = c.tutar,
                                 masraf = c.masraf,
                                 nettutar = c.nettutar,
                                 musteri_adi = c.customer.Ad,
                                 tarih = c.tarih,
                                 vade_tarih = c.vade_tarih,
                                 tahsilat_tarih = c.tahsilat_tarih,
                                 alinan = c.alinan == true ? "Alınan Çek" : "Verilen Çek",
                                 tur = c.alinan,
                                 Aciklama = c.islem,
                                 cekildi = c.cekildi

                             }).ToList();
                }
                else
                {
                    // çekilme önemli değil /alınma verilme durumlarına göre
                    sorgu = (from c in dc.cekhesaps
                             where c.iptal == false && c.cekildi == cekildi && c.alinan == alinan 
                             && c.vade_tarih < kritikTarih
                             select new cekrepo
                             {
                                 cek_id = c.cek_id,
                                 Musteri_ID = c.Musteri_ID,
                                 belge_no = c.belge_no,
                                 tutar = c.tutar,
                                 masraf = c.masraf,
                                 nettutar = c.nettutar,
                                 musteri_adi = c.customer.Ad,
                                 tarih = c.tarih,
                                 vade_tarih = c.vade_tarih,
                                 tahsilat_tarih = c.tahsilat_tarih,
                                 alinan = c.alinan == true ? "Alınan Çek" : "Verilen Çek",
                                 tur = c.alinan,
                                 Aciklama = c.islem,
                                 cekildi = c.cekildi

                             }).ToList();
                }

            }

            oz.adet = sorgu.Count;
            oz.cekler = sorgu;
            oz.toplam_tutar = sorgu.Sum(x => x.nettutar);
            return oz;
        }
        public string CekTahsilat(int cekID, int? kasa_id, int? banka_id)
        {
            string sonuc = "";
            //kasa mı banka mı kontrolü sayfada yapılacak- ayrı tuşlarla yaparsın
            cekhesap cek = dc.cekhesaps.FirstOrDefault(x => x.cekildi == false && x.iptal == false && x.cek_id == cekID);
            if (cek != null)
            {

                cek.cekildi = true;
                if (kasa_id != null)
                {
                    cek.kasa_id = kasa_id;
                    sonuc = "Çek tutarı kasanıza";
                }
                if (banka_id != null)
                {
                    cek.banka_id = banka_id;
                    sonuc = "Çek tutarı bankaya";
                }
                KaydetmeIslemleri.kaydetR(dc);
                sonuc += " aktarıldı!";
            }
            else
            {
                sonuc = "Çek kaydı bulunamadı!";
            }
            return sonuc;
        }
        //çekler çek hesaplarından iptal edilemez, çekler ile yalnız tahsil edilir ya da ödeme yapılır.
        // aynen kasahareketlerinin iptal edilemediği gibi.
        //iptaller sadece müşteri ödemelers(yani) hareket tablosundan olur. böylece  diğer hesaplar da iptal edilmiş olur.

    }
    public class cekozet
    {
        public decimal toplam_tutar { get; set; }
        public int adet { get; set; }
        public List<cekrepo> cekler { get; set; }

    }
    public class cekrepo
    {
        public int cek_id { get; set; }
        public string belge_no { get; set; }
        public decimal tutar { get; set; }
        public decimal masraf { get; set; }
        public decimal nettutar { get; set; }

        public int Musteri_ID { get; set; }
        public string musteri_adi { get; set; }
        public string Aciklama { get; set; }
        public System.DateTime tarih { get; set; }
        public System.DateTime vade_tarih { get; set; }
        public Nullable<System.DateTime> tahsilat_tarih { get; set; }

        public string alinan { get; set; }
        public bool tur { get; set; }

        public bool cekildi { get; set; }
    }
}
