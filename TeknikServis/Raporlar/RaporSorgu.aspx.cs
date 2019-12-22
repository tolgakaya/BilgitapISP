using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeknikServis.Logic;
using ServisDAL;
using TeknikServis.Radius;

namespace TeknikServis.Raporlar
{
    public partial class RaporSorgu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
            string tip = Request.QueryString["tip"];
            if (tip.Equals("periyodik_rapor"))
            {
                periyot.Visible = true;
            }
        }

        protected void btnRapor_Click(object sender, EventArgs e)
        {
            string tip = Request.QueryString["tip"];
            if (!string.IsNullOrEmpty(tip))
            {
                string firma = KullaniciIslem.firma();
                using (radiusEntities dc = MyContext.Context(firma))
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    if (ay.lisansKontrol() == false)
                    {
                        Response.Redirect("/LisansError");
                    }
                    GelirGider g = new ServisDAL.GelirGider(dc);

                    string basS = datetimepicker6.Value;
                    string sonS = datetimepicker7.Value;
                    var son = DateTime.Now;
                    var bas = new DateTime(son.Year, son.Month, 1);

                    if (!String.IsNullOrEmpty(basS))
                    {
                        bas = DateTime.Parse(basS);
                    }
                    if (!String.IsNullOrEmpty(sonS))
                    {
                        son = DateTime.Parse(sonS);
                    }

                    g.baslangic = bas;
                    g.son = son;

                    if (tip.Equals("odeme_tahsilat"))
                    {
                        wrapper_genel wg = g.gonder_genel("odeme_tahsilat");
                        wg.firma = firma;
                        Session["odeme_tahsilat"] = wg;
                        Response.Redirect("/Baski.aspx?tip=odeme_tahsilat");
                    }
                    else if (tip.Equals("gider_raporu"))
                    {
                        wrapper wr = g.gonder("odeme");
                        wr.firma = firma;
                        Session["gider_raporu"] = wr;
                        Response.Redirect("/Baski.aspx?tip=gider_raporu");
                    }
                    else if (tip.Equals("tahsilat_raporu"))
                    {
                        wrapper wr = g.gonder("tahsilat");
                        wr.firma = firma;
                        Session["tahsilat_raporu"] = wr;
                        Response.Redirect("/Baski.aspx?tip=tahsilat_raporu");
                    }
                    else if (tip.Equals("satis_raporu"))
                    {
                        wrapper wr = g.gonder("satis");
                        wr.firma = firma;
                        Session["satis_raporu"] = wr;
                        Response.Redirect("/Baski.aspx?tip=satis_raporu");
                    }
                    else if (tip.Equals("odeme_tahsilat_gruplu"))
                    {
                        wrapper_genel_gruplu wg = g.gonder_gruplu("odeme_tahsilat_gruplu");
                        wg.firma = firma;
                        wg.tip = "TÜRLERİNE GÖRE ÖDEME VE TAHSİLATLAR";
                        Session["odeme_tahsilat_gruplu"] = wg;
                        Response.Redirect("/Baski.aspx?tip=odeme_tahsilat_gruplu");
                    }
                    else if (tip.Equals("odeme_tahsilat_satis"))
                    {
                        wrapper_genel_gruplu wg = g.gonder_gruplu_satisli("odeme_tahsilat_satis");
                        wg.firma = firma;
                        wg.tip = "TÜRLERİNE GÖRE ÖDEME/TAHSİLAT/SATIŞ";
                        Session["odeme_tahsilat_satis"] = wg;
                        Response.Redirect("/Baski.aspx?tip=odeme_tahsilat_satis");
                    }
                    else if (tip.Equals("periyodik_rapor"))
                    {
                        int per = 7;
                        string prS = txtPeriyot.Text;
                        if (!String.IsNullOrEmpty(prS))
                        {
                            per = Int32.Parse(prS);
                        }
                        wrapper_genel_periyodik wg = g.periyodik_rapor(per, bas, son);
                        wg.firma = firma;
                        Session["periyodik_rapor"] = wg;
                        Response.Redirect("/Baski.aspx?tip=periyodik_rapor");
                    }
                }


            }
        }
    }
}