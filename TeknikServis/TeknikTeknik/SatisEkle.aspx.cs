using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikTeknik
{
    public partial class SatisEkle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    ServisIslemleri s = new ServisIslemleri(dc);
                    cihazGoster(s);

                    drdPaketler.AppendDataBoundItems = true;

                    drdPaketler.DataSource = s.servis_paketleri();
                    drdPaketler.DataValueField = "paket_id";
                    drdPaketler.DataTextField = "paket_adi";
                    drdPaketler.DataBind();
                    txtGarantiSuresi.Text = "24";

                }
            }


        }
        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];

            DateTime karar_tarihi = DateTime.Now;
            string tarS = tarih2.Value;
            if (!String.IsNullOrEmpty(tarS))
            {
                karar_tarihi = DateTime.Parse(tarS);
            }

            if (!String.IsNullOrEmpty(custidd))
            {


                //yeni ekleme


                int custid = Int32.Parse(custidd);

                string paket = drdPaketler.SelectedValue;
                int? paket_id = null;
                if (paket != "-1")
                {
                    paket_id = Int32.Parse(paket);
                }
                string kimlik = Araclar.KimlikUret(10);

                if (paket_id == null)
                {
                    if (!string.IsNullOrEmpty(txtKDV.Text) && !String.IsNullOrEmpty(txtYekun.Text))
                    {

                        string islem = "Satış";
                        if (!String.IsNullOrEmpty(txtIslemParca.Value))
                        {
                            islem = txtIslemParca.Value;
                        }
                        decimal kdv = Decimal.Parse(txtKDV.Text);
                        decimal yekun = Decimal.Parse(txtYekun.Text);
                        string aciklama = "Satış";
                        if (!String.IsNullOrEmpty(txtAciklama.Text))
                        {
                            aciklama = txtAciklama.Text;
                        }

                        int adet = 1;
                        string adet_s = txtAdet.Text;
                        if (!String.IsNullOrEmpty(adet_s))
                        {
                            adet = Int32.Parse(adet_s);
                        }
                        int cihaz_id = -1;
                        string cihaz = txtCihazAdiGoster.Value;
                        if (grdCihaz.SelectedIndex > -1)
                        {

                            cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);

                        }

                        int? secilen_cihaz = null;
                        string sure = hdnGarantiSure.Value;
                        int gsure = 1;
                        if (!string.IsNullOrEmpty(sure))
                        {
                            gsure = Int32.Parse(sure);
                        }
                        if (cihaz_id > -1)
                        {

                            secilen_cihaz = (int)cihaz_id;
                        }
                        string konu = "Satış-" + islem + "-" + cihaz;
                        karar_wrap w = new karar_wrap { aciklama = aciklama, adet = adet, cihaz_adi = cihaz, cihaz_gsure = gsure, cihaz_id = secilen_cihaz, yekun = yekun, kdv = kdv, islemParca = islem };

                        //bu satış eklenmeden önceki cari durumunu döndürüyor
                        //böylece gelen değere göre cariden ödeme akaydı giriliyor
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            ServisIslemleri s = new ServisIslemleri(dc);
                            decimal? cari = s.servisEkleKararli(null, custid, "firma", islem, kimlik, konu, karar_tarihi, w, User.Identity.Name);
                            if (cari != null)
                            {
                                //FaturaIslemleri fat = new FaturaIslemleri(dc);
                                //fat.FaturaOdeCariEntegre(custid, karar_tarihi, (decimal)cari,User.Identity.Name);
                                Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);
                            }
                            else
                            {
                                //stok sorunu var
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                sb.Append(@"<script type='text/javascript'>");

                                sb.Append(" alertify.error('Paket stoklarından biri yada birkaçı yetersiz!');");

                                sb.Append(@"</script>");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddsHideModalScript", sb.ToString(), false);
                            }
                        }


                        //Response.Redirect("/Sonuc");

                    }
                    else
                    {
                        //uyarı


                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append(" alertify.error('Lütfen alanları doldurun yada bir PAKET seçin!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddsHideModalScript", sb.ToString(), false);
                    }
                }
                else
                {
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        decimal? cari = s.servisEkleKararli(paket_id, custid, "firma", "Satış", kimlik, "Satış", karar_tarihi, null, User.Identity.Name);
                        if (cari != null)
                        {
                            //FaturaIslemleri fat = new FaturaIslemleri(dc);
                            //fat.FaturaOdeCariEntegre(custid, karar_tarihi, (decimal)cari,User.Identity.Name);
                            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);
                        }
                        else
                        {
                            //stok sorunu var
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");

                            sb.Append(" alertify.error('Cihaz stoğu yetersiz!');");

                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddsHideModalScript", sb.ToString(), false);
                        }
                    }

                }



            }


        }





        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                cihazGoster(s);
            }

            string cihaz = (grdCihaz.SelectedRow.FindControl("btnRandom") as LinkButton).Text;
            string sure = grdCihaz.SelectedRow.Cells[4].Text;
            txtCihazAdiGoster.Value = cihaz;
            hdnGarantiSure.Value = sure.ToString();

        }

        protected void btnAddCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            //sb.Append("document.getElementById('ContentPlaceHolder1_txtAra').value = '';");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }


        protected void btnAddCihazRecord_Click(object sender, EventArgs e)
        {
            string gs = txtGarantiSuresi.Text;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                s.CihazEkle(txtCihazAdi.Text, txtCihazAciklama.Text, gs);
                cihazGoster(s);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            //sb.Append("document.getElementById('ContentPlaceHolder1_txtAra').value = document.getElementById('ContentPlaceHolder1_txtAdi').value;");
            //sb.Append("alert('Record Added Successfully');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                cihazGoster(s);
            }

        }

        private void cihazGoster(ServisIslemleri s)
        {
            string arama_terimi = txtAra.Value;
            if (!String.IsNullOrEmpty(arama_terimi))
            {

                grdCihaz.DataSource = s.CihazGoster(arama_terimi);
                grdCihaz.DataBind();

            }
            else
            {
                grdCihaz.DataSource = s.CihazGoster();
                grdCihaz.DataBind();
            }
        }
    }
}