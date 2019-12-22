using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeknikServis
{
    public partial class Sonuc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (!User.Identity.IsAuthenticated)
            //{
            //    System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            //}
            if (!IsPostBack) //check if the webpage is loaded for the first time.
            {
                ViewState["PreviousPage"] =
            Request.UrlReferrer;//Saves the Previous page url in ViewState
            }
            Session["teller"] = null;
            if (Session["mesaj"] != null)
            {
                Dictionary<string, string> mesajlar = (Dictionary<string, string>)Session["mesaj"];
                GridView1.DataSource = mesajlar;
                GridView1.DataBind();

            }
            if (Session["mesele"] != null)
            {
                baslik.InnerHtml = Session["mesele"].ToString();
            }
        }
        protected void btnGeri_Click(object sender, EventArgs e)
        {
            if (ViewState["PreviousPage"] != null)	//Check if the ViewState 
            //contains Previous page URL
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());//Redirect to 
                //Previous page by retrieving the PreviousPage Url from ViewState.
            }
        }
        protected void btnAna_Click(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }
    }
}