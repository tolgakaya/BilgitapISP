using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using TeknikServis.Models;

using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
using System.Collections.Generic;
namespace TeknikServis.Admin
{
    public partial class Kullanicilar1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            if (!IsPostBack)
            {
                //drdGrup.AppendDataBoundItems = true;

                drdRol.DataSource = KullaniciIslem.Roller();
                drdRol.DataValueField = "Id";
                drdRol.DataTextField = "Name";
                drdRol.DataBind();

                drdRoll.DataSource = KullaniciIslem.Roller();
                drdRoll.DataValueField = "Id";
                drdRoll.DataTextField = "Name";
                drdRoll.DataBind();

                GridView1.DataSource = KullaniciIslem.KulaniciRoller();
                GridView1.DataBind();

            }


        }
        public void MusteriAra(object sender, EventArgs e)
        {
            string s = txtAra.Value;

            GridView1.DataSource = KullaniciIslem.KulaniciRoller(s);
            GridView1.DataBind();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            string id = hdnID.Value;
            string eposta = txtEposta.Text;
            string kullanici = txtKullanici.Text;
            string yeniSif = txtYeniSifre.Text;
            string eskiRol = hdnRolID.Value.Trim();
            string yeniRol = drdRol.SelectedItem.Text;

            IdentityResult sonuc = new IdentityResult();
            if (!String.IsNullOrEmpty(yeniSif))
            {
                sonuc = KullaniciIslem.updateKullanici(id, kullanici, eposta, yeniSif);
            }
            else
            {
                sonuc = KullaniciIslem.updateKullanici(id, kullanici, eposta);
            }

            if (!eskiRol.Equals(yeniRol))
            {
                sonuc = KullaniciIslem.updateRole(id, yeniRol, eskiRol);
            }

            if (sonuc.Succeeded)
            {
                GridView1.DataSource = KullaniciIslem.KulaniciRoller();
                GridView1.DataBind();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt güncellendi!');");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
            }
            else
            {
                //sb.Append(" alertify.error('" + deger + "');");
                string hata = sonuc.Errors.FirstOrDefault();
                //Session["mesele"] = hata;
                //Response.Redirect("/Sonuc.aspx");
                //string felan = "felan @.com oldu";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Bu email adresi ile daha önceden kayıt yapılmmş!');");
                sb.Append("$('#editModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
            }



        }
        protected void btnDel_Click(object sender, EventArgs e)
        {

            string id = hdnID.Value;

            KullaniciIslem.delKullanici(id);

            GridView1.DataSource = KullaniciIslem.KulaniciRoller();
            GridView1.DataBind();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kayıt silindi!');");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript2", sb.ToString(), false);


        }



        protected void btnAddRecord_Click(object sender, EventArgs e)
        {

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();


            string eposta = txtAddEposta.Text;
            string kullanici = txtAddKullanici.Text;
            string rol = drdRoll.SelectedItem.Text.Trim();

            string id = Page.User.Identity.GetUserId();
            ApplicationUser kullanicimiz = manager.FindById(id);

            string adres = kullanicimiz.Adres;
            string telefon = kullanicimiz.Tel;
            string web = kullanicimiz.Web;
            string TamFirma = kullanicimiz.TamFirma;
            string firma = kullanicimiz.Firma;

            string resimYol = kullanicimiz.resimYol;

            var user = new ApplicationUser() { UserName = kullanici, Email = eposta, Firma = firma, Adres = adres, Tel = telefon, Web = web, TamFirma = TamFirma, resimYol = resimYol };

            //var user = new ApplicationUser() { UserName = kullanici, Email = eposta, Firma = firma };
            IdentityResult result = manager.Create(user, txtTell.Text);
            if (result.Succeeded)
            {

                if (!manager.IsInRole(manager.FindByEmail(eposta).Id, rol))
                {

                    result = manager.AddToRole(manager.FindByEmail(eposta).Id, rol);
                    if (result.Succeeded)
                    {
                        GridView1.DataSource = KullaniciIslem.KulaniciRoller();
                        GridView1.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt yapıldı!');");
                        sb.Append("$('#addModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Rol kaydında sorun oluştu!');");
                        sb.Append("$('#addModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);

                    }


                }

            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.error('Kullanıcı adı ve email adresiyle daha önceden kayıt yapılmış');");
                sb.Append("$('#addModal').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);


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
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                string code2 = e.CommandArgument.ToString().Trim().ToLower();

                DetailsView1.DataSource = KullaniciIslem.firmaKullanicilari(code2);
                DetailsView1.DataBind();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript2", sb.ToString(), false);
            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    string code2 = e.CommandArgument.ToString().Trim().ToLower();
                    string userid = User.Identity.GetUserId();
                    if (code2 != userid)
                    {
                        KullaniciIslem.delKullanici(code2);

                        GridView1.DataSource = KullaniciIslem.KulaniciRoller();
                        GridView1.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kendi hesabınızı silemezssiniz');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                string id = Convert.ToString(arg[0]).Trim();
                int index = Convert.ToInt32(arg[1]);
                string rolid = Convert.ToString(arg[2]).Trim();


                GridViewRow gvrow = GridView1.Rows[index];

                drdRol.SelectedValue = rolid;
                hdnID.Value = id.ToString();
                //hdnRolID.Value = rolid.ToString();
                txtEposta.Text = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);
                txtKullanici.Text = HttpUtility.HtmlDecode(gvrow.Cells[2].Text);
                hdnRolID.Value = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);

                lblResult.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);

            }
        }
    }
}