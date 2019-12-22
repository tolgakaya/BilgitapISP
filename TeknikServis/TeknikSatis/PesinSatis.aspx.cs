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

namespace TeknikServis.TeknikSatis
{
    public partial class PesinSatis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("servis"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
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

                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    pos_banka_look pb = m.posbankalar();

                    drdPos.AppendDataBoundItems = true;
                    drdPos.DataSource = pb.poslar;
                    drdPos.DataValueField = "pos_id";
                    drdPos.DataTextField = "pos_adi";

                    drdPos.DataBind();
                    CihazAraa(dc);
                }
                barkod.Focus();
               
                // detaylara bakalım
                DetayGoster();
            }





        }

        private void DetayGoster()
        {
            if (Session["satisdetay"] != null)
            {

                List<satis_helper> detaylar = (List<satis_helper>)Session["satisdetay"];

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
                //toplam_kdv.Text = kdv.ToString();
                //toplam_tutar.Text = tutar.ToString();
                toplam_yekun.Text = yekun.ToString();

                grdDetay.DataSource = detaylar;
                grdDetay.DataBind();
                //upBilgi.Update();


            }
            else
            {
                grdDetay.DataSource = null;
                grdDetay.DataBind();
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


            string arama_terimi = txtCihazAra.Value;
            if (!String.IsNullOrEmpty(arama_terimi))
            {
                CihazMalzeme c = new CihazMalzeme(dc);
                grdCihaz.DataSource = c.CihazListesi2(arama_terimi);

                grdCihaz.DataBind();
            }

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }
        protected void grdCihaz_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ekle"))
            {
                List<satis_helper> detaylar = new List<satis_helper>();

                if (Session["satisdetay"] != null)
                {
                    detaylar = (List<satis_helper>)Session["satisdetay"];
                }

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                int cihaz_id = Convert.ToInt32(arg[0]);
                int grupid = Convert.ToInt32(arg[2]);
                int index = Convert.ToInt32(arg[1]);
                GridViewRow row = grdCihaz.Rows[index];
                string cihaz_adi = row.Cells[2].Text;
                string stokS = row.Cells[4].Text;
                int stok = 0;
                if (!string.IsNullOrEmpty(stokS))
                {
                    stok = Int32.Parse(stokS);
                }
                string fiyats = row.Cells[5].Text;

                if (stok > 0)
                {
                    //cihaz daha önceden eklenmiş mi bakalım
                    satis_helper c = detaylar.FirstOrDefault(x => x.cihaz_id == cihaz_id);
                    if (c != null)
                    {
                        c.adet++;
                        c.yekun += decimal.Parse(fiyats);
                        c.tutar += decimal.Parse(fiyats);
                    }
                    else
                    {
                        //listeye bu cihazı da ekleyelim
                        satis_helper yeni = new satis_helper();
                        yeni.cihaz_id = cihaz_id;
                        yeni.grupid = grupid;
                        yeni.cihaz_adi = cihaz_adi;
                        yeni.adet = 1;
                        yeni.tutar = decimal.Parse(fiyats);
                        yeni.yekun = decimal.Parse(fiyats);
                        detaylar.Add(yeni);


                    }
                    Session["satisdetay"] = detaylar;

                    DetayGoster();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kayıt eklendi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Sıfır stokla satış yapamazssınız!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript33", sb.ToString(), false);

                }




            }
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

                    if (Session["satisdetay"] != null)
                    {
                        List<satis_helper> detaylar = (List<satis_helper>)Session["satisdetay"];

                        int id = Convert.ToInt32(e.CommandArgument);

                        satis_helper d = detaylar.FirstOrDefault(x => x.cihaz_id == id);
                        detaylar.Remove(d);
                        Session["satisdetay"] = detaylar;
                        DetayGoster();
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }
                    else
                    {
                        List<DetayRepo> detaylar = new List<DetayRepo>();
                        if (Session["alimdetay"] != null)
                        {
                            detaylar = (List<DetayRepo>)Session["alimdetay"];
                        }
                    }


                }


            }
        }
        protected void grdDetay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnAlimKaydet_Click(object sender, EventArgs e)
        {

            DateTime islem_tarih = DateTime.Now;
            //burada hesaplanan yekun değiştirilirse aradaki iskonto her kaleme eşit olarak dağıtılacak
            decimal iskonto_oran = 0.0M;
            if (!string.IsNullOrEmpty(toplam_yekun2.Text))
            {
                decimal asil = Decimal.Parse(toplam_yekun.Text);
                decimal iskontolu = Decimal.Parse(toplam_yekun2.Text);
                decimal fark = asil - iskontolu;
                iskonto_oran = Convert.ToDecimal(fark * 100 / asil);
            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                HizliSatis a = new HizliSatis(dc);
                List<satis_helper> liste = new List<satis_helper>();
                if (Session["satisdetay"] != null)
                {
                    liste = (List<satis_helper>)Session["satisdetay"];
                }

                a.Nakit(liste, iskonto_oran, User.Identity.Name);
            }


            Session["satisdetay"] = null;
            DetayGoster();
            toplam_yekun.Text = "";
            toplam_yekun2.Text = "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.error('Satış kaydedildi!');");

            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);


            // Response.Redirect("/TeknikAlim/Alimlar");

        }
        protected void btnKart_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#onayModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);
        }
        protected void btnKartKaydet_Click(object sender, EventArgs e)
        {

            // string custidd = Request.QueryString["custid"];


            //if (!String.IsNullOrEmpty(custidd))
            //{


            int pos_id = Int32.Parse(drdPos.SelectedValue);
            if (pos_id > -1)
            {

                DateTime islem_tarih = DateTime.Now;

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    HizliSatis a = new HizliSatis(dc);
                    List<satis_helper> liste = new List<satis_helper>();
                    if (Session["satisdetay"] != null)
                    {
                        liste = (List<satis_helper>)Session["satisdetay"];
                    }
                    decimal iskonto_oran = 0.0M;
                    if (!string.IsNullOrEmpty(toplam_yekun2.Text))
                    {
                        decimal asil = Decimal.Parse(toplam_yekun.Text);
                        decimal iskontolu = Decimal.Parse(toplam_yekun2.Text);
                        decimal fark = asil - iskontolu;
                        iskonto_oran = Convert.ToDecimal(fark * 100 / asil);
                    }
                    a.Kart(liste, pos_id, iskonto_oran, User.Identity.Name);
                }

                Session["satisdetay"] = null;
                DetayGoster();
                toplam_yekun.Text = "";
                toplam_yekun2.Text = "";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#onayModal').modal('hide');");
                sb.Append(" alertify.error('Kayıt eklendi!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnayShowModalScript", sb.ToString(), false);


            }
            // }
        }

        protected void barkod_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(barkod.Text))
            {

                string bar = barkod.Text;
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    CihazMalzeme cih = new CihazMalzeme(dc);
                    cihaz_rp rp = cih.CihazBarkod(bar);
                    if (rp != null)
                    {
                        List<satis_helper> detaylar = new List<satis_helper>();

                        if (Session["satisdetay"] != null)
                        {
                            detaylar = (List<satis_helper>)Session["satisdetay"];
                        }

                        int stok = (int)rp.bakiye;
                        int cihaz_id = rp.ID;
                        decimal fiyat = rp.fiyat;
                        int grupid = rp.grupid;
                        string cihaz_adi = rp.cihaz_adi;

                        if (stok > 0)
                        {
                            //cihaz daha önceden eklenmiş mi bakalım
                            satis_helper c = detaylar.FirstOrDefault(x => x.cihaz_id == cihaz_id);
                            if (c != null)
                            {
                                c.adet++;
                                c.yekun += fiyat;
                                c.tutar += fiyat;
                            }
                            else
                            {
                                //listeye bu cihazı da ekleyelim
                                satis_helper yeni = new satis_helper();
                                yeni.cihaz_id = cihaz_id;
                                yeni.grupid = grupid;
                                yeni.cihaz_adi = cihaz_adi;
                                yeni.adet = 1;
                                yeni.tutar = fiyat;
                                yeni.yekun = fiyat;
                                detaylar.Add(yeni);


                            }
                            Session["satisdetay"] = detaylar;

                            DetayGoster();
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.success('Kayıt eklendi!');");

                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                        }
                        else
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.error('Sıfır stokla satış yapamazssınız!');");

                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript33", sb.ToString(), false);

                        }
                    }
                }
                barkod.Text = "";

            }

        }

    }
}