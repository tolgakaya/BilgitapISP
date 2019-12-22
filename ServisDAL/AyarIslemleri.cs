using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;

namespace ServisDAL
{

    public class AyarIslemleri
    {

        radiusEntities db;

        public AyarIslemleri(radiusEntities db)
        {

            this.db = db;

        }

        #region mail ayar metotları
        public TeknikServis.Radius.ayar MailAyarR()
        {

            return db.ayars.Where(a => a.tur.Equals("mail")).FirstOrDefault();

        }
        public TeknikServis.Radius.ayar SmsAyarR()
        {
            return db.ayars.Where(a => a.tur.Equals("sms")).FirstOrDefault();

        }


        public List<paket_secim_repo> servis_paketleri()
        {

            return (from s in db.servis_pakets
                    where s.iptal == false
                    select new paket_secim_repo
                    {
                        paket_adi = s.paket_adi,
                        paket_id = s.paket_id

                    }).ToList();


        }

        public void MailAyarKaydetR(string server, string kimden, int port, string username, string pw, string aktif)
        {

            TeknikServis.Radius.ayar ayarimiz = db.ayars.Where(a => a.tur.Equals("mail")).FirstOrDefault();
            if (ayarimiz != null)
            {
                //update
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = port;
                ayarimiz.Mail_Server = server.Trim().ToLower();
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.aktif_adres = aktif;
                KaydetmeIslemleri.kaydetR(db);


            }
            else
            {
                ayarimiz = new TeknikServis.Radius.ayar();
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = port;
                ayarimiz.Mail_Server = server.Trim().ToLower();
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.Firma = "firma";

                ayarimiz.tur = "mail";
                ayarimiz.aktif_adres = aktif;
                db.ayars.Add(ayarimiz);
                KaydetmeIslemleri.kaydetR(db);
                //yeni kayıt
            }


        }
        public void SmsAyarKaydetR(string saglayici, string kimden, string username, string pw, string aktif)
        {

            TeknikServis.Radius.ayar ayarimiz = db.ayars.Where(a => a.tur.Equals("sms")).FirstOrDefault();
            if (ayarimiz != null)
            {
                //update
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = 0;
                ayarimiz.Mail_Server = saglayici;
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.aktif_adres = aktif;
                KaydetmeIslemleri.kaydetR(db);


            }
            else
            {
                ayarimiz = new TeknikServis.Radius.ayar();
                ayarimiz.Mail_PW = pw;
                ayarimiz.Mail_Port = 0;
                ayarimiz.Mail_Server = saglayici;
                ayarimiz.Mail_UserName = username;
                ayarimiz.Mail_Kimden = kimden;
                ayarimiz.Firma = "firma";

                ayarimiz.tur = "sms";
                ayarimiz.aktif_adres = aktif;
                db.ayars.Add(ayarimiz);
                KaydetmeIslemleri.kaydetR(db);

            }


        }
        #endregion

        #region servis tip

        public void tipEkleR(string ad, string aciklama, string css)
        {
            if (string.IsNullOrEmpty(css))
            {
                css = "#5367ce";
            }
            TeknikServis.Radius.service_tips tip = new TeknikServis.Radius.service_tips();
            tip.tip_ad = ad;
            tip.Firma = "firma";
            tip.aciklama = aciklama;
            tip.css = css;
            db.service_tips.Add(tip);
            KaydetmeIslemleri.kaydetR(db);



        }
        public void masrafEkleR(string ad, string aciklama, string css)
        {

            if (string.IsNullOrEmpty(css))
            {
                css = "#b8afaf";
            }

            TeknikServis.Radius.masraf_tips tip = new TeknikServis.Radius.masraf_tips();
            tip.tip_adi = ad;
            tip.Firma = "firma";

            tip.aciklama = aciklama;
            tip.css = css;
            db.masraf_tips.Add(tip);
            KaydetmeIslemleri.kaydetR(db);


        }
        public TeknikServis.Radius.service_tips tekTipR(int id)
        {
            return db.service_tips.Where(t => t.tip_id == id).FirstOrDefault();
        }
        public TeknikServis.Radius.masraf_tips masrafTipR(int id)
        {

            return db.masraf_tips.Where(t => t.tip_id == id).FirstOrDefault();
        }

