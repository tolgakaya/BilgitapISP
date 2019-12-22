using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Radius;
using ServisDAL;
using ServisDAL.Repo;
namespace TeknikServis
{
    public partial class CustQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //goster();
        }
        public void ServisAra(object sender, EventArgs e)
        {
            goster();

        }

        private void goster()
        {
            //string kimlik = txtAra.Value.Trim();

            //if (!String.IsNullOrEmpty(kimlik))
            //{
            //    //servis kimliğiymiş
            //    string gelenUrl = Request.UrlReferrer.Host;
            //    //bu url hangi firmanınmış bakalım
            //    radiusEntities dc = MyContext.Context("BILGITAP");
            //    string firma = (dc.bilgitap_firmas.Where(x => x.web.Contains(gelenUrl)).Select(x => x.firma)).FirstOrDefault();

            //    int uzunluk = kimlik.Length;
            //    if (uzunluk == 10)
            //    {
            //        gosterKimlik(kimlik, firma);

            //    }
            //    else if (uzunluk == 11)
            //    {
            //        //demek ki TC imiş

            //        gosterTc(kimlik, firma);


            //    }
            //}
        }

        private void gosterTc(string kimlik, string firma)
        {
            radiusEntities db = MyContext.Context(firma);
            Radius.customer cust = (from c in db.customers
                                    where c.TC == kimlik
                                    select c).FirstOrDefault();
            if (cust != null)
            {
                //ödenmemiş faturalardan günü gelen ödemeyi gösterelim

                List<fatura_taksit> faturalarimiz = (from f in db.faturas
                                                     where (f.odendi == null || f.odendi == false) && f.MusteriID == cust.CustID
                                                      && (f.iptal == null || f.iptal == false)
                                                     orderby f.sattis_tarih ascending
                                                     select new fatura_taksit
                                                     {
                                                         bakiye = f.bakiye,
                                                         
                                                         hesap_tur = f.tur,
                                                         ID = f.ID,
                                                        
                                                         no = f.no,
                                                         odendi = f.odendi,
                                                         odenen = f.odenen,
                                                         tc = f.tc,
                                                         telefon = f.telefon,
                                                         tutar = f.tutar,
                                                         username = ""
                                                     }).ToList();
                //gunu gelen ve geçen fatura toplamı
                decimal gunuGelenTutar = 0;
                decimal gunuGelenAdet = 0;
                if (faturalarimiz.Count > 0)
                {
                    tablo3.Visible = true;
                    gunuGelenTutar = faturalarimiz.Where(x => x.son_odeme_tarihi <= DateTime.Now).Sum(x => x.tutar);
                    gunuGelenAdet = faturalarimiz.Where(x => x.son_odeme_tarihi <= DateTime.Now).Count();
                    //bir sonraki ödeme ve tarihi
                    fatura_taksit birSonraki = faturalarimiz.FirstOrDefault(x => x.son_odeme_tarihi > DateTime.Now);

                    if (birSonraki != null)
                    {
                        sonrakiTarih.InnerHtml = birSonraki.son_odeme_tarihi.ToShortDateString();
                        sonrakiTutar.InnerHtml = birSonraki.tutar.ToString("C");
                    }
                }



                Radius.carihesap hesap = (from h in db.carihesaps
                                          where h.MusteriID == cust.CustID
                                          select h).FirstOrDefault();
                tablolar.Visible = true;
                tablo2.Visible = true;
                double kalangun = 0;
                string paket = "Pasif";
                //DateTime exp = (DateTime)cust.gecerlilik;
                //if (exp.Date > DateTime.Now.Date)
                //{
                //    kalangun = (exp.Date - DateTime.Now.Date).TotalDays;
                //    paket = "Aktif";
                //}
                //gecerlilik.InnerHtml = kalangun.ToString();
                //gecerlilikTarih.InnerHtml = exp.Date.ToShortDateString();



                string borcumuz = gunuGelenTutar.ToString() + " TL";

                if (hesap != null)
                {
                    toplamBorc.InnerHtml = hesap.ToplamBakiye.ToString() + " TL";
                }

                borc.InnerHtml = borcumuz;
                internet.InnerHtml = paket;
                ServisRepo rep = (from s in db.services
                                  where s.CustID == cust.CustID
                                  orderby s.ServiceID ascending
                                  select new ServisRepo
                                  {
                                      serviceID = s.ServiceID,
                                      custID = (int)s.CustID,
                                      musteriAdi = s.customer.Ad,
                                      kullaniciID = s.olusturan_Kullanici,
                                      sonGorevli = s.SonAtananID,

                                      aciklama = s.Aciklama,
                                      acilmaZamani = s.AcilmaZamani,
                                      kapanmaZamani = s.KapanmaZamani,
                                      urunID = s.UrunID,
                                      urunAdi = s.urun.Cinsi,
                                      kimlikNo = s.Servis_Kimlik_No,
                                      tipID = s.tip_id,
                                      servisTipi = s.service_tips.tip_ad,
                                      sonDurum = s.SonDurum
                                  }).FirstOrDefault();

                if (rep != null)
                {

                    musteri.InnerHtml = rep.musteriAdi;
                    durum.InnerHtml = rep.sonDurum;
                    aciklama.InnerHtml = rep.servisTipi;
                }


                List<ServisHesapRepo> kararlar = (from s in db.servicehesaps
                                                  where s.MusteriID == cust.CustID && s.iptal == false && (s.onay == null || s.onay == false)
                                                  orderby s.TarihZaman descending
                                                  select new ServisHesapRepo
                                                  {
                                                      hesapID = s.HesapID,
                                                      aciklama = s.Aciklama,
                                                      islemParca = s.IslemParca,
                                                      urun_cinsi = s.Urun_Cinsi,
                                                      kdv = s.KDV,
                                                      musteriAdi = s.customer.Ad,
                                                      musteriID =(int)s.MusteriID,
                                                      onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                                                      onaylimi = s.onay,
                                                      onayTarih = s.Onay_tarih,
                                                      tarihZaman = s.TarihZaman,
                                                      servisID = s.ServiceID,
                                                      tutar = s.Tutar,
                                                      yekun = s.Yekun

                                                  }).ToList();

                if (kararlar.Count > 0)
                {

                    GridView3.Visible = true;
                    btnOnay.Visible = true;
                    grdKararlar.Visible = true;
                    GridView3.DataSource = kararlar;
                    GridView3.DataBind();
                }
                else
                {

                    grdKararlar.Visible = false;
                    //GridView1.Visible = false;
                    //btnOnay.Visible = false;
                }


            }
            else
            {
                tablo2.Visible = false;
                grdKararlar.Visible = false;
                //GridView1.Visible = false;
                //btnOnay.Visible = false;
            }

        }

        private void gosterKimlik(string kimlik, string firma)
        {
            if (!String.IsNullOrEmpty(firma))
            {

                using (radiusEntities db = MyContext.Context(firma))
                {


                    ServisRepo rep = (from s in db.services
                                      where s.Servis_Kimlik_No == kimlik
                                      orderby s.ServiceID ascending
                                      select new ServisRepo
                                      {
                                          serviceID = s.ServiceID,
                                          custID = (int)s.CustID,
                                          musteriAdi = s.customer.Ad,
                                          kullaniciID = s.olusturan_Kullanici,
                                          sonGorevli = s.SonAtananID,

                                          aciklama = s.Aciklama,
                                          acilmaZamani = s.AcilmaZamani,
                                          kapanmaZamani = s.KapanmaZamani,
                                          urunID = s.UrunID,
                                          urunAdi = s.urun.Cinsi,
                                          kimlikNo = s.Servis_Kimlik_No,
                                          tipID = s.tip_id,
                                          servisTipi = s.service_tips.tip_ad,
                                          sonDurum = s.SonDurum
                                      }).FirstOrDefault();

                    if (rep != null)
                    {
                        tablolar.Visible = true;
                        //tablo2.Visible = true;
                        musteri.InnerHtml = rep.musteriAdi;
                        durum.InnerHtml = rep.sonDurum;
                        aciklama.InnerHtml = rep.servisTipi;


                        Radius.customer cust = (from c in db.customers
                                                where c.CustID == rep.custID
                                                select c).FirstOrDefault();
                        if (cust != null)
                        {
                            List<fatura_taksit> faturalarimiz = (from f in db.faturas
                                                                 where (f.odendi == null || f.odendi == false) && f.MusteriID == cust.CustID
                                                                  && (f.iptal == null || f.iptal == false)
                                                                 orderby f.sattis_tarih ascending
                                                                 select new fatura_taksit
                                                                 {
                                                                     bakiye = f.bakiye,

                                                                     hesap_tur = f.tur,
                                                                     ID = f.ID,

                                                                     no = f.no,
                                                                     odendi = f.odendi,
                                                                     odenen = f.odenen,
                                                                     tc = f.tc,
                                                                     telefon = f.telefon,
                                                                     tutar = f.tutar,
                                                                     username = ""
                                                                 }).ToList();
                            //gunu gelen ve geçen fatura toplamı
                            decimal gunuGelenTutar = 0;
                            decimal gunuGelenAdet = 0;
                            if (faturalarimiz.Count > 0)
                            {
                                tablo3.Visible = true;
                                gunuGelenTutar = faturalarimiz.Where(x => x.son_odeme_tarihi <= DateTime.Now).Sum(x => x.tutar);
                                gunuGelenAdet = faturalarimiz.Where(x => x.son_odeme_tarihi <= DateTime.Now).Count();
                                //bir sonraki ödeme ve tarihi
                                fatura_taksit birSonraki = faturalarimiz.FirstOrDefault(x => x.son_odeme_tarihi > DateTime.Now);

                                if (birSonraki != null)
                                {
                                    sonrakiTarih.InnerHtml = birSonraki.son_odeme_tarihi.ToShortDateString();
                                    sonrakiTutar.InnerHtml = birSonraki.tutar.ToString("C");
                                }
                            }

                            Radius.carihesap hesap = (from h in db.carihesaps
                                                      where h.MusteriID == cust.CustID
                                                      select h).FirstOrDefault();

                            if (hesap != null)
                            {
                                toplamBorc.InnerHtml = hesap.ToplamBakiye.ToString("C");
                            }
                            //tablo.Visible = true;
                            double kalangun = 0;
                            string paket = "Pasif";
                            //DateTime exp = (DateTime)cust.gecerlilik;
                            //if (exp.Date > DateTime.Now.Date)
                            //{
                            //    kalangun = (exp.Date - DateTime.Now.Date).TotalDays;
                            //    paket = "Aktif";
                            //}
                            //gecerlilik.InnerHtml = kalangun.ToString();
                            //gecerlilikTarih.InnerHtml = exp.Date.ToShortDateString();


                            //if (hesap != null)
                            //{
                            string borcumuz = gunuGelenTutar.ToString() + " TL";
                            //}

                            borc.InnerHtml = borcumuz;
                            internet.InnerHtml = paket;
                        }


                        //hesap var mı bakalım


                        List<ServisHesapRepo> kararlar = (from s in db.servicehesaps
                                                          where s.ServiceID == rep.serviceID && s.iptal == false && (s.onay == null || s.onay == false)
                                                          orderby s.TarihZaman descending
                                                          select new ServisHesapRepo
                                                          {
                                                              hesapID = s.HesapID,
                                                              aciklama = s.Aciklama,
                                                              islemParca = s.IslemParca,
                                                              urun_cinsi = s.Urun_Cinsi,
                                                              kdv = s.KDV,
                                                              musteriAdi = s.customer.Ad,
                                                              musteriID = (int)s.MusteriID,
                                                              onayDurumu = (s.onay == true ? "EVET" : "HAYIR"),
                                                              onaylimi = s.onay,
                                                              onayTarih = s.Onay_tarih,
                                                              tarihZaman = s.TarihZaman,
                                                              servisID = s.ServiceID,
                                                              tutar = s.Tutar,
                                                              yekun = s.Yekun

                                                          }).ToList();

                        if (kararlar.Count > 0)
                        {

                            GridView3.Visible = true;
                            btnOnay.Visible = true;
                            grdKararlar.Visible = true;
                            GridView3.DataSource = kararlar;
                            GridView3.DataBind();
                        }
                        else
                        {
                            grdKararlar.Visible = false;
                            GridView3.Visible = false;
                            btnOnay.Visible = false;
                        }


                    }
                    else
                    {
                        //tablo2.Visible = false;
                        grdKararlar.Visible = false;
                    }
                }
            }
        }

        protected void btnOnay_Click(object sender, EventArgs e)
        {
            ////hepsini onaylayalım
            //string kimlik = txtAra.Value.Trim();

            //if (!String.IsNullOrEmpty(kimlik))
            //{
            //    //servis kimliğiymiş
            //    string gelenUrl = Request.UrlReferrer.Host;
            //    //bu url hangi firmanınmış bakalım
            //    radiusEntities dc = MyContext.Context("BILGITAP");
            //    string firma = (dc.bilgitap_firmas.Where(x => x.web.Contains(gelenUrl)).Select(x => x.firma)).FirstOrDefault();

            //    int uzunluk = kimlik.Length;
            //    if (uzunluk == 10)
            //    {
            //        onayKimlik(kimlik, firma);
            //        grdKararlar.Visible = false;
            //    }
            //    else if (uzunluk == 11)
            //    {
            //        //demek ki TC imiş

            //        onaylaTc(kimlik, firma);
            //        grdKararlar.Visible = false;

            //    }
            //}

        }

        //onaylandıktan sonra firmanın kendisine sms gönderilecek

        private void onayKimlik(string kimlik, string firma)
        {
            if (!String.IsNullOrEmpty(firma))
            {
                using (radiusEntities db = MyContext.Context(firma))
                {

                    ServisIslemleri servis = new ServisIslemleri(db);
                    ServisRepo rep = servis.servisAraKimlikDetayTekR(kimlik);
                    if (rep != null)
                    {
                        //hesap var mı bakalım

                        List<servicehesap> kararlar = (from s in db.servicehesaps
                                                       where s.ServiceID == rep.serviceID && s.iptal == false && (s.onay == null || s.onay == false)
                                                       orderby s.TarihZaman descending
                                                       select s).ToList();

                        if (kararlar != null)
                        {
                            foreach (servicehesap karar in kararlar)
                            {
                                karar.onay = true;
                                karar.Onay_tarih = DateTime.Now;
                                karar.Aciklama = "Müşteri Onayı";

                            }

                            KaydetmeIslemleri.kaydetR(db);
                            goster();
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.success('Kararlar Onaylandı!');");

                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript4", sb.ToString(), false);

                        }

                    }
                }
            }
        }

        private void onaylaTc(string kimlik, string firma)
        {
            radiusEntities db = MyContext.Context(firma);
            Radius.customer cust = (from c in db.customers
                                    where c.TC == kimlik
                                    select c).FirstOrDefault();
            if (cust != null)
            {
                List<servicehesap> kararlar = (from s in db.servicehesaps
                                               where s.MusteriID == cust.CustID && s.iptal == false && (s.onay == null || s.onay == false)
                                               orderby s.TarihZaman descending
                                               select s).ToList();

                if (kararlar != null)
                {
                    foreach (servicehesap karar in kararlar)
                    {
                        karar.onay = true;
                        karar.Onay_tarih = DateTime.Now;
                        karar.Aciklama = "Müşteri Onayı";

                    }

                    KaydetmeIslemleri.kaydetR(db);
                    goster();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kararlar Onaylandı!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript4", sb.ToString(), false);

                }

            }
        }
    }
}