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
    
    public partial class service
    {
        public service()
        {
            this.servicedetays = new HashSet<servicedetay>();
            this.servicehesaps = new HashSet<servicehesap>();
        }
    
        public int ServiceID { get; set; }
        public Nullable<int> CustID { get; set; }
        public Nullable<int> usta_id { get; set; }
        public string usta { get; set; }
        public Nullable<int> tamirci_id { get; set; }
        public string son_dis_servis { get; set; }
        public string SonAtananID { get; set; }
        public string Aciklama { get; set; }
        public System.DateTime AcilmaZamani { get; set; }
        public Nullable<System.DateTime> KapanmaZamani { get; set; }
        public Nullable<int> UrunID { get; set; }
        public string Servis_Kimlik_No { get; set; }
        public int tip_id { get; set; }
        public string SonDurum { get; set; }
        public Nullable<bool> iptal { get; set; }
        public string olusturan_Kullanici { get; set; }
        public string Firma { get; set; }
        public string kullanici { get; set; }
        public string Baslik { get; set; }
        public Nullable<int> durum_id { get; set; }
        public string barcode { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual service_faturas service_faturas { get; set; }
        public virtual service_tips service_tips { get; set; }
        public virtual ICollection<servicedetay> servicedetays { get; set; }
        public virtual ICollection<servicehesap> servicehesaps { get; set; }
        public virtual urun urun { get; set; }
        public virtual customer customer { get; set; }
        public virtual customer customer1 { get; set; }
    }
}