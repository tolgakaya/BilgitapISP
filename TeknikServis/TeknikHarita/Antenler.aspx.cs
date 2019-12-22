using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.IO;
using System.Text;
using TeknikServis.Radius;
using ServisDAL;

namespace BilgitapServis
{
    public partial class Antenler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            using (radiusEntities db = MyContext.Context(KullaniciIslem.firma()))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(db);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    gosterR(db, txtAra.Value);
                }

            }


        }

        private void gosterR(radiusEntities db, string s)
        {
            //adminse hepsini görür
            if (!String.IsNullOrEmpty(s))
            {
                GridView1.DataSource = db.antens.Where(a => a.anten_adi.Contains(s)).ToList();
            }
            else
            {
                GridView1.DataSource = db.antens.ToList();
            }

            GridView1.DataBind();

        }

        public void MusteriAra(object sender, EventArgs e)
        {

            using (radiusEntities db = MyContext.Context(KullaniciIslem.firma()))
            {
                gosterR(db, txtAra.Value);
            }
        }
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("/TeknikHarita/HaritaAntenKaydet.aspx");

        //}
        protected void btnAdd2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikHarita/HaritaAntenKaydet3");

        }
        protected void btnHaritadaGoster_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikHarita/ButunAntenler.aspx");

        }
        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //(e.Row.FindControl("btnDuzenle") as LinkButton).PostBackUrl = "~/TeknikHarita/HaritaAntenKaydet?id=" + DataBinder.Eval(e.Row.DataItem, "anten_id");
                (e.Row.FindControl("btnDuzenle2") as LinkButton).PostBackUrl = "~/TeknikHarita/HaritaAntenKaydet3?id=" + DataBinder.Eval(e.Row.DataItem, "anten_id");
                (e.Row.FindControl("btnTekAnten") as LinkButton).PostBackUrl = "~/TeknikHarita/TekAnten?id=" + DataBinder.Eval(e.Row.DataItem, "anten_id");
                (e.Row.FindControl("btnKayitlilar") as LinkButton).PostBackUrl = "~/TeknikHarita/TekAnten2?id=" + DataBinder.Eval(e.Row.DataItem, "anten_id");
                (e.Row.FindControl("btnMusteriler") as LinkButton).PostBackUrl = "~/TeknikHarita/MusteriAnten2?antenid=" + DataBinder.Eval(e.Row.DataItem, "anten_id");

            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("updatim"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/TeknikHarita/HaritaAntenKaydet.aspx?id=" + code.ToString());

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {

                    int id = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities db = MyContext.Context(KullaniciIslem.firma()))
                    {
                        anten ant = db.antens.Where(x => x.anten_id == id).FirstOrDefault();
                        if (ant != null)
                        {
                            db.antens.Remove(ant);
                            db.SaveChanges();
                            gosterR(db, txtAra.Value);

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
                            sb.Append(" alertify.error('Anten bulunamadı!');");

                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript4", sb.ToString(), false);
                        }
                    }




                }

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Antenler-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Antenler-" + DateTime.Now.ToString();
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

        protected void btnButunAntenMusteriKapsam_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikHarita/ButunAntenMusteri");
        }

        protected void btnButunAntenMusteriKayitli_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikHarita/ButunAntenMusteri2");
        }
    }
}