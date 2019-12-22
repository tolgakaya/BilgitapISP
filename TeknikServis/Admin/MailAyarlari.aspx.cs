using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.Admin
{
    public partial class MailAyarlari : System.Web.UI.Page
    {

        string firma;
        public MailAyarlari()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            else
            {
                firma = KullaniciIslem.firma();

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            if (!IsPostBack)
            {
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    AyarIslemleri islem = new AyarIslemleri(dc);

                    drdDurum.AppendDataBoundItems = true;
                    drdDurum.DataSource = islem.durumListesiR().Where(x => x.baslangicmi == false && x.sonmu == false && x.onaymi == false && x.kararmi == false).ToList();
                    drdDurum.DataTextField = "Durum";
                    drdDurum.DataValueField = "Durum_ID";
                    drdDurum.DataBind();
                }

            }

        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {


            try
            {
                kaydet();
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
                Session["mesaj"] = mesajlar;
                Response.Redirect("/Sonuc.aspx");
            }


        }

        private void kaydet()
        {

            string secilen = drdDurum.SelectedValue;
            int index = drdDurum.SelectedIndex;

            if (index > 0 && index < 6)
            {
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    Radius.mail_ayars ayarimiz = dc.mail_ayars.Where(x => x.tur == secilen).FirstOrDefault();
                    if (ayarimiz == null)
                    {
                        Radius.mail_ayars ayar = new Radius.mail_ayars();
                        ayar.tur = secilen;
                        ayar.Firma = firma;
                        ayar.gonderen = txtGonderen.Text;
                        ayar.durum = secilen;
                        ayar.iliski_id = -1;
                        ayar.body_mesaj = txtMesaj.Text;

                        ayar.resim_yol = "";//kullanicinin resim yolunu atalım
                        ayar.Telefon = txtTelefon.Text;
                        ayar.url = txtUrl.Text;
                        ayar.konu = txtKonu.Text;
                        ayar.FirmaTam = txtFirmaTam.Text;
                        ayar.Adres = txtAdres.Text;
                        if (chcAktif.Checked == true)
                        {
                            ayar.aktif = chcAktif.Checked;
                        }
                        dc.mail_ayars.Add(ayar);
                    }
                    else
                    {


                        ayarimiz.gonderen = txtGonderen.Text;
                        ayarimiz.iliski_id = -1;
                        ayarimiz.body_mesaj = txtMesaj.Text;
                        ayarimiz.resim_yol = "";//kullanicinin resim yolunu atalım
                        ayarimiz.Telefon = txtTelefon.Text;
                        ayarimiz.url = txtUrl.Text;
                        ayarimiz.konu = txtKonu.Text;
                        ayarimiz.FirmaTam = txtFirmaTam.Text;
                        ayarimiz.Adres = txtAdres.Text;
                        if (chcAktif.Checked == true)
                        {
                            ayarimiz.aktif = chcAktif.Checked;
                        }

                    }

                    dc.SaveChanges();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kaydedildi!');");
                    sb.Append(@"</script>");

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);

                }

            }
            else
            {

                int id = Convert.ToInt32(drdDurum.SelectedValue);
                //string mesele = id.ToString();
                //durumun pozisyonuna bakalım
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    AyarIslemleri ay = new AyarIslemleri(dc);
                    TeknikServis.Radius.service_durums durum = ay.tekDurumR(id);

                    Radius.mail_ayars ayarimiz = dc.mail_ayars.Where(x => x.iliski_id == id).FirstOrDefault();


                    if (ayarimiz == null)
                    {
                        //mesele += " Ayarımız nullmuş";
                        Radius.mail_ayars ayar = new Radius.mail_ayars();
                        ayar.tur = durum.Durum;
                        ayar.Firma = firma;
                        ayar.gonderen = txtGonderen.Text;
                        ayar.iliski_id = id;
                        ayar.durum = durum.Durum;
                        ayar.body_mesaj = txtMesaj.Text;


                        ayar.resim_yol = "";//kullanicinin resim yolunu atalım
                        ayar.Telefon = txtTelefon.Text;
                        ayar.url = txtUrl.Text;
                        ayar.konu = txtKonu.Text;
                        ayar.FirmaTam = txtFirmaTam.Text;
                        ayar.Adres = txtAdres.Text;
                        if (chcAktif.Checked == true)
                        {
                            ayar.aktif = chcAktif.Checked;
                        }

                        dc.mail_ayars.Add(ayar);

                        dc.SaveChanges();
                    }
                    else
                    {
                        //mesele += " Ayarımız bulduk";
                        //mesele += " " + ayarimiz.ID.ToString();
                        ayarimiz.gonderen = txtGonderen.Text;
                        ayarimiz.iliski_id = id;
                        ayarimiz.body_mesaj = txtMesaj.Text;
                        ayarimiz.durum = durum.Durum;

                        ayarimiz.resim_yol = "";//kullanicinin resim yolunu atalım
                        ayarimiz.Telefon = txtTelefon.Text;
                        ayarimiz.url = txtUrl.Text;
                        ayarimiz.konu = txtKonu.Text;
                        ayarimiz.FirmaTam = txtFirmaTam.Text;
                        if (chcAktif.Checked == true)
                        {
                            ayarimiz.aktif = chcAktif.Checked;
                        }
                        dc.SaveChanges();

                    }



                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kaydedildi!');");
                    sb.Append(@"</script>");

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);

                }
                //Session["mesele"] = mesele;
                //Response.Redirect("/Sonuc");

            }


        }

        private void goster()
        {
            if (drdDurum.SelectedValue != null)
            {
                string secilen = drdDurum.SelectedValue;

                int secilenIndex = drdDurum.SelectedIndex;

                string gonderen = "";
                string mesaj = "";
                string firmaTam = "";
                string telefon = "";
                string url = "";
                string konu = "";
                string adres = "";
                string kritik = "";

                if (secilenIndex > 0 && secilenIndex < 6)
                {
                    //secilen.Equals("yaklasan_taksit") || secilen.Equals("genel")
                    //ön tanımlı durumların hepsi için türü arayacaz
                    using (radiusEntities dc = MyContext.Context(firma))
                    {

                        Radius.mail_ayars ayar = dc.mail_ayars.Where(x => x.tur == secilen).FirstOrDefault();
                        if (ayar != null)
                        {
                            gonderen = ayar.gonderen;
                            mesaj = ayar.body_mesaj;
                            firmaTam = ayar.FirmaTam;
                            telefon = ayar.Telefon;
                            url = ayar.url;
                            konu = ayar.konu;
                            adres = ayar.Adres;

                            if (ayar.aktif == true)
                            {
                                chcAktif.Checked = true;
                            }
                            else
                            {
                                chcAktif.Checked = false;
                            }
                        }
                        else
                        {
                            AyarCurrent cur = new AyarCurrent(dc);
                            var genel = cur.get();

                            gonderen = genel.adi;

                            firmaTam = genel.adi;
                            telefon = genel.tel;

                            url = genel.web;

                            adres = genel.adres;

                        }

                    }

                }
                else if (secilen.Equals("sec"))
                {
                    chcAktif.Checked = false;
                }
                else
                {

                    int id = Convert.ToInt32(drdDurum.SelectedValue);


                    //durumun pozisyonuna bakalım
                    using (radiusEntities dc = MyContext.Context(firma))
                    {


                        AyarIslemleri ay = new AyarIslemleri(dc);
                        TeknikServis.Radius.service_durums durum = ay.tekDurumR(id);

                        Radius.mail_ayars ayar = dc.mail_ayars.Where(x => x.iliski_id == id).FirstOrDefault();
                        if (ayar != null)
                        {
                            gonderen = ayar.gonderen;
                            mesaj = ayar.body_mesaj;
                            firmaTam = ayar.FirmaTam;
                            telefon = ayar.Telefon;
                            url = ayar.url;
                            konu = ayar.konu;
                            adres = ayar.Adres;
                            kritik = ayar.kritik_gun.ToString();
                            if (ayar.aktif == true)
                            {
                                chcAktif.Checked = true;
                            }
                            else
                            {
                                chcAktif.Checked = false;
                            }
                        }
                        else
                        {
                            AyarCurrent cur = new AyarCurrent(dc);
                            var genel = cur.get();

                            gonderen = genel.adi;

                            firmaTam = genel.adi;
                            telefon = genel.tel;

                            url = genel.web;

                            adres = genel.adres;

                        }
                    }



                }
                txtMesaj.Text = mesaj;
                txtGonderen.Text = gonderen;
                txtAdres.Text = adres;
                txtFirmaTam.Text = firmaTam;
                txtKonu.Text = konu;
                txtTelefon.Text = telefon;
                txtUrl.Text = url;


            }
        }

        protected void drdDurum_SelectedIndexChanged(object sender, EventArgs e)
        {

            goster();
        }
    }
}