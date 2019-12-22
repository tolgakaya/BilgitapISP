using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ServisDAL;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class FaturaManuel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            //müşteri adıyla geliyorsa müşteri bilgilerini gösterelim
            string custID = Request.QueryString["custID"];
            if (!String.IsNullOrEmpty(custID))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MusteriIslemleri mu = new MusteriIslemleri(dc);
                    Radius.customer musteri = mu.musteriTekR(Int32.Parse(custID));
                    txtIsim.Value = musteri.Ad;
                    txtVD.Text = musteri.Adres;
                    txtTC.Text = musteri.TC;
                }


            }

            KalemGoster();

        }

        private void KalemGoster()
        {
            if (Session["kalemler"] != null)
            {
                List<Kalem> kalemler = (List<Kalem>)Session["kalemler"];
                decimal tutar2 = kalemler.Sum(x => x.tutar);
                decimal kdv = Math.Round(kalemler.Sum(y => y.tutar * 0.18M), 2);
                decimal oiv = 0;
                decimal yekun = tutar2 + kdv;
                txtTutar2.Text = tutar2.ToString();
                txtKDV2.Text = kdv.ToString();
                txtYekun2.Text = yekun.ToString();
                txtOIV2.Text = oiv.ToString();

                grdTip.DataSource = kalemler;
                grdTip.DataBind();
            }
        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            string isim = txtIsim.Value;
            decimal KDV = Decimal.Parse(txtKDV2.Text);
            decimal OIV = Decimal.Parse(txtOIV2.Text);
            DateTime tarih = DateTime.Now; //tarihin ne olacağını bilmiyorum
            string TC = txtTC.Text;
            string VD = txtVD.Text;
            string tarihs = tarih2.Value;
            if (!String.IsNullOrEmpty(tarihs))
            {
                tarih = DateTime.Parse(tarihs);
            }
            Decimal Tutar = Decimal.Parse(txtTutar2.Text);
            Decimal Yekun = Decimal.Parse(txtYekun2.Text);
            string yaziIle = "YALNIZ " + Araclar.yaziyaCevir(Yekun);
            if (Session["kalemler"] != null)
            {
                List<Kalem> kalemler = (List<Kalem>)Session["kalemler"];
                //string firma = TeknikServis.Logic.KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    FaturaBas bas = new FaturaBas(dc);
                    InternetFaturasi internet = bas.FaturaManuel(isim, KDV, OIV, tarih, TC, VD, Tutar, Yekun, kalemler);
                    Session["Fatura_Bilgisi"] = internet;
                }


                Response.Redirect("/Baski.aspx?tip=manuel");
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Hiç fatura kalemi eklemediniz!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
            }


        }
        protected void btnAddTip_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalTip').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2", sb.ToString(), false);
        }
        protected void btnAddRecordTip_Click(object sender, EventArgs e)
        {
            string cinsi = txtCinsi.Text;
            decimal fiyat = Decimal.Parse(txtFiyat.Text);
            decimal tutar = Decimal.Parse(txtTutar.Text);
            int miktar = Int32.Parse(txtMik.Text);

            Kalem kalem = new Kalem();
            kalem.cinsi = cinsi;
            kalem.fiyat = fiyat;
            kalem.mik = miktar;
            kalem.tutar = tutar;

            List<Kalem> kalemler = new List<Kalem>();
            if (Session["kalemler"] != null)
            {
                kalemler = (List<Kalem>)Session["kalemler"];
                kalemler.Add(kalem);
                Session["kalemler"] = kalemler;
            }
            else
            {

                kalemler.Add(kalem);
                Session["kalemler"] = kalemler;
            }

            decimal tutar2 = kalemler.Sum(x => x.tutar);
            decimal kdv = Math.Round(kalemler.Sum(y => y.tutar * 0.18M), 2);
            decimal oiv = 0;
            decimal yekun = tutar2 + kdv;
            txtTutar2.Text = tutar2.ToString();
            txtKDV2.Text = kdv.ToString();
            txtYekun2.Text = yekun.ToString();
            txtOIV2.Text = oiv.ToString();

            grdTip.DataSource = kalemler;
            grdTip.DataBind();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kayıt yapıldı!');");
            // sb.Append(alert);
            sb.Append("$('#addModalTip').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2", sb.ToString(), false);


        }

        protected void grdTip_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


        }

        protected void grdTip_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    if (Session["kalemler"] != null)
                    {
                        List<Kalem> detaylar = (List<Kalem>)Session["kalemler"];

                        string cinsi = Convert.ToString(e.CommandArgument);

                        Kalem d = detaylar.FirstOrDefault(x => x.cinsi == cinsi);
                        detaylar.Remove(d);
                        Session["kalemler"] = detaylar;
                        KalemGoster();
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }
                    //else
                    //{
                    //    List<DetayRepo> detaylar = new List<DetayRepo>();
                    //    if (Session["alimdetay"] != null)
                    //    {
                    //        detaylar = (List<DetayRepo>)Session["alimdetay"];
                    //    }
                    //}




                }


                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }
        }
    }
}