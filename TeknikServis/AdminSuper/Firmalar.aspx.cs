using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Radius;

namespace TeknikServis.AdminSuper
{
    public partial class Firmalar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            using (firmaEntities dc = new firmaEntities())
            {
                Goster(dc);
            }
            //}
        }

        private void Goster(firmaEntities dc)
        {
            string tip = Request.QueryString["tip"];
            if (!string.IsNullOrEmpty(tip))
            {
                if (!string.IsNullOrEmpty(txtAra.Value))
                {
                    if (tip.Equals("aktif"))
                    {
                        GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null && x.firma_kod.Contains(txtAra.Value) && ((DateTime)x.expiration) > DateTime.Now).ToList();
                    }
                    else if (tip.Equals("pasif"))
                    {
                        GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null && x.firma_kod.Contains(txtAra.Value) && ((DateTime)x.expiration) < DateTime.Now).ToList();
                    }

                }
                else
                {

                    if (tip.Equals("aktif"))
                    {
                        GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null && ((DateTime)x.expiration) > DateTime.Now).ToList();
                    }
                    else if (tip.Equals("pasif"))
                    {
                        GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null && ((DateTime)x.expiration) < DateTime.Now).ToList();
                    }

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtAra.Value))
                {
                    GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null && x.firma_kod.Contains(txtAra.Value)).ToList();
                }
                else
                {
                    GridView1.DataSource = dc.firmas.Where(x => x.firma_kod != null).ToList();
                }
            }


            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("lisansla"))
            {

                string[] gelenler = e.CommandArgument.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                hdnID.Value = gelenler[0];
                hdnConfig.Value = gelenler[1];

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript2", sb.ToString(), false);
            }
            else if (e.CommandName.Equals("sil"))
            {
                string confirmValue = Request.Form["confirm_value"];
                List<string> liste = confirmValue.Split(new char[] { ',' }).ToList();
                int sayimiz = liste.Count - 1;
                string deger = liste[sayimiz];

                if (deger == "Yes")
                {
                    string[] gelenler = e.CommandArgument.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    hdnID.Value = gelenler[0];
                    hdnConfig.Value = gelenler[1];

                    using (Radius.radiusEntities dc = Radius.MyContext.Context(hdnConfig.Value))
                    {
                        //ayargenele kayıt
                        lisanla(-12, dc);
                    }
                    using (firmaEntities df = new firmaEntities())
                    {
                        //firmasa kayıt
                        //göster
                        var f = df.firmas.FirstOrDefault(x => x.config == (hdnConfig.Value));
                        if (f != null)
                        {

                            f.expiration = DateTime.Now.AddMonths(-12);
                            f.yenileme_tarihi = DateTime.Now;

                            df.SaveChanges();
                            Goster(df);

                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append(" alertify.error('Lisans iptal edildi');");
                            sb.Append("$('#editModal').modal('hide');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);
                        }
                    }
                }
                //lisansı iptal etmek için lisans süresini geriye alacaz

            }
        }
        public void MusteriAra(object sender, EventArgs e)
        {
            using (firmaEntities dc = new firmaEntities())
            {
                Goster(dc);
            }
        }

        protected void btnLisansKaydet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAy.Text))
            {
                using (Radius.radiusEntities dc = Radius.MyContext.Context(hdnConfig.Value))
                {
                    //ayargenele kayıt
                    lisanla(Int32.Parse(txtAy.Text), dc);
                }
                using (firmaEntities df = new firmaEntities())
                {
                    //firmasa kayıt
                    //göster
                    var f = df.firmas.FirstOrDefault(x => x.config == (hdnConfig.Value));
                    if (f != null)
                    {
                        DateTime eskiExp = (DateTime)f.expiration;
                        DateTime yeniExp = DateTime.Now.AddMonths(Int32.Parse(txtAy.Text));
                        if (eskiExp != null && ((DateTime)eskiExp).Date > DateTime.Now.Date)
                        {
                            yeniExp = ((DateTime)eskiExp).AddMonths(Int32.Parse(txtAy.Text));

                        }
                        f.expiration = yeniExp;
                        f.yenileme_tarihi = DateTime.Now;

                        df.SaveChanges();
                        Goster(df);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append(" alertify.error('Kayıt yapıldı');");
                        sb.Append("$('#editModal').modal('hide');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript2", sb.ToString(), false);
                    }
                }
            }
        }

        public void lisanla(int ay, Radius.radiusEntities dc)
        {
            //eski expire bugünden küçükse bugüne ekle
            //bugünden büyükse expirın üstüne ekle

            ayargenel ag = dc.ayargenels.FirstOrDefault();
            DateTime? eskiExp = ag.lisanstarih;
            DateTime yeniExp = DateTime.Now.AddMonths(ay);
            if (eskiExp != null && ((DateTime)eskiExp).Date > DateTime.Now.Date)
            {
                yeniExp = ((DateTime)eskiExp).AddMonths(ay);

            }
            ag.lisanstarih = yeniExp;
            dc.SaveChanges();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime tarih = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "expiration"));
                if (tarih.Date < DateTime.Now.Date)
                {
                    e.Row.CssClass = "danger";
                }


            }
        }

        protected void btnYeni_Click(object sender, EventArgs e)
        {
            Response.Redirect("/AdminSuper/AdminOlustur");
        }



    }
}