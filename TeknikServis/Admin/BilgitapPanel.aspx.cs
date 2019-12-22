using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServisDAL;
using TeknikServis.Logic;

namespace TeknikServis.Admin
{
    public partial class BilgitapPanel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!User.Identity.IsAuthenticated || (!User.IsInRole("Admin") && !User.IsInRole("mudur")))
            {
                System.Web.HttpContext.Current.Response.Redirect("/Account/Giris");
            }
                string firma = KullaniciIslem.firma();
                //if (kullanici.UserName != "canEditUser@wingtiptoys.com")
                //{
                string key = "panel-" + firma;
                PanelDetay p;
                if (Cache[key] != null)
                {
                    p = (PanelDetay)Cache[key];
                }
                else
                {
                    using (Radius.radiusEntities dc=Radius.MyContext.Context(firma))
                    {
                        TekPanel pan = new TekPanel(dc);
                        p = pan.Goster();
                        Cache.Insert(key, p, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
                    }
               
                }


                netBorc.InnerHtml = p.netBorc.ToString("C");
                netAlacak.InnerHtml = p.netAlacak.ToString("C");
                netBakiye.InnerHtml = p.netBakiye.ToString("C");
                onayBekleyen.InnerHtml = p.onayBekleyen.ToString();
                servisSayisi.InnerHtml = p.servisSayisi.ToString();
                emanetSayisi.InnerHtml = p.emanetSayisi.ToString();
                musteriler.InnerHtml = p.musteriSayisi.ToString() + " Adet";
                
                yaklasanOdeme.InnerHtml = p.yaklasanOdeme.ToString() + " Adet";
                kasa.InnerHtml = p.kasaBakiye.ToString("C");
                bankahesap.InnerHtml = p.bankaBakiye.ToString("C");
                poshesaplari.InnerHtml = p.posBakiye.ToString("C");

                //}
     

          

        }
      

        protected void btnYaklasan_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Musteri.aspx?tur=borc");
        }

        protected void btnKrediYaklas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Musteri.aspx?tur=abone");
        }

        protected void btnMusteriler_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Musteri");
        }

        protected void btnKasa_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikCari/Hareketler");
        }

        protected void btnBanka_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikCari/Bankalar");
        }

        protected void btnPos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/TeknikCari/Poslar");
        }
    }
}