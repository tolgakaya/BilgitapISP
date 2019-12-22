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
    public partial class Poslar : System.Web.UI.Page
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


            PosBanka s = new PosBanka(dc);
            if (!IsPostBack)
            {
                List<Radius.pos_tanims> poss = s.PosListe();
                drdKart.AppendDataBoundItems = true;
                drdKart.DataSource = poss;
                drdKart.DataValueField = "pos_id";
                drdKart.DataTextField = "pos_adi";
                drdKart.DataBind();

                drdPosBanka.AppendDataBoundItems = true;
                drdPosBanka.DataSource = s.bankalar();
                drdPosBanka.DataValueField = "banka_id";
                drdPosBanka.DataTextField = "banka_adi";
                drdPosBanka.DataBind();
            }


            int? pos_id = null;
            bool? aktif = null;
            bool cekilen = false;
            int index = drdKart.SelectedIndex;
            int aktifS = drdAktif.SelectedIndex;
            if (index > 1)
            {
                pos_id = Int32.Parse(drdKart.SelectedValue);
            }


            if (aktifS > 0)
            {
                aktif = true;

            }

            if (chcCekilen.Checked == true)
            {
                cekilen = true;
            }

            if (cekilen == true)
            {
                posozet ozet = s.HesaplarCekilen(pos_id);
                txtAdet.InnerHtml = "İşlem :" + ozet.adet.ToString();
                txtTutar.InnerHtml = "Tahsilat :" + ozet.tahsilat_tutar.ToString("C");
                txtKomisyon.InnerHtml = "Komisyon :" + ozet.komisyon_tutar.ToString("C");
                txtNet.InnerHtml = "NET :" + ozet.net_tutar.ToString("C");
                btnTransfer.Visible = false;
                GridView1.DataSource = ozet.poshesaplar;
                GridView1.DataBind();

            }
            else
            {
                posozet ozet = s.Hesaplar(pos_id, aktif);
                txtAdet.InnerHtml = "İşlem :" + ozet.adet.ToString();
                txtTutar.InnerHtml = "Tahsilat :" + ozet.tahsilat_tutar.ToString("C");
                txtKomisyon.InnerHtml = "Komisyon :" + ozet.komisyon_tutar.ToString("C");
                txtNet.InnerHtml = "NET :" + ozet.net_tutar.ToString("C");
                if (pos_id != null && ozet.net_tutar > 0 && aktif == true)
                {
                    btnTransfer.Visible = true;
                }
                else
                {
                    btnTransfer.Visible = false;
                }
                GridView1.DataSource = ozet.poshesaplar;
                GridView1.DataBind();

            }

        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    decimal tipi = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "giris"));
            //    if (tipi > 0)
            //    {
            //        e.Row.CssClass = "info";

            //    }
            //    else
            //    {
            //        e.Row.CssClass = "danger";
            //    }

            //}

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

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            int index = drdKart.SelectedIndex;
            if (index > 1)
            {
                int id = Int32.Parse(drdKart.SelectedValue);
                hdnCekID.Value = id.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }
        }

        protected void btnTransferKaydet_Click(object sender, EventArgs e)
        {

            string s = hdnCekID.Value;
            if (!string.IsNullOrEmpty(s))
            {
                string firma = KullaniciIslem.firma();

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    PosBanka p = new PosBanka(dc);
                    p.Transfer(Int32.Parse(s), User.Identity.Name);
                    goster_kasa(dc);
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append(" alertify.success('Transfer tamamlandı!');");
                sb.Append("$('#onayModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);

            }

        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            int banka_id = Int32.Parse(drdPosBanka.SelectedValue);
            if (banka_id > -1)
            {
                string ad = txtPosAdi.Text;
                decimal komisyon = Decimal.Parse(txtPosKomisyon.Text);
                int sure = Int32.Parse(txtPosSure.Text);
                string firma = KullaniciIslem.firma();

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    PosBanka p = new PosBanka(dc);
                    p.YeniPos(ad, sure, komisyon, banka_id);
                    goster_kasa(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");

                sb.Append(" alertify.success('Pos tanımlandı!');");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
            }

        }

        protected void btnEkle_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }
    }
}