        public void tipGuncelleR(int id, string ad, string aciklama, string css)
        {
            TeknikServis.Radius.service_tips tip = tekTipR(id);
            tip.tip_ad = ad;
            tip.aciklama = aciklama;
            if (!string.IsNullOrEmpty(css))
            {
                tip.css = css;
            }
            KaydetmeIslemleri.kaydetR(db);

        }
        public void masrafGuncelleR(int id, string ad, string aciklama, string css)
        {

            TeknikServis.Radius.masraf_tips tip = masrafTipR(id);
            tip.tip_adi = ad;
            tip.aciklama = aciklama;
            if (!string.IsNullOrEmpty(css))
            {
                tip.css = css;
            }

            KaydetmeIslemleri.kaydetR(db);

        }
        public void tipSilR(int id)
        {

            TeknikServis.Radius.service_tips tip = tekTipR(id);
            tip.iptal = true;
            KaydetmeIslemleri.kaydetR(db);

        }
        public void masrafSilR(int id)
        {
            TeknikServis.Radius.masraf_tips tip = masrafTipR(id);
            tip.iptal = true;
            KaydetmeIslemleri.kaydetR(db);
        }
        public List<TeknikServis.Radius.service_tips> tipListesiR()
        {

            return (from t in db.service_tips
                    where t.iptal == null
                    orderby t.tip_id descending
                    select t).ToList();
        }
        public List<TeknikServis.Radius.service_tips> tipListesiGrid()
        {

            return (from t in db.service_tips
                    where t.iptal == null && t.tip_id > 0
                    select t).ToList();
        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiR()
        {

            return (from t in db.masraf_tips
                    where t.iptal == false
                    select t).ToList();

        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiGrid()
        {

            return (from t in db.masraf_tips
                    where t.iptal == false && t.tip_id > 0
                    select t).ToList();

        }
        public List<TeknikServis.Radius.service_tips> tipListesiTekliR(int id)
        {

            return (from t in db.service_tips
                    where t.tip_id == id
                    select t).ToList();
        }
        public List<TeknikServis.Radius.masraf_tips> masrafListesiTekliR(int id)
        {

            return (from t in db.masraf_tips
                    where t.tip_id == id
                    select t).ToList();

        }
        #endregion
        private bool kontrol(bool baslangic, bool son, bool karar, bool onay)
        {

            bool flag = false;
            if (baslangic == true || son == true || karar == true || onay == true)
            {

                if (baslangic == true)
                {
                    if (son != true && karar != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (son == true)
                {
                    if (baslangic != true && karar != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (karar == true)
                {
                    if (son != true && baslangic != true && onay != true)
                    {
                        flag = true;
                    }
                }
                if (onay == true)
                {
                    if (son != true && baslangic != true && karar != true)
                    {
                        flag = true;
                    }
                }
            }
            else
            {
                flag = true;
            }
            return flag;

        }
        public bool servisDurumEkleR(string durum, bool sms, bool mail, bool what, bool baslangic, bool son, bool karar, bool onay)
        {
            if (kontrol(baslangic, son, karar, onay) == true)
            {

                //başka bir başlangıç varsa değiştirelim
                //bunu triggerla yapabiliriz
                List<TeknikServis.Radius.service_durums> durumlar = durumListesiR();

                if (baslangic == true)
                {
                    //şimdi bu durumlardaki bütün başlangıçları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.baslangicmi == true)
                        {
                            //bunu false yapalım
                            d.baslangicmi = false;
                        }

                    }
                }
                if (son == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.sonmu == true)
                        {
                            //bunu false yapalım
                            d.sonmu = false;
                        }
                    }
                }
                if (karar == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.kararmi == true)
                        {
                            //bunu false yapalım
                            d.kararmi = false;
                        }
                    }
                }
                if (onay == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.onaymi == true)
                        {
                            //bunu false yapalım
                            d.onaymi = false;
                        }
                    }
                }


                TeknikServis.Radius.service_durums yeni = new TeknikServis.Radius.service_durums();
                yeni.Durum = durum;
                yeni.Firma = "firma";
                yeni.SMS = sms;
                yeni.Mail = mail;
                yeni.Whatsapp = what;
                yeni.sonmu = son;
                yeni.baslangicmi = baslangic;
                yeni.kararmi = karar;
                yeni.onaymi = onay;

                db.service_durums.Add(yeni);
                KaydetmeIslemleri.kaydetR(db);


                return true;
            }
            else
            {
                return false;
            }
        }

        public TeknikServis.Radius.service_durums tekDurumR(int id)
        {

            return db.service_durums.Where(p => p.Durum_ID == id && (p.iptal == null || p.iptal == false)).FirstOrDefault();

        }

        public TeknikServis.Radius.service_durums tekDurumBaslangicR()
        {

            return db.service_durums.Where(p => p.baslangicmi == true && (p.iptal == null || p.iptal == false)).FirstOrDefault();

        }

        public TeknikServis.Radius.service_durums tekDurumSonmuR()
        {

            return db.service_durums.Where(p => p.sonmu == true && (p.iptal == null || p.iptal == false)).FirstOrDefault();

        }

        public bool servisDurumGuncelleR(int id, string durum, bool sms, bool mail, bool what, bool baslangic, bool son, bool karar, bool onay)
        {

            if (kontrol(baslangic, son, karar, onay) == true)
            {
                //başka bir başlangıç varsa değiştirelim
                //bunu triggerla yapabiliriz
                List<TeknikServis.Radius.service_durums> durumlar = durumListesiR();

                if (baslangic == true)
                {
                    //şimdi bu durumlardaki bütün başlangıçları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.baslangicmi == true)
                        {
                            //bunu false yapalım
                            d.baslangicmi = false;
                        }

                    }
                }
                if (son == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.sonmu == true)
                        {
                            //bunu false yapalım
                            d.sonmu = false;
                        }
                    }
                }
                if (karar == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.kararmi == true)
                        {
                            //bunu false yapalım
                            d.kararmi = false;
                        }
                    }
                }
                if (onay == true)
                {
                    //şimdi bu durumlardaki bütün sonları değiştirelim
                    foreach (TeknikServis.Radius.service_durums d in durumlar)
                    {
                        if (d.onaymi == true)
                        {
                            //bunu false yapalım
                            d.onaymi = false;
                        }
                    }
                }


                TeknikServis.Radius.service_durums yeni = tekDurumR(id);
                yeni.Durum = durum;
                yeni.SMS = sms;
                yeni.Mail = mail;
                yeni.Whatsapp = what;
                yeni.baslangicmi = baslangic;
                yeni.sonmu = son;
                yeni.kararmi = karar;
                yeni.onaymi = onay;

                KaydetmeIslemleri.kaydetR(db);

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<TeknikServis.Radius.service_durums> durumListesiR()
        {

            return db.service_durums.Where(d => (d.iptal == null || d.iptal == false)).ToList();

        }

        public List<TeknikServis.Radius.service_durums> durumListesiTekliR(int id)
        {

            return db.service_durums.Where(d => d.Durum_ID == id).ToList();


        }

        public void durumSilR(int id)
        {

            TeknikServis.Radius.service_durums durum = tekDurumR(id);
            durum.iptal = true;
            KaydetmeIslemleri.kaydetR(db);


        }



    }

}
