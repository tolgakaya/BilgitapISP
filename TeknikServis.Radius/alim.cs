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
    
    public partial class alim
    {
        public alim()
        {
            this.alim_detays = new HashSet<alim_detays>();
        }
    
        public int alim_id { get; set; }
        public int CustID { get; set; }
        public string konu { get; set; }
        public string aciklama { get; set; }
        public Nullable<System.DateTime> alim_tarih { get; set; }
        public string belge_no { get; set; }
        public string Firma { get; set; }
        public bool iptal { get; set; }
        public decimal toplam_tutar { get; set; }
        public decimal toplam_kdv { get; set; }
        public decimal toplam_yekun { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual ICollection<alim_detays> alim_detays { get; set; }
        public virtual customer customer { get; set; }
    }
}
