using ServisDAL;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class CariHesapBorclu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
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
            string terim = txtAra.Value;
            string firma = KullaniciIslem.firma();
            ServisIslemleri s = new ServisIslemleri(dc);
            if (Session["teller"] != null)
            {
                smsSayi.InnerHtml = Session["teller"].ToString().Split((new char[] { ',' }), StringSplitOptions.RemoveEmptyEntries).Length.ToString() + " Adet";
            }
            string tip = Request.QueryString["tip"];


            GridView1.DataSource = s.butunBorcuOlanHesaplarR( tip,terim);

            GridView1.DataBind();

        }

        public void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }

        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikCari/CariDetay.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
                (e.Row.FindControl("btnDetayK") as LinkButton).PostBackUrl = "~/TeknikCari/CariDetay.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
            }
        }



        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Borçlu Extre-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Borçlu Extre-" + DateTime.Now.ToString();
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
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSms_Click(object sender, EventArgs e)
        {
            Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ekle"))
            {

                string no = e.CommandArgument.ToString();

                if (Session["teller"] != null)
                {

                    string teller = Session["teller"].ToString();
                    if (!teller.Contains(no))
                    {
                        teller += no + ",";
                        Session["teller"] = teller;
                    }

                }
                else
                {
                    string teller = "";

                    teller += no + ",";
                    Session["teller"] = teller;

                }
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    goster(dc);
                }


            }
        }

        protected void drdKritik_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drdKritik.SelectedValue.Equals("son"))
            {
                Response.Redirect("/TeknikCari/CariHesapBorclu.aspx?tip=son");
            }
            else if (drdKritik.SelectedValue.Equals("a"))
            {
                Response.Redirect("/TeknikCari/CariHesapBorclu.aspx");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipi = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "son_mesaj"));
                if (!String.IsNullOrEmpty(tipi))
                {
                    e.Row.CssClass = "danger";
                }

            }
        }
    }
}