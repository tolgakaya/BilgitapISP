using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using ServisDAL;
using TeknikServis.Radius;

namespace TeknikServis.TeknikTeknik
{
    public partial class ServisMaliyetler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }

            if (!IsPostBack)
            {
                ViewState["sayfa"] = 1;
                GosterSayfaliView(1);

            }


        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            GosterSayfaliAra();

        }

        private void Goster()
        {
            string basS = datetimepicker6.Value;

            string kritik = drdKritik.SelectedValue;

            string tarmirci = Request.QueryString["tamirci"];
            if (!String.IsNullOrEmpty(tarmirci))
            {
                int id = Int32.Parse(tarmirci);

                DateTime? bas = null;
                if (!String.IsNullOrEmpty(basS))
                {
                    bas = DateTime.Parse(basS);
                }
                bool? kapanma = null;
                if (kritik.Equals("acik"))
                {
                    kapanma = false;
                }
                else if (kritik.Equals("tamam"))
                {
                    kapanma = true;
                }
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    ServisIslemleri s = new ServisIslemleri(dc);
                    List<ServisDAL.Repo.ServisRepo> liste = s.servisTamirciRapor(id, kapanma, bas);
                    int adet = liste.Count;
                    decimal maliyet = 0;
                    decimal yekun = 0;
                    decimal fark = 0;
                    if (adet > 0)
                    {
                        maliyet = liste.Sum(x => x.maliyet);
                        yekun = liste.Sum(x => x.yekun);
                        fark = yekun - maliyet;
                    }
                    txtHesapAdet.InnerHtml = "Adet:" + adet.ToString();
                    txtHesapFark.InnerHtml = "Fark: " + fark.ToString("C");
                    txtHesapMaliyet.InnerHtml = "Maliyet: " + maliyet.ToString("C");
                    txtHesapYekun.InnerHtml = "Tutar: " + yekun.ToString("C");
                    Repeater1.DataSource = liste;
                    Repeater1.DataBind();
                }

            }
        }

        private void Rapor()
        {
            string basS = datetimepicker6.Value;

            string kritik = drdKritik.SelectedValue;

            string tarmirci = Request.QueryString["tamirci"];

            DateTime? bas = null;
            if (!String.IsNullOrEmpty(basS))
            {
                bas = DateTime.Parse(basS);
            }
            bool? kapanma = null;
            if (kritik.Equals("acik"))
            {
                kapanma = false;
            }
            else if (kritik.Equals("tamam"))
            {
                kapanma = true;
            }
            //using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            //{
            radiusEntities dc = MyContext.Context(KullaniciIslem.firma());
            ServisIslemleri s = new ServisIslemleri(dc);
            List<ServisDAL.Repo.ServisRepo> liste = new List<ServisDAL.Repo.ServisRepo>();
            if (!String.IsNullOrEmpty(tarmirci))
            {
                int id = Int32.Parse(tarmirci);
                liste = s.servisTamirciRapor(id, kapanma, bas);

            }
            else
            {
                liste = s.servisRapor(kapanma, bas);
            }

            maliyet mal = new maliyet();
            mal.servis_listesi = liste;
            int adet = liste.Count;
            decimal maliyet = 0;
            decimal yekun = 0;
            decimal fark = 0;
            if (adet > 0)
            {
                maliyet = liste.Sum(x => x.maliyet);
                yekun = liste.Sum(x => x.yekun);
                fark = yekun - maliyet;
            }
            mal.toplam_fark = fark;
            mal.toplam_maliyet = maliyet;
            mal.toplam_tutar = yekun;
            mal.adet = adet;
            mal.basTarih = bas == null ? DateTime.Now.AddYears(-1) : (DateTime)bas;

            Session["servis_maliyet"] = mal;
            Response.Redirect("/Baski.aspx?tip=servis_maliyet");
            //}

        }

        private void GosterSayfaliView(int no)
        {
            string basS = datetimepicker6.Value;

            string kritik = drdKritik.SelectedValue;

            string tarmirci = Request.QueryString["tamirci"];

            DateTime? bas = null;
            if (!String.IsNullOrEmpty(basS))
            {
                bas = DateTime.Parse(basS);
            }
            bool kapanma = false;
            if (kritik.Equals("acik"))
            {
                kapanma = false;
            }
            else if (kritik.Equals("tamam"))
            {
                kapanma = true;
            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                ServisIslemleri s = new ServisIslemleri(dc);
                //List<ServisDAL.Repo.ServisRepo> liste = s.servisTamirci(id, kapanma, bas);
                int perpage = 2;
                if (!string.IsNullOrEmpty(txtSayfalama.Text))
                {
                    perpage = Int32.Parse(txtSayfalama.Text);
                }
                sayfali sayfa = new sayfali();
                if (!String.IsNullOrEmpty(tarmirci))
                {
                    int id = Int32.Parse(tarmirci);
                    sayfa = s.servisTamirciSayfali(id, kapanma, no, perpage, bas);
                }
                else
                {
                    sayfa = s.servisSayfali(kapanma, no, perpage, bas);
                }

                List<ServisDAL.Repo.ServisRepo> liste = sayfa.servis_listesi;
                int kayit_sayisi = sayfa.kayit_sayisi;

                //double sayfa_sayisi = 1.5;
                double sayfa_sayisi = (double)kayit_sayisi / (double)perpage;
                //double nok = (double)no;
                if (no < sayfa_sayisi)
                {
                    btnIleri.Visible = true;

                }
                else
                {
                    btnIleri.Visible = false;
                }
                if (no <= 1)
                {
                    btnGeri.Visible = false;
                }
                else
                {
                    btnGeri.Visible = true;
                }

                //view_.Text = "sayfa sayısı: " + sayfa_sayisi.ToString() + " no: " + no.ToString() +" kayıt: "+kayit_sayisi.ToString();
                int adet = liste.Count;
                decimal maliyet = 0;
                decimal yekun = 0;
                decimal fark = 0;
                if (adet > 0)
                {
                    maliyet = liste.Sum(x => x.maliyet);
                    yekun = liste.Sum(x => x.yekun);
                    fark = yekun - maliyet;
                }
                txtHesapAdet.InnerHtml = "Adet:" + adet.ToString();
                txtHesapFark.InnerHtml = "Fark: " + fark.ToString("C");
                txtHesapMaliyet.InnerHtml = "Maliyet: " + maliyet.ToString("C");
                txtHesapYekun.InnerHtml = "Tutar: " + yekun.ToString("C");
                Repeater1.DataSource = liste;
                Repeater1.DataBind();
            }


        }

        private void GosterSayfaliAra()
        {
            ViewState["sayfa"] = 1;
            string basS = datetimepicker6.Value;
            string kritik = drdKritik.SelectedValue;
            string tarmirci = Request.QueryString["tamirci"];

            DateTime? bas = null;
            if (!String.IsNullOrEmpty(basS))
            {
                bas = DateTime.Parse(basS);
            }
            bool kapanma = false;
            if (kritik.Equals("acik"))
            {
                kapanma = false;
            }
            else if (kritik.Equals("tamam"))
            {
                kapanma = true;
            }
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                //List<ServisDAL.Repo.ServisRepo> liste = s.servisTamirci(id, kapanma, bas);
                int perpage = 2;
                if (!string.IsNullOrEmpty(txtSayfalama.Text))
                {
                    perpage = Int32.Parse(txtSayfalama.Text);
                }
                sayfali sayfa = new sayfali();
                if (!String.IsNullOrEmpty(tarmirci))
                {
                    int id = Int32.Parse(tarmirci);
                    sayfa = s.servisTamirciSayfali(id, kapanma, 1, perpage, bas);
                }
                else
                {
                    sayfa = s.servisSayfali(kapanma, 1, perpage, bas);
                }

                List<ServisDAL.Repo.ServisRepo> liste = sayfa.servis_listesi;
                int kayit_sayisi = sayfa.kayit_sayisi;




                //double sayfa_sayisi = 1.5;
                double sayfa_sayisi = (double)kayit_sayisi / (double)perpage;
                //double nok = (double)no;
                if (1 < sayfa_sayisi)
                {
                    btnIleri.Visible = true;

                }
                else
                {
                    btnIleri.Visible = false;
                }

                btnGeri.Visible = false;

                //view_.Text = "sayfa sayısı: " + sayfa_sayisi.ToString() + " no: " + 1.ToString() + " kayıt: " + kayit_sayisi.ToString();


                int adet = liste.Count;
                decimal maliyet = 0;
                decimal yekun = 0;
                decimal fark = 0;
                if (adet > 0)
                {
                    maliyet = liste.Sum(x => x.maliyet);
                    yekun = liste.Sum(x => x.yekun);
                    fark = yekun - maliyet;
                }
                txtHesapAdet.InnerHtml = "Adet:" + adet.ToString();
                txtHesapFark.InnerHtml = "Fark: " + fark.ToString("C");
                txtHesapMaliyet.InnerHtml = "Maliyet: " + maliyet.ToString("C");
                txtHesapYekun.InnerHtml = "Tutar: " + yekun.ToString("C");
                Repeater1.DataSource = liste;
                Repeater1.DataBind();
            }


        }

        protected void btnServis_Command(object sender, CommandEventArgs e)
        {

        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //int no = 1;
            //if (ViewState["sayfa"] != null)
            //{
            //    string sayfano = ViewState["sayfa"].ToString();

            //     no = Int32.Parse(sayfano);


            //}
            //GosterSayfaliView(no);
            //if (e.CommandName == "musteri")
            //{

            //    string id = e.CommandArgument.ToString();
            //    string path = "/MusteriDetayBilgileri.aspx?custid=" + id;
            //    Response.Redirect(path);
            //}
            //if (e.CommandName == "gonder")
            //{
            //    //GosterSayfaliView(no);
            //    string[] arg = new string[2];
            //    arg = e.CommandArgument.ToString().Split(';');

            //    string servisid = arg[0];
            //    string kimlik = arg[1];

            //    string path = "/TeknikTeknik/ServisDetayList.aspx?servisid=" + servisid + "&kimlik=" + kimlik;
            //    Response.Redirect(path);
            //}
        }

        protected void btnGeri_Click(object sender, EventArgs e)
        {


            if (ViewState["sayfa"] != null)
            {
                string sayfano = ViewState["sayfa"].ToString();

                int no = Int32.Parse(sayfano);
                no = no - 1;
                ViewState["sayfa"] = no.ToString();
                GosterSayfaliView(no);
            }

            //if (ViewState["sayfa"] != null)
            //{
            //    view_.Text = ViewState["sayfa"].ToString();
            //}


        }

        protected void btnIleri_Click(object sender, EventArgs e)
        {


            if (ViewState["sayfa"] != null)
            {
                string sayfano = ViewState["sayfa"].ToString();

                int no = Int32.Parse(sayfano);

                no = no + 1;
                ViewState["sayfa"] = no.ToString();
                GosterSayfaliView(no);
            }

            //if (ViewState["sayfa"] != null)
            //{
            //    view_.Text = ViewState["sayfa"].ToString();
            //}


        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btnMusteri = (LinkButton)e.Item.FindControl("btnMusteri");
                btnMusteri.PostBackUrl = "/MusteriDetayBilgileri.aspx?custid=" + btnMusteri.CommandArgument;

                LinkButton btnUsta = (LinkButton)e.Item.FindControl("btnUsta");
                if (!String.IsNullOrEmpty(btnUsta.CommandArgument))
                {
                    btnUsta.Visible = true;
                    btnUsta.PostBackUrl = "/MusteriDetayBilgileri.aspx?custid=" + btnUsta.CommandArgument;
                }
                else
                {
                    btnUsta.Visible = false;
                }



                LinkButton btnServis = (LinkButton)e.Item.FindControl("btnServis");
                string[] arg = new string[2];
                arg = btnServis.CommandArgument.ToString().Split(';');

                string servisid = arg[0];
                string kimlik = arg[1];
                string custid = arg[2];
                string path = "/TeknikTeknik/Servis.aspx?servisid=" + servisid + "&kimlik=" + kimlik + "&&custid=" + custid;
                btnServis.PostBackUrl = path;
            }
        }

        protected void btnRapor_Click(object sender, EventArgs e)
        {
            Rapor();
        }

    }

}