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
    public partial class KartExtre : System.Web.UI.Page
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
            string baslik = "KART ÖDEMELERİ-";

            string firma = KullaniciIslem.firma();
            Kart c = new Kart(dc);

            bool? cekildi = null;
            int? kart_id = null;
            string kartS = Request.QueryString["kartid"];
            string cekildiS = Request.QueryString["cekildi"];
            if (!String.IsNullOrEmpty(kartS))
            {
                kart_id = Int32.Parse(kartS);

            }
            if (!String.IsNullOrEmpty(cekildiS))
            {
                cekildi = Boolean.Parse(cekildiS);
                if (cekildi == true)
                {
                    baslik += "Ödenen-";
                }
                else
                {
                    baslik += "Ödenmemiş-";
                }
            }
            baslikk.InnerHtml = baslik;
            kartozet ozet = c.Birikenler(kart_id, cekildi);
            txtAdet.InnerHtml = ozet.adet.ToString() + "İŞLEM";
            txtTutar.InnerHtml = ozet.tutar.ToString("C") + "TUTAR";
            Session["alacak"] = ozet.tutar;
            GridView1.DataSource = ozet.hesaplar;
            GridView1.DataBind();
            if (!IsPostBack)
            {
        
                List<kart_tanims> kartlar = dc.kart_tanims.Where(x => x.Firma == firma  && x.kart_id > -1).ToList();
                drdKart.AppendDataBoundItems = true;
                drdKart.DataSource = kartlar;
                drdKart.DataValueField = "kart_id";
                drdKart.DataTextField = "kart_adi";

            }
        }
        protected void drdCekildi_SelectedIndexChanged(object sender, EventArgs e)
        {

            string kartid = Request.QueryString["kartid"];

            string uri = "/TeknikCari/KartExtre.aspx?gun=" + "0";

            if (drdCekildi.SelectedValue != "0" && drdCekildi.SelectedValue != "-1")
            {
                uri += "&cekildi=" + drdCekildi.SelectedValue;

            }

            if (!String.IsNullOrEmpty(kartid))
            {
                uri += "&kartid=" + kartid;

            }


            Response.Redirect(uri);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                hdnCekID.Value = id.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }



        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool cekilme = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "cekildi"));
                LinkButton link = e.Row.Cells[0].Controls[1] as LinkButton;

                if (cekilme == true)
                {
                    link.Visible = false;
                }
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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    (e.Row.FindControl("btnOde") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
            //}
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }


        protected void drdAlinan_SelectedIndexChanged(object sender, EventArgs e)
        {


            string cekildiS = Request.QueryString["cekildi"];

            string uri = "/TeknikCari/KartExtre.aspx?gun=0";

            if (drdKart.SelectedValue != "sec" && drdKart.SelectedValue != "hepsi")
            {
                uri += "&kartid=" + drdKart.SelectedValue;
            }
            if (!String.IsNullOrEmpty(cekildiS))
            {
                uri += "&cekildi=" + cekildiS;
            }
            Response.Redirect(uri);

        }

        protected void btnKasaKaydet_Click(object sender, EventArgs e)
        {
            // bu kart_id miz
            string id = hdnCekID.Value.Trim();

            Response.Redirect("/TeknikCari/Ode.aspx?kartid=" + id + "&custid=-1");

        }

        protected void btnYeni_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikCari/Kartlar");
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append(@"<script type='text/javascript'>");
            //sb.Append("$('#addModal').modal('show');");
            //sb.Append(@"</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }

        protected void btnKartKaydet_Click(object sender, EventArgs e)
        {
            string ad = txtKartAdi.Text;
            string aciklama = txtKartAciklama.Text;
            decimal bakiye = Decimal.Parse(txtDevredenBakiye.Text);
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


    }
}