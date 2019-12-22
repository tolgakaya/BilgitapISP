using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using System.Linq;
using TeknikServis.Radius;

namespace TeknikServis.TeknikAlim
{
    public partial class SatinAl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            string firma = KullaniciIslem.firma();

            using (radiusEntities dc = MyContext.Context(firma))
            {
                if (!IsPostBack)
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    CihazAraa(dc);

                    string s = txtAra.Value;

                    if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
                    {
                        MusteriIslemleri m = new MusteriIslemleri(dc);

                        GridView1.DataSource = m.musteriAraR2(s, "tedarikci");
                        GridView1.DataBind();

                    }
                }
             
                // detaylara bakalım
                DetayGoster();
               
            }

        }

        private void DetayGoster()
        {
            if (Session["alimdetay"] != null)
            {

                List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                //deratların özelliklerini toplama atalım
                decimal kdv = 0;
                decimal tutar = 0;
                decimal yekun = 0;
                int adet = 0;

                if (detaylar.Count > 0)
                {
                    kdv = detaylar.Sum(x => x.kdv);
                    tutar = detaylar.Sum(x => x.tutar);
                    yekun = detaylar.Sum(x => x.yekun);
                }
                toplam_kdv.Text = kdv.ToString();
                toplam_tutar.Text = tutar.ToString();
                toplam_yekun.Text = yekun.ToString();

                grdDetay.DataSource = detaylar;
                grdDetay.DataBind();
                //upBilgi.Update();


            }
        }
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            string firma = KullaniciIslem.firma();

            if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
            {
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriArama(dc, s);
                    GridView1.DataBind();
                }

            }
        }


        public void CihazAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
            }

        }
        protected void CihazAraa(radiusEntities dc)
        {
            string firma = KullaniciIslem.firma();
            CihazMalzeme c = new CihazMalzeme(dc);
            string terim = txtCihazAra.Value;
            if (string.IsNullOrEmpty(terim))
            {
                grdCihaz.DataSource = c.CihazListesiComp();

            }
            else
            {
                grdCihaz.DataSource = c.CihazListesiComp(terim, true);

            }
            if (!IsPostBack)
            {
                drdGrup.AppendDataBoundItems = true;
                drdGrup.DataSource = c.CihazGruplar();
                drdGrup.DataValueField = "grupid";
                drdGrup.DataTextField = "grupadi";
                drdGrup.DataBind();

            }
            grdCihaz.DataBind();
        }
        private void MusteriArama(radiusEntities dc, string s)
        {
            MusteriIslemleri m = new MusteriIslemleri(dc);

            GridView1.DataSource = m.musteriAraR2(s, "tedarikci");

        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }
        //Handles Add button click in add modal popup
        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            //musteri ekleme yöntemi sonra radius yöntemi olarak burada tekrarlanacak.
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                //string id = lblMusID.Text;
                string ad = txtAdi.Text;
                string soyad = txtSoyAdi.Text;
                string adres = txtAdress.Text;
                string email = txtEmail.Text;
                string tel = txtTell.Text;
                string tc = Araclar.KimlikUret(11);
                string kullaniciAdi = Araclar.KimlikUret(10);
                string kim = txtKim.Text;
                string sifre = Araclar.KimlikUret(10);

                string unvan = txtDuzenUnvan.Text;
                if (string.IsNullOrEmpty(unvan))
                {
                    unvan = ad + " " + soyad;
                }

                m.musteriEkleR(ad, soyad, unvan, adres, tel, tel, email, kim, tc, "0", "0", false, true, false, false,null);
                MusteriArama(dc, ad);
                GridView1.DataBind();
                GridView1.SelectedIndex = -1;
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("document.getElementById('ContentPlaceHolder1_txtAra').value = document.getElementById('ContentPlaceHolder1_txtAdi').value;");
            //sb.Append("alert('Record Added Successfully');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("detail"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                // int code = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    DetailsView1.DataSource = m.musteriTekListeR(code);
                    DetailsView1.DataBind();
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
            }


        }



        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = txtAra.Value;
            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                MusteriIslemleri m = new MusteriIslemleri(dc);
                if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
                {

                    MusteriArama(dc, s);

                }
            }


            txtAciklama.Text = GridView1.SelectedValue.ToString();
            List<DetayRepo> detaylar = new List<DetayRepo>();
            if (Session["alimdetay"] != null)
            {
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);

                detaylar = (List<DetayRepo>)Session["alimdetay"];
                List<DetayRepo> musteriSorgusu = detaylar.Where(x => x.cust_id != musteri_id).ToList();
                if (musteriSorgusu.Count > 0)
                {
                    //hepsini değiştirelim
                    foreach (DetayRepo rep in detaylar)
                    {
                        rep.cust_id = musteri_id;
                    }
                    Session["alimdetay"] = detaylar;
                }
            }

        }



        protected void grdCihaz_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdCihaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                CihazAraa(dc);
            }

        }

        protected void grdDetay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {

                    if (Session["alimdetay"] != null)
                    {
                        List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];

                        int id = Convert.ToInt32(e.CommandArgument);

                        DetayRepo d = detaylar.FirstOrDefault(x => x.cihaz_id == id);
                        detaylar.Remove(d);
                        Session["alimdetay"] = detaylar;
                        DetayGoster();
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }


                }


            }
        }

        protected void grdDetay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnDetayEkle_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
        }

        protected void btnDetayKaydet_ClickEski(object sender, EventArgs e)
        {
            if (grdCihaz.SelectedValue != null && GridView1.SelectedValue != null)
            {
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);
                int cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                int adet = Int32.Parse(txtAdet.Text);
                //buradaki oranı alarakkdv miktarını hesaplayalım
                //ve yekunu de buna göre hesaplayalım
                decimal kdvOran = decimal.Parse(txtKdv.Text) / 100;
                decimal tutar = decimal.Parse(txtTutar.Text);
                decimal kdv = tutar * kdvOran;

                decimal yekun = tutar + kdv;

                string ad = grdCihaz.SelectedRow.Cells[2].Text;

                List<DetayRepo> detaylar = new List<DetayRepo>();
                if (Session["alimdetay"] != null)
                {
                    detaylar = (List<DetayRepo>)Session["alimdetay"];
                }

                DetayRepo detay = new DetayRepo();
                detay.aciklama = txtDetayAciklama.Text;
                detay.adet = adet;
                detay.alim_id = 0;
                detay.cihaz_adi = ad;
                detay.cihaz_id = cihaz_id;
                detay.cust_id = musteri_id;
                detay.kdv = kdv;
                detay.tutar = tutar;
                detay.yekun = yekun;

                detaylar.Add(detay);
                Session["alimdetay"] = detaylar;
                DetayGoster();
                upBilgi.Update();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kalem Eklendi!');");
                sb.Append("$('#detayModal').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Lütfen önce kişi ve cihaz seçiniz!');");

                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
            }
        }
        protected void btnDetayKaydet_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedValue != null)
            {
                int? cihaz_id = null;
                int musteri_id = Convert.ToInt32(GridView1.SelectedValue);
                decimal kdvOran = decimal.Parse(txtKdv.Text) / 100;
                decimal tutar = decimal.Parse(txtTutar.Text);
                //buradaki oranı alarakkdv miktarını hesaplayalım
                //ve yekunu de buna göre hesaplayalım
                decimal kdv = tutar * kdvOran;
                decimal yekun = tutar + kdv;
                int adet = Int32.Parse(txtAdet.Text);
                string ad = "";

                List<DetayRepo> detaylar = new List<DetayRepo>();
                if (Session["alimdetay"] != null)
                {
                    detaylar = (List<DetayRepo>)Session["alimdetay"];
                }

                if (grdCihaz.SelectedValue != null)
                {

                    cihaz_id = Convert.ToInt32(grdCihaz.SelectedValue);
                    ad = grdCihaz.SelectedRow.Cells[2].Text;

                }
                DetayRepo detay = new DetayRepo();
                detay.aciklama = txtDetayAciklama.Text;
                detay.adet = adet;
                detay.alim_id = 0;
                detay.cihaz_adi = ad;
                detay.cihaz_id = cihaz_id;
                detay.cust_id = musteri_id;
                detay.kdv = kdv;
                detay.tutar = tutar;
                detay.yekun = yekun;

                detaylar.Add(detay);
                Session["alimdetay"] = detaylar;
                DetayGoster();
                upBilgi.Update();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kalem Eklendi!');");
                sb.Append("$('#detayModal').modal('hide');");
                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);

            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Lütfen önce kişi seçiniz!');");

                sb.Append(@"</script>");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetayShowModalScript", sb.ToString(), false);
            }

        }
        protected void btnCihazKaydet_Click(object sender, EventArgs e)
        {
            string ad = cihaz_adi.Text;
            string acik = aciklama.Text;
            int sure = Int32.Parse(garanti_suresi.Text);
            int grupid = Int32.Parse(drdGrup.SelectedValue);
            string bar = barkod.Text;

            string firma = KullaniciIslem.firma();
            using (radiusEntities dc = MyContext.Context(firma))
            {
                CihazMalzeme m = new CihazMalzeme(dc);
                m.Yeni(ad, acik, sure, grupid, bar);
                CihazAraa(dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Cihaz tanımlandı!');");
            sb.Append("$('#cihazModal').modal('hide');");

            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);


        }

        protected void btnYeniCihaz_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#detayModal').modal('hide');");
            sb.Append("$('#cihazModal').modal('show');");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CihazShowModalScript", sb.ToString(), false);
        }

        protected void btnAlimKaydet_Click(object sender, EventArgs e)
        {
            if (GridView1.SelectedValue != null)
            {

                int custid = Int32.Parse(GridView1.SelectedValue.ToString());

                DateTime islem_tarih = DateTime.Now;
                string tars = tarih2.Value;

                if (!String.IsNullOrEmpty(tars))
                {
                    islem_tarih = DateTime.Parse(tars);
                }


                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    SatinAlim a = new SatinAlim(dc);
                    string aciklama = txtAciklama.Text;
                    string konu = txtKonu.Text;

                    if (Session["alimdetay"] != null)
                    {
                        List<DetayRepo> detaylar = (List<DetayRepo>)Session["alimdetay"];
                        a.detay = detaylar;

                        if (String.IsNullOrEmpty(aciklama))
                        {
                            string kalemler = "";
                            foreach (var d in detaylar)
                            {
                                if (d.cihaz_id != null)
                                {
                                    kalemler += " " + d.adet.ToString() + " adet " + d.cihaz_adi + "-";
                                }
                                else
                                {
                                    kalemler += " " + d.adet.ToString() + " adet " + d.aciklama + "-";
                                }

                            }
                            if (String.IsNullOrEmpty(aciklama))
                            {
                                aciklama = kalemler;
                            }

                        }

                    }


                    AlimRepo hesap = new AlimRepo();


                    hesap.aciklama = aciklama;
                    hesap.konu = konu;
                    hesap.alim_tarih = islem_tarih;
                    hesap.belge_no = txtBelgeNo.Text;
                    hesap.CustID = custid;


                    string kdvS = toplam_kdv2.Text;
                    string tutarS = toplam_tutar2.Text;
                    string yekunS = toplam_yekun2.Text;

                    if (!String.IsNullOrEmpty(kdvS))
                    {
                        hesap.toplam_kdv = Decimal.Parse(kdvS);
                    }
                    else
                    {
                        hesap.toplam_kdv = Decimal.Parse(toplam_kdv.Text);
                    }
                    if (!String.IsNullOrEmpty(tutarS))
                    {
                        hesap.toplam_tutar = Decimal.Parse(tutarS);
                    }
                    else
                    {
                        hesap.toplam_tutar = Decimal.Parse(toplam_tutar.Text);
                    }

                    if (!String.IsNullOrEmpty(yekunS))
                    {
                        hesap.toplam_yekun = decimal.Parse(yekunS);
                    }
                    else
                    {
                        hesap.toplam_yekun = decimal.Parse(toplam_yekun.Text);
                    }


                    a.hesap = hesap;


                    string s = a.alim_kaydet(User.Identity.Name);
                }

                Session["alimdetay"] = null;

                Response.Redirect("/TeknikAlim/Alimlar");

            }
        }
    }
}