using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace BilgitapServis
{
    public partial class HaritaAntenKaydet : System.Web.UI.Page
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
        protected void btnAntenKaydet_Click(object sender, EventArgs e)
        {

            string aciStr = txtAci.Value;
            string ad = txtAntenAdi.Value;
            string A = labelPozisyonA.Value;
            string B = labelPozisyonB.Value;
            string C = labelPozisyonC.Value;


            if (!String.IsNullOrEmpty(ad) || !String.IsNullOrWhiteSpace(ad) || !String.IsNullOrEmpty(aciStr) || !String.IsNullOrEmpty(A) || !String.IsNullOrEmpty(B) || !String.IsNullOrEmpty(C))
            {

                List<string> listeA = A.Split((new char[] { ')', '(', ',' }), StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> listeB = B.Split((new char[] { ')', '(', ',' }), StringSplitOptions.RemoveEmptyEntries).ToList();
                List<string> listeC = C.Split((new char[] { ')', '(', ',' }), StringSplitOptions.RemoveEmptyEntries).ToList();

                string idd = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(idd))
                {
                    //anten update
                    int id = Int32.Parse(idd);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        anten aranan = dc.antens.Where(a => a.anten_id == id).FirstOrDefault();
                        if (aranan != null)
                        {
                            aranan.anten_adi = ad;
                            aranan.center_Lat = listeA[0];
                            aranan.center_Long = listeA[1];
                            aranan.start_Lat = listeB[0];
                            aranan.start_Long = listeB[1];
                            aranan.end_Lat = listeC[0];
                            aranan.end_Long = listeC[1];

                            dc.SaveChanges();
                            Response.Redirect("/TeknikHarita/Antenler.aspx");
                        }
                    }


                }
                else
                {
                    //yeniantenkaydı
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        anten yeni = new anten();
                        yeni.anten_adi = ad;
                        yeni.center_Lat = listeA[0];
                        yeni.center_Long = listeA[1];
                        yeni.start_Lat = listeB[0];
                        yeni.start_Long = listeB[1];
                        yeni.end_Long = listeC[0];
                        yeni.end_Lat = listeC[1];
                        dc.antens.Add(yeni);
                        dc.SaveChanges();
                        Response.Redirect("/TeknikHarita/Antenler.aspx");
                    }

                }

            }
            //

        }

        protected void btnAna_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }
    }
}