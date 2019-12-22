using ServisDAL;
using System;
using System.Collections.Generic;
using TeknikServis.Logic;
using System.Web.UI;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikCari
{
    public partial class Ode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("servis"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }

            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                string custidd = Request.QueryString["custid"];

                if (!IsPostBack)
                {

                    
                    //string man = bayi(kullanici);
                    //string man2 = bayi2(kul);
                    var poslar = dc.kart_tanims.Where(x => x.kart_id != -1).ToList();
                    var bankalar = dc.bankas.ToList();
                    var tipler = dc.masraf_tips.OrderBy(x => x.tip_id).ToList();

                    drdMasrafTip.AppendDataBoundItems = true;
                    drdMasrafTip.DataSource = tipler;
                    drdMasrafTip.DataValueField = "tip_id";
                    drdMasrafTip.DataTextField = "tip_adi";

                    drdPos.AppendDataBoundItems = true;
                    drdPos.DataSource = poslar;
                    drdPos.DataValueField = "kart_id";
                    drdPos.DataTextField = "kart_adi";

                    drdBanka.AppendDataBoundItems = true;
                    drdBanka.DataSource = bankalar;
                    drdBanka.DataValueField = "banka_id";
                    drdBanka.DataTextField = "banka_adi";


                    if (Session["alacak"] != null)
                    {
                        txtTutar.Text = Session["alacak"].ToString();
                        Session["alacak"] = null;
                    }
                    DataBind();

                }
            }
         
        }

        
        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            string aciklama = txtAciklama.Text;
 
            string card = Request.QueryString["kartid"];
            string tarihimiz = tarih2.Value;
            DateTime tar = DateTime.Now;
            if (!String.IsNullOrEmpty(tarihimiz))
            {
                tar = DateTime.Parse(tarihimiz);
            }

            if (!String.IsNullOrEmpty(custidd))
            {
                if (String.IsNullOrEmpty(card))
                {
                    bool standart = false;
                    int? masraf_tipi = null;
                    string secilen_tip = drdMasrafTip.SelectedValue;
                    if (secilen_tip != "-1")
                    {

                        standart = false;
                    }
                    else
                    {
                        standart = true;
                    }

                    masraf_tipi = Int32.Parse(secilen_tip);
                    int custid = Int32.Parse(custidd);


                    decimal tutar = Decimal.Parse(txtTutar.Text);
                    using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                    {
                        Odeme o = new Odeme(dc);

                        o.masraf_id = masraf_tipi;
                        o.masraf_tipi =  drdMasrafTip.SelectedItem.ToString();
                        o.OdemeMiktar = tutar;
                        o.OdemeTarih = tar;
                        o.Musteri_ID = custid;
                        o.KullaniciID = custid.ToString();
                        o.kullanici = "firma";
                        o.Aciklama = aciklama;
                        o.mahsup = false;
                        o.duzensiz = standart;
                        o.Nakit(User.Identity.Name);

                     
                     
                    }
               

                    //makbuzYazdir(custid, tutar, aciklama, kullanici);

                    if (chcKal.Checked != true)
                    {
                        Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append(" alertify.success('Kayıt yapıldı');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
                    }

                }
                else
                {
                    int kartid = Int32.Parse(card);

                    decimal tutar = Decimal.Parse(txtTutar.Text);
                    using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                    {
                        Odeme o = new Odeme(dc);
                        o.KartaOdemeKasa(kartid, tutar, tar, aciklama,User.Identity.Name);
                    }
                   
             
                    Response.Redirect("/TeknikCari/OdemeTahsilatlar");
                }

            }

        }


        private void makbuzYazdir(int custid, decimal tutar, string aciklama, kullanici_repo kullanici)
        {

             
            string firma = kullanici.Firma;

            string firmaTam = kullanici.FirmaTam;
            string web = kullanici.Web;
            string tel = kullanici.Telefon;
            string adr = kullanici.Adres;
            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
            {
                FaturaPrinter pr = new FaturaPrinter(dc);
                //pr.MakbuzBas(custid, firmaTam, tel, web, aciklama, adr, tutar.ToString());
            }

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
            string card = Request.QueryString["kartid"];

            string tarihimiz = tarih2.Value;
            DateTime tar = DateTime.Now;
            if (!String.IsNullOrEmpty(tarihimiz))
            {
                tar = DateTime.Parse(tarihimiz);
            }
            if (!String.IsNullOrEmpty(custidd))
            {
                
                int custid = Int32.Parse(custidd);
                int pos_id = Int32.Parse(drdPos.SelectedValue);
                int tak = 1;
                bool standart = false;
                int? masraf_tipi = null;
                string secilen_tip = drdMasrafTip.SelectedValue;
                if (secilen_tip != "-1")
                {

                    standart = false;
                }
                else
                {
                    standart = true;
                }
                bool transfer = false;
                if (chcPesin.Checked == true)
                {
                    transfer = true;
                }
                masraf_tipi = Int32.Parse(secilen_tip);
                if (!String.IsNullOrEmpty(txtTaksit.Text))
                {
                    tak = Int32.Parse(txtTaksit.Text);
                }

                if (String.IsNullOrEmpty(card))
                {
                    if (pos_id > -1)
                    {
                        //bool standart = false;
                        //if (chcDuzensiz.Checked == true)
                        //{
                        //    standart = false;
                        //    //standart olursa normal ödeme oluyor
                        //    //değilse cariyi etkilemiyor
                        //}
                        //else
                        //{
                        //    standart = true;
                        //}

                        decimal tutar = Decimal.Parse(txtTutar.Text);
                        string aciklama = txtAciklama.Text;
                        using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                        {
                            Odeme o = new Odeme(dc);
                            o.OdemeMiktar = tutar;
                            o.masraf_id = masraf_tipi;
                            o.masraf_tipi = drdMasrafTip.SelectedItem.ToString();
                            o.OdemeTarih = tar;
                            o.Musteri_ID = custid;
                            o.KullaniciID = custid.ToString();
                            o.kullanici = "firma";
                            o.Aciklama = aciklama;
                            o.mahsup = false;

                            o.duzensiz = standart;
                            o.Kart(tak, pos_id, transfer,User.Identity.Name);
                            //makbuzYazdir(custid, tutar, aciklama, kullanici);
                        }
                     

                        Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);
                    }
                    else
                    {
                        if (pos_id > -1)
                        {
                            int kartid = Int32.Parse(card);
                            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                            {
                                Kart k = new Kart(dc);
                                k.ExtreOde(kartid, "Kart", tak, pos_id, null,User.Identity.Name);
                            }
                           
                            Response.Redirect("/TeknikCari/OdemeTahsilatlar");
                        }


                    }
                }
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

        //protected void btnCek_Click(object sender, EventArgs e)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    sb.Append(@"<script type='text/javascript'>");
        //    sb.Append("$('#cekModal').modal('show');");
        //    sb.Append(@"</script>");
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CekShowModalScript", sb.ToString(), false);
        //}

        protected void btnBankaKaydet_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            string card = Request.QueryString["kartid"];
            
            int banka_id = Int32.Parse(drdBanka.SelectedValue);
            int custid = Int32.Parse(custidd);
            
            DateTime tar = DateTime.Now;
            string tarihimiz = tarih2.Value;

            if (!String.IsNullOrEmpty(tarihimiz))
            {
                tar = DateTime.Parse(tarihimiz);
            }
            if (!String.IsNullOrEmpty(custidd))
            {
                if (String.IsNullOrEmpty(card))
                {

                    if (banka_id > -1)
                    {
                        decimal tutar = Decimal.Parse(txtTutar.Text);

                        string aciklama = txtAciklama.Text;

                        bool standart = false;
                        int? masraf_tipi = null;
                        string secilen_tip = drdMasrafTip.SelectedValue;
                        if (secilen_tip != "-1")
                        {

                            standart = false;
                        }
                        else
                        {
                            standart = true;
                        }
                        masraf_tipi = Int32.Parse(secilen_tip);
                        using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                        {
                            Odeme o = new Odeme(dc);
                            o.OdemeMiktar = tutar;
                            o.OdemeTarih = tar;
                            o.masraf_id = masraf_tipi;
                            o.masraf_tipi = drdMasrafTip.SelectedItem.ToString();
                            o.Musteri_ID = custid;
                            o.KullaniciID = custid.ToString();
                            o.kullanici = "firma";
                            o.Aciklama = aciklama;
                            o.mahsup = false;
                            o.duzensiz = standart;
                            o.Banka(banka_id,User.Identity.Name);

                            //makbuzYazdir(custid, tutar, aciklama, kullanici);

                        }
           

                        Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);
                    }
                    else
                    {
                        int kartid = Int32.Parse(card);
                        if (banka_id > -1)
                        {
                            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                            {
                                Kart k = new Kart(dc);
                                k.ExtreOde(kartid, "Banka", null, null, banka_id,User.Identity.Name);
                            }
                           
                            Response.Redirect("/TeknikCari/OdemeTahsilatlar");
                        }
                    }
                }
            }
        }

        //protected void btnCekKaydet_Click(object sender, EventArgs e)
        //{
        //    string custidd = Request.QueryString["custid"];


        //    if (!String.IsNullOrEmpty(custidd))
        //    {

        //        int custid = Int32.Parse(custidd);

        //        decimal tutar = Decimal.Parse(txtTutar.Text);

        //        string aciklama = txtAciklama.Text;

             
        //        string tarihimiz = tarih2.Value;
        //        DateTime tar = DateTime.Now;
        //        if (!String.IsNullOrEmpty(tarihimiz))
        //        {
        //            tar = DateTime.Parse(tarihimiz);
        //        }
        //        string belge_no = txtCekBelgeNo.Text;
        //        DateTime vade_tarih = DateTime.Parse(tarih.Value);
        //        //decimal masraf = Decimal.Parse(txtCekMasraf.Text);
        //        bool standart = false;
        //        int? masraf_tipi = null;
        //        string secilen_tip = drdMasrafTip.SelectedValue;
        //        if (secilen_tip != "-1")
        //        {

        //            standart = false;
        //        }
        //        else
        //        {
        //            standart = true;
        //        }
        //        masraf_tipi = Int32.Parse(secilen_tip);

        //        using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
        //        {
        //            Odeme o = new Odeme(dc);
        //            o.OdemeMiktar = tutar;
        //            o.OdemeTarih = tar;
        //            o.masraf_tipi = drdMasrafTip.SelectedItem.ToString();
        //            o.masraf_id = masraf_tipi;
        //            o.Musteri_ID = custid;
        //            o.KullaniciID = custid.ToString();
        //            o.kullanici = "firma";
        //            o.Aciklama = aciklama;
        //            o.mahsup = false;
        //            o.duzensiz = standart;
        //            o.Cek(belge_no, vade_tarih);


        //            //makbuzYazdir(custid, tutar, aciklama, kullanici);
        //        }
            


        //        Response.Redirect("/TeknikCari/Odemeler.aspx?custid=" + custid);


        //    }
        //}

        protected void grdMahsup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}