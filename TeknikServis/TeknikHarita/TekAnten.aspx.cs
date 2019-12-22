using ServisDAL;
using System;
using System.Web;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace BilgitapServis
{
    public partial class TekAnten : System.Web.UI.Page
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
        protected void btnMusteriler_Click(object sender, EventArgs e)
        {
            //seçilen antendeki müşteri listesine gönderiyor
            string s = txtResult.Value;
            Session["antendekiler"] = s;
            if (Session["kriter"] != null)
            {
                Session["kriter"] = null;
            }
            Response.Redirect("/Musteri.aspx");

        }
        protected void btnAna_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void btnHaritayaGore_Click(object sender, EventArgs e)
        {
            //txtNoktalardaki id-koordinat iikilisini müşteri koordinatı olarak güncelle

            //Session["mesele"] = txtNoktalar.Value;
            //Response.Redirect("/sonuc.aspx");
            if (!String.IsNullOrEmpty(txtNoktalar.Value))
            {
                using (TeknikServis.Radius.radiusEntities dc = TeknikServis.Radius.MyContext.Context(TeknikServis.Logic.KullaniciIslem.firma()))
                {
                    ServisDAL.MusteriIslemleri m = new ServisDAL.MusteriIslemleri(dc);
                    m.haritayaGoreKaydet(txtNoktalar.Value);
                    Response.Redirect("/Musteri.aspx?tip=hepsi");
                }
            }

        }

        protected void btnAnteneMusteri_Click(object sender, EventArgs e)
        {
            //txtNoktalardaki id-koordinat stringini antenid si ile müşterilere kaydet.
            string id = Request.QueryString["id"];
            if (!String.IsNullOrEmpty(txtNoktalar.Value) && !String.IsNullOrEmpty(id))
            {
                using (TeknikServis.Radius.radiusEntities dc = TeknikServis.Radius.MyContext.Context(TeknikServis.Logic.KullaniciIslem.firma()))
                {
                    ServisDAL.MusteriIslemleri m = new ServisDAL.MusteriIslemleri(dc);
                    m.haritayaGoreanteneKaydet(txtNoktalar.Value, Convert.ToInt32(id));
                    if (Session["kriter"] != null)
                    {
                        Session["kriter"] = null;
                    }
                    Response.Redirect("/Musteri.aspx?tip=hepsi");
                }
            }
        }

    }
}