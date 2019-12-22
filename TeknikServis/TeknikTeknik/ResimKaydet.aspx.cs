using System;
using System.Web;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;
using ServisDAL;
using TeknikServis.Logic;
using System.Linq;
using System.Collections.Generic;

namespace TeknikServis
{
    [ScriptService]
    public partial class ResimKaydet : System.Web.UI.Page
    {
        static string path = HttpContext.Current.Server.MapPath("/Uploads/");
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod()]

        public static void UploadPic(string imageData, string servis, string aciklama, string durum, bool ress, string baslik, string kullanici)
        {
            //resim yüklendiği zaman bütün işlemleri burada tamamlayacaz.
            //yani detay ekleme işlemleri burada tamamlanacak.
            //böylece veritabanınaresim yolu yazılırken bu isim kullanılacak.
            using (Radius.radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            {
                ServisIslemleri ser = new ServisIslemleri(dc);
                int durumID = Int32.Parse(durum);
                int servisID = Int32.Parse(servis);
                string resimYolu = "-";
                if (ress == true)
                {
                    string isim = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
                    string fileNameWitPath = path + isim;
                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            byte[] data = Convert.FromBase64String(imageData);
                            bw.Write(data);
                            bw.Close();
                        }
                    }
                    resimYolu = "/Uploads/" + isim;
                }


                ser.servisDetayEkleR(servisID, resimYolu, aciklama, durumID, kullanici, baslik);
            }

        }

        [WebMethod()]
        public static void MusteriUpdate(List<string> bilgiler)
        {
            //return musteriler.ToArray();
            string mesele = "";
            foreach (var n in bilgiler)
            {
                mesele += "gelen: " + n+"<br/>";
            }
            //return mesele;
            HttpContext.Current.Session["mesele"] = mesele;
            //HttpContext.Current.Response.Redirect("/Sonuc");
            //resim yüklendiği zaman bütün işlemleri burada tamamlayacaz.
            //yani detay ekleme işlemleri burada tamamlanacak.
            //böylece veritabanınaresim yolu yazılırken bu isim kullanılacak.
            //using (Radius.radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            //{

            //}

        }


        //[WebMethod()]
        //public static void MusteriUpdate(nokta[] noktalar)
        //{
        //    //return noktalar;
        //    //return musteriler.ToArray();
        //    //return noktalar;

        //    string mesele = "";
        //    foreach (var n in noktalar)
        //    {
        //        mesele += "gelen: " + n.id;
        //    }
        //    //return mesele;
        //    HttpContext.Current.Session["mesele"] = mesele;
        //    //HttpContext.Current.Response.Redirect("/Sonuc");
        //    //resim yüklendiği zaman bütün işlemleri burada tamamlayacaz.
        //    //yani detay ekleme işlemleri burada tamamlanacak.
        //    //böylece veritabanınaresim yolu yazılırken bu isim kullanılacak.
        //    //using (Radius.radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
        //    //{

        //    //}

        //}

    }
    public class nokta
    {
        public int id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}