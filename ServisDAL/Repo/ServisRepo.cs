using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisDAL.Repo
{
    public class ServisRepo
    {
        public int serviceID { get; set; }
        public int custID { get; set; }
        public string musteriAdi { get; set; }
        public string baslik { get; set; }
        public string kullaniciID { get; set; }
        public string kullanici { get; set; }
        public string sonGorevli { get; set; }
        public string usta { get; set; }
        public int? usta_id { get; set; }
        public string son_dis_servis { get; set; }
        public int? tamirci_id { get; set; }
        public string aciklama { get; set; }
        public DateTime acilmaZamani { get; set; }
        public DateTime? kapanmaZamani { get; set; }
        public int? urunID { get; set; }
        public string urunAdi { get; set; }

        public string kimlikNo { get; set; }
        public int tipID { get; set; }
        public string servisTipi { get; set; }
        public string sonDurum { get; set; }
        public int? sonDurumID { get; set; }
        public string sonGorevliID { get; set; }
        public string css { get; set; }
        public string adres { get; set; }
        public string telefon { get; set; }
        public decimal tutar { get; set; }
        public decimal yekun { get; set; }
        public decimal maliyet { get; set; }
        public decimal fark { get; set; }
        public List<ServisHesapRepo> hesaplar { get; set; }
    }
    public class ServisDetayRepo
    {
        public int detayID { get; set; }
        public int servisID { get; set; }
        public DateTime tarihZaman { get; set; }
        public string belgeYol { get; set; }
        public string aciklama { get; set; }
        public int durumID { get; set; }
        public string durumAdi { get; set; }
        public string gorevliID { get; set; }
        public string baslik { get; set; }
        public string kullanici { get; set; }
    }

    public class ServisHesapRepo
    {
        public int hesapID { get; set; }
        public int musteriID { get; set; }
        public string musteriAdi { get; set; }
        public string ustaAdi { get; set; }
        public string disServis { get; set; }
        public int servisID { get; set; }
        public string islemParca { get; set; }
        public string urun_cinsi { get; set; }
        public decimal? tutar { get; set; }
        public decimal? kdv { get; set; }
        public decimal yekun { get; set; }
        public string aciklama { get; set; }
        public string cihaz { get; set; }
        public DateTime tarihZaman { get; set; }
        public bool onaylimi { get; set; }
        public string onayDurumu { get; set; }
        public DateTime? onayTarih { get; set; }
        public decimal? birim_maliyet { get; set; }
        public decimal? toplam_maliyet { get; set; }
        public string servis_aciklama { get; set; }
        public string kimlik { get; set; }
        public string kullanici { get; set; }
    }
    public class urunRepo
    {
        public int urunID { get; set; }
        public int musteriID { get; set; }
        public string musteriAdi { get; set; }
        public string cinsi { get; set; }
        public DateTime? garantiBaslangic { get; set; }
        public DateTime? garantiBitis { get; set; }
        public int? garantiSuresi { get; set; }
        public string aciklama { get; set; }
        public string belgeYol { get; set; }
        public string imei { get; set; }
        public string serino { get; set; }
        public string digerno { get; set; }

    }
    public class yedekUrunRepo
    {
        public int yedekID { get; set; }
        public int musteriID { get; set; }
        public string musteriAdi { get; set; }
        public string urunAciklama { get; set; }
        public DateTime verilmeTarihi { get; set; }
        public DateTime? donusTarih { get; set; }
        public string donmeDurumu { get; set; }
        public string kullanici { get; set; }
    }
    public class musteriOdemeRepo
    {
        public int odemeID { get; set; }
        public int musteriID { get; set; }
        public string musteriAdi { get; set; }
        public string kullaniciID { get; set; }
        //public string kullaniciAdi { get; set; }
        public decimal odemeMiktari { get; set; }
        public DateTime odemeTarih { get; set; }
        public DateTime? islem_tarihi { get; set; }
        public string aciklama { get; set; }
        public string tahsilatOdeme_turu { get; set; }
        public string tahsilat_odeme { get; set; }
        public int? pos_id { get; set; }
        public string islem_adres { get; set; }
        public string masraf_tipi { get; set; }
        public string kullanici { get; set; }

    }
    
    public class cariHesapRepo
    {
        public int musteriID { get; set; }
        public string musteriAdi { get; set; }
        public decimal netBorclanma { get; set; }
        public decimal netAlacak { get; set; }
        public decimal netBakiye { get; set; }
        public string tel { get; set; }
        public DateTime? son_mesaj { get; set; }
    }
    
    

}
