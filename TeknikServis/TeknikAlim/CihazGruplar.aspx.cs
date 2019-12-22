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
    public partial class CihazGruplar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                if (!IsPostBack)
                {
                    Ara(dc);
                }
            }

        }

        private void Ara(radiusEntities dc)
        {

            CihazMalzeme s = new CihazMalzeme(dc);
            string terim = txtAra.Value;

            grdAlimlar.DataSource = s.gruplar(terim);

            grdAlimlar.DataBind();

        }

        protected void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Ara(dc);
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
            string isim = "Stok Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(grdAlimlar, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Stok Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToWord(grdAlimlar, isim);
        }


        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //güncelleme modalı açılacak
            if (e.CommandName == "guncelle")
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int grupid = Convert.ToInt32(arg[0]);
                int index = Convert.ToInt32(arg[1]);


                GridViewRow row = grdAlimlar.Rows[index];
                hdnGrupID.Value = grupid.ToString();
                grupadid.Text = row.Cells[2].Text;
                kdvd.Text = row.Cells[3].Text;
                otvd.Text = row.Cells[4].Text;
                oivd.Text = row.Cells[5].Text;


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append("$('#updateModal').modal('show');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
            }

        }

        protected void btnYeni_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }
        protected void btnCihazKaydet_Click(object sender, EventArgs e)
        {
            string grupad = grupadi.Text;
            decimal kdvorani = Convert.ToDecimal(kdv.Text);
            decimal otvorani = Convert.ToDecimal(otv.Text);
            decimal oivorani = Convert.ToDecimal(oiv.Text);
            string firma = KullaniciIslem.firma();

            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.GrupEkle(grupad, kdvorani, otvorani, oivorani);
                Ara(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


        }

        protected void btnCihazUpdate_Click(object sender, EventArgs e)
        {

            int grupid = Int32.Parse(hdnGrupID.Value);

            string grupad = grupadid.Text;
            decimal kdvorani = Convert.ToDecimal(kdvd.Text);
            decimal otvorani = Convert.ToDecimal(otvd.Text);
            decimal oivorani = Convert.ToDecimal(oivd.Text);


            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazMalzeme m = new CihazMalzeme(dc);


                m.GrupDuzenle(grupid, grupad, kdvorani, otvorani, oivorani);
                Ara(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Güncelleme tamamlandı!');");
            sb.Append("$('#updateModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateShowModalScript", sb.ToString(), false);
        }

        protected void grdAlimlar_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }



    }
}