using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    public bool kisiarama
    {
        get { return kisiara.Visible; }
        set { kisiara.Visible = value; }
    }
    public bool servisarama
    {

        get { return servisara.Visible; }
        set { servisara.Visible = value; }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        userdiv.DataBind();
        IPrincipal use = HttpContext.Current.User;
        if (use.IsInRole("Admin"))
        {

            ayar.Visible = true;
            ayarLi2.Visible = true;
            faturaLi.Visible = true;
            //panelLi.Visible = true;
            maliyetLi.Visible = true;
            //liSms.Visible = true;
            //liMail.Visible = false;
        }
        else if (use.IsInRole("mudur"))
        {
            //panelLi.Visible = true;
            maliyetLi.Visible = true;
        }

        else
        {
            ayar.Visible = false;
            ayarLi2.Visible = false;
            cihazLi.Visible = false;
            roporLi.Visible = false;
            stokLi.Visible = false;
            satisListesiLi.Visible = false;

            alimLi.Visible = false;
            if (use.IsInRole("servis"))
            {
                cariHesapLi.Visible = false;

                satisLi.Visible = false;
                satistopLi.Visible = false;
            }

            paraHesapLi.Visible = false;

            //butunAntenLink.HRef = "/ButunAntenlerBayi.aspx?bayi=" + kul.owner;
        }


        //DataBind();

    }



    protected string resim()
    {
        string yol = "/Uploads/" + KullaniciIslem.firma() + ".png";
        return yol;
        //return KullaniciIslem.resimYol();
        //using (TeknikServis.Radius.radiusEntities dc=TeknikServis.Radius.MyContext.Context(KullaniciIslem.firma()))
        //{
        //    ServisDAL.AyarCurrent ay = new ServisDAL.AyarCurrent(dc);
        //    TeknikServis.Radius.ayargenel a = ay.get();
        //    return re
        //}
    }
    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {

        HttpContext.Current.Cache.Remove("config");
        Context.GetOwinContext().Authentication.SignOut();
    }
    public void MusteriAra(object sender, EventArgs e)
    {
        string kimlik = txtAra.Value.Trim();

        if (!String.IsNullOrEmpty(kimlik))
        {
            Session["kriter_must"] = kimlik;

            string baseUrl = "/TeknikTeknik/ServislerCanli";

            Response.Redirect(baseUrl);

        }

    }
    public void KisiAra(object sender, EventArgs e)
    {
        string kimlik = txtKisi.Value.Trim();

        if (!String.IsNullOrEmpty(kimlik))
        {
            //anten haritasından gelen müşteri aramasını burada iptal edelim.
            if (Session["antendekiler"] != null)
            {
                Session["antendekiler"] = null;
            }
            Session["kriter"] = kimlik;

            string baseUrl = "/Musteri";

            Response.Redirect(baseUrl);

        }

    }
}
