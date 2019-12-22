using ServisDAL;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikCari
{
    public partial class OdemeTahsilatlar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
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
                    goster(dc);
                }
              
            }


        }


        private void goster(radiusEntities dc)
        {
            int gun = 90;
            string gunS = Request.QueryString["gun"];
            if (!String.IsNullOrEmpty(gunS))
            {
                gun = Int32.Parse(gunS);
            }

            Hareket s = new Hareket(dc);

            GenelGorunum gor = s.Gosterge(gun);
            txtAlinanCek.InnerHtml = gor.alinan_cek_toplami.ToString("C");
            txtBanka.InnerHtml = gor.banka_hesaplar_toplami.ToString("C");
            txtExtre.InnerHtml = gor.kart_extre_toplami.ToString("C");
            txtKasa.InnerHtml = gor.kasa_bakiye.ToString("C");
            txtPos.InnerHtml = gor.poslarda_birikenler.ToString("C");
            txtVerilenCek.InnerHtml = gor.verilen_cek_toplami.ToString("C");

            GridView1.DataSource = gor.islemler;

            GridView1.DataBind();
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
                if (confirmValue == "Yes")
                {

                    int id = Convert.ToInt32(e.CommandArgument);

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        Hareket s = new Hareket(dc);
                        s.odemeIptalR(id, User.Identity.Name);
                        goster(dc);
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }

        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipi = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tahsilat_odeme"));
                if (tipi.Equals("odeme"))
                {
                    e.Row.CssClass = "danger";
                }
                string tur = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tahsilatOdeme_turu"));
                if (tur.Equals("cari"))
                {
                    e.Row.Cells[0].Controls[3].Visible = false;
                }

            }
        }


        protected void btnEkle_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            if (!String.IsNullOrEmpty(custidd))
            {
                string url = "/TeknikCari/OdemeEkle.aspx?custid=" + custidd;
                Response.Redirect(url);
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Müşteri Ödemeleri-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Müşteri Ödemeleri-" + DateTime.Now.ToString();
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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int ids = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "musteriID"));
                if (ids < 0)
                {
                    (e.Row.FindControl("btnOde") as LinkButton).Visible = false;
                    (e.Row.FindControl("btnOdeK") as LinkButton).Visible = false;
                }
                else
                {
                    (e.Row.FindControl("btnOde") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
                    (e.Row.FindControl("btnOdeK") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");

                }


                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                    (e.Row.FindControl("delLinkK") as LinkButton).Visible = false;
                }

            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }

        protected void drdKritik_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string tur = Request.QueryString["tur"];
            string kritikGun = drdKritik.SelectedValue;

            if (kritikGun.Equals("0"))
            {
                kritikGun = "2";
            }
            string uri = "/TeknikCari/OdemeTahsilatlar.aspx?gun=" + kritikGun;


            Response.Redirect(uri);

        }
    }
}