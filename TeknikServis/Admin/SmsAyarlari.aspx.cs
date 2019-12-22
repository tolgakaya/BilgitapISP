using ServisDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis.Admin
{
    public partial class SmsAyarlari : System.Web.UI.Page
    {

        string firma;
        public SmsAyarlari()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            else
            {
                firma = KullaniciIslem.firma();

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris.aspx");
            }

            if (!IsPostBack)
            {
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    AyarIslemleri islem = new AyarIslemleri(dc);

                    drdDurum.AppendDataBoundItems = true;
                    drdDurum.DataSource = islem.durumListesiR();
                    drdDurum.DataTextField = "Durum";
                    drdDurum.DataValueField = "Durum_ID";
                    drdDurum.DataBind();
                }

            }

        }

        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                kaydet();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(" alertify.success('Ayar kaydedildi!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript3", sb.ToString(), false);

            }
            catch (DbEntityValidationException ex)
            {
                Dictionary<string, string> mesajlar = new Dictionary<string, string>();
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        string propName = err.PropertyName;
                        string errMess = err.ErrorMessage;
                        mesajlar.Add(propName, errMess);
                    }
                }
                Session["mesaj"] = mesajlar;
                Response.Redirect("/sonuc.aspx");
            }


        }

        private void kaydet()
        {
            if (drdDurum.SelectedValue != null)
            {
                string secilen = drdDurum.SelectedValue;


                if (secilen.Equals("yaklasan_taksit"))
                {
                    using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
                    {
                        Radius.sms_ayars ayarimiz = dc.sms_ayars.Where(x => x.tur == secilen).FirstOrDefault();
                        if (ayarimiz == null)
                        {
                            Radius.sms_ayars ayar = new Radius.sms_ayars();
                            ayar.tur = secilen;
                            ayar.Firma = firma;
                            ayar.gonderen = txtGonderen.Text;
                            ayar.iliski_id = -1;
                            ayar.mesaj = txtMesaj.Text;

                            ayar.kritik_gun = 1;
                            if (chcAktif.Checked == true)
                            {
                                ayar.aktif = chcAktif.Checked;
                            }
                            dc.sms_ayars.Add(ayar);
                        }
                        else
                        {
                            ayarimiz.gonderen = txtGonderen.Text;
                            ayarimiz.iliski_id = -1;
                            ayarimiz.mesaj = txtMesaj.Text;
                            if (chcAktif.Checked == true)
                            {
                                ayarimiz.aktif = chcAktif.Checked;
                            }

                        }

                        dc.SaveChanges();
                    }
                }
                else
                {

                    int id = Convert.ToInt32(drdDurum.SelectedValue);
                    using (radiusEntities dc = MyContext.Context(firma))
                    {
                        Radius.sms_ayars ayarimiz = dc.sms_ayars.Where(x =>  x.iliski_id == id).FirstOrDefault();

                        if (ayarimiz == null)
                        {
                            Radius.sms_ayars ayar = new Radius.sms_ayars();
                            ayar.tur = "durum";
                            ayar.Firma = firma;
                            ayar.gonderen = txtGonderen.Text;
                            ayar.iliski_id = id;
                            ayar.mesaj = txtMesaj.Text;

                            ayar.kritik_gun = 1;
                            if (chcAktif.Checked == true)
                            {
                                ayar.aktif = chcAktif.Checked;
                            }
                            dc.sms_ayars.Add(ayar);
                        }
                        else
                        {

                            ayarimiz.gonderen = txtGonderen.Text;
                            ayarimiz.iliski_id = id;
                            ayarimiz.mesaj = txtMesaj.Text;
                            if (chcAktif.Checked == true)
                            {
                                ayarimiz.aktif = chcAktif.Checked;
                            }

                        }

                        dc.SaveChanges();
                    }

                }

            }
        }



        private void goster()
        {
            if (drdDurum.SelectedValue != null)
            {
                string secilen = drdDurum.SelectedValue;


                string gonderen = "";
                string mesaj = "";

                if (secilen.Equals("yaklasan_taksit"))
                {
                    using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
                    {
                        Radius.sms_ayars ayar = dc.sms_ayars.Where(x =>  x.tur == secilen).FirstOrDefault();

                        if (ayar != null)
                        {
                            gonderen = ayar.gonderen;
                            mesaj = ayar.mesaj;
                            if (ayar.aktif == true)
                            {
                                chcAktif.Checked = true;
                            }
                            else
                            {
                                chcAktif.Checked = false;
                            }
                        }
                    }


                }
                else if (secilen.Equals("sec"))
                {
                    chcAktif.Checked = false;
                }
                else
                {

                    int id = Convert.ToInt32(drdDurum.SelectedValue);
                    using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
                    {
                        Radius.sms_ayars ayar = dc.sms_ayars.Where(x =>  x.iliski_id == id).FirstOrDefault();
                        if (ayar != null)
                        {
                            gonderen = ayar.gonderen;
                            mesaj = ayar.mesaj;
                            if (ayar.aktif == true)
                            {
                                chcAktif.Checked = true;
                            }
                            else
                            {
                                chcAktif.Checked = false;
                            }
                        }
                    }

                }
                txtMesaj.Text = mesaj;
                txtGonderen.Text = gonderen;


            }
        }

        protected void drdDurum_SelectedIndexChanged(object sender, EventArgs e)
        {

            goster();
        }
    }
}