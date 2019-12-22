using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;
using TeknikServis.Radius;

namespace TeknikServis
{
    public partial class Baski : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //querystringe göre baskı yapılacakların 
            string tip = Request.QueryString["tip"];
            if (!String.IsNullOrEmpty(tip))
            {
                if (tip.Equals("tahsilat"))
                {
                    if (Session["Makbuz_Gorunum"] != null)
                    {
                        Makbuz_Gorunum gor = (Makbuz_Gorunum)Session["Makbuz_Gorunum"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.MakbuzBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("emanet"))
                {
                    if (Session["Makbuz_Gorunum"] != null)
                    {
                        Makbuz_Gorunum gor = (Makbuz_Gorunum)Session["Makbuz_Gorunum"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.EmanetBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("baslama"))
                {
                    if (Session["Servis_Baslama"] != null)
                    {
                        Servis_Baslama gor = (Servis_Baslama)Session["Servis_Baslama"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.BaslamaBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("servis_maliyet"))
                {
                    if (Session["servis_maliyet"] != null)
                    {
                        maliyet liste = (maliyet)Session["servis_maliyet"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.ServisMaliyet(liste, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("manuel"))
                {
                    if (Session["Fatura_Bilgisi"] != null)
                    {
                        InternetFaturasi gor = (InternetFaturasi)Session["Fatura_Bilgisi"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            AyarCurrent ay = new AyarCurrent(dc);

                            pr.ManuelBas(gor, baskiGoster, ay.get().cift_taraf, KullaniciIslem.firma());
                        }

                    }
                }
                else if (tip.Equals("extre"))
                {
                    if (Session["extre"] != null)
                    {
                        extre gor = (extre)Session["extre"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.ExtreBas(gor, baskiGoster);
                        }


                    }
                }

                else if (tip.Equals("gider_raporu"))
                {
                    if (Session["gider_raporu"] != null)
                    {
                        wrapper gor = (wrapper)Session["gider_raporu"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderBas(gor, KullaniciIslem.firma(), baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("tahsilat_raporu"))
                {
                    if (Session["tahsilat_raporu"] != null)
                    {
                        wrapper gor = (wrapper)Session["tahsilat_raporu"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderBas(gor, KullaniciIslem.firma(), baskiGoster);
                        }

                    }
                }

                else if (tip.Equals("satis_raporu"))
                {
                    if (Session["satis_raporu"] != null)
                    {
                        wrapper gor = (wrapper)Session["satis_raporu"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderBas(gor, KullaniciIslem.firma(), baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("odeme_tahsilat"))
                {
                    if (Session["odeme_tahsilat"] != null)
                    {
                        wrapper_genel gor = (wrapper_genel)Session["odeme_tahsilat"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderGenelBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("odeme_tahsilat_gruplu"))
                {
                    if (Session["odeme_tahsilat_gruplu"] != null)
                    {
                        wrapper_genel_gruplu gor = (wrapper_genel_gruplu)Session["odeme_tahsilat_gruplu"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderGenelGrupluBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("odeme_tahsilat_satis"))
                {
                    if (Session["odeme_tahsilat_satis"] != null)
                    {
                        wrapper_genel_gruplu gor = (wrapper_genel_gruplu)Session["odeme_tahsilat_satis"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.GelirGiderGenelGrupluBas(gor, baskiGoster);
                        }

                    }
                }
                else if (tip.Equals("periyodik_rapor"))
                {
                    if (Session["periyodik_rapor"] != null)
                    {
                        wrapper_genel_periyodik gor = (wrapper_genel_periyodik)Session["periyodik_rapor"];
                        using (radiusEntities dc = MyContext.Context(KullaniciIslem.firma()))
                        {
                            FaturaPrinter pr = new FaturaPrinter(dc);
                            pr.PeriyodikRaporBas(gor, baskiGoster);
                        }


                    }
                }
            }
        }
    }
}