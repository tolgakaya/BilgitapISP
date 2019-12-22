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
    
    public partial class musteriodemeler
    {
        public musteriodemeler()
        {
            this.cekhesaps = new HashSet<cekhesap>();
            this.kart_hesaps = new HashSet<kart_hesaps>();
            this.kasaharekets = new HashSet<kasahareket>();
            this.satislars = new HashSet<satislar>();
            this.poshesaps = new HashSet<poshesap>();
        }
    
        public string KullaniciID { get; set; }
        public string no { get; set; }
        public decimal OdemeMiktar { get; set; }
        public System.DateTime OdemeTarih { get; set; }
        public string Aciklama { get; set; }
        public int Odeme_ID { get; set; }
        public int Musteri_ID { get; set; }
        public bool iptal { get; set; }
        public string Firma { get; set; }
        public string kullanici { get; set; }
        public int taksit_no { get; set; }
        public string tahsilat_turu { get; set; }
        public string tahsilat_odeme { get; set; }
        public Nullable<int> pos_id { get; set; }
        public Nullable<int> kart_id { get; set; }
        public Nullable<int> taksit_sayisi { get; set; }
        public Nullable<int> banka_id { get; set; }
        public string belge_no { get; set; }
        public Nullable<System.DateTime> vade_tarih { get; set; }
        public Nullable<decimal> masraf { get; set; }
        public Nullable<int> fatura_id { get; set; }
        public Nullable<System.DateTime> extre_tarih { get; set; }
        public Nullable<bool> mahsup { get; set; }
        public string mahsup_key { get; set; }
        public Nullable<int> iade_id { get; set; }
        public Nullable<bool> standart { get; set; }
        public string masraf_tipi { get; set; }
        public Nullable<int> masraf_id { get; set; }
        public Nullable<System.DateTime> islem_tarihi { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual banka banka { get; set; }
        public virtual ICollection<cekhesap> cekhesaps { get; set; }
        public virtual cihaz_garantis cihaz_garantis { get; set; }
        public virtual ICollection<kart_hesaps> kart_hesaps { get; set; }
        public virtual kart_tanims kart_tanims { get; set; }
        public virtual ICollection<kasahareket> kasaharekets { get; set; }
        public virtual masraf_tips masraf_tips { get; set; }
        public virtual pos_tanims pos_tanims { get; set; }
        public virtual ICollection<satislar> satislars { get; set; }
        public virtual ICollection<poshesap> poshesaps { get; set; }
        public virtual customer customer { get; set; }
    }
}