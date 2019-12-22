using ServisDAL;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Web.UI;
using TeknikServis.Radius;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.IO;
using System.Text;
using ServisDAL.Repo;

namespace TeknikServis.TeknikTeknik
{
    public partial class Servis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    //BindList(dc);
                    //goster(dc);
                    ortak(dc);
                    cihazGoster(dc);
                    ustaGoster(dc);
                }
                //else
                //{
                //    goster(dc);
                //}
            }
            if (!User.IsInRole("Admin") && !User.IsInRole("mudur"))
            {
                btnSonlandir.Visible = false;
                btnSonlandirK.Visible = false;
                txtHesapMaliyet.Visible = false;
            }

        }

        private void ortak(radiusEntities dc)
        {
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];

            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id))
            {
                int servisid = Int32.Parse(servis_id);

                TekServis tek = new TekServis(dc, servisid);
                ServisInfo s = tek.servis();
                ServisRepo genel = s.genel;

                btnTopluOnay.Visible = true;
                btnMusteriDetayim.Visible = true;
                btnMusteriDetayimK.Visible = true;


                var liste = s.kararlar;
                int adet = 0;
                decimal mal = 0;
                decimal tutar = 0;
                if (liste.Count > 0)
                {
                    adet = liste.Count;
                    mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                    tutar = (decimal)liste.Sum(x => x.yekun);
                }
                txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                txtServisTutar.Value = tutar.ToString("C");
                GridView1.DataSource = liste;

                if (s.genel.kapanmaZamani != null)
                {
                    btnTopluOnay.Visible = false;
                    btnTamirciye.Visible = false;
                    btnUstaAta.Visible = false;
                    btnPaketEkle.Visible = false;
                    btnEkleH.Visible = false;

                }


                //listview kısmı
                string kimlikNo = Request.QueryString["kimlik"];
                string durum = Request.QueryString["durum"];
                string eski_durum = Request.QueryString["eski_durum"];

                if (!String.IsNullOrEmpty(kimlikNo) && !String.IsNullOrWhiteSpace(kimlikNo))
                {
                    ServisIslemleri islem = new ServisIslemleri(dc);

                    drdPaketler.AppendDataBoundItems = true;
                    drdPaketler.DataSource = islem.servis_paketleri();
                    drdPaketler.DataValueField = "paket_id";
                    drdPaketler.DataTextField = "paket_adi";
                    drdPaketler.DataBind();

                    txtKimlikNo.Value = kimlikNo;
                    txtMusteri.InnerHtml = genel.musteriAdi + " - " + genel.telefon;
                    txtKonu.InnerHtml = genel.baslik;

                    //int sayi = ser.aciklama.Split(new[] { '\r', '\n' }).Length;
                    //txtServisAciklama.Rows = sayi + 1;
                    txtServisAciklama.InnerHtml = genel.aciklama;
                    txtServisAdresi.InnerHtml = genel.adres;
                    txtDurum.Value = genel.sonDurum;
                    txtTarih.Value = genel.acilmaZamani.ToString();
                    txtServisID.Value = genel.serviceID.ToString();
                    txtServisCihaz.Value = genel.urunAdi;
                    txtServisUsta.Value = genel.usta;
                    txtKullanici.Value = genel.kullanici;
                    if (genel.kapanmaZamani != null)
                    {
                        btnSonlandir.Visible = false;
                        btnEkle.Visible = false;
                        btnSonlandirK.Visible = false;
                        btnEkleK.Visible = false;
                    }
                    hdnDurumID.Value = genel.sonDurumID.ToString();
                    hdnCustID.Value = genel.custID.ToString();
                    hdnAtananID.Value = genel.sonGorevliID.ToString();
                    //bakalım görevli değişmiş mi?
                    string yeniGorevli = Request.QueryString["eleman"];
                    if (!String.IsNullOrEmpty(yeniGorevli))
                    {
                        if (yeniGorevli != hdnAtananID.Value)
                        {

                            islem.Atama(yeniGorevli, kimlikNo);
                        }
                    }

                    ListView1.DataSource = s.detaylar;
                    ListView1.DataBind();


                }
                string smsQ = Request.QueryString["sms"];
                string mailQ = Request.QueryString["mail"];
                MesajGonder(kimlikNo, durum, eski_durum, servis_id, smsQ, mailQ, dc);
            }
            cihazGoster(dc);
            ustaGoster(dc);
            GridView1.DataBind();

        }
        private void goster(radiusEntities dc)
        {


            ServisIslemleri s = new ServisIslemleri(dc);
           
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];

            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id))
            {
                btnTopluOnay.Visible = true;
                btnMusteriDetayim.Visible = true;
                btnMusteriDetayimK.Visible = true;
                //btnServisDetaylari.Visible = true;
                int servisid = Int32.Parse(servis_id);
                var liste = s.servisKararListesiDetayR(servisid);
                int adet = 0;
                decimal mal = 0;
                decimal tutar = 0;
                if (liste.Count > 0)
                {
                    adet = liste.Count;
                    mal = (decimal)liste.Sum(x => x.toplam_maliyet);
                    tutar = (decimal)liste.Sum(x => x.yekun);
                }
                txtHesapAdet.InnerHtml = " Adet: " + adet.ToString();
                txtHesapMaliyet.InnerHtml = "Maliyet:" + mal.ToString("C");
                txtHesapTutar.InnerHtml = "Tutar: " + tutar.ToString("C");
                txtServisTutar.Value = tutar.ToString("C");
                GridView1.DataSource = liste;
                //burada servisID var buna göre servisin detaylarına gidilebilir
                if (s.servis_kapalimi(servisid))
                {
                    btnTopluOnay.Visible = false;
                    btnTamirciye.Visible = false;
                    btnUstaAta.Visible = false;
                    btnPaketEkle.Visible = false;
                    btnEkleH.Visible = false;

                }

            }



            //cihazGoster(dc);
            //ustaGoster(dc);
            GridView1.DataBind();
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

                //drdPaketler.AppendDataBoundItems = true;
                //drdPaketler.DataSource = islem.servis_paketleri();
                //drdPaketler.DataValueField = "paket_id";
                //drdPaketler.DataTextField = "paket_adi";
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

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            string servisidd = Request.QueryString["servisid"];
            string custidd = Request.QueryString["custid"];
            string hesapidd = hdnHesapIDDuzen.Value;
            string kimlik = Request.QueryString["kimlik"];
            int? custid = null;
            if (!String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                if (!String.IsNullOrEmpty(servisidd) && String.IsNullOrEmpty(hesapidd))
                {

                    int adet = 1;
                    string adet_s = txtAdet.Text;
                    if (!String.IsNullOrEmpty(adet_s))
                    {
                        adet = Int32.Parse(adet_s);
                    }

                    //yeni ekleme
                    ServisIslemleri s = new ServisIslemleri(dc);

                    int servisid = Int32.Parse(servisidd);

                    string islem = txtIslemParca.Value;

                    decimal kdv = Decimal.Parse(txtKDVOraniDuzenle.Text);
                    decimal yekun = Decimal.Parse(txtYekun.Text);
                    string aciklama = txtAciklama.Text;

                    int cihaz_id = -1;
                    string cihaz = txtCihazAdiGoster.Value;
                    if (grdCihaz.SelectedIndex > -1)
                    {
                        cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                    }

                    DateTime karar_tarihi = DateTime.Now;
                    string tarS = tarih2.Value;
                    if (!String.IsNullOrEmpty(tarS))
                    {
                        karar_tarihi = DateTime.Parse(tarS);
                    }

                    if (cihaz_id > -1)
                    {
                        string sure = hdnGarantiSure.Value;
                        s.serviceKararEkleRYeniCihazli(servisid, custid, islem, kdv, yekun, aciklama, (int)cihaz_id, adet, cihaz, sure, karar_tarihi, User.Identity.Name);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append(" alertify.success('Hesap!');");
                        sb.Append("$('#yeniModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);


                    }
                    else
                    {

                        try
                        {
                            s.serviceKararEkleRYeni(servisid, custid, islem, kdv, yekun, aciklama, adet, karar_tarihi, User.Identity.Name);
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");

                            sb.Append(" alertify.success('Hesap Kaydedildi!');");
                            sb.Append("$('#yeniModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);

                        }
                        catch (DbEntityValidationException ex)
                        {
                            Dictionary<string, string> mesajlar = new Dictionary<string, string>();
                            foreach (var errs in ex.EntityValidationErrors)
                            {
                                foreach (var err in errs.ValidationErrors)
                                {
                                    string propName = err.PropertyName;
                                    string errMess = err.ErrorMessage;
                                    mesajlar.Add(propName, errMess);
                                }
                            }
                            HttpContext.Current.Session["mesaj"] = mesajlar;
                            HttpContext.Current.Response.Redirect("/Deneme.aspx");
                        }

                    }

                    if (custid != null)
                    {
                        MesajGonder(dc, s, servisid, (int)custid, islem, yekun);
                    }

                }
                else
                {
                    ServisGuncelle(servisidd, custidd, hesapidd, kimlik, dc);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap güncellendi!');");
                    sb.Append("$('#yeniModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniHideModalScript", sb.ToString(), false);
                }

                goster(dc);
            }
        }

        private void ServisGuncelle(string servisidd, string custidd, string hesapidd, string kimlik, radiusEntities dc)
        {
            int? custid = null;
            if (String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }
            if (!String.IsNullOrEmpty(hesapidd))
            {
                //update işlemi

                ServisIslemleri s = new ServisIslemleri(dc);
                int adet = 1;
                int cihaz_id = -1;
                string adet_s = txtAdet.Text;

                if (!String.IsNullOrEmpty(adet_s))
                {
                    adet = Int32.Parse(adet_s);
                }
                string cihaz = txtCihazAdiGoster.Value;
                if (grdCihaz.SelectedValue != null)
                {
                    cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                }
                int hesapid = Int32.Parse(hesapidd);
                string islem = txtIslemParca.Value;
                decimal kdvOran = Decimal.Parse(txtKDVOraniDuzenle.Text);

                decimal yekun = Decimal.Parse(txtYekun.Text);
                string aciklama = txtAciklama.Text;



                //urun bilgisi yok normal güncelleme yapıyoruz
                if (cihaz_id > -1)
                {
                    //cihazlı
                    s.serviceKararGuncelleCihazli(hesapid, islem, aciklama, kdvOran, yekun, cihaz, cihaz_id, adet, User.Identity.Name);
                }
                else
                {
                    s.serviceKararGuncelleR(hesapid, islem, aciklama, kdvOran, yekun, User.Identity.Name);
                }
                goster(dc);
            }
        }

        private void MesajGonder(radiusEntities dc, ServisIslemleri s, int servisid, int custid, string islem, decimal yekun)
        {
            if (chcMail.Checked == true || chcSms.Checked == true)
            {
                MusteriIslemleri musteri = new MusteriIslemleri(dc);
                Radius.service serr = s.servisTekR(servisid);
                Radius.customer musteri_bilgileri = musteri.musteriTekR(custid);
                if (chcMail.Checked == true)
                {
                    string ekMesaj = "Yapılacak işlem: <b>" + islem + "</b><br/>" + "Tutar :<b>" + yekun.ToString("C");


                    ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                    mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, serr.Servis_Kimlik_No, "karar_bekleniyor", ekMesaj);

                }
                if (chcSms.Checked == true)
                {
                    //sms izni alınacak
                    ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                    AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                    string ekMesajSms = "Servis No: " + serr.Servis_Kimlik_No + " İşlem: " + islem + " Tutar: " + yekun.ToString() + "TL";
                    sms.SmsGonder("durum", (int)serr.durum_id, ayarimiz, musteri_bilgileri.telefon, ekMesajSms);

                }
            }
        }

        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                cihazGoster(dc);
            }

            string cihaz = (grdCihaz.SelectedRow.FindControl("btnRandom") as LinkButton).Text;
            string sure = grdCihaz.SelectedRow.Cells[4].Text;
            txtCihazAdiGoster.Value = cihaz;
            hdnGarantiSure.Value = sure.ToString();

        }



        protected void btnAddCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#yeniModal').modal('hide');");
            sb.Append("$('#addModalCihaz').modal('show');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
        }

        protected void btnAddCihazRecord_Click(object sender, EventArgs e)
        {
            string gs = txtGarantiSuresi.Text;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                s.CihazEkle(txtCihazAdi.Text, txtCihazAciklama.Text, gs);
                cihazGoster(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append(" alertify.success('Kayıt Eklendi!');");

            sb.Append("$('#addModalCihaz').modal('hide');");
            //sb.Append("$('#yeniModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
        }
        protected void CihazAra(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                cihazGoster(dc);
            }
        }

        private void cihazGoster(radiusEntities dc)
        {
            CihazMalzeme c = new CihazMalzeme(dc);

            string arama_terimi = txtAra.Value;
            grdCihaz.DataSource = c.CihazListesi2(arama_terimi);
            grdCihaz.DataBind();


        }

        protected void btnMusteriDetayim_Click(object sender, EventArgs e)
        {
            string custidd = Request.QueryString["custid"];
            Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    int hesapID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.servisKararIptalR(hesapID, User.Identity.Name);
                        goster(dc);
                    }


                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }


                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }

            else if (e.CommandName.Equals("onay"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int hesapID = Convert.ToInt32(arg[0]);
                int musteriID = Convert.ToInt32(arg[2]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                string islem = row.Cells[2].Text;
                string yekun = row.Cells[5].Text;
                string servisid = row.Cells[10].Text;


                hdnHesapIDH.Value = hesapID.ToString();
                hdnMusteriIDH.Value = musteriID.ToString();

                hdnServisIDDH.Value = servisid;
                hdnYekunnH.Value = yekun;
                hdnIslemmH.Value = islem;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModalH').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
            }

            else if (e.CommandName.Equals("duzenle"))
            {
                //burada tamircili yada normal hesap olduğu kontrol edilecek
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int hesapid = Convert.ToInt32(arg[0]);

                grdCihaz.SelectedIndex = -1;

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    TeknikServis.Radius.servicehesap hesap = s.tekHesapR(hesapid);
                    if (hesap.tamirci_id == null)
                    {
                        hdnHesapIDDuzen.Value = hesapid.ToString();
                        txtAciklama.Text = hesap.Aciklama;
                        txtIslemParca.Value = hesap.IslemParca;
                        //burda normal KDV miktarı varama text alanında sadece oranını gösteriyoruz
                        txtKDVDuzenle.Text = hesap.KDV.ToString();

                        decimal orand = (decimal)((hesap.KDV * 100) / (hesap.Yekun - hesap.KDV));
                        string oran = Math.Round(orand, 2).ToString();
                        txtKDVOraniDuzenle.Text = oran;
                        //txtTutar.Text = hesap.Tutar.ToString();
                        txtYekun.Text = hesap.Yekun.ToString();
                        txtAdet.Text = hesap.adet.ToString();
                        txtCihazAdiGoster.Value = hesap.cihaz_adi;
                        hdnGarantiSure.Value = hesap.cihaz_gsure.ToString();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#yeniModal').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniShowModalScript", sb.ToString(), false);


                    }
                    else
                    {

                        hdnHesapIDDuzenTamirci.Value = hesapid.ToString();
                        txtAciklamaTamirci.Text = hesap.Aciklama;
                        txtIslemParcaTamirci.Value = hesap.IslemParca;
                        hdnTamirciID.Value = hesap.tamirci_id.ToString();
                        decimal orand = (decimal)((hesap.KDV * 100) / (hesap.Yekun - hesap.KDV));
                        string oran = Math.Round(orand, 2).ToString();
                        txtKDVOraniDuzenleTamirci.Text = oran;
                        txtKDVDuzenleTamirci.Text = hesap.KDV.ToString();
                        txtYekunTamirci.Text = hesap.Yekun.ToString();
                        txtTamirciMaliyet.Text = hesap.toplam_maliyet.ToString();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#tamirciModal').modal('show');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciShowModalScript", sb.ToString(), false);

                    }

                }



            }

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                    (e.Row.FindControl("delLinkk") as LinkButton).Visible = false;
                }
            }
        }
        protected void btnEkle_ClickH(object sender, EventArgs e)
        {
            string servisidd = Request.QueryString["servisid"];
            string custidd = Request.QueryString["custid"];
            hdnHesapIDDuzen.Value = "";
            grdCihaz.SelectedIndex = -1;
            if (!String.IsNullOrEmpty(custidd) && !String.IsNullOrEmpty(servisidd))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#yeniModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "yeniModalScript", sb.ToString(), false);
            }
            else
            {
                Response.Redirect("/TeknikTeknik/ServislerCanli.aspx?=" + custidd);
            }



        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
            ExportHelper.ToExcell(GridView1, isim);
        }
        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string isim = "Servis Kararları-" + DateTime.Now.ToString();
            ExportHelper.ToWord(GridView1, isim);
        }

        protected void btnPrnt_Click(object sender, EventArgs e)
        {
            //Session["ctrl"] = GridView1;
            //ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script language=javascript>window.open('Print.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>");

            GridView1.AllowPaging = false;
            GridView1.RowStyle.ForeColor = System.Drawing.Color.Black;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            string gridHTML = sw.ToString().Replace("\"", "'")
                .Replace(System.Environment.NewLine, "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload = new function(){");
            sb.Append("var printWin = window.open('', '', 'left=0");
            sb.Append(",top=0,width=1000,height=600,status=0');");
            sb.Append("printWin.document.write(\"");
            sb.Append(gridHTML);
            sb.Append("\");");
            sb.Append("printWin.document.close();");
            sb.Append("printWin.focus();");
            sb.Append("printWin.print();");
            sb.Append("printWin.close();};");
            sb.Append("</script>");

            ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "GridPrint", sb.ToString(), false);
            GridView1.AllowPaging = true;



        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string onay = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "onayDurumu"));
                LinkButton link = e.Row.Cells[0].Controls[3] as LinkButton;
                LinkButton link2 = e.Row.Cells[0].Controls[5] as LinkButton;
                //LinkButton link3 = e.Row.Cells[0].Controls[7] as LinkButton;
                if (onay == "EVET")
                {
                    link.Visible = false;
                    link2.Visible = false;
                    //link3.Visible = false;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }

        }

        protected void btnServisDetaylari_Click(object sender, EventArgs e)
        {

            //string servis_id = Request.QueryString["servisid"];
            //string cust_id = Request.QueryString["custid"];
            //string kimlik = Request.QueryString["kimlik"];
            //if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id) && !String.IsNullOrEmpty(kimlik))
            //{
            //    var url = "/TeknikTeknik/ServisDetayList.aspx?servisid=" + servis_id + "&kimlik=" + kimlik;
            //    Response.Redirect(url);
            //}
        }

        protected void btnOnay_ClickH(object sender, EventArgs e)
        {
            string hesapS = hdnHesapIDH.Value;

            string musteriID = hdnMusteriIDH.Value.Trim();
            int custid = Int32.Parse(musteriID);
            int hesapID = Int32.Parse(hesapS);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                ServisIslemleri ser = new ServisIslemleri(dc);

                if (musteriID != "-99")
                {
                    //burada cariyi çekecez
                    musteri_bilgileri musteri_bilgileri = ser.servisKararOnayR(hesapID, User.Identity.Name);
                    //karar onayında stok kontrolü yapılıyor
                    //eğer stok yoksa müşteri bilgileri boş döndürülüyor
                    if (!string.IsNullOrEmpty(musteri_bilgileri.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, musteri_bilgileri.caribakiye, User.Identity.Name);

                        if (chcMailH.Checked == true || chcSmsH.Checked == true)
                        {
                            int servisid = Int32.Parse(hdnServisIDDH.Value);
                            string islem = hdnIslemmH.Value;
                            string yekun = hdnYekunnH.Value;
                            Radius.service serr = ser.servisTekR(servisid);
                            if (chcMailH.Checked == true)
                            {
                                string ekMesaj = "Yapılacak işlem: <b>" + islem + "</b><br/>" + "Tutar :<b>" + yekun + "TL";
                                ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                                mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.ad, serr.Servis_Kimlik_No, "karar_onaylandi", ekMesaj);

                            }
                            if (chcSmsH.Checked == true)
                            {
                                string ekMesajSms = "ServisNo: " + serr.Servis_Kimlik_No + "İşlem: " + islem + "Tutar: " + yekun + " TL";
                                ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                                AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                                sms.SmsGonder("durum", (int)serr.durum_id, ayarimiz, musteri_bilgileri.tel, ekMesajSms);

                            }
                        }
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModalH').modal('hide');");

                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModalH').modal('hide');");

                        sb.Append("alertify.error('Cihaz stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }
                else
                {
                    musteri_bilgileri bil = ser.servisKararOnayNoMusteri(hesapID, User.Identity.Name);
                    if (!string.IsNullOrEmpty(bil.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, bil.caribakiye, User.Identity.Name);
                        //Response.Redirect("/Deneme.aspx?felan=" + musteriID);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#onayModalH').modal('hide');");
                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);

                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModalH').modal('hide');");

                        sb.Append("alertify.error('Cihaz stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }

                goster(dc);
            }

        }

        protected void btnPaket_Click(object sender, EventArgs e)
        {
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];
            int paket_id = Int32.Parse(drdPaketler.SelectedValue);
            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id) && paket_id > -1)
            {
                int servisid = Int32.Parse(servis_id);
                int custid = Int32.Parse(cust_id);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    if (custid != -99)
                    {
                        s.servisKararPaketli(servisid, paket_id, custid, User.Identity.Name);
                    }
                    else
                    {
                        s.servisKararPaketli(servisid, paket_id, null, User.Identity.Name);
                    }
                    goster(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#paketModal').modal('hide');");
                sb.Append("alertify.success('Hesaplar kaydedildi!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PaketModalScript", sb.ToString(), false);

            }

        }

        protected void btnPaketEkle_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("$('#paketModal').modal('show');");

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PaketModalScript", sb.ToString(), false);
        }

        protected void btnTopluOnay_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("$('#topluModal').modal('show');");

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "TopluModalScript", sb.ToString(), false);
        }

        protected void btnTopluOnayKaydet_Click(object sender, EventArgs e)
        {
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];

            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id))
            {
                int servisid = Int32.Parse(servis_id);
                int custid = Int32.Parse(cust_id);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {

                    ServisIslemleri s = new ServisIslemleri(dc);
                    if (custid != -99)
                    {
                        Musteri_Mesaj musteri_bilgileri = s.kararOnaytoplu(servisid, custid, User.Identity.Name);
                        if (!string.IsNullOrEmpty(musteri_bilgileri.musteri_adi))
                        {
                            //FaturaIslemleri fat = new FaturaIslemleri(dc);
                            //fat.FaturaOdeCariEntegre(custid, DateTime.Now, musteri_bilgileri.caribakiye, User.Identity.Name);

                            if (chcMailToplu.Checked == true || chcSmsToplu.Checked == true)
                            {

                                string islem = hdnIslemmH.Value;
                                string yekun = hdnYekunnH.Value;
                                Radius.service serr = s.servisTekR(servisid);
                                if (chcMailToplu.Checked == true)
                                {
                                    string ekMesaj = "Yapılacak işlem: <b>" + musteri_bilgileri.islemler + "</b><br/>" + "Tutar :<b>" + musteri_bilgileri.tutar + "TL";
                                    ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                                    mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.musteri_adi, serr.Servis_Kimlik_No, "karar_onaylandi", ekMesaj);

                                }
                                if (chcSmsToplu.Checked == true)
                                {
                                    string ekMesajSms = "ServisNo: " + serr.Servis_Kimlik_No + "İşlem: " + musteri_bilgileri.islemler + "Tutar: " + musteri_bilgileri.tutar + " TL";
                                    ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                                    AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                                    string mesele = sms.SmsGonder("durum", (int)serr.durum_id, ayarimiz, musteri_bilgileri.telefon, ekMesajSms);


                                }
                            }
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.success('onaylandı!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            //stok kontrolüünde bazı hesaplar çakılmış
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.error('Bazı ürün stokları sıfır görünüyor!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }


                    }
                    else
                    {
                        musteri_bilgileri mes = s.kararOnaytoplu(servisid, User.Identity.Name);
                        if (!string.IsNullOrEmpty(mes.ad))
                        {
                            //FaturaIslemleri fat = new FaturaIslemleri(dc);
                            //fat.FaturaOdeCariEntegre(custid, DateTime.Now, mes.caribakiye, User.Identity.Name);

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.success('Hesaplar onaylandı!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }
                        else
                        {
                            //stok kontrolüünde bazı hesaplar çakılmış
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#topluModal').modal('hide');");
                            sb.Append("alertify.error('Bazı ürün stokları sıfır görünüyor!');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PModalScript", sb.ToString(), false);
                        }

                    }

                    goster(dc);


                }
            }
        }

        protected void grdMusteri_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
                hdnTamirciID.Value = grdMusteri.SelectedValue.ToString();
            }

        }

        protected void grdMusteri_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnKaydetTamirci_Click(object sender, EventArgs e)
        {
            string servisidd = Request.QueryString["servisid"];
            string custidd = Request.QueryString["custid"];
            string hesapidd = hdnHesapIDDuzenTamirci.Value;
            string kimlik = Request.QueryString["kimlik"];
            int? custid = null;
            if (!String.IsNullOrEmpty(custidd))
            {
                custid = Int32.Parse(custidd);

            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);

                int servisid = Int32.Parse(servisidd);

                string islem = txtIslemParcaTamirci.Value;

                decimal kdv = Decimal.Parse(txtKDVOraniDuzenleTamirci.Text);
                decimal yekun = Decimal.Parse(txtYekunTamirci.Text);
                string aciklama = txtAciklamaTamirci.Text;
                decimal maliyet = Decimal.Parse(txtTamirciMaliyet.Text);
                int tamirci_id = Convert.ToInt32(hdnTamirciID.Value);
                DateTime tarihi = DateTime.Now;
                if (!String.IsNullOrEmpty(tarihtamirci.Value))
                {
                    tarihi = DateTime.Parse(tarihtamirci.Value);
                }

                if (!String.IsNullOrEmpty(servisidd) && String.IsNullOrEmpty(hesapidd))
                {
                    //string firma = KullaniciIslem.firma();

                    //yeni ekleme

                    s.serviceKararEkleTamirci(servisid, custid, tamirci_id, islem, kdv, yekun, maliyet, aciklama, tarihi, User.Identity.Name);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Hesap!');");
                    sb.Append("$('#tamirciModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);
                }
                else if (!String.IsNullOrEmpty(servisidd) && !String.IsNullOrEmpty(hesapidd))
                {
                    int hesapid = Int32.Parse(hesapidd);
                    s.serviceKararGuncelleTamirci(hesapid, tamirci_id, islem, kdv, yekun, maliyet, aciklama, tarihi, User.Identity.Name);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append(@"<script type='text/javascript'>");

                    sb.Append(" alertify.success('Güncelledi!');");
                    sb.Append("$('#tamirciModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);
                }

                goster(dc);
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
        protected void btnTamirciye_Click(object sender, EventArgs e)
        {
            string servisidd = Request.QueryString["servisid"];
            string custidd = Request.QueryString["custid"];
            hdnHesapIDDuzenTamirci.Value = "";
            hdnTamirciID.Value = "";
            //hdnHesapIDDuzen.Value = "";

            if (!String.IsNullOrEmpty(custidd) && !String.IsNullOrEmpty(servisidd))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#tamirciModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tamirciModalScript", sb.ToString(), false);
            }
            else
            {
                Response.Redirect("/TeknikTeknik/ServislerCanli.aspx?=" + custidd);
            }

        }
        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
            }

        }
        private void musteriGoster(radiusEntities dc)
        {
            string s = txtMusteriSorgu.Value;
            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                grdMusteri.DataSource = m.musteriAraR2(s, "tamirci");
                grdMusteri.DataBind();

            }
        }

        protected void UstaAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ustaGoster(dc);
            }

        }
        private void ustaGoster(radiusEntities dc)
        {

            string s = txtUsta.Value;

            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                grdUsta.DataSource = m.musteriAraR2(s, "usta");
                grdUsta.DataBind();

            }
        }
        protected void grdUsta_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ustaGoster(dc);
            }

        }

        protected void btnUsta_Click(object sender, EventArgs e)
        {
            string s = grdUsta.SelectedValue.ToString();
            string servis = Request.QueryString["servisid"];
            if (!string.IsNullOrEmpty(s) && !string.IsNullOrEmpty(servis))
            {
                int servis_id = Int32.Parse(servis);
                int tamirci_id = Int32.Parse(s);
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri ser = new ServisIslemleri(dc);
                    ser.usta_ata(tamirci_id, servis_id, User.Identity.Name);
                    goster(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Usta ataması kaydedildi!');");
                sb.Append("$('#ustaModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ustaModalScript", sb.ToString(), false);

            }

        }

        protected void btnUstaAta_Click(object sender, EventArgs e)
        {
            string servisidd = Request.QueryString["servisid"];
            string custidd = Request.QueryString["custid"];


            if (!String.IsNullOrEmpty(custidd) && !String.IsNullOrEmpty(servisidd))
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#ustaModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ustaModalScript", sb.ToString(), false);

            }
            else
            {
                Response.Redirect("/TeknikTeknik/ServislerCanli.aspx?=" + custidd);
            }
        }

        protected void btnFire_Click(object sender, EventArgs e)
        {

            string servis = Request.QueryString["servisid"];
            if (!string.IsNullOrEmpty(servis))
            {
                int servis_id = Int32.Parse(servis);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri ser = new ServisIslemleri(dc);
                    ser.usta_fire(servis_id, User.Identity.Name);
                    goster(dc);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Usta görevden alındı!');");
                sb.Append("$('#ustaModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ustaModalScript", sb.ToString(), false);

            }
        }
        protected void btnHesaplar_Click(object sender, EventArgs e)
        {
            //string id = txtServisID.Value;
            //string custID = hdnCustID.Value;

            //string kimlik = Request.QueryString["kimlik"];

            //if (!String.IsNullOrEmpty(custID) && !String.IsNullOrEmpty(id))
            //{
            //    string url = "/TeknikTeknik/ServisHesaplar.aspx?servisid=" + id + "&custid=" + custID;
            //    if (!String.IsNullOrEmpty(kimlik))
            //    {
            //        url = "/TeknikTeknik/ServisHesaplar.aspx?servisid=" + id + "&custid=" + custID + "&kimlik=" + kimlik;
            //    }
            //    Response.Redirect(url);
            //}


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
            DataPager2.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                BindList(dc);
            }

        }
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            pager.Visible = DataPager1.TotalRowCount > DataPager1.MaximumRows;
            pager.Visible = DataPager2.TotalRowCount > DataPager2.MaximumRows;
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

            //string id = txtServisID.Value;
            //int servisid = Int32.Parse(id);

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

        protected void btnHarita_Click(object sender, EventArgs e)
        {
            string custID = hdnCustID.Value;

            Response.Redirect("/TeknikHarita/HaritaTekMusteri.aspx?id=" + custID);
        }
    }
}