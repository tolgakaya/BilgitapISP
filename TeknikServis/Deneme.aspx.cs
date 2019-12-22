using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Radius;
using ServisDAL;
using TeknikServis.Logic;
using System.Data;
using System.Reflection;
using System.IO;
using System.Text;

using System.Web.Services.Description;

using DevExpress.XtraExport;
using System.Diagnostics;
using System.Collections;
namespace TeknikServis
{
    public partial class Deneme : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {



            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(@"<script type='text/javascript'>");

            //sb.Append(" alertify.success('Kayıt Güncellendi!');");
            ////sb.Append(" alert('Kayıt Güncellendi!');");

            //sb.Append(@"</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);

          

            //if (Session["mesaj"] != null)
            //{
            //    Dictionary<string, string> mesajlar = (Dictionary<string, string>)Session["mesaj"];
            //    GridView1.DataSource = mesajlar;
            //    GridView1.DataBind();

            //}
            //if (Session["mesele"] != null)
            //{
            //    baslik.InnerHtml = Session["mesele"].ToString();
            //}

            //if (Session["kayit"] != null)
            //{
            //    wrap w = (wrap)Session["kayit"];
            //    txtAdmin.Text = w.admin;
            //    txtAdres.Text = w.adres;
            //    txtFirma.Text = w.firma;
            //    txtResimyol.Text = w.resimyol;
            //    txttelefon.Text = w.telefon;
            //    txtWeb.Text = w.web;
            //    txtTamFirma.Text = w.TamFirma;
            //}

        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            //geçici

            string firma = txtFirma.Text;
            string admin = txtAdmin.Text;
            string resimYol = txtResimyol.Text;
            string tamFirma = txtTamFirma.Text;
            string web = txtWeb.Text;
            string telefon = txttelefon.Text;
            string adres = txtAdres.Text;

            //string firma = "DEMO";
            //string admin = "demo";
            //string resimYol = txtResimyol.Text;
            //string tamFirma = "Demo Ltd Şti";
            //string web = "-";
            //string telefon = "05069468693";
            //string adres = "Demo şehri";

            Baslangic b = new Baslangic(firma);
            b.admin = admin;
            b.resimyol = resimYol;
            b.TamFirma = tamFirma;
            b.telefon = telefon;
            b.web = web;
            b.adres = adres;
            Session["mesele"] = b.Kur();
            Response.Redirect("/Deneme.aspx");

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            //Hareket h = new Hareket("TOL");
            //GenelGorunum g = h.Gosterge("Serdar", 100);
            //GridView1.DataSource = g.islemler;
            //GridView1.DataBind();
            //string mesele = "";
            //mesele += "banka-" + g.banka_hesaplar_toplami;
            //mesele += " kasa-" + g.kasa_bakiye;
            //mesele += " extre-" + g.kart_extre_toplami;
            //mesele += " pos-" + g.poslarda_birikenler;
            //mesele += " cek-" + g.alinan_cek_toplami;
            //Session["mesele"] = mesele;

        }

