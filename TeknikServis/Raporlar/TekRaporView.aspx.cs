using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Radius;
using ServisDAL;
using TeknikServis.Logic;

namespace TeknikServis.Raporlar
{
    public partial class TekRaporView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Ara();


        }

        protected void btnAra_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void Ara()
        {
            using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
            {
                AyarCurrent ay = new AyarCurrent(dc);
                if (ay.lisansKontrol() == false)
                {
                    Response.Redirect("/LisansError");
                }
                TekRapor r = new TekRapor(dc);
                r.baslama_tarihi = datetimepicker6.Value.Trim();
                r.bitis_tarihi = datetimepicker7.Value.Trim();
                gunlukrapor gunluk = r.gunluk();
                kasalbl.Text = "Kasa: " + gunluk.kasabakiye.ToString("C");
                bankalbl.Text = "Banka: " + gunluk.bankabakiye.ToString("C");
                poslbl.Text = "Pos: " + gunluk.posbakiye.ToString("C");
                kartlbl.Text = "Kartlar: " + gunluk.kartbakiye.ToString("C");
                cariborclbl.Text = "CariBorç: " + gunluk.cariborc.ToString("C");
                carialacaklbl.Text = "CariAlacak: " + gunluk.carialacak.ToString("C");
                caribakiyelbl.Text = "CariBakiye: " + gunluk.caribakiye.ToString("C");
                acilanservissayisi.Text = "Açılan Servis: " + gunluk.acilan_servis_sayisi.ToString() + " Adet";
                if (gunluk.acilan_servis_sayisi > 0)
                {
                    acilanPanel.Visible = true;
                    grdAcilanServis.DataSource = gunluk.acilan_servisler;
                }
                else
                {
                    acilanPanel.Visible = false;
                }
                kapananservissayisi.Text = "Kapanan Servis: " + gunluk.kapanan_servis_sayisi.ToString() + " Adet";
                if (gunluk.kapanan_servis_sayisi > 0)
                {
                    kapananPanel.Visible = true;
                    grdKapananServis.DataSource = gunluk.kapanan_servisler;
                }
                else
                {
                    kapananPanel.Visible = false;
                }
                kapananservistutari.Text = "Kapanan Tutar: " + gunluk.kapanan_servis_tutari.ToString("C");
                kapananservismaliyeti.Text = "Kapanan Maliyet: " + gunluk.kapanan_servis_maliyeti.ToString("C");
                acilandisservissayisi.Text = "Yeni Dış: " + gunluk.acilan_dis_servis_sayisi.ToString() + " Adet";
                if (gunluk.acilan_dis_servis_sayisi > 0)
                {
                    yeniDisPanel.Visible = true;
                    grdDisAcilan.DataSource = gunluk.acilan_dis_servisler;
                }
                else
                {
                    yeniDisPanel.Visible = false;
                }

                acilandisservisMaliyeti.Text = "Yeni Dış Maliyeti: " + gunluk.acilan_dis_servis_maliyeti.ToString("C");
                tamamlanandisservisSayisi.Text = "Tamamlanan Dış: " + gunluk.tamamlanan_dis_servis_sayisi.ToString() + " Adet";
                if (gunluk.tamamlanan_dis_servis_sayisi > 0)
                {
                    tamamDisPanel.Visible = true;
                    grdDisTamam.DataSource = gunluk.tamamlanan_dis_servisler;
                }
                else
                {
                    tamamDisPanel.Visible = false;
                }

                tamamlanandisservisMaliyeti.Text = "Tamamlanan Dış Maliyeti: " + gunluk.tamamlanan_dis_servis_maliyeti.ToString("C");
                odemesayisi.Text = "Ödeme Sayısı: " + gunluk.odemeler_sayisi.ToString() + " Adet";
                odemetoplami.Text = "Ödemeler: " + gunluk.odemeler_toplami.ToString("C");
                tahsilatsayisi.Text = "Tahsilat Sayısı: " + gunluk.tahsilat_sayisi.ToString() + " Adet";
                tahsilattoplami.Text = "Tahsilatlar: " + gunluk.tahsilat_toplami.ToString("C");
                if (gunluk.odemeler_sayisi > 0 || gunluk.tahsilat_sayisi > 0)
                {
                    grdOdeme.DataSource = gunluk.odeme_tahsilatlar;
                    odemePanel.Visible = true;
                }
                else
                {
                    odemePanel.Visible = false;
                }
                satinalmasayisi.Text = "Satın Alma: " + gunluk.satinalma_sayisi.ToString() + " Adet";
                if (gunluk.satinalma_sayisi > 0)
                {
                    grdAlimlar.DataSource = gunluk.satinalimlar;
                    alimPanel.Visible = true;
                }
                else
                {
                    alimPanel.Visible = false;
                }

                if (gunluk.emanettekiler_toplami > 0)
                {
                    grdEmanetYeni.DataSource = gunluk.emanettekiler;
                    emanetVerilenPanel.Visible = true;
                }
                else
                {
                    emanetVerilenPanel.Visible = false;
                }
                if (gunluk.emanetten_donenler_toplami > 0)
                {
                    grdEmanetDonen.DataSource = gunluk.emanet_donenler;
                    emanetDonenPanel.Visible = true;
                }
                else
                {
                    emanetDonenPanel.Visible = false;
                }

                satinalmaToplami.Text = "Satın Alma: " + gunluk.satinalma_toplami.ToString("C");
                iadeSayisi.Text = "İade: " + gunluk.iade_sayisi.ToString() + " Adet";
                iadeTutari.Text = "İade: " + gunluk.iade_toplami.ToString("C");
                pesinsatisSayisi.Text = "Peşin Satış: " + gunluk.pesin_satis_sayisi.ToString() + " Adet";
                pesinSatisTutari.Text = "Peşin Satış: " + gunluk.pesin_satis_toplami.ToString("C");
                emanetVerilenSayisi.Text = "Verilen Emanet: " + gunluk.emanettekiler_toplami.ToString() + " Adet";
                emanettenDonenlerSayisi.Text = "Dönen Emanet: " + gunluk.emanetten_donenler_toplami.ToString() + " Adet";

                DataBind();

            }
        }


        protected void grdAcilanServis_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");


            }
        }

        protected void grdKapananServis_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");
                (e.Row.FindControl("btnKucuk") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?servisid=" + DataBinder.Eval(e.Row.DataItem, "serviceID") + "&custid=" + DataBinder.Eval(e.Row.DataItem, "custID") + "&kimlik=" + DataBinder.Eval(e.Row.DataItem, "kimlikNo");


            }
        }
        protected void grdAcilanServis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string css = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "css"));
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(css);
            }
        }
        protected void grdKapananServis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string css = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "css"));
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(css);
            }
        }


        protected void grdAcilanServis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAcilanServis.PageIndex = e.NewPageIndex;
            //göster();
        }
        protected void grdKapananServis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAcilanServis.PageIndex = e.NewPageIndex;
            //göster();
        }

        protected void grdDisAcilan_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string m = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "musteriID"));
                string s = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "servisID"));
                string k = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "kimlik"));

                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;
                (e.Row.FindControl("btnServisK") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;


            }
        }

        protected void grdDisTamam_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string m = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "musteriID"));
                string s = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "servisID"));
                string k = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "kimlik"));

                (e.Row.FindControl("btnServis") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;
                (e.Row.FindControl("btnServisK") as LinkButton).PostBackUrl = "~/TeknikTeknik/Servis.aspx?custid=" + m + "&servisid=" + s + "&kimlik=" + k;


            }
        }

        protected void grdDisAcilan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDisAcilan.PageIndex = e.NewPageIndex;
            //goster

        }
        protected void grdDisTamam_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDisTamam.PageIndex = e.NewPageIndex;
            //goster

        }
        protected void grdOdeme_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string tipi = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tahsilat_odeme"));
                if (tipi.Equals("odeme"))
                {
                    e.Row.CssClass = "danger";
                }
                string tur = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "tahsilatOdeme_turu"));
                if (tur.Equals("cari"))
                {
                    e.Row.Cells[0].Controls[3].Visible = false;
                }

            }
        }
        protected void grdOdeme_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdOdeme.PageIndex = e.NewPageIndex;
            //goster

        }
        protected void grdAlimlar_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("btnDetay") as LinkButton).PostBackUrl = "~/TeknikAlim/AlimDetaylar.aspx?alimid=" + DataBinder.Eval(e.Row.DataItem, "alim_id");
                (e.Row.FindControl("btnTedarikci") as LinkButton).PostBackUrl = "~/MusteriDetayBilgileri.aspx?custid=" + DataBinder.Eval(e.Row.DataItem, "CustID");

            }

        }
        protected void grdAlimlar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlimlar.PageIndex = e.NewPageIndex;
            //ara

        }

    }
}