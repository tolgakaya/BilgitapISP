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
    
    public partial class servis_paket_detays
    {
        public int id { get; set; }
        public int paket_id { get; set; }
        public string paket_adi { get; set; }
        public string IslemParca { get; set; }
        public Nullable<decimal> Tutar { get; set; }
        public Nullable<decimal> KDV { get; set; }
        public decimal Yekun { get; set; }
        public string Aciklama { get; set; }
        public int adet { get; set; }
        public Nullable<int> cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public Nullable<int> cihaz_gsure { get; set; }
        public string Firma { get; set; }
        public bool iptal { get; set; }
    
        public virtual cihaz cihaz { get; set; }
        public virtual servis_pakets servis_pakets { get; set; }
    }
}
