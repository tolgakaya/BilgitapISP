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
    
    public partial class masraf_tips
    {
        public masraf_tips()
        {
            this.musteriodemelers = new HashSet<musteriodemeler>();
        }
    
        public int tip_id { get; set; }
        public string tip_adi { get; set; }
        public string aciklama { get; set; }
        public string Firma { get; set; }
        public string css { get; set; }
        public bool iptal { get; set; }
    
        public virtual ICollection<musteriodemeler> musteriodemelers { get; set; }
    }
}
