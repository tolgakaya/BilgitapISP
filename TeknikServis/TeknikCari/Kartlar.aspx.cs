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
    public partial class Kartlar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
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
            Kart s = new Kart(dc);
            GridView1.DataSource = s.KartListe();
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("guncel"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int id = Convert.ToInt32(arg[0]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                hdnKartID.Value = id.ToString();

                txtKartAdiup.Text = row.Cells[2].Text;
                txtKartAciklamaup.Text = row.Cells[3].Text;
                tarihup.Text = row.Cells[4].Text;

                string s = row.Cells[5].Text.Split(new char[] { ' ' })[0];
                txtDevredenBakiyeup.Text = s;
                hdnBakiye.Value = s;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#upModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UppShowModalScript", sb.ToString(), false);

            }

        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            //yenibanka tanımlama
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

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
                //kart odemeerini göster
                (e.Row.FindControl("btnOde") as LinkButton).PostBackUrl = "~/TeknikCari/KartExtre2.aspx?kartid=" + DataBinder.Eval(e.Row.DataItem, "kart_id");

                (e.Row.FindControl("btnExtreOde") as LinkButton).PostBackUrl = "~/TeknikCari/Ode.aspx?kartid=" + DataBinder.Eval(e.Row.DataItem, "kart_id") + "&custid=-1";
                (e.Row.FindControl("btnOdemeler") as LinkButton).PostBackUrl = "~/TeknikCari/KartExtreOdemeler.aspx?kartid=" + DataBinder.Eval(e.Row.DataItem, "kart_id");

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

        protected void btnKartKaydet_Click(object sender, EventArgs e)
        {
            string ad = txtKartAdi.Text;
            string aciklama = txtKartAciklama.Text;
            //decimal bakiye = Decimal.Parse(txtDevredenBakiye.Text);
            DateTime et = DateTime.Parse(tarih.Text);
             
          

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Kart p = new Kart(dc);
                p.Yeni(ad, et, aciklama);
                goster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append(" alertify.success('Kredi kartı tanımlandı!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }

        protected void btnKartKaydetup_Click(object sender, EventArgs e)
        {
            string ad = txtKartAdiup.Text;
            string aciklama = txtKartAciklamaup.Text;
            decimal bakiye = Decimal.Parse(txtDevredenBakiyeup.Text);
            int id = Int32.Parse(hdnKartID.Value);
            DateTime et = DateTime.Parse(tarihup.Text);
             
        

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Kart p = new Kart(dc);

                string mevcutBakiye = hdnBakiye.Value;

                string yeniBakiye = txtDevredenBakiyeup.Text;
                if (mevcutBakiye != yeniBakiye)
                {
                    p.Guncelle(ad, et, aciklama, id);
                    p.CariGuncelle(bakiye, id);
                }
                else
                {
                    p.Guncelle(ad, et, aciklama, id);
                }
                goster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append(" alertify.success('Kredi kartı güncellendi!');");
            sb.Append("$('#upModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
    }
}