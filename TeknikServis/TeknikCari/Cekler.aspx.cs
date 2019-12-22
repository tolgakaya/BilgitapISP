using ServisDAL;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikCari
{
    public partial class Cekler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                  System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                goster(dc);
            }
           
        }

        private void goster(radiusEntities dc)
        {
            string baslik = "ÇEKLER-";
            int gun = 2;
            string gunS = Request.QueryString["gun"];
            if (!String.IsNullOrEmpty(gunS))
            {
                gun = Int32.Parse(gunS);
            }

            baslik += gun.ToString() + "-Güne Kadar-";
  
            CekSenet c = new CekSenet(dc);

            bool? cekildi = null;
            bool? alinan = null;
            string alinanS = Request.QueryString["alinan"];
            string cekildiS = Request.QueryString["cekildi"];
            if (!String.IsNullOrEmpty(alinanS))
            {
                alinan = Boolean.Parse(alinanS);
                if (alinan == true)
                {
                    baslik += "Alınan-";
                }
                else
                {
                    baslik += "Verilen-";
                }
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
            cekozet ozet = c.Cekler(gun, cekildi, alinan);
            GridView1.DataSource = ozet.cekler;
            GridView1.DataBind();
            if (!IsPostBack)
            {
                var bankalar = dc.bankas.ToList();
                drdBanka.AppendDataBoundItems = true;
                drdBanka.DataSource = bankalar;
                drdBanka.DataValueField = "banka_id";
                drdBanka.DataTextField = "banka_adi";
            }
        }
        protected void drdCekildi_SelectedIndexChanged(object sender, EventArgs e)
        {

            string gunS = Request.QueryString["gun"];
            if (String.IsNullOrEmpty(gunS))
            {
                gunS = "7";
            }

            string alinanS = Request.QueryString["alinan"];

            string uri = "/TeknikCari/Cekler.aspx?gun=" + gunS;

            if (drdCekildi.SelectedValue != "0" && drdCekildi.SelectedValue != "-1")
            {
                uri += "&cekildi=" + drdCekildi.SelectedValue;

            }

            if (!String.IsNullOrEmpty(alinanS))
            {
                uri += "&alinan=" + alinanS;

            }


            Response.Redirect(uri);

        }
        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

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
            else if (e.CommandName.Equals("bank"))
            {
                int id = Convert.ToInt32(e.CommandArgument);
                hdnCekIdBanka.Value = id.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#bankaModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BankaShowModalScript", sb.ToString(), false);
            }


        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool tipi = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "tur"));
                if (tipi == false)
                {
                    e.Row.CssClass = "danger";
                }


                bool cekilme = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "cekildi"));
                LinkButton link = e.Row.Cells[0].Controls[1] as LinkButton;
                LinkButton link2 = e.Row.Cells[0].Controls[3] as LinkButton;
                if (cekilme == true)
                {
                    link.Visible = false;
                    link2.Visible = false;
                }

            }

        }


        protected void btnEkle_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];
            if (!String.IsNullOrEmpty(custidd))
            {
                string url = "/TeknikCari/OdemeEkle.aspx?custid=" + custidd;
                Response.Redirect(url);
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

        protected void drdKritik_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kritikGun = drdKritik.SelectedValue;

            //string gunS = Request.QueryString["gun"];
            string alinanS = Request.QueryString["alinan"];
            string cekildiS = Request.QueryString["cekildi"];

            string uri = "/TeknikCari/Cekler.aspx?gun=" + kritikGun;

            //if (!String.IsNullOrEmpty(gunS))
            //{

            //}

            if (!String.IsNullOrEmpty(alinanS))
            {
                uri += "&alinan=" + alinanS;
            }
            if (!String.IsNullOrEmpty(cekildiS))
            {
                uri += "&cekildi=" + cekildiS;
            }


            Response.Redirect(uri);

        }

        protected void drdAlinan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gunS = Request.QueryString["gun"];
            if (String.IsNullOrEmpty(gunS))
            {
                gunS = "7";
            }

            string cekildiS = Request.QueryString["cekildi"];

            string uri = "/TeknikCari/Cekler.aspx?gun=" + gunS;

            if (drdAlinan.SelectedValue != "0" && drdAlinan.SelectedValue != "-1")
            {
                uri += "&alinan=" + drdAlinan.SelectedValue;
            }
            if (!String.IsNullOrEmpty(cekildiS))
            {
                uri += "&cekildi=" + cekildiS;
            }
            Response.Redirect(uri);

        }

        protected void btnKasaKaydet_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(hdnCekID.Value);

             
          
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CekSenet c = new CekSenet(dc);
                KasaIslemleri k = new KasaIslemleri(dc);
                int kasa_id = k.AktifKasa();
                c.CekTahsilat(id, kasa_id, null);
                goster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Çek Ödemesi kaydedildi!');");
            sb.Append("$('#onayModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

        }

        protected void btnBankaKaydet_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(hdnCekIdBanka.Value);
            int banka_id = Convert.ToInt32(drdBanka.SelectedValue);
            if (banka_id > -1)
            {
                using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                {
                    CekSenet c = new CekSenet(dc);

                    c.CekTahsilat(id, null, banka_id);
                    goster(dc);
                }
              

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Çek Ödemesi kaydedildi!');");
                sb.Append("$('#bankaModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
            }

        }

    }
}