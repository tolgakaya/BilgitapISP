
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Web;
using DevExpress.XtraReports;
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using TeknikServis.Radius;
using DevExpress.XtraPrinting.BarCode;
using System.Linq;

namespace ServisDAL
{
    public class FaturaPrinter
    {

        private radiusEntities dc;
        public FaturaPrinter(radiusEntities dc)
        {

            this.dc = dc;
        }


        public void InternetBas(int fatID, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge, bool? cift_taraf, string firma)
        {
            FaturaBas bas = new FaturaBas(dc);
            InternetFaturasi internet = bas.FaturaBilgileriInternet(fatID);
            gosterge.Report = FaturaBas(internet.Bilgiler, internet.Kalemler, cift_taraf, firma);

        }
        public void ManuelBas(InternetFaturasi internet, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge, bool? cift_taraf, string firma)
        {

            gosterge.Report = FaturaBas(internet.Bilgiler, internet.Kalemler, cift_taraf, firma);

        }
        public void ServisBas(int servisID, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge, bool? cift_taraf, string firma)
        {
            FaturaBas bas = new FaturaBas(dc);
            Baski_Gorunum faturaBilgisi = bas.FaturaBilgileriServis(servisID);

            List<Kalem> kalemler = bas.FaturaKalemleriServis(servisID);

            gosterge.Report = FaturaBas(faturaBilgisi, kalemler, cift_taraf, firma);


        }

        public void PesinBas(int odeme_id, string unvan, string tc, string vd, string adres, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge, bool? cift_taraf, string firma)
        {
            FaturaBas bas = new FaturaBas(dc);
            InternetFaturasi i = bas.FaturaBilgileriPesin(odeme_id, unvan, tc, vd, adres);
            gosterge.Report = FaturaBas(i.Bilgiler, i.Kalemler, cift_taraf, firma);


        }
       
        //TAHSİLAT MAKBUZU
        public void MakbuzBas(Makbuz_Gorunum faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            makbuzX m = new makbuzX();

            string yol = "/Raporlar/" + faturaBilgisi.firma + "makbuz.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.aciklama.Text = faturaBilgisi.Aciklama;
            m.kullanici.Text = faturaBilgisi.kullanici;
            m.gecerlilik.Text = faturaBilgisi.yaziile;
            m.musteriAdi.Text = faturaBilgisi.Musteri;
            m.tarih.Text = faturaBilgisi.Tarih.ToShortDateString() + " Saat: " + faturaBilgisi.Saat.ToShortTimeString();
            m.telefon.Text = faturaBilgisi.FirmaTelefon;
            m.tutar.Text = faturaBilgisi.Tutar + " TL " + "tahsil edildi";
            m.adres.Text = faturaBilgisi.Adres;
            m.firmaAdi.Text = faturaBilgisi.FirmaTam;
            m.telefon.Text = faturaBilgisi.FirmaTelefon;
            m.web.Text = faturaBilgisi.Web;
            gosterge.Report = m;
        }

