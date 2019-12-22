using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikTeknik
{
    public partial class ServisPaketler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            this.Master.kisiarama = false;
            this.Master.servisarama = false;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
                // detaylara bakalım
                DetayGoster(dc);
            }



        }

        private void DetayGoster(radiusEntities dc)
        {
            if (!IsPostBack)
            {
                string pid = Request.QueryString["paketid"];
                if (!String.IsNullOrEmpty(pid))
                {
                    //paketin detaylarını çekelim
                    string firma = KullaniciIslem.firma();
                    ServisPaketli p = new ServisPaketli(dc);
                    PaketOzet oz = p.PaketGoster(Int32.Parse(pid));
                    Session["detay"] = oz.detaylar;
                    txtAciklama.Text = oz.paket_bilgileri.aciklama;
                    txtPaketAdi.Text = oz.paket_bilgileri.paket_adi;


                }
            }


            if (Session["detay"] != null)
            {
                List<Detay_Repo> detaylar = (List<Detay_Repo>)Session["detay"];

                //deratların özelliklerini toplama atalım

                decimal tutar = 0;



                if (detaylar.Count > 0)
                {

                    tutar = detaylar.Sum(x => x.Yekun);

                }

                toplam_tutar.Text = tutar.ToString();

                grdDetay.DataSource = detaylar;
                grdDetay.DataBind();
                upBilgi.Update();
            }
        }



        public void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
            }

        }
        protected void CihazAraa(radiusEntities dc)
        {

            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtCihazAra.Value;
            if (string.IsNullOrEmpty(terim))
            {
                grdCihaz.DataSource = c.CihazListesiComp();

            }
            else
            {
                grdCihaz.DataSource = c.CihazListesiComp(terim, true);

            }
            if (!IsPostBack)
            {
                drdGrup.AppendDataBoundItems = true;
                drdGrup.DataSource = c.CihazGruplar();
                drdGrup.DataValueField = "grupid";
                drdGrup.DataTextField = "grupadi";

            }
            grdCihaz.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }


        protected void grdCihaz_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cihaz = (grdCihaz.SelectedRow.FindControl("btnRandom") as LinkButton).Text;
            string sure = grdCihaz.SelectedRow.Cells[4].Text;
            txtCihaz.Text = cihaz;
            txtGarantiSure.Value = sure.ToString();


        }

        protected void grdDetay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    if (Session["detay"] != null)
                    {
                        List<Detay_Repo> detaylar = (List<Detay_Repo>)Session["detay"];

                        string id = e.CommandArgument.ToString();

                        Detay_Repo d = detaylar.FirstOrDefault(x => x.detay_id == id);
                        detaylar.Remove(d);
                        Session["detay"] = detaylar;
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            DetayGoster(dc);
                        }

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }


                }


            }
        }

        protected void grdDetay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnDetayEkle_Click(object sender, EventArgs e)
        {
            grdCihaz.SelectedIndex = -1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
        }

        protected void btnDetayKaydet_Click(object sender, EventArgs e)
        {

            int? cihaz_id = null;
            string ad = "";
            if (grdCihaz.SelectedValue != null)
            {
                cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                ad = grdCihaz.SelectedRow.Cells[2].Text;
            }
            int adet = Int32.Parse(txtAdet.Text);

            decimal kdv = 18;
            if (!string.IsNullOrEmpty(txtKdv.Text))
            {
                kdv = decimal.Parse(txtKdv.Text);
            }



            decimal yekun = decimal.Parse(txtYekun.Text);

            decimal kdvTutari = Math.Round((yekun * kdv) / (kdv + 100), 2);

            List<Detay_Repo> detaylar = new List<Detay_Repo>();
            if (Session["detay"] != null)
            {
                detaylar = (List<Detay_Repo>)Session["detay"];
            }

            Detay_Repo detay = new Detay_Repo();
            detay.Aciklama = txtDetayAciklama.Text;
            detay.adet = adet;
            detay.detay_id = Araclar.KimlikUret(5);
            detay.cihaz_adi = ad;
            detay.cihaz_id = cihaz_id;
            detay.IslemParca = txtIslemParca.Text;
            detay.paket_id = 0;
            detay.cihaz_gsure = Int32.Parse(txtGarantiSure.Value);

            //buradaki hesaplanmış kdv

            detay.KDV = kdvTutari;

            detay.Yekun = yekun;

            detaylar.Add(detay);
            Session["detay"] = detaylar;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                DetayGoster(dc);
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kalem Eklendi!');");
            sb.Append("$('#detayModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);

        }

        protected void btnCihazKaydet_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            int sure = Int32.Parse(garanti_suresi.Text);
            int grupid = Int32.Parse(drdGrup.SelectedValue);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.Yeni(ad, acik, sure, grupid, "");
                CihazAraa(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


        }

        protected void btnYeniCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('hide');");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }

        protected void btnAlimKaydet_Click(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisPaketli a = new ServisPaketli(dc);
                string pid = Request.QueryString["paketid"];

                PaketRepo hesap = new PaketRepo();
                hesap.aciklama = txtAciklama.Text;
                hesap.paket_adi = txtPaketAdi.Text;
                hesap.tutar = Decimal.Parse(toplam_tutar.Text);


                a.paket = hesap;


                if (Session["detay"] != null)
                {
                    a.detay = (List<Detay_Repo>)Session["detay"];

                }

                if (String.IsNullOrEmpty(pid))
                {
                    a.PaketKaydet();
                }
                else
                {
                    int id = Int32.Parse(pid);
                    a.paket_guncelle(id);
                }

                Session["detay"] = null;
            }

            Response.Redirect("/TeknikTeknik/ServisPaketleri");

        }
    }
}