using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using System.Text;
namespace ServisDAL
{
    public class MusteriIslemleri
    {
        radiusEntities db;

        public MusteriIslemleri(radiusEntities db)
        {

            this.db = db;

        }
        #region musteri arramalari
        //Burası sorgu sayısını azaltmak için yedek olarak kullanılacak
        public pos_banka_look posbankalar()
        {
            pos_banka_look pb = new pos_banka_look();

            List<pos_repo> poslarr = (from p in db.pos_tanims

                                      select new pos_repo
                                      {
                                          pos_adi = p.pos_adi,
                                          pos_id = p.pos_id
                                      }).ToList();

            List<bank_repo> bankalarr = (from p in db.bankas

                                         select new bank_repo
                                         {
                                             banka_adi = p.banka_adi,
                                             banka_id = p.banka_id
                                         }).ToList();
            pb.bankalar = bankalarr;
            pb.poslar = poslarr;

            return pb;
        }

        public TeknikServis.Radius.customer musteriTekR(int id)
        {
            return (from c in db.customers
                    where c.CustID == id
                    select c).FirstOrDefault();

        }

        //aktifleştirme triggerdan kredi yüklendiği zaman oluyor
        public void istihbarat_kaydet(int id, string not)
        {
            customer cust = (from c in db.customers
                             where c.CustID == id
                             select c).FirstOrDefault();
            if (cust != null)
            {
                cust.istihbarat = not;
                KaydetmeIslemleri.kaydetR(db);
            }

        }
        public string musteriTEL(int id)
        {
            return (from c in db.customers
                    where c.CustID == id
                    select c.telefon).FirstOrDefault();
        }

        public string musteriMail(int id)
        {
            return (from c in db.customers
                    where c.CustID == id
                    select c.email).FirstOrDefault();

        }

        public string musteriTekAdR(int id)
        {
            return (from c in db.customers
                    where c.CustID == id
                    select c.Ad).FirstOrDefault();

        }

