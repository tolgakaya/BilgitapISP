using ServisDAL;
using System;
using System.Collections.Generic;
using TeknikServis.Logic;
using System.Web.UI;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class OdemeEkle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("servis"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                string custidd = Request.QueryString["custid"];

                musteriGoster(dc);

                if (!IsPostBack)
                {


                    pos_banka_look pb = m.posbankalar();

                    drdPos.AppendDataBoundItems = true;
                    drdPos.DataSource = pb.poslar;
                    drdPos.DataValueField = "pos_id";
                    drdPos.DataTextField = "pos_adi";




                    drdBanka.AppendDataBoundItems = true;
                    drdBanka.DataSource = pb.bankalar;
                    drdBanka.DataValueField = "banka_id";
                    drdBanka.DataTextField = "banka_adi";

                    DataBind();

                    //if (Session["borc"] != null)
                    //{
                    //    txtTutar.Text = Session["borc"].ToString();
                    //    Session["borc"] = null;
                    //}



                }
            }

        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            DateTime odeme_tarihi = DateTime.Now;
            string tar = tarih2.Value;
            if (!String.IsNullOrEmpty(tar))
            {
                odeme_tarihi = DateTime.Parse(tar);
            }

            if (!String.IsNullOrEmpty(custidd))
            {

                int custid = Int32.Parse(custidd);

                decimal tutar = Decimal.Parse(txtTutar.Text);

                string aciklama = txtAciklama.Text;

                string firma = KullaniciIslem.firma();



                using (radiusEntities dc = MyContext.Context(firma))
                {
                    //MusteriIslemleri m = new MusteriIslemleri(dc);

                    //FaturaIslemleri fat = new FaturaIslemleri(dc);


                    //FaturaOzet ozet = fat.FaturaOdeTur(custid, tutar, "Nakit", null, aciklama, null, "", null, false, "", null, odeme_tarihi, User.Identity.Name);
                    Tahsilat t = new Tahsilat(dc);
                    t.Aciklama = aciklama;
                    t.kullanici = User.Identity.Name;
                    t.KullaniciID = User.Identity.Name;
                    t.mahsup = false;
                    t.Musteri_ID = custid;
                    t.OdemeMiktar = tutar;
                    t.OdemeTarih = odeme_tarihi;
                    t.Nakit(User.Identity.Name);
                    //Session["mesele"] = ozet.artik.ToString();
                    //Response.Redirect("/Sonuc");

                    if (cbYazdir.Checked == true)
                    {
                        makbuzYazdir(custid, tutar, aciklama, dc);
                    }
                }

                //Response.Redirect("/Sonuc.aspx");
                Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);


            }


        }

        private void TahsilatMesaj(string firma, Radius.customer must, FaturaOzet ozet)
        {
            //tahsilatta mesaja gerek olmaz
            /*  if (chcSms.Checked == true)
              {
                  string ekMesajSms = "Geçerlilik Tarihi: " + Convert.ToDateTime(ozet.KrediMesaj.gecerlilik).ToShortDateString() + "Kullanıcı adı: " + ozet.KrediMesaj.kullaniciAdi + "Şifreniz: " + ozet.KrediMesaj.sifre;
                  AyarIslemleri ayarimiz = new AyarIslemleri(firma);
                  ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(firma);
                  sms.SmsGonder("kredi", -1, KullaniciIslem.kullanici_convert(kullanici), firma, ayarimiz, must.telefon, ekMesajSms);

              }
              if (chcMail.Checked == true)
              {
                  string ekMesaj = "Geçerlilik Tarihi: <b> " + Convert.ToDateTime(ozet.KrediMesaj.gecerlilik).ToShortDateString() + "</b> Kullanıcı adı: <b> " + ozet.KrediMesaj.kullaniciAdi + "</b> Şifreniz: <b> " + ozet.KrediMesaj.sifre + "</b>";
                  ServisDAL.MailIslemleri mi = new MailIslemleri(KullaniciIslem.kullanici_convert(kullanici));
                  mi.SendingMail(must.email, must.Ad, "", -1, "kredi", ekMesaj);
              }*/
        }

        private void makbuzYazdir(int custid, decimal tutar, string aciklama, radiusEntities dc)
        {

            ServisDAL.AyarCurrent ay = new AyarCurrent(dc);
            FaturaBas bas = new FaturaBas(dc);
            Makbuz_Gorunum faturaBilgisi = bas.MakbuzBilgileri(custid, aciklama, ay.get(), tutar, User.Identity.Name);
            Session["Makbuz_Gorunum"] = faturaBilgisi;
            Response.Redirect("/Baski.aspx?tip=tahsilat");

        }

        protected void btnKart_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
        }

        protected void btnKartKaydet_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            DateTime odeme_tarihi = DateTime.Now;
            string tar = tarih2.Value;
            if (!String.IsNullOrEmpty(tar))
            {
                odeme_tarihi = DateTime.Parse(tar);
            }
            if (!String.IsNullOrEmpty(custidd))
            {


                int pos_id = Int32.Parse(drdPos.SelectedValue);
                if (pos_id > -1)
                {
                    int custid = Int32.Parse(custidd);

                    decimal tutar = Decimal.Parse(txtTutar.Text);

                    string aciklama = txtAciklama.Text;
                    string firma = KullaniciIslem.firma();
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        //MusteriIslemleri m = new MusteriIslemleri(dc);

                        //FaturaIslemleri fat = new FaturaIslemleri(dc);

                        //FaturaOzet ozet = fat.FaturaOdeTur(custid, tutar, "Kart", pos_id, aciklama, null, "", null, false, "", null, odeme_tarihi, User.Identity.Name);

                        Tahsilat t = new Tahsilat(dc);
                        t.Aciklama = aciklama;
                        t.kullanici = User.Identity.Name;
                        t.KullaniciID = User.Identity.Name;
                        t.mahsup = false;
                        t.Musteri_ID = custid;
                        t.OdemeMiktar = tutar;
                        t.OdemeTarih = odeme_tarihi;
                        t.Kart(pos_id, User.Identity.Name);
                        if (cbYazdir.Checked == true)
                        {
                            makbuzYazdir(custid, tutar, aciklama, dc);
                        }

                    }

                    Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);

                }
            }
        }

        protected void btnKartMahsup_Click(object sender, EventArgs e)
        {
            hdnMahsup.Value = "Kart";
            txtYansiyan.Text = txtTutar.Text;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#mahsupModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "MahsupShowModalScript", sb.ToString(), false);
        }

        protected void btnAyni_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            DateTime odeme_tarihi = DateTime.Now;
            string tar = tarih2.Value;
            if (!String.IsNullOrEmpty(tar))
            {
                odeme_tarihi = DateTime.Parse(tar);
            }

            if (!String.IsNullOrEmpty(custidd))
            {

                int custid = Int32.Parse(custidd);

                decimal tutar = Decimal.Parse(txtTutar.Text);

                string aciklama = txtAciklama.Text;
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    //MusteriIslemleri m = new MusteriIslemleri(dc);

                    Tahsilat t = new Tahsilat(dc);
                    t.Aciklama = aciklama;
                    t.kullanici = User.Identity.Name;
                    t.KullaniciID = User.Identity.Name;
                    t.mahsup = false;
                    t.Musteri_ID = custid;
                    t.OdemeMiktar = tutar;
                    t.OdemeTarih = odeme_tarihi;
                    t.Ayni(User.Identity.Name);

                    if (cbYazdir.Checked == true)
                    {
                        makbuzYazdir(custid, tutar, aciklama, dc);
                    }
                }

                Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);


            }
        }

        protected void btnBanka_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#bankaModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BankaShowModalScript", sb.ToString(), false);
        }

        protected void btnBankaKaydet_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];


            DateTime odeme_tarihi = DateTime.Now;
            string tar = tarih2.Value;
            if (!String.IsNullOrEmpty(tar))
            {
                odeme_tarihi = DateTime.Parse(tar);
            }

            if (!String.IsNullOrEmpty(custidd))
            {
                int banka_id = Int32.Parse(drdBanka.SelectedValue);
                if (banka_id > -1)
                {
                    int custid = Int32.Parse(custidd);

                    decimal tutar = Decimal.Parse(txtTutar.Text);

                    string aciklama = txtAciklama.Text;
                    string firma = KullaniciIslem.firma();

                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        //MusteriIslemleri m = new MusteriIslemleri(dc);

                        //FaturaIslemleri fat = new FaturaIslemleri(dc);


                        //fat.FaturaOdeTur(custid, tutar, "Banka", banka_id, aciklama, null, "", null, false, "", null, odeme_tarihi, User.Identity.Name);

                        Tahsilat t = new Tahsilat(dc);
                        t.Aciklama = aciklama;
                        t.kullanici = User.Identity.Name;
                        t.KullaniciID = User.Identity.Name;
                        t.mahsup = false;
                        t.Musteri_ID = custid;
                        t.OdemeMiktar = tutar;
                        t.OdemeTarih = odeme_tarihi;
                        t.Banka(banka_id, User.Identity.Name);
                        if (cbYazdir.Checked == true)
                        {
                            makbuzYazdir(custid, tutar, aciklama, dc);
                        }


                    }


                    Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);

                }
            }
        }
        protected void grdMahsup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnKartMahsupKaydet_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];


            if (!String.IsNullOrEmpty(custidd))
            {
                string m = hdnMahsup.Value;
                if (m.Equals("Kart"))
                {
                    kartMahsupKaydet(custidd);
                }


            }
        }
        private void kartMahsupKaydet(string custidd)
        {
            //string custidd = Request.QueryString["custid"];
            DateTime odeme_tarihi = DateTime.Now;
            string tar = tarih2.Value;
            if (!String.IsNullOrEmpty(tar))
            {
                odeme_tarihi = DateTime.Parse(tar);
            }
            if (grdMahsup.SelectedValue != null)
            {
                int mahsup_id = Convert.ToInt32(grdMahsup.SelectedValue);

                //mahsup_id'yi seçemiyor arkadaş yardımcı olunacak
                int custid = Int32.Parse(custidd);

                decimal tutar = Decimal.Parse(txtTutar.Text);

                string aciklama = txtAciklama.Text;


                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    //MusteriIslemleri m = new MusteriIslemleri(dc);

                    //FaturaIslemleri fat = new FaturaIslemleri(dc);

                    string mahsup_key = AletEdavat.KimlikUret(20);
                    decimal yansiyan = tutar;
                    string yansiyanS = txtYansiyan.Text;
                    if (!String.IsNullOrEmpty(yansiyanS))
                    {
                        yansiyan = Decimal.Parse(yansiyanS);
                    }
                    string musteri = dc.customers.FirstOrDefault(x => x.CustID == custid).Ad;
                    string tedarikci = dc.customers.FirstOrDefault(x => x.CustID == mahsup_id).Ad;
                    if (String.IsNullOrEmpty(aciklama))
                    {
                        aciklama = musteri + " kartıyla " + tedarikci + " ödemesi yapıldı";
                    }

                    //fat.FaturaOdeTur(custid, tutar, "Kart", null, aciklama, null, "", null, true, mahsup_key, null, odeme_tarihi, User.Identity.Name);
                    Tahsilat t = new Tahsilat(dc);
                    t.Aciklama = aciklama;
                    t.kullanici = User.Identity.Name;
                    t.KullaniciID = User.Identity.Name;
                    t.mahsup = true;
                    t.mahsup_key = mahsup_key;
                    t.Musteri_ID = custid;
                    t.OdemeMiktar = tutar;
                    t.OdemeTarih = odeme_tarihi;
                    t.Mahsup(User.Identity.Name);

                    //Session["mesele"] = mahsup_id.ToString();
                    ////Response.Redirect("/Sonuc");
                    Odeme o = new Odeme(dc);
                    o.kullanici = "firma";
                    o.KullaniciID = mahsup_id.ToString();
                    o.mahsup = true;
                    o.mahsup_key = mahsup_key;
                    //buradaki müşteri ID' seçilen ikinci müşteri olmalı
                    o.Musteri_ID = mahsup_id;
                    o.OdemeMiktar = yansiyan;
                    o.OdemeTarih = DateTime.Now;
                    o.Aciklama = aciklama;
                    o.duzensiz = true;
                    o.masraf_id = -1;
                    o.masraf_tipi = "Satın Alma";
                    o.Kart(1, -1, false, User.Identity.Name);
                }



                Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);

            }
        }

        private void musteriGoster(radiusEntities dc)
        {

            string s = txtMusteriSorgu.Value;

            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                grdMahsup.DataSource = m.musteriAraCari(s);
                grdMahsup.DataBind();

            }
        }

        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
            }

        }

    }
}