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
    
    public partial class kart_hesaps
    {
        public int id { get; set; }
        public int kart_id { get; set; }
        public int odeme_id { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
        public System.DateTime extre_tarih { get; set; }
        public decimal tutar { get; set; }
        public string aciklama { get; set; }
        public decimal toplam_tutar { get; set; }
        public bool cekildi { get; set; }
        public bool iptal { get; set; }
        public string Firma { get; set; }
        public string islem { get; set; }
        public Nullable<int> Musteri_ID { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual kart_tanims kart_tanims { get; set; }
        public virtual musteriodemeler musteriodemeler { get; set; }
        public virtual customer customer { get; set; }
    }
}