        private void SatinAl()
        {
            AlimRepo al = new AlimRepo();
            al.aciklama = "vvvv";
            al.alim_tarih = DateTime.Now;
            al.belge_no = "ççç";
            al.CustID = 19;
            //al.Firma = "TOL";
            //al.iptal = 0;
            al.konu = "fffff0";
            //al.owner = "Serdar";

            List<DetayRepo> detaylar = new List<DetayRepo>();
            DetayRepo d = new DetayRepo();
            d.yekun = 100;
            d.tutar = 90;
            //d.owner = "Serdar";
            d.kdv = 10;
            //d.iptal = false;
            //d.Firma = "TOL";
            d.cust_id = 19;
            d.cihaz_id = 4;
            d.cihaz_adi = "felan";
            d.alim_id = -1;
            d.adet = 3;
            d.aciklama = "aaaaa";
            detaylar.Add(d);


            DetayRepo d2 = new DetayRepo();
            //d2.yekun = 100;
            //d2.tutar = 90;
            ////d2.owner = "Serdar";
            //d2.kdv = 10;
            ////d2.iptal = false;
            ////d2.Firma = "TOL";
            //d2.cust_id = 19;
            //d2.cihaz_id = 4;
            //d2.cihaz_adi = "felan";
            //d2.alim_id = -1;
            //d2.adet = 3;
            //d2.aciklama = "aaaaa";
            //detaylar.Add(d2);
            //SatinAlim islem = new SatinAlim("TOL", "Serdar");
            //islem.detay = detaylar;
            //islem.hesap = al;
            //try
            //{
            //    islem.alim_kaydet();
            //}
            //catch (Exception ex)
            //{

            //    Session["mesele"] = ex.Message;
            //    Response.Redirect("/Deneme.aspx");
            //}
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        private void logGoster2()
        {
            //DateTime tarihDate3 = DateTime.Parse("27.02.2015 00:00:00");
            ////DateTime tarihDate4 = DateTime.Parse("26.02.2015 00:05:00");
            //DateTime tarihDate = DateTime.Parse("27.02.2015 23:59:00");
            ////DateTime tarihDate2 = DateTime.Parse("27.02.2015 22:20:00");

            //radiusEntities dc = MyContext.Context("LOG");
            //List<radacct> sorgu2 = (from r in dc.radaccts
            //                        where r.nasipaddress == "85.109.132.211" && r.acctstarttime <= tarihDate && r.acctstoptime >= tarihDate
            //                        select r).ToList();
            //List<radacct> sorgu = (from r in dc.radaccts
            //                       where r.nasipaddress == "85.109.132.211" && r.acctstarttime <= tarihDate3 && r.acctstoptime >= tarihDate3
            //                       select r).ToList();

            //List<string> sorguBirlesik = (from b in sorgu
            //                              from i in sorgu2
            //                              where b.username == i.username
            //                              select b.username).Distinct().ToList();
            //List<Radius.rm_users> kullanicilar = (from u in dc.rm_users
            //                                      from s in sorguBirlesik
            //                                      where u.username == s
            //                                      select u).ToList();

            ////26.02.2015 12:04:00 Tarihinde aktifler
            //GridView1.DataSource = kullanicilar;
            //GridView1.DataBind();

            //////27.02.2015 22:19:00 tarihinde aktifler
            ////GridView2.DataSource = sorgu2;
            ////GridView2.DataBind();



            ////her iki tarihte aktif olan kullanıcı bilgileri

            //GridView2.DataSource = sorgu.Union(sorgu2);
            //GridView2.DataBind();



        }
        private void logGoster3()
        {
            //DateTime tarihDate3 = DateTime.Parse("26.02.2015 12:04:00");

            DateTime tarihDate = DateTime.Parse("27.02.2015 22:19:00");


            //radiusEntities dc = MyContext.Context("TOL");
            //List<rm_syslog> sorgu2 = (from r in dc.rm_syslog
            //                          where r.ip == "172.18.182.101"
            //                          select r).ToList();

            //List<string> sorguBirlesik = (
            //                              from i in sorgu2

            //                              select i.username).Distinct().ToList();

            //List<Radius.rm_users> kullanicilar = (from u in dc.rm_users
            //                                      from s in sorguBirlesik
            //                                      where u.username == s
            //                                      select u).ToList();

            //26.02.2015 12:04:00 Tarihinde aktifler

            //27.02.2015 22:19:00 tarihinde aktifler
            //GridView2.DataSource = sorgu2;
            //GridView2.DataBind();


            //her iki tarihte aktif olan kullanıcı bilgileri
            //GridView3.DataSource = kullanicilar;
            //GridView3.DataBind();



        }

        private string duz(string yazi)
        {
            if (!String.IsNullOrEmpty(yazi))
            {
                yazi = yazi.Trim();

                if (yazi.Contains("&Ccedil;"))
                {
                    yazi = yazi.Replace("&Ccedil;", "Ç");

                }
                if (yazi.Contains("&Ouml;"))
                {
                    yazi = yazi.Replace("&Ouml;", "Ö");

                }
                if (yazi.Contains("&Uuml;"))
                {
                    yazi = yazi.Replace("&Uuml;", "Ü");

                }
                if (yazi.Contains("&ouml;"))
                {
                    yazi = yazi.Replace("&ouml;", "ö");

                }

                if (yazi.Contains("&ccedil;"))
                {
                    yazi = yazi.Replace("&ccedil;", "ç");

                }
                if (yazi.Contains("&uuml;"))
                {
                    yazi = yazi.Replace("&uuml;", "ü");

                }
                if (yazi.Contains("&Uuml;"))
                {
                    yazi = yazi.Replace("&Uuml;", "ü");

                }
                if (yazi.Contains("陌"))
                {
                    yazi = yazi.Replace("陌", "İ");

                }
                if (yazi.Contains("艦"))
                {
                    yazi = yazi.Replace("艦", "Ş");

                }
                if (yazi.Contains("臑"))
                {
                    yazi = yazi.Replace("臑", "Ğ");

                }

                if (yazi.Contains("谋"))
                {
                    yazi = yazi.Replace("谋", "ı");

                }

                if (yazi.Contains("臒"))
                {
                    yazi = yazi.Replace("臒", "ğ");

                }
                if (yazi.Contains("艧"))
                {
                    yazi = yazi.Replace("艧", "ş");

                }

            }
            return yazi;
        }
        private string duzTrimsiz(string yazi)
        {
            if (!String.IsNullOrEmpty(yazi))
            {


                if (yazi.Contains("&Ccedil;"))
                {
                    yazi = yazi.Replace("&Ccedil;", "Ç");

                }
                if (yazi.Contains("&Ouml;"))
                {
                    yazi = yazi.Replace("&Ouml;", "Ö");

                }
                if (yazi.Contains("&Uuml;"))
                {
                    yazi = yazi.Replace("&Uuml;", "Ü");

                }
                if (yazi.Contains("&ouml;"))
                {
                    yazi = yazi.Replace("&ouml;", "ö");

                }

                if (yazi.Contains("&ccedil;"))
                {
                    yazi = yazi.Replace("&ccedil;", "ç");

                }
                if (yazi.Contains("&uuml;"))
                {
                    yazi = yazi.Replace("&uuml;", "ü");

                }
                if (yazi.Contains("&Uuml;"))
                {
                    yazi = yazi.Replace("&Uuml;", "ü");

                }
                if (yazi.Contains("陌"))
                {
                    yazi = yazi.Replace("陌", "İ");

                }
                if (yazi.Contains("艦"))
                {
                    yazi = yazi.Replace("艦", "Ş");

                }


            }
            return yazi;
        }
        protected void btnDuzelt_Click(object sender, EventArgs e)
        {
            yaziDuzelt();
            //tcSenk();
        }
        private void tcSenk()
        {
            /*  radiusEntities dc = MyContext.Context("TOL");

              List<rm_users> kullanicilar = dc.rm_users.ToList();
              List<customer> musteriler = dc.customers.ToList();
              foreach (rm_users cust in kullanicilar)
              {
                  if (String.IsNullOrEmpty(cust.taxid))
                  {
                      string tc = musteriler.FirstOrDefault(x => x.Radius_UserName == cust.username).TC;
                      cust.taxid = tc;
                  }

              }
              KaydetmeIslemleri.kaydetR(dc);*/
        }

        private void yaziDuzelt()
        {
            radiusEntities dc = MyContext.Context("TOL");

            List<customer> musteriler = dc.customers.ToList();
            foreach (customer cust in musteriler)
            {

                string ad = duz(cust.Ad);
                string adres = duz(cust.Adres);
                string aciklama = duz(cust.tanimlayici);

                cust.Ad = ad;
                cust.Adres = adres;
                cust.tanimlayici = aciklama;
                KaydetmeIslemleri.kaydetR(dc);
            }

        }

        protected void btnDeneme_Click(object sender, EventArgs e)
        {
            radiusEntities dc = MyContext.Context("TOL");


            //bütün müşterileri çekelim
            //List<rm_users> kullanicilar = dc.rm_users.ToList();
            //List<rm_users2> yeniler = dc.rm_users2.ToList();

            //foreach (rm_users2 kul in yeniler)
            //{
            //    rm_users eski = kullanicilar.FirstOrDefault(x => x.username == kul.username);
            //    ServisDAL.MusteriIslemleri m = new ServisDAL.MusteriIslemleri("TOL");
            //    if (eski == null)
            //    {
            //        string ad = "adi";
            //        if (!String.IsNullOrEmpty(kul.firstname))
            //        {
            //            ad = kul.firstname;
            //        }
            //        string last = "adi";
            //        if (!String.IsNullOrEmpty(kul.lastname))
            //        {
            //            last = kul.lastname;
            //        }
            //        string adres = "adi";
            //        if (!String.IsNullOrEmpty(kul.address))
            //        {
            //            adres = kul.address;
            //        }
            //        string mobile = "adi";
            //        if (!String.IsNullOrEmpty(kul.mobile))
            //        {
            //            mobile = kul.mobile;
            //        }
            //        string email = "adi";
            //        if (!String.IsNullOrEmpty(kul.email))
            //        {
            //            email = kul.email;
            //        }
            //        string comment = "adi";
            //        if (!String.IsNullOrEmpty(kul.comment))
            //        {
            //            comment = kul.comment;
            //        }
            //        string tax = kul.username;
            //        if (!String.IsNullOrEmpty(kul.taxid))
            //        {
            //            tax = kul.taxid;
            //        }
            //        string own = "serdartol";
            //        if (!String.IsNullOrEmpty(kul.owner))
            //        {
            //            own = kul.owner;
            //        }

            //radcheck ch = dc.radchecks.Where(X => X.username == kul.username).FirstOrDefault();
            ////if (ch != null)
            ////{
            ////    List<radcheck> chler = dc.radchecks.Where(X => X.username == kul.username).ToList();
            ////    foreach (radcheck c in chler)
            ////    {
            ////        dc.radchecks.Remove(c);
            ////    }
            ////    KaydetmeIslemleri.kaydetR(dc);
            ////}

            //m.musteriEkleR(kul.firstname, kul.lastname, kul.address, kul.mobile, kul.mobile, kul.email, kul.comment, kul.taxid, kul.owner, kul.username, "sifree", false);
            //    }


            //}
            //foreach (rm_users kul in kullanicilar)
            //{
            //    rm_users2 ikinci = yeniler.FirstOrDefault(x => x.username == kul.username);
            //    if (ikinci != null)
            //    {
            //        DateTime gecerlilik = ikinci.expiration;

            //        kul.expiration = gecerlilik;

            //    }

            //}
            //KaydetmeIslemleri.kaydetR(dc);

        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            //GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");
            //g.baslangic = DateTime.Now.AddDays(-30);
            //g.son = DateTime.Now;
            //Session["odeme_tahsilat"] = g.gonder_genel("odeme_tahsilat");
            //Response.Redirect("/Baski.aspx?tip=odeme_tahsilat");

        }

        protected void Button1_Click3(object sender, EventArgs e)
        {
            //GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");
            //g.baslangic = DateTime.Now.AddDays(-30);
            //g.son = DateTime.Now;
            //Session["gider_raporu"] = g.gonder("odeme");
            //Response.Redirect("/Baski.aspx?tip=gider_raporu");

        }
        protected void Button1_Click4(object sender, EventArgs e)
        {
            //GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");
            //g.baslangic = DateTime.Now.AddDays(-30);
            //g.son = DateTime.Now;
            //Session["tahsilat_raporu"] = g.gonder("tahsilat");
            //Response.Redirect("/Baski.aspx?tip=tahsilat_raporu");

        }
        protected void Button1_Click5(object sender, EventArgs e)
        {
            //    GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");
            //    g.baslangic = DateTime.Now.AddDays(-30);
            //    g.son = DateTime.Now;
            //    Session["odeme_tahsilat_gruplu"] = g.gonder_gruplu("odeme_tahsilat_gruplu");
            //    Response.Redirect("/Baski.aspx?tip=odeme_tahsilat_gruplu");

        }
        protected void Button1_Click6(object sender, EventArgs e)
        {
            //GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");
            //g.baslangic = DateTime.Now.AddDays(-30);
            //g.son = DateTime.Now;
            //Session["odeme_tahsilat_satis"] = g.gonder_gruplu_satisli("odeme_tahsilat_satis");
            //Response.Redirect("/Baski.aspx?tip=odeme_tahsilat_satis");

        }
        protected void Button1_Click7(object sender, EventArgs e)
        {
            //GelirGider g = new ServisDAL.GelirGider("TOL", "Serdar");

            //Session["periyodik_rapor"] = g.periyodik_rapor(30, DateTime.Now.AddDays(-60), DateTime.Now);
            //Response.Redirect("/Baski.aspx?tip=periyodik_rapor");


        }
        protected void btnCariTel_Click(object sender, EventArgs e)
        {
            radiusEntities dc = MyContext.Context("TOL");
            List<carihesap> cariler = dc.carihesaps.ToList();
            if (cariler.Count > 0)
            {
                foreach (carihesap h in cariler)
                {
                    customer c = dc.customers.FirstOrDefault(x => x.CustID == h.MusteriID);
                    if (c != null)
                    {
                        string tel = c.telefon;
                        if (!String.IsNullOrEmpty(tel))
                        {
                            h.telefon = tel;
                        }
                        string ad = c.Ad;
                        if (!String.IsNullOrEmpty(ad))
                        {
                            h.adi = ad;
                        }
                    }

                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }

        protected void btnSMS_Click(object sender, EventArgs e)
        {
            //SmsNetGsm sms = new SmsNetGsm("5052505264", "MATRIX");
            //sms.mesaj = "DENEME";
            //sms.gonderen = "TOLINTERNET";
            //string[] teller = new string[] { "05069468693" };
            //sms.tel_nolar = teller;
            //Session["mesele"] = sms.TekMesajGonder();
            //Response.Redirect("/Sonuc");
            foreach (System.Collections.DictionaryEntry entry in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove((string)entry.Key);
            }


        }

        protected void btnSatis_Click(object sender, EventArgs e)
        {
            //ServisIslemleri s = new ServisIslemleri("TOL");
            //string kimlik = Araclar.KimlikUret(10);
            ////s.servisEkleKararli(1, 52, "onemlidegil", "otomatik servis", null, 1, "0", kimlik, "oto satis", "Serdar", DateTime.Now);

            //Response.Redirect("/Musteri");

        }

        protected void btnCihazGuncelle_Click(object sender, EventArgs e)
        {
            //kullanici_repo kul=KullaniciIslem.currentKullanici();
            //Kurulum k = new Kurulum(kul.Firma);
            //k.CihazGuncelle();
            //radiusEntities dc = MyContext.Context("TOL");
            //List<adminliste> adminler = dc.adminlistes.ToList();
            //int count = 0;
            //foreach (adminliste ad in adminler)
            //{
            //    bayi_carihesaps h = new bayi_carihesaps();
            //    h.adi = ad.FirmaTam;
            //    h.NetAlacak = 0;
            //    h.NetBorc = 0;
            //    h.telefon = ad.tel;
            //    h.ToplamAlacak = 0;
            //    h.ToplamBakiye = 0;
            //    h.ToplamBorc = 0;
            //    h.ToplamOdedigimiz = 0;
            //    h.ToplamOdenen = 0;
            //    h.username = ad.username;
            //    dc.bayi_carihesaps.Add(h);
            //    KaydetmeIslemleri.kaydetR(dc);
            //    count++;
            //}
            //Session["mesele"] = count.ToString();
            //Response.Redirect("/Deneme.aspx");

        }

        protected void btnMusteriOnline_Click(object sender, EventArgs e)
        {
            //MusteriOnline m = new MusteriOnline("TOL", "5027546699");
            //MusteriDetay detay = m.DetayGoster();
            //string fiyat = detay.musteri.paket_fiyat.ToString();
            //FaturaIslemleri fat = new FaturaIslemleri("TOL");
            //FaturaOzet ozet = fat.FaturaOdeTur(1, 1000, "TOL", false, 7, "Kart", -2, "felan filan", null, "", null, false, "", null, DateTime.Now, null);
            //Session["mesele"] = 0;
            //Response.Redirect("/Sonuc");
        }

        protected void btnOdemeDeneme_Click(object sender, EventArgs e)
        {
            //Odeme o = new Odeme("TOL", "Serdar");
            //o.kullanici = "Serdar";
            //o.KullaniciID = "Serdar";
            //o.mahsup = true;
            //o.mahsup_key = "123456789";
            ////buradaki müşteri ID' seçilen ikinci müşteri olmalı
            //o.Musteri_ID = 59;
            //o.OdemeMiktar = 100;
            //o.OdemeTarih = DateTime.Now;
            //o.Aciklama = "elle müşteri kartı denemesi";
            //o.duzensiz = true;
            //o.masraf_id = -1;
            //o.masraf_tipi = "Satın Alma";
            //o.Kart(1, -1, false);
        }

        protected void btnRadcheck_Click(object sender, EventArgs e)
        {
            //radiusEntities dc = MyContext.Context("DURU");
            //List<radcheck> liste = dc.radchecks.ToList();
            //List<rm_users> kullanicilar = dc.rm_users.ToList();
            //foreach (rm_users r in kullanicilar)
            //{
            //    if (r.owner != "haliltayamir")
            //    {
            //        dc.rm_users.Remove(r);
            //    }
            //}
            //dc.SaveChanges();

            //foreach (radcheck k in liste)
            //{
            //    rm_users r = kullanicilar.FirstOrDefault(x => x.username == k.username);
            //    if (r == null)
            //    {
            //        dc.radchecks.Remove(k);

            //    }
            //}

            //dc.SaveChanges();
        }

        protected void btnIslemTarih_Click(object sender, EventArgs e)
        {
            //radiusEntities dc = MyContext.Context("TOL");
            //List<fatura> faturalar = dc.faturas.Where(x => x.sattis_tarih == null && x.tur == "Taksit").ToList();
            //foreach (fatura f in faturalar)
            //{
            //    f.sattis_tarih = f.islem_tarihi;

            //}

            //dc.SaveChanges();
            yaziDuzelt();
        }

        protected void caching_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Cache.Remove("config");

            List<string> keys = new List<string>();

            IDictionaryEnumerator enumerator = Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            for (int i = 0; i < keys.Count; i++)
            {
                Cache.Remove(keys[i]);
            }
            foreach (System.Collections.DictionaryEntry entry in HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove((string)entry.Key);

            }
            // DateTime bas = DateTime.Now;
            // Performans p = new Performans("TOL");
            // List<customer> list = p.liste();
            // string s = "";
            // foreach (var item in list)
            // {
            //    s += item.Ad;
            // }

            // p.ekle();
            // p.sil();
            // DateTime son = DateTime.Now;

            //int sonuc= (son - bas).Milliseconds;
            ////Debug.Write(sonuc);
            //DateTime bas2 = DateTime.Now;
            //using (radiusEntities dc=MyContext.Context("TOL"))
            //{
            //    entegre p2 = new entegre(dc);
            //    List<customer> list2 = p2.liste();
            //    string s2 = "";
            //    foreach (var item in list2)
            //    {
            //        s2 += item.Ad;
            //    }

            //    p2.ekle();
            //    p2.sil();

            //}
            //DateTime son2 = DateTime.Now;

            //int sonuc2 = (son2 - bas2).Milliseconds;
            ////Debug.Write(sonuc2);
            ////txtCache.Text = (sonuc2 - sonuc).ToString();
            //Debug.Write((sonuc2 - sonuc).ToString());
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
           
            using (radiusEntities dc = MyContext.Context("TOL"))
            {
                Kurulum k = new Kurulum(MyContext.Context("TOL"));
                k.ClearDB();
                //TestSinifi t = new TestSinifi(dc);
                //t.Stress();
                //t.SatisEkle();
            }

            //using (radiusEntities dc = MyContext.Context("TOL"))
            //{
            //    Abonelik a = new Abonelik(dc);
            //    a.KrediYukleToplu(282, 1, 3);

            //}
        }

        protected void btnCache_Click(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context("TOL"))
            {
                AyarCurrent cur = new AyarCurrent(dc);
                ayargenel ay = cur.get();
                txtCache.Text = ay.sonfatura.ToString();
            }
        }



    }
}