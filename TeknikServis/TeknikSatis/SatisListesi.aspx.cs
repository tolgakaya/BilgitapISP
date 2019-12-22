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

namespace TeknikServis.TeknikSatis
{
    public partial class SatisListesi : System.Web.UI.Page
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
                    Ara(dc);
                }
             
            }


        }

        private void Ara(radiusEntities dc)
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


            HizliSatis s = new HizliSatis(dc);

            //string cust_id = Request.QueryString["custid"];
            //if (!string.IsNullOrEmpty(cust_id))
            //{
            //    btnMusteriDetayim.Visible = true;
            //}
            List<satis_helper> satis = s.liste(baslangic, bitis);

            int adet = satis.Count;
            decimal toplam = 0;
            if (adet > 0)
            {
                toplam = satis.Sum(x => x.yekun);
            }
            txtAdet.InnerHtml = "İşlem :" + adet.ToString();

            txtYekun.InnerHtml = "Yekün :" + toplam.ToString("C");
            grdAlimlar.DataSource = satis;
            grdAlimlar.DataBind();
        }

        protected void AlimAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
            }
        }


        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    int satis_id = Convert.ToInt32(e.CommandArgument);

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        HizliSatis al = new HizliSatis(dc);
                        al.iptal(satis_id, User.Identity.Name);
                        Ara(dc);
                    }
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }

            }
        }

        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
            }

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

        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikAlim/AlimDetaylar.aspx?alimid=" + DataBinder.Eval(e.Row.DataItem, "alim_id");

            //}

        }

        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            //string custidd = Request.QueryString["custid"];
            //Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        protected void btnYeni_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikSatis/PesinSatis");
        }

    }
}