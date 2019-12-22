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

namespace TeknikServis
{
    public partial class FaturaBasim : System.Web.UI.Page
    {
        string firma;

        public FaturaBasim()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            else
            {
                firma = KullaniciIslem.firma();

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            if (Session["idint"] != null)
            {
                int id = Int32.Parse(Session["idint"].ToString());
                yazdirmaInternet(id);

            }
            else if (Session["ids"] != null)
            {
                int id = Int32.Parse(Session["ids"].ToString());
                yazdirmaServis(id);

            }
            else if (Session["pesin"] != null)
            {
                Pesin p = (Pesin)Session["pesin"];
                yazdirmaPesin(p.id, p.unvan, p.tc, p.vd, p.adres);

            }

            using (radiusEntities dc = MyContext.Context(firma))
            {
                HepsiniGoster(dc);
            }




        }

        private void HepsiniGoster(radiusEntities dc)
        {
            string durum = Request.QueryString["durum"];
            string tip = Request.QueryString["tip"];

            if (!String.IsNullOrEmpty(durum))
            {
                //panelContents.Visible = true;
                baslik.Text = "BASILMIŞ FATURALAR";
                btnBasilmis.Visible = true;
                btnGuncel.Visible = false;
                //arama.Visible = false;
                GridView1.Columns[1].Visible = false;
                Ara(dc);
            }
            else
            {
                baslik.Text = "BASILMAMIŞ FATURALAR";
                btnBasilmis.Visible = false;
                btnGuncel.Visible = true;
                GridView1.Columns[0].Visible = false;
                GridView1.Columns[3].Visible = false;
                GridView1.Columns[4].Visible = false;

                if (String.IsNullOrEmpty(tip))
                {

                    tip = "Hepsi";
                }

                DateTime baslangic = DateTime.Now.AddMonths(-1);
                if (!String.IsNullOrEmpty(datetimepicker6.Value))
                {
                    baslangic = DateTime.Parse(datetimepicker6.Value);
                }
                DateTime bitis = DateTime.Now;
                if (!String.IsNullOrEmpty(datetimepicker7.Value))
                {
                    bitis = DateTime.Parse(datetimepicker7.Value);
                }

                AyarCurrent cur = new AyarCurrent(dc);
                ayargenel ay = cur.get();
                DateTime? sonfatura = ay.sonfatura;

                FaturaBas bas = new FaturaBas(dc);
                List<Baski_Gorunum> liste = bas.BasimListesi(firma, baslangic, bitis,tip, sonfatura);
                if (liste != null)
                {
                    string adet = liste.Count().ToString() + " Adet";
                    string tutar = "Tutar: " + liste.Sum(x => x.Tutar).ToString();
                    string kdv = "KDV: " + liste.Sum(x => x.KDV).ToString();
                    string oiv = "ÖİV: " + liste.Sum(x => x.OIV).ToString();
                    string yekun = "Yekün: " + liste.Sum(x => x.Yekun).ToString();

                    txtAdet.InnerHtml = adet;
                    txtTutar.InnerHtml = tutar;
                    txtKDV.InnerHtml = kdv;
                    txtOIV.InnerHtml = oiv;
                    txtYekun.InnerHtml = yekun;
                }
                GridView1.DataSource = liste;


                GridView1.DataBind();

            }
        }
      

        protected void Button2_Click(object sender, EventArgs e)
        {
            //basılmış faturalarda arama yapalım
            using (radiusEntities dc = MyContext.Context(firma))
            {
                Ara(dc);
            }
        }

