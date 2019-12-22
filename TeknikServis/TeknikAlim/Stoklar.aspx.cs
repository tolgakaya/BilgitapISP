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
    public partial class Stoklar : System.Web.UI.Page
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

            CihazMalzeme s = new CihazMalzeme(dc);
            string terim = txtAra.Value;

            grdAlimlar.DataSource = s.CihazListesi2(terim);
            grdAlimlar.DataBind();

            if (!IsPostBack)
            {
                drdGrup.DataSource = s.CihazGruplar();
                drdGrup.DataValueField = "grupid";
                drdGrup.DataTextField = "grupadi";
                drdGrup.DataBind();

                drdGrupDuzen.DataSource = s.CihazGruplar();
                drdGrupDuzen.DataValueField = "grupid";
                drdGrupDuzen.DataTextField = "grupadi";
                drdGrupDuzen.DataBind();
            }



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
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikAlim/AlimDetaylar.aspx?cihazid=" + DataBinder.Eval(e.Row.DataItem, "ID");
            }
        }

        protected void grdAlimlar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //güncelleme modalı açılacak
            if (e.CommandName == "guncelle")
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int ID = Convert.ToInt32(arg[0]);
                int index = Convert.ToInt32(arg[1]);
                string grupid = Convert.ToString(arg[2]);

                GridViewRow row = grdAlimlar.Rows[index];
                hdnCihazID.Value = ID.ToString();
                cihaz_adi_up.Text = row.Cells[1].Text;
                aciklama_up.Text = row.Cells[2].Text;
                garanti_suresi_up.Text = row.Cells[3].Text;
                stok_up.Text = row.Cells[6].Text;
                satis_up.Text = row.Cells[8].Text;
                hdnCihazStok.Value = row.Cells[6].Text;
                hdnCihazMaliyet.Value = row.Cells[7].Text;
                maliyet_up.Text = row.Cells[7].Text;
                barcode_up.Text = row.Cells[9].Text;
                drdGrupDuzen.SelectedValue = grupid;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append("$('#updateModal').modal('show');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
            }
            else if (e.CommandName == "musteri")
            {
                string id = e.CommandArgument.ToString();
                Response.Redirect("/TeknikAlim/MusteriUrunAra.aspx?cihazid=" + id);
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
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            int sure = Int32.Parse(garanti_suresi.Text);

            string firma = KullaniciIslem.firma();
            int grupid = Int32.Parse(drdGrup.SelectedValue);
            string bar = barcode.Text.Trim();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.Yeni(ad, acik, sure, grupid, bar);
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
            string ad = cihaz_adi_up.Text;
            string acik = aciklama_up.Text;
            int sure = Int32.Parse(garanti_suresi_up.Text);
            int bak = Int32.Parse(stok_up.Text);
            int cihazid = Int32.Parse(hdnCihazID.Value);

            string firma = KullaniciIslem.firma();
            string stokS = hdnCihazStok.Value;
            string yeniStoks = stok_up.Text;
            string yenimalS = maliyet_up.Text;
            string malS = hdnCihazMaliyet.Value;
            string bar = barcode_up.Text.Trim();

            decimal maliyet = 0;
            if (!string.IsNullOrEmpty(yenimalS))
            {
                maliyet = Decimal.Parse(yenimalS);
            }
            int grupid = Int32.Parse(drdGrupDuzen.SelectedValue);
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);

                if (stokS != yeniStoks || malS != yenimalS)
                {

                    m.StokGuncelle(bak, cihazid, maliyet);
                }
                if (!String.IsNullOrEmpty(satis_up.Text))
                {

                    decimal fiyat = Decimal.Parse(satis_up.Text);
                    m.FiyatGuncelle(cihazid, fiyat);

                }

                m.CihazGuncelle(ad, acik, sure, cihazid, grupid, bar);
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tipi = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "bakiye"));
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

        protected void btnGrup_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#grupModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "GrupShowModalScript", sb.ToString(), false);
        }

        protected void btnGrupKaydet_Click(object sender, EventArgs e)
        {
            string grupad = grupadig.Text;
            decimal kdvorani = Convert.ToDecimal(kdvg.Text);
            decimal otvorani = Convert.ToDecimal(otvg.Text);
            decimal oivorani = Convert.ToDecimal(oivg.Text);
            string firma = KullaniciIslem.firma();

            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.GrupEkle(grupad, kdvorani, otvorani, oivorani);

                Ara(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Grup tanımlandı!');");
            sb.Append("$('#grupModal').modal('hide');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "GrupShowModalScript", sb.ToString(), false);
        }




    }
}