using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.Admin
{
    public partial class BayiAyarlari : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }

            LogoYukle();
            if (!IsPostBack)
            {

                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    var ay = dc.ayargenels.FirstOrDefault();
                    if (ay != null)
                    {
                        FirmaTam.Text = ay.adi;
                        Adres.Text = ay.adres;
                        Email.Text = ay.email;
                        Tel.Text = ay.tel;

                        if (ay.sonfatura != null)
                        {
                            tarih2.Value = ay.sonfatura.ToString();
                        }
                        txtUrl.Text = ay.web;
                        if (ay.cift_taraf == true)
                        {
                            chcCiftTaraf.Checked = true;
                        }

                    }
                }


            }

        }


        private void LogoYukle()
        {
            //dosya türü, boyutu kontrolü yapılacak
            foreach (string s in Request.Files)
            {
                string path = HttpContext.Current.Server.MapPath("/Uploads/");
                HttpPostedFile file = Request.Files[s];
                int fileSizeInBytes = file.ContentLength;


                string firma = KullaniciIslem.firma();
                string isim = firma + ".png";
                string fileNameWitPath = path + isim;
                file.SaveAs(fileNameWitPath);
            }
        }


        protected void btnKaydet_Click(object sender, EventArgs e)
        {

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                var ay = dc.ayargenels.FirstOrDefault();
                if (ay != null)
                {
                    //güncelle
                    ay.adi = FirmaTam.Text;
                    ay.adres = Adres.Text;
                    ay.email = Email.Text;
                    ay.fifo = true;
                    ay.tel = Tel.Text;
                    ay.cift_taraf = chcCiftTaraf.Checked;

                    //ay.lisanstarih = null;
                    if (!String.IsNullOrEmpty(tarih2.Value))
                    {
                        ay.sonfatura = DateTime.Parse(tarih2.Value);
                    }
                    ay.web = txtUrl.Text;

                    ServisDAL.KaydetmeIslemleri.kaydetR(dc);
                    HttpContext.Current.Cache.Remove("config");
                    HttpContext.Current.Cache.Add("config", ay, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.High, null);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Ayar kaydedildi!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }
                else
                {
                    ayargenel ayy = new ayargenel();
                    ayy.adi = FirmaTam.Text;
                    ayy.adres = Adres.Text;
                    ayy.email = Email.Text;
                    ayy.tel = Tel.Text;
                    ayy.fifo = true;
                    //ayy.lisanstarih = null;
                    if (!String.IsNullOrEmpty(tarih2.Value))
                    {
                        ayy.sonfatura = DateTime.Parse(tarih2.Value);
                    }
                    ayy.web = txtUrl.Text;
                    dc.ayargenels.Add(ayy);
                    ServisDAL.KaydetmeIslemleri.kaydetR(dc);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Ayar kaydedildi!');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                }


            }
        }
    }
}