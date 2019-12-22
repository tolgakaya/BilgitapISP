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

namespace TeknikServis.TeknikAlim
{
    public partial class AlimDetaylar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            if (!IsPostBack)
            {
                Ara();
            }
          

        }

        private void Ara()
        {
            DateTime baslangic = DateTime.Now.AddMonths(-3);
            if (!String.IsNullOrEmpty(datetimepicker6.Value))
            {
                baslangic = DateTime.Parse(datetimepicker6.Value);
            }
            DateTime bitis = DateTime.Now;
            if (!String.IsNullOrEmpty(datetimepicker7.Value))
            {
                bitis = DateTime.Parse(datetimepicker7.Value);
            }

            string firma = KullaniciIslem.firma();
            using (radiusEntities dc=MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                SatinAlim s = new SatinAlim(dc);

                string cust_id = Request.QueryString["custid"];
                string cihaz_id = Request.QueryString["cihazid"];
                string alim_id = Request.QueryString["alimid"];
                if (!string.IsNullOrEmpty(cust_id))
                {
                    btnMusteriDetayim.Visible = true;
                }
                DetayOzet ozet = s.Detaylar(baslangic, bitis, cust_id, cihaz_id, alim_id);
                txtIslemAdet.InnerHtml = "İşlem :" + ozet.islem_adet.ToString();
                txtToplamAdet.InnerHtml = "Miktar :" + ozet.toplam_adet.ToString();

                txtYekun.InnerHtml = "Yekün :" + ozet.yekun.ToString("C");
                grdAlimlar.DataSource = ozet.detaylar;
                grdAlimlar.DataBind();
            }
           
        }

        protected void AlimAra(object sender, EventArgs e)
        {
            Ara();
        }
     

        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            Ara();

        }
        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            grdAlimlar.AllowPaging = false;
            grdAlimlar.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdAlimlar.RenderControl(hw);
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

            ScriptManager.RegisterStartupScript(grdAlimlar, this.GetType(), "GridPrint", sb.ToString(), false);
            grdAlimlar.AllowPaging = true;

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(grdAlimlar, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToWord(grdAlimlar, isim);
        }

    

        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }
    }
}