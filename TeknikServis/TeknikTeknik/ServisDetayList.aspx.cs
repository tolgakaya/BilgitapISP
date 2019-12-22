using ServisDAL;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Web.UI;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class ServisDetayList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }
            if (!IsPostBack)
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    BindList(dc);
                }

            }

        }


        protected void btnEkle_Click(object sender, EventArgs e)
        {
            string id = txtServisID.Value;
            string durum = hdnDurumID.Value;
            string atanan = hdnAtananID.Value;
            string kimlik = Request.QueryString["kimlik"];
            Response.Redirect("/TeknikTeknik/ServisDetay.aspx?id=" + id + "&durum=" + durum + "&atanan=" + atanan + "&kimlik=" + kimlik);

        }

        protected void btnHesaplar_Click(object sender, EventArgs e)
        {
            string id = txtServisID.Value;
            string custID = hdnCustID.Value;

            string kimlik = Request.QueryString["kimlik"];

            if (!String.IsNullOrEmpty(custID) && !String.IsNullOrEmpty(id))
            {
                string url = "/TeknikTeknik/ServisHesaplar.aspx?servisid=" + id + "&custid=" + custID;
                if (!String.IsNullOrEmpty(kimlik))
                {
                    url = "/TeknikTeknik/ServisHesaplar.aspx?servisid=" + id + "&custid=" + custID + "&kimlik=" + kimlik;
                }
                Response.Redirect(url);
            }


        }
        protected void btnServis_Click(object sender, EventArgs e)
        {
            string id = txtServisID.Value;
            string custID = hdnCustID.Value;

            string kimlik = Request.QueryString["kimlik"];

            if (!String.IsNullOrEmpty(custID) && !String.IsNullOrEmpty(id))
            {

                if (!String.IsNullOrEmpty(kimlik))
                {
                    string url = "/TeknikTeknik/Servis.aspx?servisid=" + id + "&custid=" + custID + "&kimlik=" + kimlik;
                    Response.Redirect(url);
                }

            }


        }
        private void BindList(radiusEntities dc)
        {
            string kimlikNo = Request.QueryString["kimlik"];
            string durum = Request.QueryString["durum"];
            string eski_durum = Request.QueryString["eski_durum"];

            string servis_id = Request.QueryString["servisid"];
            if (!String.IsNullOrEmpty(kimlikNo) && !String.IsNullOrWhiteSpace(kimlikNo))
            {
                ServisIslemleri islem = new ServisIslemleri(dc);
                ServisDAL.Repo.ServisRepo ser = islem.servisAraKimlikDetayTekR(kimlikNo);
                if (ser != null)
                {
                    txtKimlikNo.Value = kimlikNo;
                    txtMusteri.InnerHtml = ser.musteriAdi + " - " + ser.telefon;
                    txtKonu.InnerHtml = ser.baslik;

                    //int sayi = ser.aciklama.Split(new[] { '\r', '\n' }).Length;
                    //txtServisAciklama.Rows = sayi + 1;
                    txtServisAciklama.InnerHtml = ser.aciklama;
                    txtServisAdresi.InnerHtml = ser.adres;
                    txtDurum.Value = ser.sonDurum;
                    txtTarih.Value = ser.acilmaZamani.ToString();
                    txtServisID.Value = ser.serviceID.ToString();
                    if (ser.kapanmaZamani != null)
                    {
                        btnSonlandir.Visible = false;

                        btnEkle.Visible = false;
                        btnSonlandirK.Visible = false;

                        btnEkleK.Visible = false;
                    }
                    hdnDurumID.Value = ser.sonDurumID.ToString();
                    hdnCustID.Value = ser.custID.ToString();
                    hdnAtananID.Value = ser.sonGorevliID.ToString();
                    //bakalım görevli değişmiş mi?
                    string yeniGorevli = Request.QueryString["eleman"];
                    if (!String.IsNullOrEmpty(yeniGorevli))
                    {
                        if (yeniGorevli != hdnAtananID.Value)
                        {

                            islem.Atama(yeniGorevli, kimlikNo);
                        }
                    }

                    ListView1.DataSource = islem.detayListesiDetayKimlikR(kimlikNo);
                    ListView1.DataBind();

                }
            }
            string smsQ = Request.QueryString["sms"];
            string mailQ = Request.QueryString["mail"];
            MesajGonder(kimlikNo, durum, eski_durum, servis_id, smsQ, mailQ, dc);

        }

        private void MesajGonder(string kimlikNo, string durum, string eski_durum, string servis_id, string smsQ, string mailQ, radiusEntities dc)
        {
            if (!String.IsNullOrEmpty(smsQ) && !String.IsNullOrEmpty(mailQ))
            {
                if (smsQ == "1" || mailQ == "1")
                {
                    if (!String.IsNullOrEmpty(durum) && !String.IsNullOrEmpty(eski_durum))
                    {
                        int durum_id = Int32.Parse(durum);
                        int eski_durum_id = Int32.Parse(eski_durum);

                        if (durum_id != eski_durum_id)
                        {
                            int servisid = Int32.Parse(servis_id);

                            string firma = KullaniciIslem.firma();

                            AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                            //daha önceki durum değimiş mi ona göre mail atma kararı verilecek
                            //aynı durumda yeni bir şey ekleniyorsa mail atılmayacak
                            TeknikServis.Radius.service_durums durum_ayar = ayarimiz.tekDurumR(durum_id);

                            int custID = Int32.Parse(hdnCustID.Value);
                            TeknikServis.Radius.customer musteri_bilgileri = dc.customers.Where(p => p.CustID == custID).FirstOrDefault();

                            if (mailQ == "1")
                            {
                                if (durum_ayar.Mail == true)
                                {
                                    //mail gönder

                                    string mail = musteri_bilgileri.email;

                                    //bu bilgileri mail temasına kendileri koysun
                                    string adres = "";// rep.Adres;
                                    string tel = "";// rep.Telefon;
                                    string eposta = "";//  rep.Eposta;
                                    string FirmaTam = "";//  rep.FirmaTam;
                                    string web = "";// rep.Web;
                                    //EPosta.SendingMail(ayarimiz, firma, mail, konu, musteri_bilgileri.Ad, FirmaTam, adres, tel, web, durum_ayar.Durum, kimlikNo, "bayi", "tema");
                                    ServisDAL.MailIslemleri mi = new MailIslemleri(dc);

                                    //Response.Redirect("/Default.aspx?id=" + gonder);
                                    if (!String.IsNullOrEmpty(mail))
                                    {


                                        if (durum_ayar.sonmu == true)
                                        {
                                            //servis kapatma maili gönderilecek
                                            mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, "sonlanma", "");

                                        }
                                        else if (durum_ayar.kararmi == true)
                                        {
                                            //Servis kararı için mail atılacak
                                            mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, "karar_bekleniyor", "");
                                        }
                                        else if (durum_ayar.onaymi == true)
                                        {
                                            mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, "karar_onaylandi", "");
                                        }
                                        else if (durum_ayar.baslangicmi == true)
                                        {
                                            mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, "baslangic", "");
                                        }
                                        else
                                        {
                                            mi.SendingMailDurum(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, durum_id, "");

                                        }
                                    }

                                }
                            }

                            if (smsQ == "1" && durum_ayar.SMS == true)
                            {
                                string ekMesaj = "Servis No: " + kimlikNo + "Servis durumu: " + durum_ayar.Durum;
                                ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                                string mesele = sms.SmsGonder("durum", durum_id, ayarimiz, musteri_bilgileri.telefon, ekMesaj);
                                //  Session["mesele"] = mesele;
                                //  Response.Redirect("/Deneme.aspx");
                            }

                        }

                    }
                }
            }
        }



        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                TextBox resim = (TextBox)e.Item.FindControl("txtYol");
                if (String.IsNullOrEmpty(resim.Text) || resim.Text == "-")
                {
                    HtmlGenericControl cerceve = (HtmlGenericControl)e.Item.FindControl("resimCerceve");
                    cerceve.Visible = false;
                }

                //ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                //// you would use your actual data item type here, not "object"
                //ServisDetayRepo repo = (ServisDetayRepo)dataItem.DataItem;


                //string currentAciklama = repo.aciklama;


                //int sayi = currentAciklama.Split(new[] { '\r', '\n' }).Length;
                //TextBox txtDetayimiz = (TextBox)e.Item.FindControl("txtDetayAciklama");
                //txtDetayimiz.Rows = sayi + 1;


            }
        }
        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            //set current page startindex, max rows and rebind to false
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                BindList(dc);
            }

        }
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            pager.Visible = DataPager1.TotalRowCount > DataPager1.MaximumRows;
        }

        protected void btnOnay_Click(object sender, EventArgs e)
        {

            kullanici_repo rep = KullaniciIslem.currentKullanici();

            string id = txtServisID.Value;
            int servisid = Int32.Parse(id);
            int custID = Int32.Parse(hdnCustID.Value);

            string firma = rep.Firma;
            string kimlikNo = Request.QueryString["kimlik"];

            using (radiusEntities dc = MyContext.Context(firma))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                int durum_id = ser.servisKapatR(servisid, User.Identity.Name);
                if (custID != -99)
                {
                    if (chcMail.Checked == true)
                    {


                        TeknikServis.Radius.customer musteri_bilgileri = dc.customers.Where(p => p.CustID == custID).FirstOrDefault();

                        ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                        mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, kimlikNo, "sonlanma", "");
                    }

                    if (chcSms.Checked == true)
                    {


                        TeknikServis.Radius.customer musteri_bilgileri = dc.customers.Where(p => p.CustID == custID).FirstOrDefault();
                        AyarIslemleri ayarimiz = new AyarIslemleri(dc);

                        string ekMesaj = "Servis No: " + kimlikNo;
                        ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                        sms.SmsGonder("durum", durum_id, ayarimiz, musteri_bilgileri.telefon, ekMesaj);

                    }

                }
            }


            //kapatma belgesi yazdırılacak/yada burada olmadan yazdırılabilir.
            string url = "/TeknikTeknik/ServisDetayList.aspx?kimlik=" + kimlikNo;
            Response.Redirect(url);
        }
        protected void btnSonlandir_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);

        }
        protected void btnBelge_Click(object sender, EventArgs e)
        {

            string id = txtServisID.Value;
            int servisid = Int32.Parse(id);

            //burada servis başlama için bir geçiş sınıfıyla Session'a atıp Baski.aspx'e gönder
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                FaturaBas bas = new FaturaBas(dc);
                AyarCurrent ay = new AyarCurrent(dc);
                Servis_Baslama servisBilgisi = bas.ServisBilgileri(txtKimlikNo.Value.Trim(), ay.get());
                Session["Servis_Baslama"] = servisBilgisi;
            }

            string uri = "/Baski.aspx?tip=baslama";
            Response.Redirect(uri);

            //string kimlikNo = Request.QueryString["kimlik"];
            //string url = "/TeknikTeknik/ServisBelgesi.aspx?kimlik=" + kimlikNo;
            //Response.Redirect(url);
        }

        protected void btnYol_Click(object sender, EventArgs e)
        {

            string custID = hdnCustID.Value;

            Response.Redirect("/TeknikHarita/MusteriYolu.aspx?id=" + custID);
        }
    }
}