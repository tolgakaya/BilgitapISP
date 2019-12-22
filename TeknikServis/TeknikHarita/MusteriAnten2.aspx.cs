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

namespace TeknikServis.TeknikHarita
{
    public partial class MusteriAnten2 : System.Web.UI.Page
    {
        string s = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            s += "sayfa load postback-";
            string antenid = Request.QueryString["antenid"];
            if (!String.IsNullOrEmpty(antenid))
            {
                btnHepsiniTasi.Visible = true;
            }
            else
            {
                btnHepsiniTasi.Visible = false;
            }


        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //s += "sayfa load -";
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                if (!IsPostBack)
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    //drdAntenler.AppendDataBoundItems = true;
                    drdAntenler.DataSource = m.antenler();
                    drdAntenler.DataTextField = "anten_adi";
                    drdAntenler.DataValueField = "anten_id";
                    drdAntenler.DataBind();

                    Goster(dc);
                }


            }

        }

        private void Goster(radiusEntities dc)
        {
            s += "goster-";
            string terim = txtAra.Value;
            string antenid = Request.QueryString["antenid"];

            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(antenid))
            {
                musteri_tel_mail musteri_bilgileri = m.antenMusterileri(Convert.ToInt32(antenid));
                GridView1.DataSource = musteri_bilgileri.musteriler;
                baslik.InnerHtml = musteri_bilgileri.anten_adi + " Kayıtlı Müşteriler";
                hdnTeller.Value = musteri_bilgileri.teller;

            }
            else if (String.IsNullOrEmpty(terim))
            {
                musteri_tel_mail musteri_bilgileri = m.musteriListesiR();
                GridView1.DataSource = musteri_bilgileri.musteriler;
                baslik.InnerHtml = " Kayıtlı Müşteriler";
                hdnTeller.Value = musteri_bilgileri.teller;

            }
            else
            {
                musteri_tel_mail musteri_bilgileri = m.musteriAraR(terim);
                GridView1.DataSource = musteri_bilgileri.musteriler;
                baslik.InnerHtml = " Kayıtlı Müşteriler";
                hdnTeller.Value = musteri_bilgileri.teller;
            }

            GridView1.DataBind();


        }
        public void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Goster(dc);

            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                s += "row data bound-";
                string antenid = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "antenid"));

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {

                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    List<anten> antenler = m.antenler();
                    DropDownList ddlCountries = (e.Row.FindControl("ddlAnten") as DropDownList);
                    ddlCountries.AppendDataBoundItems = true;
                    ddlCountries.DataSource = antenler;
                    ddlCountries.DataTextField = "anten_adi";
                    ddlCountries.DataValueField = "anten_id";
                    ddlCountries.DataBind();
                    //string antenadi = (e.Row.FindControl("lblCountry") as Label).Text;

                    if (!String.IsNullOrEmpty(antenid))
                    {
                        ddlCountries.Items.FindByValue(antenid).Selected = true;

                    }

                }



            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            s += "buton click-";

            //ViewState["tip"] = "kaydet";
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                //Goster(dc);
                Kaydet(dc);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kaydedildi!');");

                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                //Goster(dc);
            }
            //s += "click- antenid=" + secilen_anten + "-" + "custid=" + custid.ToString() + "|||";
            //Session["mesele"] = s;
            //Response.Redirect("/Sonuc");



        }

        private void Kaydet(radiusEntities dc)
        {
            MusteriIslemleri m = new MusteriIslemleri(dc);

            foreach (GridViewRow row in GridView1.Rows)
            {

                //mesele += "EKledim";
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int custid = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value.ToString());
                    //int custid = Convert.ToInt32(row.Cells[0].Text);
                    int? antenid = null;
                    //string secilen_anten = row.Cells[3].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem.Value;
                    DropDownList DDL = (DropDownList)row.FindControl("ddlAnten");
                    string secilen_anten = DDL.SelectedValue.ToString();
                    if (secilen_anten != "-1")
                    {
                        antenid = Convert.ToInt32(secilen_anten);
                    }

                    m.antenMusteriGuncelle(custid, antenid);
                }


            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                Goster(dc);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Kişi ve Firmalar-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Kişi ve Firmalar-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
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

        protected void btnSms_Click(object sender, EventArgs e)
        {
            Session["teller"] = hdnTeller.Value;
            Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");

        }

        protected void btnMail_Click(object sender, EventArgs e)
        {

            Response.Redirect("/TeknikHarita/Antenler");

        }

        protected void btnHepsiniTasi_Click(object sender, EventArgs e)
        {
            string eskianten = Request.QueryString["antenid"];
            hdnEskiAnten.Value = eskianten;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
        }

        protected void btnAntenKaydet_Click(object sender, EventArgs e)
        {
            int? antenid = null;
            string eskianten = hdnEskiAnten.Value;
            if (!string.IsNullOrEmpty(eskianten))
            {

                int eskiid = Convert.ToInt32(eskianten);
                if (drdAntenler.SelectedValue != "-1")
                {
                    antenid = Convert.ToInt32(drdAntenler.SelectedValue);
                }
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    m.antenTasi(eskiid, antenid);
                    Response.Redirect("/TeknikHarita/MusteriAnten?antenid=" + antenid.ToString());
                    //Goster(dc);
                }

            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('hide');");
            sb.Append(" alertify.success('Taşıma tamamlandı!');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnTekAnten") as LinkButton).PostBackUrl = "~/TeknikHarita/HaritaTekMusteri?id=" + DataBinder.Eval(e.Row.DataItem, "CustID");

                var antenid = DataBinder.Eval(e.Row.DataItem, "antenid");
                if (antenid != null)
                {
                    (e.Row.FindControl("btnTekAnten2") as LinkButton).PostBackUrl = "~/TeknikHarita/HaritaTekMusteriKayitli?id=" + DataBinder.Eval(e.Row.DataItem, "antenid") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "CustID").ToString();

                }
                else
                {
                    (e.Row.FindControl("btnTekAnten2") as LinkButton).Visible = false;
                }


            }
        }

        protected void drdAntenler_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                //Goster(dc);
                //Kaydet(dc);
                //Goster(dc);
            }
        }
    }
}