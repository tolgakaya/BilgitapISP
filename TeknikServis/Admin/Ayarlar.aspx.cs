using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using ServisDAL;
using TeknikServis.Radius;

namespace TeknikServis.Admin
{
    public partial class Ayarlar : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }


            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                if (!IsPostBack)
                {
                    MailAyarGoster(ayarlar);
                    SmsAyarGoster(ayarlar);

                }


                tipGoster(ayarlar);
                durumGoster(ayarlar);
                masrafGoster(ayarlar);

            }


        }

        private void tipGoster(AyarIslemleri ayarlar)
        {
            grdTip.DataSource = ayarlar.tipListesiGrid();
            grdTip.DataBind();
        }
        private void masrafGoster(AyarIslemleri ayarlar)
        {

            grdTipM.DataSource = ayarlar.masrafListesiGrid();
            grdTipM.DataBind();
        }
        private void durumGoster(AyarIslemleri ayarlar)
        {
            GridView1.DataSource = ayarlar.durumListesiR();
            GridView1.DataBind();
        }
        private void MailAyarGoster(AyarIslemleri ayarlar)
        {

            //AyarIslemleri ayarlarimiz = new AyarIslemleri(firma);
            Radius.ayar mail_ayar = ayarlar.MailAyarR();
            if (mail_ayar != null)
            {
                txtMailKimden.Text = mail_ayar.Mail_Kimden;
                txtMailKullanici.Text = mail_ayar.Mail_UserName;
                txtMailPort.Text = mail_ayar.Mail_Port.ToString();
                txtMailServer.Value = mail_ayar.Mail_Server;
                txtMailSifre.Text = mail_ayar.Mail_PW;
                txtMailAktif.Text = mail_ayar.aktif_adres;
            }

        }

        private void SmsAyarGoster(AyarIslemleri ayarlar)
        {

            Radius.ayar mail_ayar = ayarlar.SmsAyarR();
            if (mail_ayar != null)
            {
                txtMailKimden2.Text = mail_ayar.Mail_Kimden;
                txtMailKullanici2.Text = mail_ayar.Mail_UserName;

                drdSaglayici.SelectedValue = mail_ayar.Mail_Server;
                txtMailSifre2.Text = mail_ayar.Mail_PW;
                txtSmsAktif.Text = mail_ayar.aktif_adres;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(lblID.Value);
            string durum = txtDurum.Text;
            bool sms = chcSMS.Checked;
            bool mail = chcMail.Checked;
            bool what = chcWhat.Checked;
            string dur = rdDurum.SelectedValue;

            bool baslangic = false;
            bool son = false;
            bool karar = false;
            bool onay = false;
            if (String.Equals(dur, "baslangic"))
            {
                baslangic = true;
            }
            else if (String.Equals(dur, "son"))
            {
                son = true;
            }
            else if (String.Equals(dur, "karar"))
            {
                karar = true;
            }
            else if (String.Equals(dur, "onay"))
            {
                onay = true;
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                bool oldumu = ayarlar.servisDurumGuncelleR(id, durum, sms, mail, what, baslangic, son, karar, onay);

                if (oldumu == true)
                {
                    durumGoster(ayarlar);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Durum güncelleme yapıldı!');");
                    sb.Append("$('#editModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);
                }
            }



        }

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {

            string dur = rdDurumYeni.SelectedValue;

            string durum = txtAddDurum.Text;
            bool sms = chcAddSMS.Checked;
            bool mail = chcAddMail.Checked;
            bool what = chcAddWhat.Checked;


            bool baslangic = false;
            bool son = false;
            bool karar = false;
            bool onay = false;
            if (String.Equals(dur, "baslangic"))
            {
                baslangic = true;
            }
            else if (String.Equals(dur, "son"))
            {
                son = true;
            }
            else if (String.Equals(dur, "karar"))
            {
                karar = true;
            }
            else if (String.Equals(dur, "onay"))
            {
                onay = true;
            }

            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                bool oldumu = ayarlar.servisDurumEkleR(durum, sms, mail, what, baslangic, son, karar, onay);
                if (oldumu == true)
                {
                    durumGoster(ayarlar);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append(" alertify.success('Kayıt yapıldı!');");
                    // sb.Append(alert);
                    sb.Append("$('#addModal').modal('hide');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);
                }
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

                int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    DetailsView1.DataSource = ayarlar.durumListesiTekliR(code2);
                    DetailsView1.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModal').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript2", sb.ToString(), false);
                }

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.durumSilR(code2); ;

                        GridView1.DataSource = ayarlar.durumListesiR();
                        GridView1.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);
                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = GridView1.Rows[index];

                LinkButton link = gvrow.Cells[2].Controls[1] as LinkButton;


                lblID.Value = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);

                txtDurum.Text = HttpUtility.HtmlDecode(link.Text);


                chcSMS.Checked = (gvrow.Cells[3].Controls[0] as CheckBox).Checked;
                chcMail.Checked = (gvrow.Cells[4].Controls[0] as CheckBox).Checked;
                chcWhat.Checked = (gvrow.Cells[5].Controls[0] as CheckBox).Checked;

                if ((gvrow.Cells[6].Controls[0] as CheckBox).Checked)
                {
                    rdDurum.SelectedValue = "son";
                }
                else if ((gvrow.Cells[7].Controls[0] as CheckBox).Checked)
                {
                    rdDurum.SelectedValue = "baslangic";
                }
                else if ((gvrow.Cells[8].Controls[0] as CheckBox).Checked)
                {
                    rdDurum.SelectedValue = "karar";
                }
                else if ((gvrow.Cells[9].Controls[0] as CheckBox).Checked)
                {
                    rdDurum.SelectedValue = "onay";
                }
                else
                {
                    rdDurum.SelectedIndex = -1;
                }


                lblResult.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);

            }
        }

        protected void btnMailKaydet_Click(object sender, EventArgs e)
        {

            //AyarIslemleri ayarimiz = new AyarIslemleri(firma);
            string serverimiz = txtMailServer.Value;
            string kimden = txtMailKimden.Text;
            int port = Int32.Parse(txtMailPort.Text);
            string username = txtMailKullanici.Text;
            string pw = txtMailSifre.Text;
            string adres = txtMailAktif.Text;
            string aktif = txtMailAktif.Text;
            if (!String.IsNullOrEmpty(serverimiz))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    ayarlar.MailAyarKaydetR(serverimiz, kimden, port, username, pw, aktif);
                }


            }

        }

        protected void btnSmsKaydet_Click(object sender, EventArgs e)
        {
            string saglayici = drdSaglayici.SelectedValue;
            string gonderen = txtMailKimden2.Text;

            string kull = txtMailKullanici2.Text;
            string sifre = txtMailSifre2.Text;
            string aktif = txtSmsAktif.Text;
            if (!String.IsNullOrEmpty(saglayici))
            {
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    ayarlar.SmsAyarKaydetR(saglayici, gonderen, kull, sifre, aktif);
                }


            }

        }
        protected void grdTip_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    DetailsViewTip.DataSource = ayarlar.tipListesiTekliR(code2);
                    DetailsViewTip.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModalTip').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScriptTip", sb.ToString(), false);
                }

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.tipSilR(code2); ;

                        grdTip.DataSource = ayarlar.tipListesiGrid();
                        grdTip.DataBind();

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);
                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = grdTip.Rows[index];

                LinkButton link = gvrow.Cells[2].Controls[1] as LinkButton;

                txtTipID.Value = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);
                txtTipAd.Text = HttpUtility.HtmlDecode(link.Text);
                txtTipAciklama.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
                txtCssGuncelle.Value = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);

                Label2.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModalTip').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScriptTip2", sb.ToString(), false);

            }
        }
        protected void grdTipM_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Equals("detail"))
            {

                int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());
                using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarIslemleri ayarlar = new AyarIslemleri(dc);
                    DetailsViewTipM.DataSource = ayarlar.masrafListesiTekliR(code2);
                    DetailsViewTipM.DataBind();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("$('#detailModalTipM').modal('show');");
                    sb.Append(@"</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScriptTip", sb.ToString(), false);
                }

            }
            else if (e.CommandName.Equals("del"))
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    int code2 = Int32.Parse(e.CommandArgument.ToString().Trim());

                    using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarIslemleri ayarlar = new AyarIslemleri(dc);
                        ayarlar.masrafSilR(code2); ;

                        masrafGoster(ayarlar);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.success('Kayıt silindi!');");

                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
                    }

                }

            }
            else if (e.CommandName.Equals("editRecord"))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvrow = grdTipM.Rows[index];

                LinkButton link = gvrow.Cells[2].Controls[1] as LinkButton;

                hdnTipIDM.Value = HttpUtility.HtmlDecode(gvrow.Cells[1].Text);
                txtTipAdM.Text = HttpUtility.HtmlDecode(link.Text);
                txtTipAciklamaM.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
                txtCssGuncelleM.Value = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);

                Label2.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModalTipM').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScriptTip2M", sb.ToString(), false);

            }
        }

        protected void btnAddTip_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalTip').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2", sb.ToString(), false);
        }
        protected void btnAddTipM_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModalTipM').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScriptTip2M", sb.ToString(), false);
        }

        protected void btnSaveTip_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(txtTipID.Value);
            string ad = txtTipAd.Text;
            string aciklama = txtTipAciklama.Text;
            string css = txtCssGuncelle.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipGuncelleR(id, ad, aciklama, css);

                tipGoster(ayarlar);
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append(" alertify.success('Kaydedildi!');");
            sb.Append("$('#editModalTip').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);

        }
        protected void btnSaveTipM_Click(object sender, EventArgs e)
        {

            int id = Int32.Parse(hdnTipIDM.Value);
            string ad = txtTipAdM.Text;
            string aciklama = txtTipAciklamaM.Text;
            string css = txtCssGuncelleM.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafGuncelleR(id, ad, aciklama, css);

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kaydedildi!');");
                sb.Append("$('#editModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
            }


        }
        protected void btnDelTip_Click(object sender, EventArgs e)
        {
            int code2 = Int32.Parse(txtTipID.Value);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipSilR(code2); ;

                tipGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt silindi!');");
                sb.Append("$('#editModalTip').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTip", sb.ToString(), false);
            }

        }
        protected void btnDelTipM_Click(object sender, EventArgs e)
        {
            int code2 = Int32.Parse(hdnTipIDM.Value);
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafSilR(code2); ;

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt silindi!');");
                sb.Append("$('#editModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScriptTipM", sb.ToString(), false);
            }

        }

        protected void btnAddRecordTip_Click(object sender, EventArgs e)
        {

            //int id = Int32.Parse(txtTipIDGoster.Text);
            string ad = txtTipAdGoster.Text;
            string aciklama = txtTipAciklamaGoster.Text;
            string css = txtCss.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.tipEkleR(ad, aciklama, css);

                tipGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt yapıldı!');");
                // sb.Append(alert);
                sb.Append("$('#addModalTip').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2", sb.ToString(), false);
            }



        }
        protected void btnAddRecordTipM_Click(object sender, EventArgs e)
        {

            //int id = Int32.Parse(txtTipIDGoster.Text);
            string ad = txtTipAdGosterM.Text;
            string aciklama = txtTipAciklamaGosterM.Text;
            string css = txtCssM.Value.Trim();
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarIslemleri ayarlar = new AyarIslemleri(dc);
                ayarlar.masrafEkleR(ad, aciklama, css);

                masrafGoster(ayarlar);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Kayıt yapıldı!');");
                // sb.Append(alert);
                sb.Append("$('#addModalTipM').modal('hide');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScriptTip2M", sb.ToString(), false);
            }



        }



        protected void btnPayPal_Click(object sender, EventArgs e)
        {

        }
    }
}