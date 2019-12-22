using ServisDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class ServislerCanli : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            this.Master.servisarama = false;
            this.Master.kisiarama = false;
            kullanici_repo kullanici = KullaniciIslem.currentKullanici();

            string firma = kullanici.Firma;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                if (!IsPostBack)
                {
                    List<Radius.service_tips> tipler = ayarimiz.tipListesiR();
                    drdTip.AppendDataBoundItems = true;
                    drdTip.DataSource = ayarimiz.tipListesiR();
                    drdTip.DataValueField = "tip_id";
                    drdTip.DataTextField = "tip_ad";
                    drdTip.DataBind();


                    drdTipSec.AppendDataBoundItems = true;
                    drdTipSec.DataSource = ayarimiz.tipListesiR();
                    drdTipSec.DataValueField = "tip_id";
                    drdTipSec.DataTextField = "tip_ad";
                    drdTipSec.DataBind();

                    gosterHepsi(kullanici, dc);
                }


            }


        }

        private void gosterHepsi(kullanici_repo kull, radiusEntities dc)
        {
            string custidd = Request.QueryString["custid"];
            string urun_kimlik = Request.QueryString["urun_kimlik"];

            string servis_tipi = Request.QueryString["servistipi"];
            string kelime = "";
            string kulid = null;

            if (!User.IsInRole("Admin") && !User.IsInRole("mudur"))
            {
                kulid = kull.id;
            }

            if (!String.IsNullOrEmpty(txtAra.Value))
            {
                kelime = txtAra.Value;
            }
            else if (Session["kriter_must"] != null)
            {

                kelime = Session["kriter_must"].ToString();

            }


            if (!String.IsNullOrEmpty(urun_kimlik))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                GridView1.DataSource = ser.servisUrun(urun_kimlik, servis_tipi);
                GridView1.DataBind();
            }
            else
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                GridView1.DataSource = ser.ServisListesi(servis_tipi, custidd, kulid, kelime);
                GridView1.DataBind();
            }

        }


        protected void Page_Unload(object sender, EventArgs e)
        {
            Session["kriter_must"] = null;
        }

        public void MusteriAra(object sender, EventArgs e)
        {
            kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            using (radiusEntities dc = MyContext.Context(kullanici.Firma))
            {
                gosterHepsi(kullanici, dc);
            }


        }
        private void ServisSesAra(kullanici_repo kull, radiusEntities dc)
        {
            string s = Session["kriter_must"].ToString();

            ServisIslemleri ser = new ServisIslemleri(dc);

            if (HttpContext.Current.User.IsInRole("Admin") || User.IsInRole("mudur"))
            {
                //GridView1.DataSource = ser.servisArakAcikR(s);
                baslik.InnerHtml = "Bütün Açık Servisler";
            }

            else
            {
                //GridView1.DataSource = ser.servisAraAcikKullaniciR(s, kull.id);
                baslik.InnerHtml = "Bütün Açık Servisler";
            }
        }

        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //(e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisDetayList.aspx?kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                //bütün servis hesaplarını gösterecek //oradan hesap eklenebilir yada detaytan hesap eklenebilir
                //(e.Row.FindControl("btnHesap") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisHesaplar.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                //(e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisDetayList.aspx?kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");

                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(lblID.Value);
            int musID = Int32.Parse(custIDHdn.Value);
            string aciklama = txtAciklama_4.Text;
            int tip = Int32.Parse(drdTip.SelectedValue);
            string konu = txtBaslik_2.Text;


            kullanici_repo kul = KullaniciIslem.currentKullanici();
            using (radiusEntities dc = MyContext.Context(kul.Firma))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                s.servisGuncelleR(id, musID, aciklama, tip, konu, User.Identity.Name);
                gosterHepsi(kul, dc);
                //GridView1.DataBind();
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Servis bilgisi güncellendi!');");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
            //ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction();", true); 

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikTeknik/ServisEkle.aspx");

        }
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    kullanici_repo kullanici = KullaniciIslem.currentKullanici();
                    int serviceID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(kullanici.Firma))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.servisIptalR(serviceID, User.Identity.Name);
                        gosterHepsi(kullanici, dc);
                    }


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
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
            else if (e.CommandName.Equals("editRecord"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = GridView1.Rows[index];

                //template kullandığımda bu kodu kullanmıştım.
                //LinkButton link=gvrow.Cells[2].Controls[1] as LinkButton;
                lblID.Value = HttpUtility.HtmlDecode(gvrow.Cells[2].Text);

                txtBaslik_2.Text = HttpUtility.HtmlDecode(gvrow.Cells[5].Text);
                custIDHdn.Value = HttpUtility.HtmlDecode(gvrow.Cells[13].Text);
                txtmusteriAdi_3.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);
                txtAciklama_4.Text = HttpUtility.HtmlDecode(gvrow.Cells[6].Text);
                txtSonDurum_5.Text = HttpUtility.HtmlDecode(gvrow.Cells[8].Text);
                string urunumuz = HttpUtility.HtmlDecode(gvrow.Cells[9].Text).Trim();
                if (String.IsNullOrEmpty(urunumuz))
                {
                    urunumuz = "Ürün kaydı yok";
                }
                txtUrun_6.Text = urunumuz;
                //txtServisTipi_7.Text = HttpUtility.HtmlDecode(gvrow.Cells[10].Text);
                drdTip.SelectedValue = HttpUtility.HtmlDecode(gvrow.Cells[12].Text);

                lblResult.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);

            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string css = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "css"));
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(css);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Aktif Servisleri-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Aktif Servisleri-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            GridView1.AllowPaging = false;
            GridView1.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "GridPrint", sb.ToString(), false);
            GridView1.AllowPaging = true;



        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            kullanici_repo kullanici = KullaniciIslem.currentKullanici();
            using (radiusEntities dc = MyContext.Context(kullanici.Firma))
            {
                gosterHepsi(kullanici, dc);
            }

        }

        protected void drdTipSec_SelectedIndexChanged(object sender, EventArgs e)
        {

            string servis_tipi = drdTipSec.SelectedValue;
            if (servis_tipi != "-1")
            {
                string custidd = Request.QueryString["custid"];
                string urun_kimlik = Request.QueryString["urun_kimlik"];
                string uri = "/TeknikTeknik/ServislerCanli.aspx?";
                if (!String.IsNullOrEmpty(custidd))
                {
                    uri += "custid=" + custidd;
                }
                if (!String.IsNullOrEmpty(urun_kimlik))
                {
                    uri += "urun_kimlik=" + urun_kimlik;
                }
                uri += "&servistipi=" + servis_tipi;
                Response.Redirect(uri);
            }

        }

        protected void barkod_TextChanged(object sender, EventArgs e)
        {
            //(e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
            if (!String.IsNullOrEmpty(barkod.Text))
            {
                string bar = barkod.Text;
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);

                    ServisDAL.Repo.ServisRepo servis = s.servisAraBarkod(bar);
                    if (servis != null)
                    {
                        string path = "~/TeknikTeknik/Servis.aspx?servisid=" + servis.serviceID + "&custid=" + servis.custID + "&kimlik=" + bar;
                        Response.Redirect(path);
                    }
                }

            }
        }

        protected void btnHarita_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikHarita/ButunServisler");
        }
    }
}