        //admin için
        public List<TeknikServis.Radius.customer> musteriTekListeR(int id)
        {
            return (from c in db.customers
                    where c.CustID == id
                    select c).ToList();
        }
        //admin için
        public void antenMusteriGuncelle(int custid, int? antenid)
        {
            customer c = db.customers.FirstOrDefault(x => x.CustID == custid);
            //if (c!=null)
            //{
            c.antenid = antenid;
            KaydetmeIslemleri.kaydetR(db);
            //}
        }
        public void antenTasi(int mevcutAnten, int? yeniAnten)
        {
            var musteriler = db.customers.Where(x => x.antenid != null && x.antenid == mevcutAnten).ToList();
            foreach (var m in musteriler)
            {
                m.antenid = yeniAnten;
            }
            KaydetmeIslemleri.kaydetR(db);
        }
        public musteri_tel_mail antenMusterileri(int antenid)
        {
            musteri_tel_mail bilgiler = new musteri_tel_mail();
            string antenadi = db.antens.FirstOrDefault(x => x.anten_id == antenid).anten_adi;
            bilgiler.anten_adi = antenadi;
            List<customer> musteriler = db.customers.Where(x => x.antenid == antenid).ToList();
            bilgiler.musteriler = musteriler;
            if (musteriler.Count > 0)
            {
                List<string> teller = musteriler.Where(x => !String.IsNullOrEmpty(x.telefon)).Select(x => x.telefon).ToList();
                List<string> mailler = musteriler.Where(x => !String.IsNullOrEmpty(x.email)).Select(x => x.email).ToList();
                string tels = "";
                string mails = "";
                StringBuilder build = new StringBuilder();
                StringBuilder build2 = new StringBuilder();
                foreach (string t in teller)
                {
                    build.Append(t);
                    build.Append(",");

                }
                tels = build.ToString();

                foreach (string m in mailler)
                {
                    build2.Append(m);
                    build2.Append(",");

                }
                mails = build2.ToString();
                bilgiler.teller = tels;
                bilgiler.mailler = mails;
            }



            return bilgiler;
        }
        public musteri_tel_mail antenMusterileri(string antendekiler)
        {
            musteri_tel_mail bilgiler = new musteri_tel_mail();


            if (!String.IsNullOrEmpty(antendekiler))
            {
                string[] idsler = antendekiler.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (idsler.Length > 0)
                {
                    List<int> idlistesi = new List<int>();
                    for (int i = 0; i < idsler.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(idsler[i]))
                        {
                            idlistesi.Add(Convert.ToInt32(idsler[i]));
                        }
                    }
                    if (idlistesi.Count > 0)
                    {
                        List<customer> musteriler = db.customers.Where(x => idlistesi.Contains(x.CustID)).ToList();
                        bilgiler.musteriler = musteriler;
                        if (musteriler.Count > 0)
                        {
                            List<string> teller = musteriler.Where(x => !String.IsNullOrEmpty(x.telefon)).Select(x => x.telefon).ToList();
                            List<string> mailler = musteriler.Where(x => !String.IsNullOrEmpty(x.email)).Select(x => x.email).ToList();
                            string tels = "";
                            string mails = "";
                            StringBuilder build = new StringBuilder();
                            StringBuilder build2 = new StringBuilder();
                            foreach (string t in teller)
                            {
                                build.Append(t);
                                build.Append(",");

                            }
                            tels = build.ToString();

                            foreach (string m in mailler)
                            {
                                build2.Append(m);
                                build2.Append(",");

                            }
                            mails = build2.ToString();
                            bilgiler.teller = tels;
                            bilgiler.mailler = mails;
                        }
                    }
                }

            }
            return bilgiler;
        }
        public void haritayaGoreKaydet(string idlatlng)
        {
            if (!String.IsNullOrEmpty(idlatlng))
            {
                List<string> idCoord = idlatlng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (idCoord.Count > 0)
                {
                    foreach (string s in idCoord)
                    {
                        List<string> tek = s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (tek.Count > 2)
                        {
                            int id = Convert.ToInt32(tek[0]);
                            string lat = tek[1];
                            string lng = tek[2];
                            var musteri = db.customers.FirstOrDefault(x => x.CustID == id);
                            if (musteri != null)
                            {
                                musteri.Lat = lat;
                                musteri.Long = lng;
                            }

                        }
                    }
                }
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        public void haritayaGoreanteneKaydet(string idlatlng, int antenid)
        {
            if (!String.IsNullOrEmpty(idlatlng))
            {
                List<string> idCoord = idlatlng.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (idCoord.Count > 0)
                {
                    foreach (string s in idCoord)
                    {
                        List<string> tek = s.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (tek.Count > 2)
                        {
                            int id = Convert.ToInt32(tek[0]);
                            string lat = tek[1];
                            string lng = tek[2];
                            var musteri = db.customers.FirstOrDefault(x => x.CustID == id);
                            if (musteri != null)
                            {
                                musteri.Lat = lat;
                                musteri.Long = lng;
                                musteri.antenid = antenid;
                            }

                        }
                    }
                }
                KaydetmeIslemleri.kaydetR(db);
            }
        }
        public musteri_tel_mail musteriAraR(string kelime)
        {
            kelime = kelime.ToLower();
            musteri_tel_mail liste = new musteri_tel_mail();

            List<TeknikServis.Radius.customer> mler = (from c in db.customers
                                                       where (c.CustID > 0) && (c.Ad.Contains(kelime) || c.TC.Contains(kelime) || c.telefon.Contains(kelime) ||
                                                       c.telefon_cep.Contains(kelime) || c.email.Contains(kelime) ||
                                                       c.Adres.Contains(kelime) || c.tanimlayici.Contains(kelime))
                                                       orderby c.CustID descending
                                                       select c).ToList();

            string teller = "";
            string mailler = "";
            foreach (customer c in mler)
            {
                if (!String.IsNullOrEmpty(c.telefon) && c.telefon.Length > 9)
                {
                    teller += c.telefon + ",";
                }
                if (!String.IsNullOrEmpty(c.email) && c.email.Length > 7)
                {
                    mailler += c.email + ";";
                }

            }
            liste.musteriler = mler;
            liste.mailler = mailler;
            liste.teller = teller;


            return liste;

        }
        public List<customer> musteriAraR2(string kelime, string tip)
        {
            bool musteri = false;
            bool tedarikci = false;
            bool usta = false;
            bool tamirci = false;

            if (string.Equals(tip, "musteri"))
            {
                musteri = true;
            }
            else if (string.Equals(tip, "tedarikci"))
            {
                tedarikci = true;
            }
            else if (string.Equals(tip, "usta"))
            {
                usta = true;
            }
            if (string.Equals(tip, "tamirci"))
            {
                tamirci = true;
            }

            kelime = kelime.ToLower();


            //return (from c in db.customers
            //        where (c.CustID > 0) &&
            //        (string.Equals(tip, "musteri") ? c.musteri == true :
            //        (string.Equals(tip, "tedarikci") ? c.tedarikci == true :
            //        (string.Equals(tip, "tamirci") ? c.disservis == true :
            //        (string.Equals(tip, "usta") ? c.usta == true : c.CustID > 0))))
            //        && (c.Ad.Contains(kelime) || c.TC.Contains(kelime) || c.telefon.Contains(kelime) ||
            //        c.telefon_cep.Contains(kelime) || c.email.Contains(kelime) ||
            //        c.Adres.Contains(kelime) || c.tanimlayici.Contains(kelime))
            //        orderby c.CustID descending
            //        select c).ToList();

            return (from c in db.customers
                    where (c.CustID > 0) && c.musteri == musteri && c.tedarikci == tedarikci && c.usta == usta && c.disservis == tamirci && (c.Ad.Contains(kelime) || c.TC.Contains(kelime) || c.telefon.Contains(kelime) ||
                    c.telefon_cep.Contains(kelime) || c.email.Contains(kelime) ||
                    c.Adres.Contains(kelime) || c.tanimlayici.Contains(kelime))
                    orderby c.CustID descending
                    select c).ToList();

        }
        public List<customer> musteriAraCari(string kelime)
        {


            kelime = kelime.ToLower();


            return (from c in db.customers
                    where (c.CustID > 0) && (c.Ad.Contains(kelime) || c.TC.Contains(kelime) || c.telefon.Contains(kelime) ||
                    c.telefon_cep.Contains(kelime) || c.email.Contains(kelime) ||
                    c.Adres.Contains(kelime) || c.tanimlayici.Contains(kelime))
                    orderby c.CustID descending
                    select c).ToList();



        }
        #endregion

        //admin için
        public musteri_tel_mail musteriListesiR()
        {
            musteri_tel_mail liste = new musteri_tel_mail();

            List<TeknikServis.Radius.customer> mler = db.customers.Where(x => x.CustID > 0).OrderByDescending(x => x.CustID).ToList();
            string teller = "";
            string mailler = "";
            foreach (customer c in mler)
            {
                if (!String.IsNullOrEmpty(c.telefon) && c.telefon.Length > 9)
                {
                    teller += c.telefon + ",";
                }
                if (!String.IsNullOrEmpty(c.email) && c.email.Length > 7)
                {
                    mailler += c.email + ";";
                }

            }
            liste.musteriler = mler;
            liste.mailler = mailler;
            liste.teller = teller;


            return liste;

        }


        public string mailListesiR()
        {
            string mailler = "";
            List<string> liste = db.customers.Where(x => x.CustID > 0 && !String.IsNullOrEmpty(x.email)).OrderByDescending(x => x.CustID).Select(x => x.email).ToList();

            foreach (string s in liste)
            {
                if (!String.IsNullOrEmpty(s))
                {
                    mailler += s + ";";
                }
            }
            return mailler;
        }
        public string mailListesiSR()
        {
            string mailler = "";
            List<string> liste = db.customers.Where(x => x.CustID > 0 && !String.IsNullOrEmpty(x.email)).OrderByDescending(x => x.CustID).Select(x => x.email).ToList();

            foreach (string s in liste)
            {
                if (!String.IsNullOrEmpty(s))
                {
                    mailler += s + ";";
                }
            }
            return mailler;
        }

        public List<anten> antenler()
        {
            return db.antens.ToList();
        }

        //bayi-manager için current kullanicinin Id'si
        //normal kullanici için bu current kullanicinin owner ID'si bayiID parametresi olacak.
        public void musteriEkleR(string ad, string soyad, string unvan, string adres, string tel, string telCep, string email, string kim, string tc, string prim_kar, string prim_satis, bool chcMusteri, bool chcTedarikci, bool chcUsta, bool chcDisServis, int? antenid)
        {
            int kar_oran = 0;
            int yekun_oran = 0;
            if (!string.IsNullOrEmpty(prim_kar))
            {
                kar_oran = Int32.Parse(prim_kar);
            }
            if (!string.IsNullOrEmpty(prim_satis))
            {
                yekun_oran = Int32.Parse(prim_satis);
            }

            //müşteri ekleme normal customer tablosuna yapılacak
            customer c = new customer();
            c.Ad = ad + " " + soyad;
            c.tedarikci = chcTedarikci;
            c.musteri = chcMusteri;
            c.usta = chcUsta;
            c.disservis = chcDisServis;
            c.Adres = adres;
            c.telefon = tel;
            c.telefon_cep = telCep;
            c.email = email;
            c.tanimlayici = kim;
            c.TC = tc;
            c.Firma = "firma";
            c.prim_kar = kar_oran;
            c.prim_yekun = yekun_oran;
            c.unvan = unvan;
            c.antenid = antenid;
            db.customers.Add(c);
            KaydetmeIslemleri.kaydetR(db);
        }
        public void musteriEkleDefault(int id, string ad, string soyad, string adres, string tel, string telCep, string email, string kim, string tc, string prim_kar, string prim_satis, bool chcMusteri, bool chcTedarikci, bool chcUsta, bool chcDisServis)
        {
            int kar_oran = 0;
            int yekun_oran = 0;
            if (!string.IsNullOrEmpty(prim_kar))
            {
                kar_oran = Int32.Parse(prim_kar);
            }
            if (!string.IsNullOrEmpty(prim_satis))
            {
                yekun_oran = Int32.Parse(prim_satis);
            }

            //müşteri ekleme normal customer tablosuna yapılacak
            customer c = new customer();
            c.CustID = id;
            c.Ad = ad + " " + soyad;
            c.tedarikci = chcTedarikci;
            c.musteri = chcMusteri;
            c.usta = chcUsta;
            c.disservis = chcDisServis;
            c.Adres = adres;
            c.telefon = tel;
            c.telefon_cep = telCep;
            c.email = email;
            c.tanimlayici = kim;
            c.TC = tc;
            c.Firma = "firma";
            c.prim_kar = kar_oran;
            c.prim_yekun = yekun_oran;
            db.customers.Add(c);
            KaydetmeIslemleri.kaydetR(db);
        }

        public void musteriGuncelleR(int id, string ad, string unvan, string adres, string tel, string tc, string tanim, string prim_yekun, string prim_kar, bool chcMusteri, bool chcTedarikci, bool chcUsta, bool chcDisServis, int? antenid)
        {

            customer c = db.customers.Find(id);
            if (c != null)
            {
                int oran_yekun = 0;
                int oran_kar = 0;
                if (!string.IsNullOrEmpty(prim_yekun))
                {
                    oran_yekun = Int32.Parse(prim_yekun);
                }
                if (!string.IsNullOrEmpty(prim_kar))
                {
                    oran_kar = Int32.Parse(prim_kar);
                }
                c.Ad = ad;
                c.Adres = adres;
                c.telefon = tel;
                c.prim_yekun = oran_yekun;
                c.prim_kar = oran_kar;
                c.tanimlayici = tanim;
                c.TC = tc;
                c.disservis = chcDisServis;
                c.tedarikci = chcTedarikci;
                c.musteri = chcMusteri;
                c.usta = chcUsta;
                c.unvan = unvan;
                c.antenid = antenid;
            }
            KaydetmeIslemleri.kaydetR(db);
        }

    }

    public class musteri_tel_mail
    {
        public List<customer> musteriler { get; set; }
        public string teller { get; set; }
        public string mailler { get; set; }
        public string anten_adi { get; set; }
    }

    public class pos_banka_look
    {
        public List<pos_repo> poslar { get; set; }
        public List<bank_repo> bankalar { get; set; }

    }
    public class pos_repo
    {
        public int pos_id { get; set; }
        public string pos_adi { get; set; }

    }
    public class bank_repo
    {
        public int banka_id { get; set; }
        public string banka_adi { get; set; }

    }
}