        private void Ara(radiusEntities dc)
        {
            string baslamaS = datetimepicker6.Value;
            DateTime baslama = DateTime.Now.AddMonths(-1);
            DateTime son = DateTime.Now;

            string sonS = datetimepicker7.Value;


            if (!String.IsNullOrEmpty(baslamaS))
            {
                baslama = DateTime.Parse(datetimepicker6.Value);
            }
            if (!String.IsNullOrEmpty(sonS))
            {
                son = DateTime.Parse(datetimepicker7.Value);
            }


            FaturaBas bas = new FaturaBas(dc);
            List<Baski_Gorunum> liste = bas.BasilmisFaturalar(firma, baslama, son);
            if (liste != null)
            {
                string adet = liste.Count().ToString() + " Adet";
                string tutar = "Tutar: " + liste.Sum(x => x.Tutar).ToString();
                string kdv = "KDV: " + liste.Sum(x => x.KDV).ToString();
                string oiv = "ÖİV: " + liste.Sum(x => x.OIV).ToString();
                string yekun = "Yekün: " + liste.Sum(x => x.Yekun).ToString();

                txtAdet.InnerHtml = adet;
                txtTutar.InnerHtml = tutar;
                txtKDV.InnerHtml = kdv;
                txtOIV.InnerHtml = oiv;
                txtYekun.InnerHtml = yekun;
            }
            GridView1.DataSource = liste;

            GridView1.DataBind();
        }
        public void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc=MyContext.Context(firma))
            {
                HepsiniGoster(dc);
            }

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("faturaBas"))
            {

                //baskı işlemleri
                string info = e.CommandArgument.ToString();
                string[] arg = new string[2];
                char[] splitter = { ';' };
                arg = info.Split(splitter);
                string tur = arg[0];
                string idd = arg[1];
                int id = Int32.Parse(idd);
                if (tur.Equals("internet"))
                {
                    Session["idint"] = id;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailShowModalScript", sb.ToString(), false); string tip = Request.QueryString["tip"];

                }
                else if (tur.Equals("Servis") || tur.Equals("cihaz"))
                {
                    Session["ids"] = id;


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailShowModalScript", sb.ToString(), false); string tip = Request.QueryString["tip"];


                }
                else if (tur.Equals("pesin"))
                {
                    Session["idp"] = id;
                    //en iyisi bunu fatura bilgilerini içeren bir nesneyi
                    //addmodalde oluşturup göndermek

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#addModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false); string tip = Request.QueryString["tip"];


                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Bir hata oluştu!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }
            else if (e.CommandName.Equals("silbak"))
            {


                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    string info = e.CommandArgument.ToString();
                    string[] arg = new string[2];
                    char[] splitter = { ';' };
                    arg = info.Split(splitter);
                    string tur = arg[0].Trim();
                    string idd = arg[1];
                    int id = Int32.Parse(idd);


                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        FaturaBas f = new FaturaBas(dc);
                        f.FaturaIptal(id, tur);
                        HepsiniGoster(dc);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Fatura iptal edildi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                    }
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Bir hata oluştu!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }
            }
            else
            {
                string com = e.CommandArgument.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                //sb.Append(" alertify.error('Başkaca bir şey!');");
                sb.Append(" alertify.error('" + com + "');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
            }

        }

        private void yazdirmaInternet(int id)
        {
            using (radiusEntities dc = MyContext.Context(firma))
            {
                FaturaPrinter pri = new FaturaPrinter(dc);
                AyarCurrent ay = new AyarCurrent(dc);
                pri.InternetBas(id, faturaGoster, ay.get().cift_taraf, firma);
            }

        }
        private void yazdirmaServis(int id)
        {
            using (radiusEntities dc = MyContext.Context(firma))
            {
                FaturaPrinter pri = new FaturaPrinter(dc);
                AyarCurrent ay = new AyarCurrent(dc);

                pri.ServisBas(id, faturaGoster, ay.get().cift_taraf, firma);
            }

        }
        private void yazdirmaPesin(int id, string unvan, string tc, string vd, string adres)
        {
            using (radiusEntities dc = MyContext.Context(firma))
            {
                FaturaPrinter pri = new FaturaPrinter(dc);
                AyarCurrent ay = new AyarCurrent(dc);
                pri.PesinBas(id, unvan, tc, vd, adres, faturaGoster, ay.get().cift_taraf, firma);
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Fatura Listesi-" + DateTime.Now.ToString();
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

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(firma))
            {
                HepsiniGoster(dc);
            }
        }

        protected void btnKapat_Click(object sender, EventArgs e)
        {
            Session["idint"] = null;
            Session["ids"] = null;
            Session["pesin"] = null;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detailModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailShowModalScript", sb.ToString(), false); string tip = Request.QueryString["tip"];

        }

        protected void btnBas_Click(object sender, EventArgs e)
        {

            //string s = "";
            //foreach (GridViewRow grv in GridView1.Rows)
            //{
            //    if (grv.RowType == DataControlRowType.DataRow)
            //    {
            //        CheckBox chkBas = (grv.Cells[0].FindControl("chcBas") as CheckBox);
            //        if (chkBas != null && chkBas.Checked == true)
            //        {
            //            //EditRows.Add(grv.RowIndex);
            //            //s += grv.Cells[4].Text;
            //            s += GridView1.DataKeys[grv.RowIndex].Value.ToString();
            //        }
            //    }
            //}

            ////lblTest.Text = s;
            //Response.Write(s);
            ////Session["mesele"] = ss;
            Response.Redirect("/Sonuc.aspx");
        }

        protected void btnBas_Click1(object sender, EventArgs e)
        {
            string s = "";
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox chc = (CheckBox)row.FindControl("chcBas");
                if (chc != null && chc.Checked)
                {
                    s += GridView1.DataKeys[row.RowIndex].Value.ToString();
                }
            }
            Session["mesele"] = s;
            Response.Redirect("/Sonuc.aspx?mesele=" + s);

        }

        protected void chcBas_CheckedChanged(object sender, EventArgs e)
        {
            string s = "";
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;
            bool state = chkStatus.Checked;
            if (state == true)
            {
                s += GridView1.DataKeys[row.RowIndex].Value.ToString();
            }
            if (Session["mesele"] != null)
            {
                string ss = Convert.ToString(Session["mesele"]);
                ss += s;
            }
            else
            {
                Session["mesele"] = s;
            }

        }

        protected void btnPesinKaydet_Click(object sender, EventArgs e)
        {
            if (Session["idp"] != null)
            {
                int id = Int32.Parse(Session["idp"].ToString());
                string unvan = txtUnvan.Text;
                string vd = txtVD.Text;
                string tc = txtVN.Text;
                Pesin p = new Pesin();
                p.id = id;
                p.unvan = unvan;
                p.vd = vd;
                p.tc = tc;
                p.adres = txtAdres.Text;

                Session["pesin"] = p;
                Session["idp"] = null;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false); string tip = Request.QueryString["tip"];

            }

        }
    }
}