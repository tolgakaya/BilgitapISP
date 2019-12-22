using ServisDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeknikServis.Models;
using TeknikServis.Logic;
using System.Web.Http.Cors;
using TeknikServis.Radius;

namespace TeknikServis
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AntenController : ApiController
    {

        // GET api/<controller>
        //burada firmanın antenleri görünüyor

        public IEnumerable<Anten> GetAnten()
        {
            using (radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            {
                return (from a in dc.antens

                        select new Anten
                        {
                            anten_id = a.anten_id,
                            anten_adi = a.anten_adi,
                            center_Lat = a.center_Lat,
                            center_Long = a.center_Long,
                            start_Lat = a.start_Lat,
                            start_Long = a.start_Long,
                            end_Lat = a.end_Lat,
                            end_Long = a.end_Long,


                        }).ToList();
            }


        }


        // GET api/<controller>/5
        public Anten GetAnten(int id)
        {

            using (radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            {
                return (from a in dc.antens
                        where a.anten_id == id
                        select new Anten
                        {
                            anten_id = a.anten_id,
                            anten_adi = a.anten_adi,
                            center_Lat = a.center_Lat,
                            center_Long = a.center_Long,
                            start_Lat = a.start_Lat,
                            start_Long = a.start_Long,
                            end_Lat = a.end_Lat,
                            end_Long = a.end_Long,

                        }).FirstOrDefault();
            }

        }

        // POST api/<controller>
        public TeknikServis.Radius.anten PostAnten(TeknikServis.Radius.anten antenimiz)
        {
            using (radiusEntities dc = Radius.MyContext.Context(KullaniciIslem.firma()))
            {
                dc.antens.Add(antenimiz);
                dc.SaveChanges();
                return antenimiz;
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