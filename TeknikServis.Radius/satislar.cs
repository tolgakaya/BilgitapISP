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
    
    public partial class satislar
    {
        public satislar()
        {
            this.faturas = new HashSet<fatura>();
        }
    
        public int satis_id { get; set; }
        public int musteri_id { get; set; }
        public Nullable<int> odeme_id { get; set; }
        public int cihaz_id { get; set; }
        public int adet { get; set; }
        public decimal tutar { get; set; }
        public decimal vergi { get; set; }
        public decimal kdv { get; set; }
        public decimal otv { get; set; }
        public decimal oiv { get; set; }
        public decimal bandrol { get; set; }
        public decimal iskonto_oran { get; set; }
        public decimal yekun { get; set; }
        public System.DateTime tarih { get; set; }
        public Nullable<int> stok_id { get; set; }
        public string Firma { get; set; }
        public bool iptal { get; set; }
        public Nullable<int> fat_no { get; set; }
        public string fat_seri { get; set; }
        public Nullable<System.DateTime> basim_tarih { get; set; }
        public string unvan { get; set; }
        public string tc { get; set; }
        public string vd { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual cihaz cihaz { get; set; }
        public virtual musteriodemeler musteriodemeler { get; set; }
        public virtual ICollection<fatura> faturas { get; set; }
        public virtual customer customer { get; set; }
    }
}
