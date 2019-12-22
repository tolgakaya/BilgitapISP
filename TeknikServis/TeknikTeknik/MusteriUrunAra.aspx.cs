using ServisDAL;
using ServisDAL.Repo;
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

namespace TeknikServis.TeknikTeknik
{
    public partial class MusteriUrunAra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
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
                    ServisIslemleri m = new ServisIslemleri(dc);
                    gosterHepsi(m);
                }
             
            }


        }

        private void gosterHepsi(ServisIslemleri m)
        {
            string s = txtAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                gosterM(s, m);
            }
            else
            {
                goster(m);
            }
        }

        private void goster(ServisIslemleri m)
        {
            List<urunRepo> urunler = m.urun_listesi();
            if (urunler.Count > 0)
            {
                txtAdet.InnerHtml = urunler.Count.ToString() + " Adet müşteri cihazı";
            }
            GridView1.DataSource = urunler;
            GridView1.DataBind();

        }
        private void gosterM(string s, ServisIslemleri m)
        {
            List<urunRepo> urunler = m.urun_ara(s);
            if (urunler.Count > 0)
            {
                txtAdet.InnerHtml = urunler.Count.ToString() + " Adet müşteri cihazı";
            }

            GridView1.DataSource = urunler;
            GridView1.DataBind();

        }
        private string Mailler(ServisIslemleri m)
        {

            return m.urun_mailler();

        }
        private string Teller(ServisIslemleri m)
        {
            return m.urun_teller();

        }
        private string MaillerHepsi(ServisIslemleri m)
        {
            string s = txtAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                return MaillerM(s, m);
            }
            else
            {
                return Mailler(m);
            }
        }
        private string TellerHepsi(ServisIslemleri m)
        {
            string s = txtAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                return TellerM(s, m);
            }
            else
            {
                return Teller(m);
            }
        }
        private string MaillerM(string s, ServisIslemleri m)
        {

            return m.urun_mailler(s);

        }

        private string TellerM(string s, ServisIslemleri m)
        {
            return m.urun_teller(s);

        }
        public void MusteriAra(object sender, EventArgs e)
        {

            string s = txtAra.Value;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri m = new ServisIslemleri(dc);
                GridView1.DataSource = m.urun_ara(s);
                GridView1.DataBind();
            }



        }

        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnservisler") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServislerCanli.aspx?urun_kimlik=" + DataBinder.Eval(e.Row.DataItem, "belgeYol");
                (e.Row.FindControl("btnservislerK") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServislerCanli.aspx?urun_kimlik=" + DataBinder.Eval(e.Row.DataItem, "belgeYol");

                (e.Row.FindControl("btnMusteriDetay") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
                (e.Row.FindControl("btnMusteriDetayK") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "musteriID");
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    int urunID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.urunIptal(urunID);
                        gosterHepsi(s);
                    }


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

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
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                gosterHepsi(s);
            }
        }

        //cihazla ilgili sms ve mail atılması gerekirse buradan atılabilir
        protected void btnSms_Click(object sender, EventArgs e)
        {
            string mail = string.Empty;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri m = new ServisIslemleri(dc);
                mail = TellerHepsi(m);

            }
            if (!String.IsNullOrEmpty(mail))
            {
                Session["teller"] = mail;
                Response.Redirect("/MesajGonder.aspx?tur=sms&tip=gnltp");
            }
        }

        protected void btnMail_Click(object sender, EventArgs e)
        {

            string mail = string.Empty;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri m = new ServisIslemleri(dc);
                mail = MaillerHepsi(m);
            }

            if (!String.IsNullOrEmpty(mail))
            {
                Session["mailler"] = mail;
                Response.Redirect("/MesajGonder.aspx?tur=mail&tip=gnltp");
            }



        }
    }
}