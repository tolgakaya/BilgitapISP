using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class SatinAlim
    {

        radiusEntities dc;
        public SatinAlim(radiusEntities dc)
        {

            this.dc = dc;
        }
        public AlimRepo hesap { get; set; }
        public List<DetayRepo> detay { get; set; }



        public string alim_kaydet(string kullanici)
        {
            alim al = new alim();
            al.aciklama = hesap.aciklama;
            al.inserted = kullanici;
            al.belge_no = hesap.belge_no;
            al.CustID = hesap.CustID;
            al.alim_tarih = hesap.alim_tarih;
            al.Firma = "firma";
            al.iptal = false;//bunu değiştir false yap
            al.konu = hesap.konu;

            al.toplam_kdv = hesap.toplam_kdv;
            al.toplam_tutar = hesap.toplam_tutar;
            al.toplam_yekun = hesap.toplam_yekun;
            if (detay.Count > 0)
            {
                foreach (DetayRepo det in detay)
                {
                    alim_detays d = new alim_detays();
                    d.aciklama = det.aciklama;
                    d.inserted = kullanici;
                    d.adet = det.adet;
                    d.alim_id = al.alim_id;
                    d.cihaz_adi = det.cihaz_adi;
                    d.cihaz_id = det.cihaz_id;
                    d.cust_id = det.cust_id;
                    d.Firma = "firma";
                    d.iptal = false;
                    d.kdv = det.kdv;
                    //bu kdv sadece oran olsun yada biz bu kdvyi thesaplayalım
                    //tarih eklenecek
                    d.tarih = al.alim_tarih;
                    d.tutar = det.tutar;
                    d.yekun = det.yekun;
                    al.alim_detays.Add(d);
                }
            }
            dc.alims.Add(al);

            KaydetmeIslemleri.kaydetR(dc);

            return hesap.alim_tarih.ToString();
        }

        public AlimOzet Alimlar(DateTime baslangic, DateTime bitis, string cust_id)
        {
            AlimOzet ozet = new AlimOzet();
            List<AlimRepo> hesaplar = new List<AlimRepo>();
            if (String.IsNullOrEmpty(cust_id))
            {
                hesaplar = (from h in dc.alims
                            where h.iptal == false && h.alim_tarih <= bitis && h.alim_tarih >= baslangic
                            select new AlimRepo
                            {
                                alim_id = h.alim_id,
                                aciklama = h.aciklama,
                                alim_tarih = (DateTime)h.alim_tarih,
                                belge_no = h.belge_no,
                                konu = h.konu,
                                toplam_kdv = h.toplam_kdv,
                                toplam_tutar = h.toplam_tutar,
                                toplam_yekun = h.toplam_yekun,
                                musteri_adi = h.customer.Ad,
                                CustID = h.CustID,
                                kullanici = "Kayıt: " + h.inserted
                            }).ToList();
            }
            else
            {
                int custid = Int32.Parse(cust_id);
                hesaplar = (from h in dc.alims
                            where h.iptal == false && h.CustID == custid && h.alim_tarih <= bitis && h.alim_tarih >= baslangic
                            select new AlimRepo
                            {
                                alim_id = h.alim_id,
                                aciklama = h.aciklama,
                                alim_tarih = (DateTime)h.alim_tarih,
                                belge_no = h.belge_no,
                                konu = h.konu,
                                toplam_kdv = h.toplam_kdv,
                                toplam_tutar = h.toplam_tutar,
                                toplam_yekun = h.toplam_yekun,
                                musteri_adi = h.customer.Ad,
                                CustID = h.CustID,
                                kullanici = "Kayıt: " + h.inserted
                            }).ToList();

            }

            int islem_adet = hesaplar.Count;

            decimal yekun = 0;
            decimal tutar = 0;
            decimal kdv = 0;
            if (islem_adet > 0)
            {
                yekun = hesaplar.Sum(x => x.toplam_yekun);
                tutar = hesaplar.Sum(x => x.toplam_tutar);
                kdv = hesaplar.Sum(x => x.toplam_kdv);
            }

            ozet.alimlar = hesaplar;
            ozet.islem_adet = islem_adet;
            ozet.kdv = kdv;
            ozet.tutar = tutar;
            ozet.yekun = yekun;
            return ozet;
        }

        public DetayOzet Detaylar(DateTime baslangic, DateTime bitis, string cust_id, string cihaz_id, string alim_id)
        {
            //ya hepsi ya müşteriye göre yada cihaza göre listelenecek
            DetayOzet ozet = new DetayOzet();
            List<DetayRepo> hesaplar = new List<DetayRepo>();
            if (String.IsNullOrEmpty(cust_id))
            {
                //müşteri yoksa ya hepsi yada cihaza göre listelenecek
                if (String.IsNullOrEmpty(cihaz_id) && String.IsNullOrEmpty(alim_id))
                {
                    //hepsini göster
                    hesaplar = (from h in dc.alim_detays
                                where h.iptal == false && h.alim.alim_tarih <= bitis && h.alim.alim_tarih >= baslangic
                                select new DetayRepo
                                {
                                    detay_id = h.detay_id,
                                    aciklama = h.aciklama,
                                    adet = h.adet,
                                    alim_id = h.alim_id,
                                    cihaz_adi = h.cihaz_adi,
                                    cihaz_id = h.cihaz_id,
                                    cust_id = h.cust_id,
                                    yekun = h.yekun,
                                    tutar = h.tutar,
                                    kdv = h.kdv,
                                    musteri_adi = h.alim.customer.Ad,
                                    fiyat = Math.Round(((decimal)h.yekun / h.adet), 2),
                                    tarih = h.tarih,
                                    kullanici = h.inserted


                                }).ToList();
                }
                else
                {

                    if (!String.IsNullOrEmpty(alim_id))
                    {
                        //sadece alima göre
                        int alimid = Int32.Parse(alim_id);
                        hesaplar = (from h in dc.alim_detays
                                    where h.iptal == false && h.alim_id == alimid
                                    select new DetayRepo
                                    {
                                        detay_id = h.detay_id,
                                        aciklama = h.aciklama,
                                        adet = h.adet,
                                        alim_id = h.alim_id,
                                        cihaz_adi = h.cihaz_adi,
                                        cihaz_id = h.cihaz_id,
                                        cust_id = h.cust_id,
                                        yekun = h.yekun,
                                        tutar = h.tutar,
                                        kdv = h.kdv,
                                        musteri_adi = h.alim.customer.Ad,
                                        fiyat = Math.Round(((decimal)h.yekun / h.adet), 2),
                                        tarih = h.tarih,
                                        kullanici = h.inserted

                                    }).ToList();
                    }
                    else if (!String.IsNullOrEmpty(cihaz_id))
                    {
                        //sadece cihaza göre
                        int cihazid = Int32.Parse(cihaz_id);
                        hesaplar = (from h in dc.alim_detays
                                    where h.iptal == false && h.cihaz_id == cihazid && h.alim.alim_tarih <= bitis && h.alim.alim_tarih >= baslangic
                                    select new DetayRepo
                                    {
                                        detay_id = h.detay_id,
                                        aciklama = h.aciklama,
                                        adet = h.adet,
                                        alim_id = h.alim_id,
                                        cihaz_adi = h.cihaz_adi,
                                        cihaz_id = h.cihaz_id,
                                        cust_id = h.cust_id,
                                        yekun = h.yekun,
                                        tutar = h.tutar,
                                        kdv = h.kdv,
                                        musteri_adi = h.alim.customer.Ad,
                                        fiyat = Math.Round(((decimal)h.yekun / h.adet), 2),
                                        tarih = h.tarih,
                                        kullanici = h.inserted

                                    }).ToList();
                    }

                }

            }
            else
            {
                //sadece müşteriye göre göster
                int custid = Int32.Parse(cust_id);

                hesaplar = (from h in dc.alim_detays
                            where h.iptal == false && h.cust_id == custid && h.alim.alim_tarih <= bitis && h.alim.alim_tarih >= baslangic
                            select new DetayRepo
                            {
                                detay_id = h.detay_id,
                                aciklama = h.aciklama,
                                adet = h.adet,
                                alim_id = h.alim_id,
                                cihaz_adi = h.cihaz_adi,
                                cihaz_id = h.cihaz_id,
                                cust_id = h.cust_id,
                                yekun = h.yekun,
                                tutar = h.tutar,
                                kdv = h.kdv,
                                musteri_adi = h.alim.customer.Ad,
                                tarih = h.tarih,
                                kullanici = h.inserted

                            }).ToList();

            }

            int islem_adet = hesaplar.Count;
            int adet = 0;
            decimal yekun = 0;

            if (islem_adet > 0)
            {
                yekun = hesaplar.Sum(x => x.yekun);
                adet = hesaplar.Sum(x => x.adet);

            }

            ozet.detaylar = hesaplar;
            ozet.islem_adet = islem_adet;

            ozet.yekun = yekun;
            return ozet;
        }

        public void AlimIptal(int alim_id, string kul)
        {
            alim a = dc.alims.FirstOrDefault(x => x.alim_id == alim_id);
            if (a != null)
            {
                a.iptal = true;
                a.deleted = kul;
                KaydetmeIslemleri.kaydetR(dc);
            }
        }
    }
    public class AlimOzet
    {
        public int islem_adet { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal yekun { get; set; }
        public List<AlimRepo> alimlar { get; set; }
    }

    public class DetayOzet
    {
        public int islem_adet { get; set; }
        public int toplam_adet { get; set; }
        public decimal yekun { get; set; }
        public List<DetayRepo> detaylar { get; set; }
    }
    public class DetayRepo
    {
        public int detay_id { get; set; }
        public int alim_id { get; set; }
        public int cust_id { get; set; }
        public string musteri_adi { get; set; }
        public Nullable<int> cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public string aciklama { get; set; }
        public int adet { get; set; }
        public decimal tutar { get; set; }
        public decimal kdv { get; set; }
        public decimal yekun { get; set; }
        public decimal fiyat { get; set; }
        public DateTime? tarih { get; set; }
        public string kullanici { get; set; }

    }
    public class AlimRepo
    {
        public int alim_id { get; set; }
        public int CustID { get; set; }
        public string musteri_adi { get; set; }
        public string konu { get; set; }
        public string aciklama { get; set; }
        public System.DateTime alim_tarih { get; set; }
        public string belge_no { get; set; }
        public decimal toplam_tutar { get; set; }
        public decimal toplam_kdv { get; set; }
        public decimal toplam_yekun { get; set; }
        public string kullanici { get; set; }
    }
}
