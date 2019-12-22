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
    public class MusteriantenController : ApiController
    {
        public static string firma { get { return KullaniciIslem.firma(); } }
        // GET api/<controller>

        public IEnumerable<Musterimiz> GetMusteri()
        {
            //deneme olarak harita-tek-antende geoyu denemek için lat kontrolsüz gönderdim
            //
            using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
            {

                List<Musterimiz> musterimiz = (from a in dc.customers
                                               where a.CustID > 0 && a.musteri == true && a.antenid != null
                                               select new Musterimiz
                                               {

                                                   musteri_id = a.CustID,
                                                   musteri_adi = a.Ad,
                                                   center_Lat = a.Lat,
                                                   center_Long = a.Long,
                                                   musteri_adres = a.Adres


                                               }).ToList();



                return musterimiz;
            }

        }
        public IEnumerable<Musterimiz> GetMusteri(int id)
        {
            //deneme olarak harita-tek-antende geoyu denemek için lat kontrolsüz gönderdim
            //
            using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
            {

                List<Musterimiz> musterimiz = (from a in dc.customers
                                               where a.CustID > 0 && a.musteri == true && a.antenid != null && a.antenid == id
                                               select new Musterimiz
                                               {

                                                   musteri_id = a.CustID,
                                                   musteri_adi = a.Ad,
                                                   center_Lat = a.Lat,
                                                   center_Long = a.Long,
                                                   musteri_adres = a.Adres


                                               }).ToList();



                return musterimiz;
            }

        }

        public IEnumerable<Musterimiz> GetMusteri(int id, int custid)
        {
            //deneme olarak harita-tek-antende geoyu denemek için lat kontrolsüz gönderdim
            //
            using (Radius.radiusEntities dc = Radius.MyContext.Context(firma))
            {

                List<Musterimiz> musterimiz = (from a in dc.customers
                                               where a.CustID == custid && a.antenid != null && a.antenid == id
                                               select new Musterimiz
                                               {

                                                   musteri_id = a.CustID,
                                                   musteri_adi = a.Ad,
                                                   center_Lat = a.Lat,
                                                   center_Long = a.Long,
                                                   musteri_adres = a.Adres


                                               }).ToList();



                return musterimiz;
            }

        }
        // GET api/<controller>/5


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
}