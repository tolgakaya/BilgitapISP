using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikAlim
{
    public partial class MusteriUrunAra : System.Web.UI.Page
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
                    CihazMalzeme m = new CihazMalzeme(dc);
                    gosterHepsi(m);
                }
               
            }


        }


        private void gosterHepsi(CihazMalzeme m)
        {
            string cihazid = Request.QueryString["cihazid"];

            string s = txtAra.Value;
            if (String.IsNullOrEmpty(cihazid))
            {
                if (!String.IsNullOrEmpty(s))
                {
                    gosterM(s, m);
                }
            }
            else
            {
                int id = Int32.Parse(cihazid);
                List<CihazRepo> liste = m.cihaz_listesi(id);
                if (liste.Count > 0)
                {
                    txtAdet.InnerHtml = liste.Count.ToString() + " Adet";
                    txtTutar.InnerHtml = liste.Sum(x => x.satis_tutar).ToString() + " TL";
                }
                GridView1.DataSource = liste;
                GridView1.DataBind();

            }


        }

        private void gosterM(string s, CihazMalzeme m)
        {
            List<CihazRepo> liste = m.cihaz_listesi(s);
            if (liste.Count>0)
            {
                txtAdet.InnerHtml = liste.Count.ToString() +" Adet";
                txtTutar.InnerHtml = liste.Sum(x => x.satis_tutar).ToString() + " TL";
            }
            GridView1.DataSource = liste;
            GridView1.DataBind();

        }



        private string MaillerM(string s, CihazMalzeme m)
        {

            return m.urun_mailler(s);

        }

        private string TellerM(int id, CihazMalzeme m)
        {

            return m.urun_teller(id);

        }
        private string MaillerM(int id, CihazMalzeme m)
        {

            return m.urun_mailler(id);

        }

        private string TellerM(string s, CihazMalzeme m)
        {

            return m.urun_teller(s);

        }
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                gosterHepsi(m);
            }

        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipi = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "durum"));
                if (tipi.Equals("iade"))
                {
                    e.Row.CssClass = "danger";
                    LinkButton link = e.Row.Cells[0].Controls[1] as LinkButton;
                    LinkButton link2 = e.Row.Cells[0].Controls[3] as LinkButton;
                    link.Visible = false;
                    link2.Visible = false;
                }

            }

        }
        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //(e.Row.FindControl("btnservisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServislerCanli.aspx?urun_kimlik=" + DataBinder.Eval(e.Row.DataItem, "belgeYol");

                (e.Row.FindControl("btnMusteriDetay") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
                (e.Row.FindControl("btnMusteriDetayK") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
            }
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
                    string firma = KullaniciIslem.firma();
                    int urunID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        CihazMalzeme m = new CihazMalzeme(dc);
                        m.garanti_iptal(urunID);
                        gosterHepsi(m);
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Garanti silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }
            }
            else if (e.CommandName.Equals("iade"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int urunID = Convert.ToInt32(arg[0]);
                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                txtIadeTutar.Text = row.Cells[10].Text;
                hdnCustID.Value = row.Cells[12].Text;
                hdnGarantiID.Value = urunID.ToString();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Müşterideki_Cihazlar-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Müşterideki_Cihazlar-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                gosterHepsi(m);
            }
        }

        //cihazla ilgili sms ve mail atılması gerekirse buradan atılabilir
        protected void btnSms_Click(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            if (!string.IsNullOrEmpty(s))
            {
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    CihazMalzeme m = new CihazMalzeme(dc);
                    string mail = TellerM(s, m);
                    if (!String.IsNullOrEmpty(mail))
                    {
                        Session["teller"] = mail;
                        Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
                    }

                }



            }
            else
            {
                string cihazid = Request.QueryString["cihazid"];
                if (!String.IsNullOrEmpty(cihazid))
                {
                    int id = Int32.Parse(cihazid);

                    string firma = KullaniciIslem.firma();
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        CihazMalzeme m = new CihazMalzeme(dc);
                        string mail = TellerM(id, m);
                        if (!String.IsNullOrEmpty(mail))
                        {
                            Session["teller"] = mail;
                            Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
                        }
                    }

                }
            }

        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            if (!string.IsNullOrEmpty(s))
            {
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    CihazMalzeme m = new CihazMalzeme(dc);

                    string mail = MaillerM(s, m);
                    if (!String.IsNullOrEmpty(mail))
                    {
                        Session["mailler"] = mail;
                        Response.Redirect("/MesajGonder.aspx?tur=mail&tip=gnltp");
                    }
                }

            }
            else
            {
                string cihazid = Request.QueryString["cihazid"];
                if (!String.IsNullOrEmpty(cihazid))
                {
                    int id = Int32.Parse(cihazid);
                    string firma = KullaniciIslem.firma();
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        CihazMalzeme m = new CihazMalzeme(dc);

                        string mail = MaillerM(id, m);
                        if (!String.IsNullOrEmpty(mail))
                        {
                            Session["mailler"] = mail;
                            Response.Redirect("/MesajGonder.aspx?tur=mail&tip=gnltp");
                        }
                    }

                }
            }

        }

        protected void btnIade_Click(object sender, EventArgs e)
        {
            string firma = KullaniciIslem.firma();
            int urunID = Int32.Parse(hdnGarantiID.Value);
            decimal iade_tutar = Decimal.Parse(txtIadeTutar.Text);
            string aciklama = txtIadeAciklama.Text;
            int custid = Int32.Parse(hdnCustID.Value);
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme s = new CihazMalzeme(dc);
                s.garanti_iade(urunID, iade_tutar, aciklama, custid, User.Identity.Name);
                gosterHepsi(s);
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.error('Cihaz iade alındı!');");
            sb.Append("$('#onayModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

        }
    }
}