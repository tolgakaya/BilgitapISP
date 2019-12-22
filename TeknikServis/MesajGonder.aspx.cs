using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class MesajGonder : System.Web.UI.Page
    {

        string firma;

        public MesajGonder()
        {
            this.firma = KullaniciIslem.firma();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            if (!IsPostBack)
            {
                Goster();
                ViewState["PreviousPage"] =
        Request.UrlReferrer;//Saves the Previous page url in ViewState
            }

        }
        private void geri()
        {
            if (ViewState["PreviousPage"] != null)	//Check if the ViewState 
            //contains Previous page URL
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());//Redirect to 
                //Previous page by retrieving the PreviousPage Url from ViewState.
            }
        }
        private void Gonder()
        {
            if (Request.QueryString["tur"] != null)
            {
                string tur = Request.QueryString["tur"];
                if (Request.QueryString["tip"] != null)
                {
                    string tip = Request.QueryString["tip"];
                    string gonderen = txtGonderen.Text;
                    string mesaj = txtMesaj.Text;
                    if (tur == "sms")
                    {

                        SmsYolla(tip, gonderen, mesaj);
                        Session["teller"] = null;
                        geri();
                    }
                    else if (tur == "mail")
                    {
                        MailYolla(tip, gonderen, mesaj);
                        Session["mailler"] = null;
                        geri();
                    }

                }
            }

        }

        private void MailYolla(string tip, string gonderen, string mesaj)
        {
            //toplu mail apisi kullanıldığı zaman eklenecek
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MailIslemleri m = new MailIslemleri(dc);

                if (tip == "gnltp")
                {

                    //genel Mail

                    if (Session["mailler"] != null)
                    {
                        string s = Session["mailler"].ToString();
                        if (!String.IsNullOrEmpty(s))
                        {

                            m.MailToplu("genel", gonderen, mesaj, s, "");
                        }

                    }
                    else
                    {
                        MusteriIslemleri musteri = new MusteriIslemleri(dc);

                        m.MailToplu("genel", gonderen, mesaj, musteri.mailListesiR(), "");
                        //Response.Redirect("/Deneme.aspx");

                    }
                }
            }

        }

        private void SmsYolla(string tip, string gonderen, string mesaj)
        {
            using (radiusEntities dc = MyContext.Context(firma))
            {
                SmsIslemleri sms = new SmsIslemleri(dc);
                AyarIslemleri ayarimiz = new AyarIslemleri(dc);

                if (tip == "ggl")
                {
                    string kritik = Request.QueryString["kritik"];

                    sms.SmsGunuGelen(ayarimiz, gonderen, mesaj, kritik);
                    //Response.Redirect("/Deneme.aspx");
                    //gunugelenSMS
                }

                else if (tip == "gnltp")
                {
                    //genel mesaj
                    if (Session["teller"] != null)
                    {
                        string s = Session["teller"].ToString();
                        if (!String.IsNullOrEmpty(s))
                        {
                            string[] teller = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            string mesaj_sonucu = sms.SmsGenel(ayarimiz, gonderen, mesaj, teller);
                            Session["mesele"] = sms.MesajSonucu(mesaj_sonucu);
                            sms.CariMesajKaydet(teller);
                            Response.Redirect("/Sonuc");

                        }

                    }
                    else
                    {

                        Session["mesele"] = "Oturumunuz zaman aşımına uğradı. Lütfen tekrar liste oluşturunuz!";
                        Response.Redirect("/Sonuc");

                    }

                }
            }

        }

        private void Goster()
        {
            if (Request.QueryString["tur"] != null)
            {
                string tur = Request.QueryString["tur"];
                string custID = Request.QueryString["custID"];
                if (Request.QueryString["tip"] != null)
                {
                    using (radiusEntities dc = MyContext.Context(firma))
                    {

                        string tip = Request.QueryString["tip"];
                        SmsIslemleri sms = new SmsIslemleri(dc);

                        if (tur == "sms")
                        {

                            if (tip == "ggl")
                            {
                                Radius.sms_ayars ayar = sms.SmsAyarGoster("yaklasan_taksit");
                                //gunugelenSMS
                                if (ayar != null)
                                {
                                    baslik.InnerHtml = "Yaklaşan Ödemeler İçin Sms";
                                    txtGonderen.Text = ayar.gonderen;
                                    txtMesaj.Text = ayar.mesaj;
                                }
                            }

                            else if (tip == "gnltp")
                            {
                                baslik.InnerHtml = "Liste İçin Sms";
                                Radius.sms_ayars ayar = sms.SmsAyarGoster("yaklasan_taksit");

                                if (ayar != null)
                                {

                                    txtGonderen.Text = ayar.gonderen;

                                }
                                if (Session["teller"] != null)
                                {
                                    string teller = Session["teller"].ToString();
                                    liste.Text = teller;
                                    int adet = teller.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                                    baslik.InnerHtml = adet.ToString() + "Adetlik Liste İçin Sms";
                                }
                            }
                        }
                        else if (tur == "mail")
                        {
                            if (tip == "ggl")
                            {
                                //gunugelenMail
                                baslik.InnerHtml = "Yaklaşan Ödemeler İçin Mail";
                            }

                            else if (tip == "gnltp")
                            {
                                //genel Mail
                                baslik.InnerHtml = "Liste İçin Mail";
                            }
                        }
                    }
                    ///
                }
            }

        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            Gonder();
        }
    }
}