using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using System.IO;
using DevExpress.XtraReports.Web;
using TeknikServis.Logic;
using DevExpress.Web;

namespace TeknikServis.Admin
{
    public partial class Tasarimci : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }

            if (!IsPostBack) //check if the webpage is loaded for the first time.
            {
                ViewState["PreviousPage"] =
            Request.UrlReferrer;//Saves the Previous page url in ViewState
            }
        }
        protected void btnGeri_Click(object sender, EventArgs e)
        {
            if (ViewState["PreviousPage"] != null)	//Check if the ViewState 
            //contains Previous page URL
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());//Redirect to 
                //Previous page by retrieving the PreviousPage Url from ViewState.
            }
        }
        protected void btnAna_Click(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Giris");
            }
            //aqyery stringle tipe göre fatura makbuz vb bütün tasarım kayıtları burada yapılacak
            if (!IsPostBack)
            {
                string tip = Request.QueryString["tip"];
                string firma = KullaniciIslem.firma();


                if (tip.Equals("fatura"))
                {
                    //fatura yolunu bulalım
                    bool? tek = false;
                    using (Radius.radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
                    {
                        AyarCurrent ay = new AyarCurrent(dc);
                        tek = ay.get().cift_taraf;
                    }
                    if (tek==true)
                    {
                        string path = "/Raporlar/" + KullaniciIslem.firma() + "fatura.repx";
                        string yol = Server.MapPath(path);
                        XtraReport report = new faturaX();
                        if (File.Exists(yol))
                        {
                            report.LoadLayout(yol);
                        }

                        ASPxReportDesigner1.OpenReport(report);
                    }
                    else
                    {
                        string path = "/Raporlar/" + KullaniciIslem.firma() + "faturaTek.repx";
                        string yol = Server.MapPath(path);
                        XtraReport report = new faturaTekX();
                        if (File.Exists(yol))
                        {
                            report.LoadLayout(yol);
                        }

                        ASPxReportDesigner1.OpenReport(report);
                    }
                  

                }
                else if (tip.Equals("makbuz"))
                {
                    //fatura yolunu bulalım
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "makbuz.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new makbuzX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
                else if (tip.Equals("servis"))
                {
                    //fatura yolunu bulalım
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "servis.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new baslamaX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
                else if (tip.Equals("servis_maliyet"))
                {
                    //fatura yolunu bulalım
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "servis_maliyet.repx";
                    string yol = Server.MapPath(path);
                    ServisDAL.Raporlama.ServisMaliyetlerX report = new ServisDAL.Raporlama.ServisMaliyetlerX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
                else if (tip.Equals("extre"))
                {
                    //fatura yolunu bulalım
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "extre.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new extreX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }


                else if (tip.Equals("gelirgider"))
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgider.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new GelirGiderX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
                else if (tip.Equals("gelirgiderozet"))
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgiderozet.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new GelirGiderOzetX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }

                else if (tip.Equals("gelirgidergruplu"))
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgidergruplu.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new GelirGiderGrupluX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
                else if (tip.Equals("gelirgideraylik"))
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgideraylik.repx";
                    string yol = Server.MapPath(path);
                    XtraReport report = new GelirGiderAylikX();
                    if (File.Exists(yol))
                    {
                        report.LoadLayout(yol);
                    }

                    ASPxReportDesigner1.OpenReport(report);

                }
            }
        }

        protected void ASPxReportDesigner1_SaveReportLayout(object sender, SaveReportLayoutEventArgs e)
        {
            string tip = Request.QueryString["tip"];
            string firma = KullaniciIslem.firma();


            if (tip.Equals("fatura"))
            {
                //fatura yolunu bulalım
                bool? ciftTaraf = false;
                using (Radius.radiusEntities dc=Radius.MyContext.Context(KullaniciIslem.firma()))
                {
                    AyarCurrent ay = new AyarCurrent(dc);
                    ciftTaraf = ay.get().cift_taraf;
                }
                if (ciftTaraf==true)
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "fatura.repx";
                    string yol = Server.MapPath(path);
                    File.WriteAllBytes(yol, e.ReportLayout);
                }
                else
                {
                    string path = "/Raporlar/" + KullaniciIslem.firma() + "faturaTek.repx";
                    string yol = Server.MapPath(path);
                    File.WriteAllBytes(yol, e.ReportLayout);
                }
               
                ASPxWebControl.RedirectOnCallback("~/");


            }
            else if (tip.Equals("makbuz"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "makbuz.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("servis"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "servis.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("extre"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "extre.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("servis_maliyet"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "servis_maliyet.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }

            else if (tip.Equals("gelirgider"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgider.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("gelirgiderozet"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgiderozet.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("gelirgidergruplu"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgidergruplu.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
            else if (tip.Equals("gelirgideraylik"))
            {
                //fatura yolunu bulalım
                string path = "/Raporlar/" + KullaniciIslem.firma() + "gelirgideraylik.repx";
                string yol = Server.MapPath(path);
                File.WriteAllBytes(yol, e.ReportLayout);
                ASPxWebControl.RedirectOnCallback("~/");

            }
        }


    }
}