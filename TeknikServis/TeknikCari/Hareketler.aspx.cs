using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using System.IO;
using System.Text;
using TeknikServis.Radius;

namespace TeknikServis.TeknikCari
{
    public partial class Hareketler : System.Web.UI.Page
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
                    goster_kasa(dc);
                }
              
            }


        }



        private void goster_kasa(radiusEntities dc)
        {

            KasaIslemleri s = new KasaIslemleri(dc);

            DateTime baslangic = DateTime.Now.AddDays(-1);
            DateTime bitis = DateTime.Now;

            if (!String.IsNullOrEmpty(datetimepicker6.Value))
            {
                baslangic = DateTime.Parse(datetimepicker6.Value);
            }
            if (!String.IsNullOrEmpty(datetimepicker7.Value))
            {
                bitis = DateTime.Parse(datetimepicker7.Value);
            }

            string tur = drdKart.SelectedValue;
            //KasaOzetOdemelerle ozet = new KasaOzetOdemelerle();
            if (drdKart.SelectedIndex == 0 || drdKart.SelectedIndex == 3)
            {
                KasaOzetOdemelerle ozet = s.HesaplarOdemeyle(baslangic, bitis, string.Empty);
                txtAdet.InnerHtml = "İşlem :" + ozet.adet.ToString();
                txtTutar.InnerHtml = "Bakiye :" + ozet.aktif_bakiye.ToString("C");
                GridView1.DataSource = ozet.islemeler;
                GridView1.DataBind();
            }
            else
            {
                KasaOzetOdemelerle ozet = s.HesaplarOdemeyle(baslangic, bitis, tur);
                txtAdet.InnerHtml = "İşlem :" + ozet.adet.ToString();
                txtTutar.InnerHtml = "Bakiye :" + ozet.aktif_bakiye.ToString("C");
                GridView1.DataSource = ozet.islemeler;
                GridView1.DataBind();
            }



        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal tipi = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "giris"));
                if (tipi > 0)
                {
                    e.Row.CssClass = "info";

                }
                else
                {
                    e.Row.CssClass = "danger";
                }

            }

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
                        goster_kasa(dc);
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }

            }

        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster_kasa(dc);
            }
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster_kasa(dc);
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
          
        }


    }
}