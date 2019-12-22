using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class EmanetVerMusteri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                  System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            MusteriAra(sender, e);

        }
      
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            if (!String.IsNullOrEmpty(s))
            {
                using (radiusEntities dc=MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    MusteriIslemleri m = new MusteriIslemleri(dc);

                    GridView1.DataSource = m.musteriAraR2(s,"musteri");
                    GridView1.DataBind();
                }
             
            }

        }
        protected void GridView1_OnRowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnEmanetVer") as LinkButton).PostBackUrl = "~/TeknikEmanet/EmanetVer.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnEmanetler") as LinkButton).PostBackUrl = "~/TeknikEmanet/Emanetler.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");

                (e.Row.FindControl("btnEmanetVerK") as LinkButton).PostBackUrl = "~/TeknikEmanet/EmanetVer.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnEmanetlerK") as LinkButton).PostBackUrl = "~/TeknikEmanet/Emanetler.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("detail"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                // int code = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
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
    }
}