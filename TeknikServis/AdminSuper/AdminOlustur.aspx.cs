using System;
using System.Linq;
using System.Web;
using TeknikServis.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace TeknikServis.AdminSuper
{
    public partial class AdminOlustur : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("canEdit"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            if (!IsPostBack)
            {
                using (firmaEntities dc = new firmaEntities())
                {
                    var siradaki = dc.firmas.Where(x => String.IsNullOrEmpty(x.firma_kod)).OrderBy(x => x.id).FirstOrDefault();
                    if (siradaki != null)
                    {
                        txtConfig.Text = siradaki.config;
                    }
                }
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {


            //ilk admini oluştur
            //ayargeneli kaydet
            //notification


            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string firmamiz = txtConfig.Text;
            string resimYol = "/Uploads/" + txtConfig.Text.ToLower() + ".png";
            var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text, Firma = txtConfig.Text, Adres = Adres.Text, Tel = Telefon.Text, Web = Web.Text, TamFirma = TamFirma.Text, resimYol = resimYol };
            IdentityResult result = manager.Create(user, Password.Text);
            if (!manager.IsInRole(manager.FindByEmail(Email.Text).Id, "Admin"))
            {
                result = manager.AddToRole(manager.FindByEmail(Email.Text).Id, "Admin");
            }
            if (result.Succeeded)
            {
                using (Radius.radiusEntities dc = Radius.MyContext.Context(txtConfig.Text))
                {
                    //ayar geneli kaydet
                    Radius.ayargenel ay = new Radius.ayargenel();
                    ay.adi = TamFirma.Text;
                    ay.adres = Adres.Text;
                    ay.email = Email.Text;
                    ay.fifo = true;
                    ay.lisanstarih = DateTime.Now.AddMonths(Int32.Parse(txtAy.Text));
                    ay.tel = Telefon.Text;
                    ay.web = Web.Text;
                    dc.ayargenels.Add(ay);
                    dc.SaveChanges();

                }
                using (firmaEntities df = new firmaEntities())
                {
                    //firma kaydını oluştur
                    var f = df.firmas.Where(x => x.config == txtConfig.Text).FirstOrDefault();
                    if (f != null)
                    {
                        f.adres = Adres.Text;
                        f.email = Email.Text;
                        f.expiration = DateTime.Now.AddMonths(Int32.Parse(txtAy.Text));
                        f.firma_kod = Firma.Text;
                        f.firma_tam = TamFirma.Text;
                        f.katilma_tarihi = DateTime.Now;
                        f.web = Web.Text;
                        f.tel = Telefon.Text;
                        f.yenileme_tarihi = DateTime.Now;
                        df.SaveChanges();
                    }
                }

                Response.Redirect("/AdminSuper/Firmalar");

            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}