using ServisDAL;
using System;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class EmanetVer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            string custidd = Request.QueryString["custid"];

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(custidd))
                {
                    int custid = Int32.Parse(custidd);
                    string aciklama = txtAciklama.Text;

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarCurrent ay = new AyarCurrent(dc);
                        if (ay.lisansKontrol() == false)
                        {
                            Response.Redirect("/LisansError");
                        }
                        MusteriIslemleri m = new MusteriIslemleri(dc);
                        Radius.customer must = m.musteriTekR(custid);
                        musteri.InnerHtml = must.Ad;
                        musteriEkBilgi.InnerHtml = must.telefon;
                    }

                    tarih.InnerHtml = DateTime.Now.ToString("D");

                }
            }


        }
        protected void yonlendir(string url, radiusEntities dc, int custid)
        {
            if (cbYazdir.Checked == true)
            {
                //Session["ctrl"] = yazdir;
                //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('../Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");
                ServisDAL.AyarCurrent ay = new AyarCurrent(dc);
                FaturaBas bas = new FaturaBas(dc);
                Makbuz_Gorunum faturaBilgisi = bas.MakbuzBilgileri(custid, txtAciklama.Text, ay.get(),0,User.Identity.Name);
                Session["Makbuz_Gorunum"] = faturaBilgisi;

                Response.Redirect("/Baski.aspx?tip=emanet");

            }
            else
            {
                Response.Redirect(url);
            }
        }
        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            string custidd = Request.QueryString["custid"];


            if (!String.IsNullOrEmpty(custidd))
            {
                //yeni ekleme

                int custid = Int32.Parse(custidd);
                string aciklama = txtAciklama.Text;
                //string firma = KullaniciIslem.firma();

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    s.emanetUrunVerR(custid, aciklama, User.Identity.Name);
                    string url = "/TeknikEmanet/Emanetler.aspx?custid=" + custid;
                    yonlendir(url, dc, custid);
                }





            }


        }

        protected void btnEmanetler_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];


            if (!String.IsNullOrEmpty(custidd))
            {
                string url = "/TeknikEmanet/Emanetler.aspx?custid=" + custidd;
                Response.Redirect(url);

            }
        }
    }
}