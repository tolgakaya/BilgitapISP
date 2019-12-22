using System;

namespace TeknikServis
{
    public partial class MusteriYolu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                  System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            if (!IsPostBack) //check if the webpage is loaded for the first time.
            {
                ViewState["PreviousPage"] =
            Request.UrlReferrer;//Saves the Previous page url in ViewState
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
            Response.Redirect("/Default.aspx");
        }
    }
}