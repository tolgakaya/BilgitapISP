using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeknikServis.Models;
using ServisDAL;
using System.Web.Http.Cors;
using TeknikServis.Logic;

namespace TeknikServis
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MusteriservisController : ApiController
    {
        public static string firma { get { return KullaniciIslem.firma(); } }
        // GET api/<controller>

        public IEnumerable<servisimiz> GetServis()
        {
            //deneme olarak harita-tek-antende geoyu denemek için lat kontrolsüz gönderdim
            //
            using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
            {

                List<servisimiz> musterimiz = (from a in dc.services
                                               where a.iptal != true && a.KapanmaZamani == null
                                               select new servisimiz
                                               {

                                                   servis_id = a.ServiceID,
                                                   musteri_id = (int)a.CustID,
                                                   musteri_adi = a.customer.Ad,
                                                   center_Lat = a.customer.Lat,
                                                   center_Long = a.customer.Long,
                                                   musteri_adres = a.customer.Adres,
                                                   kimlik=a.Servis_Kimlik_No

                                               }).ToList();

                return musterimiz;
            }

        }


        // POST api/<controller>
        public string Post([FromBody]string[] value)
        {
            if (value == null)
            {
                return "null geldi";
            }
            else if (value.Length > 0)
            {
                return "sıfır geldi";
            }
            else
            {
                return value.FirstOrDefault();
            }

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class servisimiz
    {
        public int servis_id { get; set; }
        public int musteri_id { get; set; }
        public string musteri_adi { get; set; }
        public string center_Lat { get; set; }
        public string center_Long { get; set; }
        public string musteri_adres { get; set; }
        public string kimlik { get; set; }
    }
}