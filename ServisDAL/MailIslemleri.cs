using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TeknikServis.Radius;

namespace ServisDAL
{
    public class MailIslemleri
    {
        //kullanıcıya göre
        //tema seçilecek
        //api seçilecek
        //ture göre gönderim yapılacak


        TeknikServis.Radius.radiusEntities dc;
        public MailIslemleri(radiusEntities dc)
        {

            this.dc = dc;
        }

        private TeknikServis.Radius.ayar MailApi()
        {
            AyarIslemleri ayarimiz = new AyarIslemleri(dc);
            System.Security.Principal.IPrincipal use = HttpContext.Current.User;
            TeknikServis.Radius.ayar mailApi = ayarimiz.MailAyarR();

            return mailApi;
        }
        private string TemaYol(string tur)
        {
            string tema = "";


            if (tur == "yaklasan_taksit")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/GunuGelenTema.html");
            }

            else if (tur == "genel")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/GenelTema.html");
            }
            else if (tur == "baslangic")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/ServisBaslamaTema.html");
            }
            else if (tur == "sonlanma")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/ServisSonlanmaTema.html");
            }
            else if (tur == "karar_bekleniyor")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/ServisKararTema.html");
            }
            else if (tur == "karar_onaylandi")
            {
                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/ServisKararOnayTema.html");
            }
            else if (tur == "durum")
            {
                //durum manuel olarak yazılacak
                //dolayısızyla durum_id vb ilişkisine gerek kalmıyor

                tema = HttpContext.Current.Server.MapPath("~/Temalar/Ortak/MailTema.html");
            }

            return tema;

        }
        private string TemaYolDurum(int durum_id)
        {

            //durum manuel olarak yazılacak
            //dolayısızyla durum_id vb ilişkisine gerek kalmıyor

            return HttpContext.Current.Server.MapPath("~/Temalar/Ortak/MailTema.html");

        }
        private TeknikServis.Radius.mail_ayars TemaAyar(string tur)
        {

            TeknikServis.Radius.mail_ayars tema_ayar = new TeknikServis.Radius.mail_ayars();

            //genel durumlar hariç
            tema_ayar = (from f in dc.mail_ayars
                         where f.tur == tur && f.aktif == true
                         select f).FirstOrDefault();

            return tema_ayar;
            // ondan sonra durum için kullanıcının tema değişkenlerinin tutulduğu ayar çekilip döndürülecek

        }
        private TeknikServis.Radius.mail_ayars TemaAyarDurum(int durum_id)
        {

            TeknikServis.Radius.mail_ayars tema_ayar = new TeknikServis.Radius.mail_ayars();

            //genel durumlar hariç
            tema_ayar = (from f in dc.mail_ayars
                         where f.iliski_id == durum_id && f.aktif == true
                         select f).FirstOrDefault();

            return tema_ayar;
            // ondan sonra durum için kullanıcının tema değişkenlerinin tutulduğu ayar çekilip döndürülecek

        }
        public string SendingMail(string Kime, string custom, string Kimlik, string tur, string ekMesaj)
        {
            string sonuc = "";
            try
            {
                TeknikServis.Radius.ayar mail_api = MailApi();

                if (mail_api != null)
                {
                    sonuc += "api-bulundu-";
                    TeknikServis.Radius.mail_ayars temaAyar = TemaAyar(tur);
                    if (temaAyar != null)
                    {
                        sonuc += "tema bulundu-";
                        string Kimden = mail_api.Mail_Kimden;
                        string server = mail_api.Mail_Server;
                        string kullanici = mail_api.Mail_UserName;

                        string sifre = mail_api.Mail_PW;
                        //try
                        //{
                        MailMessage m = new MailMessage(Kimden, Kime);
                        m.Subject = temaAyar.konu;
                        m.Body = PopulateBody(custom, tur, Kimlik, temaAyar, ekMesaj);
                        m.IsBodyHtml = true;
                        m.From = new MailAddress(Kimden);
                        m.BodyEncoding = System.Text.Encoding.UTF8;
                        m.Priority = MailPriority.High;

                        m.To.Add(new MailAddress(Kime));
                        //m.Bcc.Add(new MailAddress(Kime));
                        //m.CC.Add(new MailAddress(Kime));

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = server;
                        smtp.Port = 587;
                        smtp.EnableSsl = false;

                        NetworkCredential authinfo = new NetworkCredential(kullanici, sifre);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = authinfo;
                        smtp.Send(m);
                        sonuc += "mail gönderildi====>";

                    }
                    else
                    {
                        sonuc += "tema yok-";
                    }
                }
            }
            catch (Exception exx)
            {
                HttpContext.Current.Session["mesele"] = exx.Message;
                HttpContext.Current.Response.Redirect("/Sonuc.aspx");
            }


            return sonuc;



        }
        public string SendingMailDurum(string Kime, string custom, string Kimlik, int durum_id, string ekMesaj)
        {
            string sonuc = "";
            try
            {
                TeknikServis.Radius.ayar mail_api = MailApi();

                if (mail_api != null)
                {
                    sonuc += "api-bulundu-";
                    TeknikServis.Radius.mail_ayars temaAyar = TemaAyarDurum(durum_id);
                    if (temaAyar != null)
                    {
                        sonuc += "tema bulundu-";
                        string Kimden = mail_api.Mail_Kimden;
                        string server = mail_api.Mail_Server;
                        string kullanici = mail_api.Mail_UserName;

                        string sifre = mail_api.Mail_PW;
                        //try
                        //{
                        MailMessage m = new MailMessage(Kimden, Kime);
                        m.Subject = temaAyar.konu;
                        m.Body = PopulateBodyDurum(custom, durum_id, Kimlik, temaAyar, ekMesaj);
                        m.IsBodyHtml = true;
                        m.From = new MailAddress(Kimden);
                        m.BodyEncoding = System.Text.Encoding.UTF8;
                        m.Priority = MailPriority.High;

                        m.To.Add(new MailAddress(Kime));
                        //m.Bcc.Add(new MailAddress(Kime));
                        //m.CC.Add(new MailAddress(Kime));

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = server;
                        smtp.Port = 587;
                        smtp.EnableSsl = false;

                        NetworkCredential authinfo = new NetworkCredential(kullanici, sifre);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = authinfo;
                        smtp.Send(m);
                        sonuc += "mail gönderildi====>";

                    }
                    else
                    {
                        sonuc += "tema yok-";
                    }
                }
            }
            catch (Exception exx)
            {
                HttpContext.Current.Session["mesele"] = exx.Message;
                HttpContext.Current.Response.Redirect("/Sonuc.aspx");
            }


            return sonuc;



        }

        private string PopulateBody(string Customer, string turr, string Kimlik, TeknikServis.Radius.mail_ayars temaAyar, string ekMesaj)
        {
            string body = string.Empty;

            string tema = TemaYol(turr);

            using (StreamReader reader = new StreamReader(tema))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Customer}", Customer);
            body = body.Replace("{ResimYol}", temaAyar.resim_yol);
            body = body.Replace("{Url}", temaAyar.url);
            body = body.Replace("{Durum}", temaAyar.durum);
            body = body.Replace("{FirmaTam}", temaAyar.FirmaTam);
            body = body.Replace("{Adres}", temaAyar.Adres);
            body = body.Replace("{Telefon}", temaAyar.Telefon);
            body = body.Replace("{Kimlik}", Kimlik);
            body = body.Replace("{Mesaj}", temaAyar.body_mesaj);
            body = body.Replace("{Firma}", ekMesaj);

            return body;
        }

        private string PopulateBodyDurum(string Customer, int durum_id, string Kimlik, TeknikServis.Radius.mail_ayars temaAyar, string ekMesaj)
        {
            string body = string.Empty;

            string tema = TemaYolDurum(durum_id);

            using (StreamReader reader = new StreamReader(tema))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Customer}", Customer);
            body = body.Replace("{ResimYol}", temaAyar.resim_yol);
            body = body.Replace("{Url}", temaAyar.url);
            body = body.Replace("{Durum}", temaAyar.durum);
            body = body.Replace("{FirmaTam}", temaAyar.FirmaTam);
            body = body.Replace("{Adres}", temaAyar.Adres);
            body = body.Replace("{Telefon}", temaAyar.Telefon);
            body = body.Replace("{Kimlik}", Kimlik);
            body = body.Replace("{Mesaj}", temaAyar.body_mesaj);
            body = body.Replace("{Firma}", ekMesaj);

            return body;
        }



        public string MailToplu(string tur, string gonderen, string mesaj, string maillistesi, string ekMesaj)
        {
            string sonuc = "";
            TeknikServis.Radius.ayar mail_ayar = MailApi();
            if (mail_ayar != null)
            {
                sonuc += "api bulundu-";
                TeknikServis.Radius.mail_ayars temaAyar = TemaAyar(tur);
                if (temaAyar != null)
                {
                    sonuc += "tema var-";
                    string Kimden = mail_ayar.Mail_Kimden;
                    string server = mail_ayar.Mail_Server;
                    string kullanici = mail_ayar.Mail_UserName;
                    string sifre = mail_ayar.Mail_PW;

                    if (!String.IsNullOrEmpty(maillistesi))
                    {
                        sonuc += "liste var-";
                        try
                        {
                            string[] mailler = maillistesi.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                            MailMessage m = new MailMessage(Kimden, Kimden);
                            m.Subject = temaAyar.konu;
                            m.Body = PopulateBody("Sayın müşterimiz", tur, mesaj, temaAyar, ekMesaj);
                            m.IsBodyHtml = true;
                            m.From = new MailAddress(Kimden);
                            m.BodyEncoding = System.Text.Encoding.UTF8;
                            m.Priority = MailPriority.High;

                            foreach (string mail in mailler)
                            {
                                m.To.Add(new MailAddress(mail));
                            }

                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = server;
                            smtp.Port = 587;
                            smtp.EnableSsl = false;

                            NetworkCredential authinfo = new NetworkCredential(kullanici, sifre);
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = authinfo;
                            smtp.Send(m);

                        }
                        catch (Exception ex)
                        {
                            HttpContext.Current.Session["mesele"] = ex.Message;
                            HttpContext.Current.Response.Redirect("/Sonuc.aspx");

                        }
                    }
                    else
                    {
                        sonuc += "liste yok-";
                    }
                }
                else
                {
                    sonuc += "tema yok-";
                }

            }

            return sonuc;
        }



    }




}
