using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikAlim
{
    public partial class MusteriUrunler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            string firma = KullaniciIslem.firma();
            using (radiusEntities dc=MyContext.Context(firma))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    musteriGoster(dc);
                    if (GridView3.SelectedValue != null)
                    {
                        int id = Convert.ToInt32(GridView3.SelectedValue);

                        UrunAra(dc, id);
                    }
                }
            
            }
          
        }

     

        private void musteriGoster(radiusEntities dc)
        {

            string s = txtMusteriSorgu.Value;

            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                GridView3.DataSource = m.musteriAraR2(s,"musteri");
                GridView3.DataBind();
            }
        }

        protected void MusteriAra(object sender, EventArgs e)
        {
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc=MyContext.Context(firma))
            {
                musteriGoster(dc);
            }
            
        }
        protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridView3.SelectedValue != null)
            {
                 string firma = KullaniciIslem.firma();
                int id = Convert.ToInt32(GridView3.SelectedValue);
                using (radiusEntities dc=MyContext.Context(firma))
                {
                    musteriGoster(dc);
                    UrunAra(dc,id);
                }
               
            }


        }
        private void UrunAra(radiusEntities dc, int musID)
        {
            CihazMalzeme ser = new CihazMalzeme(dc);
            GridView2.DataSource = ser.cihazListesi(musID);
            GridView2.DataBind();
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
             string firma = KullaniciIslem.firma();
            if (GridView3.SelectedValue != null)
            {
                int id = Convert.ToInt32(GridView3.SelectedValue);
                using (radiusEntities dc=MyContext.Context(firma))
                {
                    musteriGoster(dc);
                    UrunAra(dc,id);
                }
               
            }
        }
        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("detail"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                // int code = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc=MyContext.Context(firma))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);

                    DetailsView1.DataSource = m.musteriTekListeR(code);
                    DetailsView1.DataBind();
                }
              
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
            }
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                //burada takas yada iade biçiminde işlem yapılacak

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                     string firma = KullaniciIslem.firma();
                    int urunID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc=MyContext.Context(firma))
                    {
                        CihazMalzeme s = new CihazMalzeme(dc);
                        s.garanti_iptal(urunID);

                        if (GridView3.SelectedValue != null)
                        {
                            int id = Convert.ToInt32(GridView3.SelectedValue);
                            musteriGoster(dc);
                            UrunAra(dc,id);
                        }
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
            else if (e.CommandName.Equals("iade"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int urunID = Convert.ToInt32(arg[0]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView2.Rows[index];
                txtIadeTutar.Text = row.Cells[8].Text;
                hdnCustID.Value = row.Cells[10].Text;

                hdnGarantiID.Value = urunID.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }
        }
        protected void btnIade_Click(object sender, EventArgs e)
        {
            string firma = KullaniciIslem.firma();
            int urunID = Int32.Parse(hdnGarantiID.Value);
            decimal iade_tutar = Decimal.Parse(txtIadeTutar.Text);
            string aciklama = txtIadeAciklama.Text;
            int custid = Int32.Parse(hdnCustID.Value);
            using (radiusEntities dc=MyContext.Context(firma))
            {
                CihazMalzeme s = new CihazMalzeme(dc);
                s.garanti_iade(urunID, iade_tutar, aciklama, custid,User.Identity.Name);
                if (GridView3.SelectedValue != null)
                {
                    int id = Convert.ToInt32(GridView3.SelectedValue);
                    musteriGoster(dc);
                    UrunAra(dc,id);
                }
            }
           

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.error('Cihaz iade alındı!');");
            sb.Append("$('#onayModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
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


    }
}