using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class HaritaMusteriKaydet : System.Web.UI.Page
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
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
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
        protected void btnMusteriKaydet_Click(object sender, EventArgs e)
        {
            //query stringle müşteri Id'si gelecekre
            string idd = Request.QueryString["id"];
            if (idd != null)
            {
                int sonuc = -1;
                int id = Int32.Parse(idd);
                using (TeknikServis.Radius.radiusEntities dc=Radius.MyContext.Context(KullaniciIslem.firma()))
                {
                    Radius.customer musteri = (from a in dc.customers
                                               where a.CustID == id
                                               select a).FirstOrDefault();
                    string s = txtLatitude.Value;
                    if (!String.IsNullOrEmpty(s))
                    {
                        List<string> liste = s.Split((new char[] { ')', '(', ',' }), StringSplitOptions.RemoveEmptyEntries).ToList();

                        musteri.Lat = liste[0];
                        musteri.Long = liste[1];
                        dc.SaveChanges();

                    }
                }

                if (sonuc>0)
                {
                    Response.Redirect("/Musteri.aspx");
                }

            }


        }
        protected void btnAna_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }
    }
}