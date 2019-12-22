using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.Radius;
using ServisDAL.Repo;

namespace ServisDAL
{
    public class TekPanel
    {
        radiusEntities dc;
      
        public TekPanel(radiusEntities dc)
        {
            this.dc = dc;
        }

        public PanelDetay Goster()
        {
            FaturaIslemleri fat = new FaturaIslemleri(dc);

            int yakOdeme = fat.yaklasanOdemeSayisi(DateTime.Now.AddDays(3));
           
            decimal kasaBakiye = 0;
            decimal bankaBakiye = 0;
            decimal posBakiye = 0;
            kasahesap h = dc.kasahesaps.FirstOrDefault();
            if (h != null)
            {
                kasaBakiye = h.ToplamBakiye;
            }
            List<banka> b = dc.bankas.ToList();
            if (b.Count > 0)
            {
                bankaBakiye = b.Sum(x => x.bakiye);
            }

            List<poshesap> po = dc.poshesaps.Where(x =>  x.iptal == false && x.cekildi == false).ToList();
            if (po.Count > 0)
            {
                posBakiye = po.Sum(x => x.net_tutar);
            }

            panel p = dc.panels.FirstOrDefault();
            int onayBekleyen = 0;
            int servisSayi = 0;
            int emanetSayi = 0;
            int musteriSayi = 0;
            decimal netBorc = 0;
            decimal netAlacak = 0;
            decimal netBakiye = 0;
            if (p != null)
            {
                onayBekleyen = p.onay_bekleyen_sayisi;
                servisSayi = p.servis_sayisi;
                emanetSayi = p.emanet_sayisi;
                musteriSayi = p.musteri_sayisi;
                netBorc = p.NetBorc;
                netAlacak = p.NetAlacak;
                netBakiye = p.ToplamBakiye;
            }
            PanelDetay pan = new PanelDetay();
            pan.bankaBakiye = bankaBakiye;
            pan.emanetSayisi = emanetSayi;
            pan.kasaBakiye = kasaBakiye;
            pan.musteriSayisi = musteriSayi;
            pan.netAlacak = netAlacak;
            pan.netBakiye = netBakiye;
            pan.netBorc = netBorc;
            pan.onayBekleyen = onayBekleyen;
            pan.posBakiye = posBakiye;
            pan.servisSayisi = servisSayi;
           
            pan.yaklasanOdeme = yakOdeme;
            return pan;
        }

    }
    public class PanelDetay
    {
        public decimal netBorc { get; set; }
        public decimal netAlacak { get; set; }
        public decimal netBakiye { get; set; }
        public int onayBekleyen { get; set; }
        public int servisSayisi { get; set; }
        public int emanetSayisi { get; set; }
        public int musteriSayisi { get; set; }
       
        public int yaklasanOdeme { get; set; }
        public decimal kasaBakiye { get; set; }
        public decimal bankaBakiye { get; set; }
        public decimal posBakiye { get; set; }

    }

}