        public void EmanetBas(Makbuz_Gorunum faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            Raporlama.emanetX m = new Raporlama.emanetX();

            string yol = "/Raporlar/" + faturaBilgisi.firma + "emanet.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.aciklama.Text = faturaBilgisi.Aciklama;
            m.musteriTel.Text = faturaBilgisi.musteriTel;
            m.musteriAdi.Text = faturaBilgisi.Musteri;
            m.musteriAdres.Text = faturaBilgisi.musteriAdres;
            m.tarih.Text = faturaBilgisi.Tarih.ToShortDateString() + " Saat: " + faturaBilgisi.Saat.ToShortTimeString();
            m.telefon.Text = faturaBilgisi.FirmaTelefon;

            m.adres.Text = faturaBilgisi.Adres;
            m.firmaAdi.Text = faturaBilgisi.FirmaTam;
            m.telefon.Text = faturaBilgisi.FirmaTelefon;
            m.web.Text = faturaBilgisi.Web;
            gosterge.Report = m;
        }
        public void BaslamaBas(Servis_Baslama servisBilgileri, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {

            baslamaX m = new baslamaX();
            string yol = "/Raporlar/" + servisBilgileri.firma + "servis.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.barBar2.Symbology = new Code93ExtendedGenerator();
            m.barBar2.Text = servisBilgileri.barkod;
            ((Code93ExtendedGenerator)m.barBar2.Symbology).CalcCheckSum = false;

            m.barBar.Symbology = new QRCodeGenerator();

            // Adjust the bar code's main properties.
            m.barBar.Text = servisBilgileri.barkod;
            m.barBar.Width = 400;
            m.barBar.Height = 150;

            // If the AutoModule property is set to false, uncomment the next line.
            m.barBar.AutoModule = true;
            // barcode.Module = 3;

            // Adjust the properties specific to the bar code type.
            ((QRCodeGenerator)m.barBar.Symbology).CompactionMode = QRCodeCompactionMode.AlphaNumeric;
            ((QRCodeGenerator)m.barBar.Symbology).ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.H;
            ((QRCodeGenerator)m.barBar.Symbology).Version = QRCodeVersion.AutoVersion;

            m.bindingSource1.DataSource = servisBilgileri.kararlar;
            m.aciklama.Text = servisBilgileri.Aciklama;

            m.musteriAdi.Text = servisBilgileri.Musteri;
            m.konu.Text = servisBilgileri.Konu;
            m.musteri_urunu.Text = servisBilgileri.musteri_urunu;
            m.urun_kod.Text = servisBilgileri.urun_kodu;
            m.musteriAdres.Text = servisBilgileri.MusteriAdres;
            m.musteriTel.Text = servisBilgileri.MusteriTel;
            m.tarih.Text = servisBilgileri.Tarih.ToShortDateString() + " Saat: " + servisBilgileri.Saat.ToShortTimeString();

            //m.kimlik.Text = "Servis Kimliği: " + servisBilgileri.kimlik;
            m.adres.Text = servisBilgileri.Adres;
            m.firmaAdi.Text = servisBilgileri.FirmaTam;
            m.telefon.Text = servisBilgileri.FirmaTelefon;
            m.web.Text = servisBilgileri.Web;
            //m.genelSartlar.Html = servisBilgileri.sartlar;
            m.tip.Text = "Servis tipi: " + servisBilgileri.tip;
            if (servisBilgileri.toplam_tutar > 0)
            {
                m.toplamTutar.Text = servisBilgileri.toplam_tutar.ToString("C");
            }
            else
            {
                m.toplamTutar.Visible = false;
                m.toplamTutarLbl.Visible = false;
            }

            m.firmaAdi.Text = servisBilgileri.FirmaTam;
            m.telefon.Text = servisBilgileri.FirmaTelefon;
            m.web.Text = servisBilgileri.Web;
            m.email.Text = servisBilgileri.email;

            m.islem.DataBindings.Add("Text", m.bindingSource1, "islem");
            m.cihaz.DataBindings.Add("Text", m.bindingSource1, "cihaz");
            m.tutar.DataBindings.Add("Text", m.bindingSource1, "tutar");
            m.aciklamaHesap.DataBindings.Add("Text", m.bindingSource1, "aciklama");
            m.adet.DataBindings.Add("Text", m.bindingSource1, "adet");

            gosterge.Report = m;
        }
        public void ExtreBas(extre faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            extreX m = new extreX();

            string yol = "/Raporlar/" + faturaBilgisi.firma + "extre.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.firmaAdi.Text = faturaBilgisi.firmaAdi;
            m.firmaAdres.Text = faturaBilgisi.firmaAdres;
            m.firmaTel.Text = faturaBilgisi.firmaTel;
            m.firmaWeb.Text = faturaBilgisi.firmaWeb;

            m.bindingSource1.DataSource = faturaBilgisi.detay;
            m.tarihAralik.Text = faturaBilgisi.aralik;
            m.tarih.Text = DateTime.Now.ToString();
            m.toplamBorc.Text = faturaBilgisi.hesap.ToplamBorc.ToString("C");
            m.toplanOdenen.Text = faturaBilgisi.hesap.ToplamOdenen.ToString("C");
            m.netBorc.Text = faturaBilgisi.hesap.NetBorc.ToString("C");
            m.toplamAlacak.Text = faturaBilgisi.hesap.ToplamAlacak.ToString("C");
            m.toplamOdedigimiz.Text = faturaBilgisi.hesap.ToplamOdedigimiz.ToString("C");
            m.netAlacak.Text = faturaBilgisi.hesap.NetAlacak.ToString("C");
            m.netBakiye.Text = faturaBilgisi.hesap.ToplamBakiye.ToString("C");
            m.musteri.Text = faturaBilgisi.Ad;
            m.tc.Text = faturaBilgisi.tc;

            m.islem.DataBindings.Add("Text", m.bindingSource1, "islem");
            m.tarihh.DataBindings.Add("Text", m.bindingSource1, "tarih", "{0:dd/MM/yyyy}");
            m.borc.DataBindings.Add("Text", m.bindingSource1, "borc");
            m.alacak.DataBindings.Add("Text", m.bindingSource1, "alacak");
            m.konu.DataBindings.Add("Text", m.bindingSource1, "konu");
            m.aciklama.DataBindings.Add("Text", m.bindingSource1, "aciklama");

            gosterge.Report = m;
        }
        public void GelirGiderBas(wrapper w, string firma, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            GelirGiderX m = new GelirGiderX();
            List<GGR> faturaBilgisi = w.liste;

            string yol = "/Raporlar/" + firma + "gelirgider.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.baslik.Text = w.tip;
            m.tarih_aralik.Text = w.baslangic.ToShortDateString() + "-" + w.son.ToShortDateString();

            m.bindingSource1.DataSource = faturaBilgisi;
            m.DetailReport.DataSource = m.bindingSource1;
            m.DetailReport.DataMember = "kalemler";

            m.grup_adi.DataBindings.Add("Text", m.bindingSource1, "grup_adi");
            m.adet.DataBindings.Add("Text", m.bindingSource1, "islem_adet");
            m.grup_toplam.DataBindings.Add("Text", m.bindingSource1, "grup_toplam");
            m.musteriAdi.DataBindings.Add("Text", m.bindingSource1, "kalemler.musteriAdi");
            m.tutarr.DataBindings.Add("Text", m.bindingSource1, "kalemler.tutarr");
            m.tarih.DataBindings.Add("Text", m.bindingSource1, "kalemler.tarih", "{0:dd/MM/yyyy}");

            m.islem.DataBindings.Add("Text", m.bindingSource1, "kalemler.islem");
            m.islem_adres.DataBindings.Add("Text", m.bindingSource1, "kalemler.islem_adres");
            m.islem_turu.DataBindings.Add("Text", m.bindingSource1, "kalemler.islem_turu");
            m.aciklama.DataBindings.Add("Text", m.bindingSource1, "kalemler.aciklama");
            gosterge.Report = m;

        }

        public void GelirGiderGenelBas(wrapper_genel faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            GelirGiderOzetX m = new GelirGiderOzetX();

            string yol = "/Raporlar/" + faturaBilgisi.firma + "gelirgiderozet.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.bindingSource1.DataSource = faturaBilgisi.liste;
            m.tahsilat_adet.Text = faturaBilgisi.tahsilat_adet.ToString();
            m.tahsilat_toplam.Text = faturaBilgisi.tahsilat_toplam.ToString("C");
            m.odeme_adet.Text = faturaBilgisi.odeme_adet.ToString();
            m.odeme_toplam.Text = faturaBilgisi.odeme_toplam.ToString("C");
            m.tarih.Text = DateTime.Now.ToString();
            m.tarih_aralik.Text = faturaBilgisi.baslama.ToShortDateString() + " ==> " + faturaBilgisi.son.ToShortDateString();



            m.grup_adi.DataBindings.Add("Text", m.bindingSource1, "grup_adi");
            m.adet.DataBindings.Add("Text", m.bindingSource1, "islem_adet");
            m.tutar.DataBindings.Add("Text", m.bindingSource1, "grup_toplam");



            gosterge.Report = m;

        }
        //burası
        public void GelirGiderGenelGrupluBas(wrapper_genel_gruplu faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            GelirGiderGrupluX m = new GelirGiderGrupluX();
            string yol = "/Raporlar/" + faturaBilgisi.firma + "gelirgidergruplu.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.bindingSource1.DataSource = faturaBilgisi;
            m.DataMember = "liste";
            m.DetailReport.DataSource = m.bindingSource1;
            m.DetailReport.DataMember = "liste.listeler";

            m.tahsilat_adet.Text = faturaBilgisi.tahsilat_adet.ToString();
            m.tahsilat_toplam.Text = faturaBilgisi.tahsilat_toplam.ToString("C");
            m.odeme_adet.Text = faturaBilgisi.odeme_adet.ToString();
            m.odeme_toplam.Text = faturaBilgisi.odeme_toplam.ToString("C");
            m.fark.Text = faturaBilgisi.fark.ToString("C");

            m.grup_adi0.DataBindings.Add("Text", m.bindingSource1, "liste.grup_adii");
            m.adet0.DataBindings.Add("Text", m.bindingSource1, "liste.islem_adett");
            m.grup_toplam0.DataBindings.Add("Text", m.bindingSource1, "liste.grup_toplamm");

            m.grup_adi.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.grup_adi");
            m.adet.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.islem_adet");
            m.tutar.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.grup_toplam");

            m.baslik.Text = faturaBilgisi.tip;
            m.tarih_aralik.Text = faturaBilgisi.baslama.ToShortDateString() + " ==> " + faturaBilgisi.son.ToShortDateString();



            gosterge.Report = m;

        }
        public void ServisMaliyet(maliyet listemiz, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            ServisDAL.Raporlama.ServisMaliyetlerX m = new ServisDAL.Raporlama.ServisMaliyetlerX();
            string yol = "/Raporlar/" + listemiz.firma + "servis_maliyet.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }

            m.bindingSource1.DataSource = listemiz;
            m.DataMember = "servis_listesi";
            m.DetailReport.DataSource = m.bindingSource1;
            m.DetailReport.DataMember = "servis_listesi.hesaplar";
            m.toplam_adet.Text = listemiz.adet.ToString();
            m.toplam_fark.Text = listemiz.toplam_fark.ToString("C");
            m.toplam_maliyet.Text = listemiz.toplam_maliyet.ToString("C");
            m.toplam_tutar.Text = listemiz.toplam_tutar.ToString("C");
            //m.tahsilat_adet.Text = faturaBilgisi.tahsilat_adet.ToString();
            //m.tahsilat_toplam.Text = faturaBilgisi.tahsilat_toplam.ToString("C");
            //m.odeme_adet.Text = faturaBilgisi.odeme_adet.ToString();
            //m.odeme_toplam.Text = faturaBilgisi.odeme_toplam.ToString("C");
            //m.fark.Text = faturaBilgisi.fark.ToString("C");

            m.servis_musteri.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.musteriAdi");
            m.servis_cihaz.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.urunAdi");
            m.servis_tutar.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.yekun");
            m.servis_maliyet.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.maliyet");
            m.kimlik.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.kimlikNo");
            m.hesap_islem.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.hesaplar.islemParca");
            m.hesap_parca.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.hesaplar.cihaz");
            m.hesap_yekun.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.hesaplar.yekun");
            m.hesap_maliyet.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.hesaplar.toplam_maliyet");
            m.hesap_tarih.DataBindings.Add("Text", m.bindingSource1, "servis_listesi.hesaplar.tarihZaman");

            m.tarih.Text = DateTime.Now.ToString();
            m.tarih_aralik.Text = listemiz.basTarih.ToShortDateString() + "'Den itibaren";



            gosterge.Report = m;

        }

        public void PeriyodikRaporBas(wrapper_genel_periyodik faturaBilgisi, DevExpress.XtraReports.Web.ASPxDocumentViewer gosterge)
        {
            GelirGiderAylikX m = new GelirGiderAylikX();
            string yol = "/Raporlar/" + faturaBilgisi.firma + "periyodikrapor.repx";
            string path = HttpContext.Current.Server.MapPath(yol);
            if (File.Exists(path))
            {
                m.LoadLayout(path);
            }
            m.bindingSource1.DataSource = faturaBilgisi;
            m.DataSource = m.bindingSource1;
            m.DataMember = "liste";
            m.DetailReport.DataSource = m.bindingSource1;
            m.DetailReport.DataMember = "liste.listeler";

            m.DetailReport1.DataSource = m.bindingSource1;
            m.DetailReport1.DataMember = "liste.listeler.listeler";



            m.ay.DataBindings.Add("Text", m.bindingSource1, "liste.ay");
            m.grup_adi0.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.grup_adii");
            m.adet0.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.islem_adett");
            m.grup_toplam0.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.grup_toplamm");

            m.grup_adi.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.listeler.grup_adi");
            m.adet.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.listeler.islem_adet");
            m.tutar.DataBindings.Add("Text", m.bindingSource1, "liste.listeler.listeler.grup_toplam");


            m.tarih_aralik.Text = faturaBilgisi.tarih_araligi;

            //m.xrChart1.DataSource = faturaBilgisi.liste;



            //List<string> gruplar = faturaBilgisi.liste.FirstOrDefault().listeler.Select(x => x.grup_adii).Distinct().ToList();
            //foreach (string g in gruplar)
            //{
            //    Series farkSeri = new Series(g, ViewType.Spline);
            //    var seriKaynak = faturaBilgisi.liste.ToList();
            //    var seriKaynak2 = seriKaynak.Select(x => x.listeler);

            //    m.xrChart1.Series.Add(farkSeri);
            //    farkSeri.DataSource = seriKaynak2.ToList();
            //    farkSeri.Label.Visible = false;
            //    // Specify data members to bind the series.
            //    farkSeri.ArgumentScaleType = ScaleType.Qualitative;

            //    farkSeri.ArgumentDataMember = "listeler.grup_adii";
            //    farkSeri.ValueScaleType = ScaleType.Numerical;
            //    farkSeri.ValueDataMembers.AddRange(new string[] { "listeler.grup_toplamm" });
            //}


            gosterge.Report = m;

        }

        //internetçiler aynı faturanın nüshasını aynı kağıda bastıkları için burada cift_taraf kontrolü yapılıyor
        public XtraReport FaturaBas(Baski_Gorunum bilgiler, List<Kalem> kalemler, bool? cift_taraf, string firma)
        {
            if (cift_taraf == true)
            {
                faturaX rapor = new faturaX();
                string yol = "/Raporlar/" + firma + "fatura.repx";
                string path = HttpContext.Current.Server.MapPath(yol);
                if (File.Exists(path))
                {
                    rapor.LoadLayout(path);
                }


                rapor.bindingSource1.DataSource = kalemler;
                rapor.lblAdi.Text = bilgiler.isim;
                rapor.lblTc.Text = bilgiler.TC;
                rapor.kdv.Text = bilgiler.KDV.ToString();
                rapor.oiv.Text = bilgiler.OIV.ToString();
                rapor.tarih.Text = bilgiler.tarih.ToShortDateString();
                rapor.vd.Text = bilgiler.VD;
                rapor.adres.Text = bilgiler.adres;
                rapor.toplam.Text = bilgiler.Tutar.ToString();
                rapor.gtoplam.Text = bilgiler.Yekun.ToString();
                rapor.yazi.Text = bilgiler.yaziIle;

                rapor.lblAdi2.Text = bilgiler.isim;
                rapor.lblTc2.Text = bilgiler.TC;
                rapor.kdv2.Text = bilgiler.KDV.ToString();
                rapor.oiv2.Text = bilgiler.OIV.ToString();
                rapor.tarih2.Text = bilgiler.tarih.ToShortDateString();
                rapor.vd2.Text = bilgiler.VD;
                rapor.adres2.Text = bilgiler.adres;
                rapor.toplam2.Text = bilgiler.Tutar.ToString();
                rapor.gtoplam2.Text = bilgiler.Yekun.ToString();
                rapor.yazi2.Text = bilgiler.yaziIle;


                rapor.cinsi.DataBindings.Add("Text", rapor.bindingSource1, "cinsi");
                rapor.miktar.DataBindings.Add("Text", rapor.bindingSource1, "miktar");
                rapor.fiyat.DataBindings.Add("Text", rapor.bindingSource1, "fiyat");
                rapor.tutar.DataBindings.Add("Text", rapor.bindingSource1, "tutar");

                rapor.cinsi2.DataBindings.Add("Text", rapor.bindingSource1, "cinsi");
                rapor.miktar2.DataBindings.Add("Text", rapor.bindingSource1, "miktar");
                rapor.fiyat2.DataBindings.Add("Text", rapor.bindingSource1, "fiyat");
                rapor.tutar2.DataBindings.Add("Text", rapor.bindingSource1, "tutar");



                return rapor;
            }
            else
            {
                faturaTekX rapor = new faturaTekX();
                string yol = "/Raporlar/" + firma + "faturaTek.repx";
                string path = HttpContext.Current.Server.MapPath(yol);
                if (File.Exists(path))
                {
                    rapor.LoadLayout(path);
                }


                rapor.bindingSource1.DataSource = kalemler;
                rapor.lblAdi.Text = bilgiler.isim;
                rapor.lblTc.Text = bilgiler.TC;
                rapor.kdv.Text = bilgiler.KDV.ToString();
                rapor.oiv.Text = bilgiler.OIV.ToString();
                rapor.tarih.Text = bilgiler.tarih.ToShortDateString();
                rapor.vd.Text = bilgiler.VD;
                rapor.adres.Text = bilgiler.adres;
                rapor.toplam.Text = bilgiler.Tutar.ToString();
                rapor.gtoplam.Text = bilgiler.Yekun.ToString();
                rapor.yazi.Text = bilgiler.yaziIle;



                rapor.cinsi.DataBindings.Add("Text", rapor.bindingSource1, "cinsi");
                rapor.miktar.DataBindings.Add("Text", rapor.bindingSource1, "miktar");
                rapor.fiyat.DataBindings.Add("Text", rapor.bindingSource1, "fiyat");
                rapor.tutar.DataBindings.Add("Text", rapor.bindingSource1, "tutar");

                return rapor;
            }


        }


    }

}
