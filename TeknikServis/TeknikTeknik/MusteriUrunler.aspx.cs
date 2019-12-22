using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.TeknikTeknik
{
    public partial class MusteriUrunler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
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
                    musteriGoster(dc);
                    if (GridView3.SelectedValue != null)
                    {
                        int id = Convert.ToInt32(GridView3.SelectedValue);

                        UrunAra(id, dc);
                    }
                }
              
            }


        }


        private void musteriGoster(radiusEntities dc)
        {

            string s = txtMusteriSorgu.Value;

            MusteriIslemleri m = new MusteriIslemleri(dc);
            if (!String.IsNullOrEmpty(s))
            {
                GridView3.DataSource = m.musteriAraR2(s,"musteri");
                GridView3.DataBind();

            }
        }

        protected void MusteriAra(object sender, EventArgs e)
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                musteriGoster(dc);
            }

        }
        protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridView3.SelectedValue != null)
            {
                int id = Convert.ToInt32(GridView3.SelectedValue);
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    musteriGoster(dc);
                    UrunAra(id, dc);
                }

            }


        }
        private void UrunAra(int musID, radiusEntities dc)
        {

            ServisIslemleri ser = new ServisIslemleri(dc);
            GridView2.DataSource = ser.urunListesiCompactR(musID);
            GridView2.DataBind();
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;

            if (GridView3.SelectedValue != null)
            {
                int id = Convert.ToInt32(GridView3.SelectedValue);
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    musteriGoster(dc);
                    UrunAra(id, dc);
                }

            }
        }
        protected void GridView3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
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
        }
        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {

                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    int urunID = Convert.ToInt32(e.CommandArgument);
                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        ServisIslemleri s = new ServisIslemleri(dc);
                        s.urunIptal(urunID);

                        if (GridView3.SelectedValue != null)
                        {
                            string firma = KullaniciIslem.firma();
                            int id = Convert.ToInt32(GridView3.SelectedValue);
                            musteriGoster(dc);
                            UrunAra(id, dc);
                        }

                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('Kayıt silindi!');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }


                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.error('" + deger + "');");

                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                }


            }
        }
        protected void btnAdd2_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal2').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);

        }
        protected void btnAddRecord2_Click(object sender, EventArgs e)
        {



            string cins = txtUrunCinsi.Text;
            string aciklama = txtUrunAciklama.Text;
            DateTime garantiBas = DateTime.Now;//DateTime.Parse(tarihGaranti.Text);
            int i = Convert.ToInt32(GridView3.SelectedValue);
            string urunKimlik = AletEdavat.KimlikUret(10);
            string imei = txtUrunImei.Text;
            string seri = txtUrunSeriNo.Text;
            string diger = txtUrunDiger.Text;

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri s = new ServisIslemleri(dc);
                s.urunEkleR(i, cins, garantiBas, 24, aciklama, urunKimlik, imei, seri, diger);

                UrunAra(i, dc);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            //sb.Append("alert('Record Added Successfully');");
            sb.Append(" alertify.success('Kayıt Eklendi!');");
            sb.Append("$('#addModal2').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);
        }
    }
}