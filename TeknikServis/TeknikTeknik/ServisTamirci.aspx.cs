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

namespace TeknikServis.TeknikTeknik
{
    public partial class ServisTamirci : System.Web.UI.Page
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
                    goster(dc);
                }
              
            }
        }
        private void goster(radiusEntities dc)
        {

            ServisIslemleri s = new ServisIslemleri(dc);

            string tamir_id = Request.QueryString["tamirid"];

            //tarih ve kapanma durumuna göre
            //sadece müşteriye göre bulmak için 
            if (!String.IsNullOrEmpty(tamir_id))
            {
                DateTime baslama = DateTime.Now.AddDays(-30);
                string tarihs = datetimepicker6.Value;
                if (!String.IsNullOrEmpty(tarihs))
                {
                    baslama = DateTime.Parse(tarihs);
                }

                string kritik = drdKritik.SelectedValue;

                bool kapanma = false;
                if (kritik.Equals("acik"))
                {
                    kapanma = false;
                }
                else if (kritik.Equals("tamam"))
                {
                    kapanma = true;
                }
                //btnMusteriDetayim.Visible = true;
                int tamirid = Int32.Parse(tamir_id);
                var liste = s.servisKararListesiTamirci(tamirid, baslama, kapanma);
                int adet = 0;
                decimal mal = 0;
                decimal tutar = 0;
                if (liste.Count > 0)
                {
                    adet = liste.Count;
                    mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                    tutar = (decimal)liste.Sum(x => x.yekun);
                }
                txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                GridView1.DataSource = liste;

            }


            GridView1.DataBind();
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
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
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {

            string tamirid = Request.QueryString["tamirid"];

            if (!String.IsNullOrEmpty(tamirid))
            {
                Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + tamirid);
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

                    int hesapID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.servisKararIptalR(hesapID, User.Identity.Name);
                        goster(dc);
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
            else if (e.CommandName.Equals("onay"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int hesapID = Convert.ToInt32(arg[0]);
                int musteriID = Convert.ToInt32(arg[2]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                string islem = row.Cells[2].Text;

                string yekun = row.Cells[8].Text;
                string servisid = row.Cells[12].Text;


                hdnHesapID.Value = hesapID.ToString();
                hdnMusteriID.Value = musteriID.ToString();

                hdnServisIDD.Value = servisid;
                hdnYekunn.Value = yekun;
                hdnIslemm.Value = islem;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string m = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "musteriID"));
                string s = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "servisID"));
                string k = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "kimlik"));

                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;
                (e.Row.FindControl("btnServisK") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;
                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                    (e.Row.FindControl("delLinkK") as LinkButton).Visible = false;
                }


            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string onay = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "onayDurumu"));
                LinkButton link = e.Row.Cells[0].Controls[3] as LinkButton;
                //LinkButton link2 = e.Row.Cells[0].Controls[4] as LinkButton;
                LinkButton link2 = e.Row.Cells[0].Controls[9] as LinkButton;
                if (onay == "EVET")
                {
                    link.Visible = false;
                    link2.Visible = false;

                }
                //string kimlik = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "kimlik"));
            }
        }

        protected void btnOnay_Click(object sender, EventArgs e)
        {
            string hesapS = hdnHesapID.Value;

            string musteriID = hdnMusteriID.Value.Trim();
            int custid = Int32.Parse(musteriID);
            int hesapID = Int32.Parse(hesapS);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                ServisIslemleri ser = new ServisIslemleri(dc);

                if (musteriID != "-99")
                {
                    //burada cariyi çekecez
                    musteri_bilgileri musteri_bilgileri = ser.servisKararOnayR(hesapID, User.Identity.Name);
                    //karar onayında stok kontrolü yapılıyor
                    //eğer stok yoksa müşteri bilgileri boş döndürülüyor
                    if (!string.IsNullOrEmpty(musteri_bilgileri.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, musteri_bilgileri.caribakiye,User.Identity.Name);

                        if (chcMail.Checked == true || chcSms.Checked == true)
                        {
                            int servisid = Int32.Parse(hdnServisIDD.Value);
                            string islem = hdnIslemm.Value;
                            string yekun = hdnYekunn.Value;
                            Radius.service serr = ser.servisTekR(servisid);
                            if (chcMail.Checked == true)
                            {
                                string ekMesaj = "Yapılacak işlem: <b>" + islem + "</b><br/>" + "Tutar :<b>" + yekun + "TL";
                                ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                                mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.ad, serr.Servis_Kimlik_No,  "karar_onaylandi", ekMesaj);

                            }
                            if (chcSms.Checked == true)
                            {
                                string ekMesajSms = "ServisNo: " + serr.Servis_Kimlik_No + "İşlem: " + islem + "Tutar: " + yekun + " TL";
                                ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                                AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                                sms.SmsGonder("durum", (int)serr.durum_id, ayarimiz, musteri_bilgileri.tel, ekMesajSms);

                            }
                        }
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

                        sb.Append("alertify.error('Cihaz stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }
                else
                {
                    musteri_bilgileri bil = ser.servisKararOnayNoMusteri(hesapID, User.Identity.Name);
                    if (!string.IsNullOrEmpty(bil.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, bil.caribakiye,User.Identity.Name);
                        //Response.Redirect("/Deneme.aspx?felan=" + musteriID);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#onayModal').modal('hide');");
                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);

                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

                        sb.Append("alertify.error('Cihaz stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }

                goster(dc);
            }

        }

    }
}