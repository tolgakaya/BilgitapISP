using ServisDAL;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using System.Collections.Generic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class Emanetler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                goster(dc);
            }


        }
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        private void goster(radiusEntities dc)
        {


            ServisIslemleri s = new ServisIslemleri(dc);
            //sadece servise göre bulmak için

            string cust_id = Request.QueryString["custid"];

            //sadece müşteriye göre bulmak için
            if (!String.IsNullOrEmpty(cust_id))
            {
                btnMusteriDetayim.Visible = true;
                int custid = Int32.Parse(cust_id);
                GridView1.DataSource = s.emanettekiUrunlerimizHepsiR(custid);

            }
            else
            {

                GridView1.DataSource = s.emanettekiUrunlerimizHepsiR();


            }

            GridView1.DataBind();
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
                    int id = Convert.ToInt32(e.CommandArgument);


                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.emanetAlR(id, User.Identity.Name);
                        goster(dc);
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Emanet teslimi alındı!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            string url = "/TeknikEmanet/EmanetVerMusteri";
            if (!String.IsNullOrEmpty(custidd))
            {
                url = "/TeknikEmanet/EmanetVer.aspx?custid=" + custidd;

            }
            Response.Redirect(url);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Müşteri Emanetleri-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Müşteri Emanetleri-" + DateTime.Now.ToString();
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
    }
}