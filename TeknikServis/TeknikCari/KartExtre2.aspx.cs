using ServisDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikCari
{
    public partial class KartExtre2 : System.Web.UI.Page
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
           
            Kart c = new Kart(dc);

            string basS = datetimepicker6.Value;
            string sonS = datetimepicker7.Value;
            var son = DateTime.Now;
            var bas = new DateTime(son.Year, son.Month, 1);

            if (!String.IsNullOrEmpty(basS))
            {
                bas = DateTime.Parse(basS);
            }
            if (!String.IsNullOrEmpty(sonS))
            {
                son = DateTime.Parse(sonS);
            }


            string kartS = Request.QueryString["kartid"];


            kartozet2 ozet = c.ExtreGorunumu(kartS, bas, son);
            baslikkk.InnerHtml = ozet.cari.kart_adi;
            spnBakiye.InnerHtml = ozet.cari.bakiye.ToString() + " TL Bakiye";
            txtAdet.InnerHtml = ozet.adet.ToString() + "İŞLEM";
            txtTutar.InnerHtml = ozet.tutar.ToString("C") + "TUTAR";
            //Session["alacak"] = ozet.tutar;
            GridView1.DataSource = ozet.hesaplar;
            GridView1.DataBind();

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                //hdnCekID.Value = id.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }



        }

       

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    bool cekilme = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "cekildi"));
            //    LinkButton link = e.Row.Cells[0].Controls[1] as LinkButton;

            //    if (cekilme == true)
            //    {
            //        link.Visible = false;
            //    }
            //}
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            //goster();
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    (e.Row.FindControl("btnOde") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
            //}

        }


        protected void btnYeniOdeme_Click(object sender, EventArgs e)
        {

            string kart = Request["kartid"];
            if (!String.IsNullOrEmpty(kart))
            {
                string url = "~/TeknikCari/Ode.aspx?kartid=" + kart + "&custid=-1";
                Response.Redirect(url);
            }
            else
            {
                string url = "~/TeknikCari/Kartlar";
                Response.Redirect(url);
            }
        }

        protected void btnKartExtresi_Click(object sender, EventArgs e)
        {
            //"~/TeknikCari/KartExtre2.aspx?kartid="
            string kart = Request["kartid"];
            string url = "~/TeknikCari/KartExtreOdemeler.aspx?kartid=" + kart;
            if (String.IsNullOrEmpty(kart))
            {

                url = "~/TeknikCari/Kartlar";
            }
          

            Response.Redirect(url);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {

            string isim = DateTime.Now.ToString();
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