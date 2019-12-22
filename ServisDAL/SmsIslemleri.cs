using ServisDAL.NetGsm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServisDAL
{
    public class SmsNetGsm
    {

        private string kullanici_adi;
        private string sifre;
        public string mesaj { get; set; }
        public string gonderen { get; set; }
        public string[] tel_nolar { get; set; }

        public SmsNetGsm(string kullaniciParam, string sifreParam)
        {
            kullanici_adi = kullaniciParam;
            sifre = sifreParam;


        }

        public string TekMesajGonder()
        {

            smsnnClient sms_gonder = new smsnnClient();

            return sms_gonder.sms_gonder_1n(kullanici_adi, sifre, "", gonderen, mesaj, tel_nolar, "TR", "", "");

        }



    }
    public class SmsIslemleri
    {

        TeknikServis.Radius.radiusEntities dc;
        public SmsIslemleri(TeknikServis.Radius.radiusEntities dc)
        {

            this.dc = dc;
        }

        public void CariMesajKaydet(string[] teller)
        {
            //tel listesine göre mesaj atılma tarihini kaydediyoruz
            if (teller.Length > 0)
            {
                foreach (string tel in teller)
                {
                    TeknikServis.Radius.carihesap hesap = dc.carihesaps.FirstOrDefault(x => x.telefon.Equals(tel));
                    hesap.son_mesaj = DateTime.Now;
                }
                KaydetmeIslemleri.kaydetR(dc);
            }
        }


        public string SmsGonder(string tur, int durum_id, AyarIslemleri ayarimiz, string tel, string ekMesaj)
        {
            string sonuc = "";


            TeknikServis.Radius.sms_ayars smsAyari = (from f in dc.sms_ayars
                                                      where f.iliski_id == durum_id && f.tur == tur && f.aktif == true
                                                      select f).FirstOrDefault();


            if (smsAyari != null)
            {

                string mesaj = "";
                string gonderen = "";


                mesaj = smsAyari.mesaj + "-" + ekMesaj;
                gonderen = smsAyari.gonderen;


                TeknikServis.Radius.ayar smsApi = ayarimiz.SmsAyarR();

                //herhangi bir smsApi tanımlanmışsa buradaki ayarı kullanarak gönderme yapacaz

                if (smsApi != null)
                {
                    if (smsApi.Mail_Server == "NETGSM")
                    {
                        SmsNetGsm gsm = new SmsNetGsm(smsApi.Mail_UserName, smsApi.Mail_PW);
                        gsm.gonderen = gonderen;
                        gsm.mesaj = mesaj;
                        gsm.tel_nolar = new string[] { tel };
                        sonuc += gsm.TekMesajGonder();
                    }
                }
                //else
                //{
                //    sonuc += "api bulunamadı";
                //}


            }
            //else
            //{
            //    sonuc += "ayar bulunamadi";
            //}
            return sonuc;
        }

        public string SmsGunuGelen(AyarIslemleri ayarimiz, string gonderen, string mesaj, string kritikGun)
        {


            //SMS gönderilecek
            //gönderilecek smsin içeriği için ayarları çekelim
            string sonuc = "";

            string kulBayisi = "";


            TeknikServis.Radius.sms_ayars smsAyari = (from f in dc.sms_ayars
                                                      where f.iliski_id == -1 && f.tur == "yaklasan_taksit" && f.aktif == true
                                                      select f).FirstOrDefault();


            if (smsAyari != null)
            {
                sonuc += "ayar bulundu-";
                FaturaIslemleri fat = new FaturaIslemleri(dc);
                DateTime sinirTarih = DateTime.Now.AddDays(1);
                if (!String.IsNullOrEmpty(kritikGun))
                {
                    int kritik = Int32.Parse(kritikGun);
                    sinirTarih = DateTime.Now.AddDays(kritik);
                }

                List<TeknikServis.Radius.customer> gunuGelenMusteriler = fat.YaklasanGunuGecenler(sinirTarih);

                if (gunuGelenMusteriler != null)
                {
                    sonuc += "müşteri sayısı-" + gunuGelenMusteriler.Count.ToString() + "-";
                    foreach (var item in gunuGelenMusteriler)
                    {
                        sonuc += item.telefon + "-";
                    }
                    //SMS api ayarlarını çekelim

                    TeknikServis.Radius.ayar smsApi = ayarimiz.SmsAyarR();

                    //herhangi bir smsApi tanımlanmışsa buradaki ayarı kullanarak gönderme yapacaz

                    if (smsApi != null)
                    {
                        sonuc += "api bulundu-";
                        if (smsApi.Mail_Server == "NETGSM")
                        {
                            sonuc += "NET GSMmiş-";
                            string[] teller = new string[gunuGelenMusteriler.Count];
                            for (int i = 0; i < gunuGelenMusteriler.Count; i++)
                            {
                                if (!String.IsNullOrEmpty(gunuGelenMusteriler[i].telefon))
                                {
                                    teller[i] = gunuGelenMusteriler[i].telefon;
                                    sonuc += teller[i] + "-";
                                }

                            }
                            SmsNetGsm gsm = new SmsNetGsm(smsApi.Mail_UserName, smsApi.Mail_PW);
                            gsm.gonderen = gonderen;
                            gsm.mesaj = mesaj;
                            gsm.tel_nolar = teller;
                            sonuc += gsm.TekMesajGonder();
                        }
                        else
                        {
                            sonuc += "NET GSM Değil-";
                        }
                    }
                    else
                    {
                        sonuc += "api bulunamadı";
                    }

                }
                else
                {
                    sonuc += "hiçbir müşteri yok-";
                }


            }
            else
            {
                sonuc += "sms ayari bulunamadi";
            }
            return sonuc;
        }

        public TeknikServis.Radius.sms_ayars SmsAyarGoster(string tur)
        {
            return (from f in dc.sms_ayars
                    where f.iliski_id == -1 && f.tur == tur
                    select f).FirstOrDefault();
        }
        public string SmsGenel(AyarIslemleri ayarimiz, string gonderen, string mesaj, string[] teller)
        {
            string mesajim = "";



            //SMS api ayarlarını çekelim

            TeknikServis.Radius.ayar smsApi = ayarimiz.SmsAyarR();



            //herhangi bir smsApi tanımlanmışsa buradaki ayarı kullanarak gönderme yapacaz

            if (smsApi != null)
            {

                if (smsApi.Mail_Server == "NETGSM")
                {
                    SmsNetGsm gsm = new SmsNetGsm(smsApi.Mail_UserName, smsApi.Mail_PW);
                    //gondericiadlariRequest req = new gondericiadlariRequest(smsApi.Mail_UserName, smsApi.Mail_PW);
                    //gondericiadlariResponse res=new gondericiadlariResponse()
                    gsm.gonderen = gonderen;
                    gsm.mesaj = mesaj;
                    gsm.tel_nolar = teller;
                    mesajim += gsm.TekMesajGonder();

                }
            }
            //else
            //{
            //    mesajim = "api yok";
            //}

            return mesajim;

        }
        public string SmsKredi(AyarIslemleri ayarimiz,  string mesaj, string[] teller)
        {
            string mesajim = "";



            //SMS api ayarlarını çekelim

            TeknikServis.Radius.ayar smsApi = ayarimiz.SmsAyarR();



            //herhangi bir smsApi tanımlanmışsa buradaki ayarı kullanarak gönderme yapacaz

            if (smsApi != null)
            {

                if (smsApi.Mail_Server == "NETGSM")
                {
                    SmsNetGsm gsm = new SmsNetGsm(smsApi.Mail_UserName, smsApi.Mail_PW);
                    //gondericiadlariRequest req = new gondericiadlariRequest(smsApi.Mail_UserName, smsApi.Mail_PW);
                    //gondericiadlariResponse res=new gondericiadlariResponse()
                    gsm.gonderen = smsApi.gonderen.ToUpper();
                    gsm.mesaj = mesaj;
                    gsm.tel_nolar = teller;
                    mesajim += gsm.TekMesajGonder();

                }
            }
            //else
            //{
            //    mesajim = "api yok";
            //}

            return mesajim;

        }
        public string MesajSonucu(string donen_mesaj)
        {
            donen_mesaj = donen_mesaj.Trim();
            string sonuc = "";

            if (donen_mesaj.Equals("20"))
            {
                sonuc = "Mesaj Gönderilemedi! Mesaj metninizdeki problemden dolayı mesajınız gönderilemedi. Bir mesaj metninde  en fazla 917 karakter bulunabilir.";
            }
            else if (donen_mesaj.Equals("30"))
            {
                sonuc = "Mesaj gönderilemedi! Api kullanıcı adınız veya şifreniz hatalı!";
            }
            else if (donen_mesaj.Equals("40"))
            {
                sonuc = "Mesaj gönderilemeledi! Mesaj başlığınızın (gönderici adınızın) sistemde tanımlı değil. Gönderici adlarınızı API ile sorgulayarak kontrol edebilirsiniz.";
            }

            else if (donen_mesaj.Equals("70"))
            {
                sonuc = "Mesaj gönderilemeledi! Hatalı sorgulama. Gönderdiğiniz parametrelerden birisi hatalı veya zorunlu alanlardan biri eksik.";
            }
            else if (donen_mesaj.Equals("100") || donen_mesaj.Equals("101"))
            {
                sonuc = "Mesaj gönderilemedi! Servis sağlayıcınızda bir sistem hatası var";
            }
            else
            {
                sonuc = "Mesajınız Gönderildi";
            }
            return sonuc;
        }

    }


}
