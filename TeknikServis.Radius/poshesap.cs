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
    
    public partial class poshesap
    {
        public int id { get; set; }
        public int pos_id { get; set; }
        public System.DateTime tahsilat_tarih { get; set; }
        public decimal tahsilat_tutar { get; set; }
        public decimal komsiyon_oran { get; set; }
        public decimal komisyon_tutar { get; set; }
        public decimal net_tutar { get; set; }
        public System.DateTime extre_tarihi { get; set; }
        public bool cekildi { get; set; }
        public int tahsilat_id { get; set; }
        public Nullable<bool> iptal { get; set; }
        public string Firma { get; set; }
        public string islem { get; set; }
        public int Musteri_ID { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual musteriodemeler musteriodemeler { get; set; }
        public virtual pos_tanims pos_tanims { get; set; }
        public virtual customer customer { get; set; }
    }
}