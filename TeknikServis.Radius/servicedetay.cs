//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeknikServis.Radius
{
    using System;
    using System.Collections.Generic;
    
    public partial class servicedetay
    {
        public int DetayID { get; set; }
        public int ServiceID { get; set; }
        public System.DateTime TarihZaman { get; set; }
        public string BelgeYol { get; set; }
        public string Aciklama { get; set; }
        public int Durum_ID { get; set; }
        public string KullaniciID { get; set; }
        public string Firma { get; set; }
        public string kullanici { get; set; }
        public Nullable<bool> iptal { get; set; }
        public string Baslik { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual service_durums service_durums { get; set; }
        public virtual service service { get; set; }
    }
}
