using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using System.IO;
using System.Text;
using System.Web;
using System.Data.Entity.Validation;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class ServisHesaplar : System.Web.UI.Page
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
                    goster(dc);
                }
              
            }

        }

        private void goster(radiusEntities dc)
        {


            ServisIslemleri s = new ServisIslemleri(dc);
            //sadece servise göre bulmak için
            if (!IsPostBack)
            {
                drdPaketler.AppendDataBoundItems = true;

                drdPaketler.DataSource = s.servis_paketleri();
                drdPaketler.DataValueField = "paket_id";
                drdPaketler.DataTextField = "paket_adi";
            }
            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];
            string tamir_id = Request.QueryString["tamirid"];
            string onayy = Request.QueryString["onay"];
            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id))
            {
                btnTopluOnay.Visible = true;
                btnTopluOnayK.Visible = true;
                btnMusteriDetayim.Visible = true;
                btnMusteriDetayimK.Visible = true;
                btnServisDetaylari.Visible = true;
                btnServis.Visible = true;
                btnServisK.Visible = true;
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
                GridView1.DataSource = liste;
                //burada servisID var buna göre servisin detaylarına gidilebilir
                if (s.servis_kapalimi(servisid))
                {
                    btnTopluOnay.Visible = false;
                    btnTamirciye.Visible = false;
                    btnUstaAta.Visible = false;
                    btnPaketEkle.Visible = false;
                    btnEkle.Visible = false;

                    btnTopluOnayK.Visible = false;
                    btnTamirciyeK.Visible = false;
                    btnUstaAtaK.Visible = false;
                    btnPaketEkleK.Visible = false;
                    btnEkleK.Visible = false;

                }

            }
            //sadece müşteriye göre bulmak için 
            else if (!String.IsNullOrEmpty(cust_id) && String.IsNullOrEmpty(servis_id))
            {
                btnMusteriDetayim.Visible = true;
                btnMusteriDetayimK.Visible = true;
                int custid = Int32.Parse(cust_id);
                var liste = s.servisKararListesiDetayMusteriyeGoreHepsiR(custid);
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
                GridView1.DataSource = liste;

            }
            //sadece müşteriye göre bulmak için 
            else if (!String.IsNullOrEmpty(tamir_id))
            {
                btnMusteriDetayim.Visible = true;
                btnMusteriDetayimK.Visible = true;
                int tamirid = Int32.Parse(tamir_id);
                var liste = s.servisKararListesiTamirci(tamirid);
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
                GridView1.DataSource = liste;

            }
            //firmanın query stringle gelen onaya göre
            if (!String.IsNullOrEmpty(onayy))
            {
                bool onay = Boolean.Parse(onayy);
                var liste = s.servisKararListesiDetayHepsiR(onay);
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
                GridView1.DataSource = liste;

            }

            cihazGoster(dc);
            ustaGoster(dc);
            GridView1.DataBind();
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
                        s.serviceKararEkleRYeniCihazli(servisid, custid, islem, kdv, yekun, aciklama, (int)cihaz_id, adet, cihaz, sure, karar_tarihi,User.Identity.Name);

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
                            s.serviceKararEkleRYeni(servisid, custid, islem, kdv, yekun, aciklama, adet, karar_tarihi,User.Identity.Name);
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
                    ServisGuncelle(servisidd, custidd, hesapidd, kimlik, dc,User.Identity.Name);
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

        private void ServisGuncelle(string servisidd, string custidd, string hesapidd, string kimlik, radiusEntities dc,string kullanici)
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
                    s.serviceKararGuncelleCihazli(hesapid, islem, aciklama, kdvOran, yekun, cihaz, cihaz_id, adet,kullanici);
                }
                else
                {
                    s.serviceKararGuncelleR(hesapid, islem, aciklama, kdvOran, yekun,kullanici);
                }

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
                    mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, serr.Servis_Kimlik_No,  "karar_bekleniyor", ekMesaj);

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
            string tamirid = Request.QueryString["tamirid"];
            if (!String.IsNullOrEmpty(custidd))
            {
                Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + custidd);
            }
            if (!String.IsNullOrEmpty(tamirid))
            {
                Response.Redirect("/MusteriDetayBilgileri.aspx?custid=" + tamirid);
            }


        }
        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            int hesapID = Convert.ToInt32(hdnID.Value);
            decimal tutar = Decimal.Parse(txtTutar.Text);
            int taksitSayisi = Int32.Parse(txtTaksitSayisi.Text);
            DateTime baslamaTarihi = DateTime.Parse(txtBaslamaTarihi.Text);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                List<taksitimiz> taksitler = ser.TaksitKaydet(hesapID, baslamaTarihi, 30, taksitSayisi, tutar);
                goster(dc);
            }

            //taksit ekleme metodu
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");

            sb.Append("$('#addModal').modal('hide');");
            sb.Append("$('#detailModal').modal('show');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);

            //if (taksitler != null)
            //{
            //    GridView3.DataSource = taksitler;
            //    GridView3.DataBind();
            //    //mail iznialınacak
            //    if (chcSmsTaksit.Checked == true || chcMailTaksit.Checked == true)
            //    {
            //        MusteriIslemleri musteri = new MusteriIslemleri(kullanici.Firma);
            //        string yekun = tutar.ToString("C");
            //        string islem = hdnIslem.Value;
            //        int servisid = Int32.Parse(hdnServisID.Value);

            //        Radius.service serr = ser.servisTekR(servisid);
            //        Radius.customer musteri_bilgileri = musteri.musteriTekR(serr.CustID);
            //        if (chcMailTaksit.Checked == true)
            //        {
            //            string ekMesaj = "Yapılacak işlem: <b>" + islem + "</b><br/>" + "Tutar :<b>" + yekun + " TL";

            //            ServisDAL.MailIslemleri mi = new MailIslemleri(KullaniciIslem.kullanici_convert(kullanici));
            //            mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.Ad, serr.Servis_Kimlik_No, (int)serr.durum_id, "karar_onaylandi", ekMesaj);

            //        }
            //        if (chcSmsTaksit.Checked == true)
            //        {
            //            string ekMesajSms = "ServisNo: " + serr.Servis_Kimlik_No + "İşlem: " + islem + "Tutar: " + yekun + " TL";

            //            ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(kullanici.Firma);
            //            AyarIslemleri ayarimiz = new AyarIslemleri(kullanici.Firma);
            //            sms.SmsGonder("durum", (int)serr.durum_id, KullaniciIslem.kullanici_convert(kullanici), kullanici.Firma, ayarimiz, musteri_bilgileri.telefon, ekMesajSms);

            //        }
            //    }

            //}
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
                        s.servisKararIptalR(hesapID,User.Identity.Name);
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
            else if (e.CommandName.Equals("taksit"))
            {
                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');

                int hesapID = Convert.ToInt32(arg[0]);

                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = GridView1.Rows[index];
                string islem = row.Cells[2].Text;
                string yekun = row.Cells[8].Text;
                string servisid = row.Cells[12].Text;

                txtTutar.Text = yekun;
                hdnIslem.Value = islem;
                hdnServisID.Value = servisid;

                hdnID.Value = hesapID.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#addModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);
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
                string yekun = row.Cells[8].Text;
                string servisid = row.Cells[12].Text;


                hdnHesapID.Value = hesapID.ToString();
                hdnMusteriID.Value = musteriID.ToString();

                hdnServisIDD.Value = servisid;
                hdnYekunn.Value = yekun;
                hdnIslemm.Value = islem;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('show');");
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

                string m = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "musteriID"));
                string s = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "servisID"));
                string k = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "kimlik"));

                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;
                (e.Row.FindControl("btnServisK") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;

                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("delLink") as LinkButton).Visible = false;
                    (e.Row.FindControl("delLinkk") as LinkButton).Visible = false;
                }


            }
        }

        protected void btnEkle_Click(object sender, EventArgs e)
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
                LinkButton link3 = e.Row.Cells[1].Controls[3] as LinkButton;
                LinkButton link4 = e.Row.Cells[1].Controls[5] as LinkButton;
                //LinkButton link3 = e.Row.Cells[0].Controls[7] as LinkButton;
                if (onay == "EVET")
                {
                    link.Visible = false;
                    link2.Visible = false;
                    link3.Visible = false;
                    link4.Visible = false;
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

            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];
            string kimlik = Request.QueryString["kimlik"];
            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id) && !String.IsNullOrEmpty(kimlik))
            {
                var url = "/TeknikTeknik/ServisDetayList.aspx?servisid=" + servis_id + "&kimlik=" + kimlik + "&custid=" + cust_id;
                Response.Redirect(url);
            }
        }

        protected void btnServis_Click(object sender, EventArgs e)
        {

            string servis_id = Request.QueryString["servisid"];
            string cust_id = Request.QueryString["custid"];
            string kimlik = Request.QueryString["kimlik"];
            if (!String.IsNullOrEmpty(servis_id) && !String.IsNullOrEmpty(cust_id) && !String.IsNullOrEmpty(kimlik))
            {
                var url = "/TeknikTeknik/Servis.aspx?servisid=" + servis_id + "&kimlik=" + kimlik + "&custid=" + cust_id;
                Response.Redirect(url);
            }
        }
        protected void btnOnay_Click(object sender, EventArgs e)
        {
            string hesapS = hdnHesapID.Value;

            string musteriID = hdnMusteriID.Value.Trim();
            int custid = Int32.Parse(musteriID);
            int hesapID = Int32.Parse(hesapS);

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {

                ServisIslemleri ser = new ServisIslemleri(dc);

                if (musteriID != "-99")
                {
                    //burada cariyi çekecez
                    musteri_bilgileri musteri_bilgileri = ser.servisKararOnayR(hesapID,User.Identity.Name);
                    //karar onayında stok kontrolü yapılıyor
                    //eğer stok yoksa müşteri bilgileri boş döndürülüyor
                    if (!string.IsNullOrEmpty(musteri_bilgileri.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, musteri_bilgileri.caribakiye,User.Identity.Name);

                        if (chcMail.Checked == true || chcSms.Checked == true)
                        {
                            int servisid = Int32.Parse(hdnServisIDD.Value);
                            string islem = hdnIslemm.Value;
                            string yekun = hdnYekunn.Value;
                            Radius.service serr = ser.servisTekR(servisid);
                            if (chcMail.Checked == true)
                            {
                                string ekMesaj = "Yapılacak işlem: <b>" + islem + "</b><br/>" + "Tutar :<b>" + yekun + "TL";
                                ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                                mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.ad, serr.Servis_Kimlik_No,  "karar_onaylandi", ekMesaj);

                            }
                            if (chcSms.Checked == true)
                            {
                                string ekMesajSms = "ServisNo: " + serr.Servis_Kimlik_No + "İşlem: " + islem + "Tutar: " + yekun + " TL";
                                ServisDAL.SmsIslemleri sms = new ServisDAL.SmsIslemleri(dc);
                                AyarIslemleri ayarimiz = new AyarIslemleri(dc);
                                sms.SmsGonder("durum", (int)serr.durum_id, ayarimiz, musteri_bilgileri.tel, ekMesajSms);

                            }
                        }
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

                        sb.Append("alertify.error('Cihaz stoğu sıfır görünüyor!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);
                    }

                }
                else
                {
                    musteri_bilgileri bil = ser.servisKararOnayNoMusteri(hesapID,User.Identity.Name);
                    if (!string.IsNullOrEmpty(bil.ad))
                    {
                        //FaturaIslemleri fat = new FaturaIslemleri(dc);
                        //fat.FaturaOdeCariEntegre(custid, DateTime.Now, bil.caribakiye,User.Identity.Name);
                        //Response.Redirect("/Deneme.aspx?felan=" + musteriID);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("$('#onayModal').modal('hide');");
                        sb.Append("alertify.success('Hesap onaylandı!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayHideModalScript", sb.ToString(), false);

                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");

                        sb.Append("$('#onayModal').modal('hide');");

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
                        s.servisKararPaketli(servisid, paket_id, custid,User.Identity.Name);
                    }
                    else
                    {
                        s.servisKararPaketli(servisid, paket_id, null,User.Identity.Name);
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
                        Musteri_Mesaj musteri_bilgileri = s.kararOnaytoplu(servisid, custid,User.Identity.Name);
                        if (!string.IsNullOrEmpty(musteri_bilgileri.musteri_adi))
                        {
                            //FaturaIslemleri fat = new FaturaIslemleri(dc);
                            //fat.FaturaOdeCariEntegre(custid, DateTime.Now, musteri_bilgileri.caribakiye,User.Identity.Name);

                            if (chcMailToplu.Checked == true || chcSmsToplu.Checked == true)
                            {

                                string islem = hdnIslemm.Value;
                                string yekun = hdnYekunn.Value;
                                Radius.service serr = s.servisTekR(servisid);
                                if (chcMailToplu.Checked == true)
                                {
                                    string ekMesaj = "Yapılacak işlem: <b>" + musteri_bilgileri.islemler + "</b><br/>" + "Tutar :<b>" + musteri_bilgileri.tutar + "TL";
                                    ServisDAL.MailIslemleri mi = new MailIslemleri(dc);
                                    mi.SendingMail(musteri_bilgileri.email, musteri_bilgileri.musteri_adi, serr.Servis_Kimlik_No,  "karar_onaylandi", ekMesaj);

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
                        musteri_bilgileri mes = s.kararOnaytoplu(servisid,User.Identity.Name);
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
                    s.serviceKararGuncelleTamirci(hesapid, tamirci_id, islem, kdv, yekun, maliyet, aciklama, tarihi,User.Identity.Name);
                
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
                grdMusteri.DataSource = m.musteriAraR2(s,"tamirci");
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
                grdUsta.DataSource = m.musteriAraR2(s,"usta");
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
    }
}