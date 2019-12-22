using ServisDAL;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class CariHesaplar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || User.IsInRole("servis") )
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
                    goster(dc);
                }
              
            }


        }

        private void goster(radiusEntities dc)
        {
            string s = txtAra.Value;
            string firma = KullaniciIslem.firma();
            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
            {

                GridView1.DataSource = m.musteriAraCari(s);
                GridView1.DataBind();
            }


        }
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;


            if (!String.IsNullOrEmpty(s) && !String.IsNullOrWhiteSpace(s))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    MusteriIslemleri m = new MusteriIslemleri(dc);
                    GridView1.DataSource = m.musteriAraCari(s); //m.musteriAraR2(s, kullanici.UserName);
                    GridView1.DataBind();
                }


            }
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnOdemeler") as LinkButton).PostBackUrl = "~/TeknikCari/Odemeler.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikCari/CariDetay.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnTahsilat") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnOde") as LinkButton).PostBackUrl = "~/TeknikCari/Ode.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                //(e.Row.FindControl("btnHesaplar") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisHesaplar.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");

                //(e.Row.FindControl("btnOdemelerK") as LinkButton).PostBackUrl = "~/TeknikCari/Odemeler.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                //(e.Row.FindControl("btnDetayK") as LinkButton).PostBackUrl = "~/TeknikCari/CariDetay.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                (e.Row.FindControl("btnOdeK") as LinkButton).PostBackUrl = "~/TeknikCari/OdemeEkle.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");
                //(e.Row.FindControl("btnHesaplarK") as LinkButton).PostBackUrl = "~/TeknikTeknik/ServisHesaplar.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");

            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("detail"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                // int code = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
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
            else if (e.CommandName.Equals("cari"))
            {
                int code = Convert.ToInt32(e.CommandArgument);
                hdnCariCustID.Value = code.ToString();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#cariModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CariModalScript", sb.ToString(), false);
            }
        }
        private void cariAra(int musID, radiusEntities dc)
        {

            Hareket ser = new Hareket(dc);

            TeknikServis.Radius.carihesap hesap = ser.tekCariR(musID);


            Session["borc"] = hesap.ToplamBakiye.ToString();


            decimal bor = hesap.NetBorc;
            decimal al = hesap.ToplamBakiye;
            decimal alacak_mahsup = hesap.NetAlacak;
            if (al < 0)
            {
                bor = 0;
                //alacağı var ise mahsup miktarı net borcu kadardır
                Session["alacak_mahsup"] = hesap.NetBorc.ToString();
                al = -al;
                txtBakiye.InnerHtml = "Bakiye :" + al.ToString() + "-Alacaklı";
            }
            else
            {
                //borcu varsa mahsup miktarı net alacağı kadardır
                Session["alacak_mahsup"] = hesap.NetAlacak.ToString();
                txtBakiye.InnerHtml = "Bakiye :" + al.ToString() + "-Borçlu";
            }
            Session["alacak"] = al.ToString();

            txtBorc.InnerHtml = "Net Borç :" + hesap.NetBorc.ToString();
            txtOdenen.InnerHtml = "Net Alacak :" + hesap.NetAlacak.ToString();


        }
        private void CariDetay(int musID, radiusEntities dc)
        {
            Hareket ser = new Hareket(dc);

            GridView2.DataSource = ser.CariDetayREski(musID, 5000);
            GridView2.DataBind();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            cariGoster();
        }

        private void cariGoster()
        {
            int id = Convert.ToInt32(GridView1.SelectedValue);

            cariOzet.Visible = true;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                cariAra(id, dc);
                CariDetay(id, dc);
            }

        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            cariGoster();
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                goster(dc);
            }
        }
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string borc = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "borc")).Trim();
                string alacak = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "alacak")).Trim();

                if (!String.IsNullOrEmpty(borc))
                {
                    if (borc.Equals("0"))
                    {
                        e.Row.CssClass = "danger";
                    }
                    else
                    {
                        e.Row.CssClass = "info";
                    }

                }

            }
        }

        protected void GridView2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                if (!User.IsInRole("Admin"))
                {
                    (e.Row.FindControl("btnCariGuncel") as LinkButton).Visible = false;
                }
            }
        }

        protected void btnCariKaydet_Click(object sender, EventArgs e)
        {
            string idd = hdnCariCustID.Value;
            string bak = txtCariBakiye.Text;
            bool borclu = false;
            if (chcBorclu.Checked == true)
            {
                borclu = true;
            }
            if (!String.IsNullOrEmpty(idd) && !String.IsNullOrEmpty(bak))
            {
                int id = Int32.Parse(idd);
                decimal bakiyee = Decimal.Parse(bak);

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    Kurulum k = new Kurulum(dc);
                    k.CariGuncelle(id, bakiyee, borclu,User.Identity.Name);
                }


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#cariModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CariModalScript", sb.ToString(), false);

            }


        }
    }
}