using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Radius;
using System.Web;

namespace ServisDAL
{

    public class Abonelik
    {
        radiusEntities dc;
        public Abonelik(radiusEntities dc)
        {
            this.dc = dc;
        }

        public List<abonelik_pakets> paketler()
        {
            return dc.abonelik_pakets.Where(x => x.iptal != true).ToList();
        }

        public void SonFaturaIptal(int custid)
        {
            var sonFatura = dc.faturas.Where(x => x.MusteriID == custid && x.iptal == false && x.tur == "Fatura").OrderByDescending(x => x.son_odeme_tarihi).FirstOrDefault();
            if (sonFatura != null)
            {
                sonFatura.iptal = true;
                //müşterinin geçerliliğini düzenlemek lazım
                int gun = sonFatura.abonelik_pakets.gun;
                customer musteri = dc.customers.FirstOrDefault(x => x.CustID == custid);
                if (musteri != null)
                {
                    musteri.expire = ((DateTime)musteri.expire).AddDays(-gun);
                }
                KaydetmeIslemleri.kaydetR(dc);
            }

        }
        public customer KrediYukleToplu(int custid, int paketid, int adet, decimal paket_tutari, int gun, DateTime islem_tarihi, string kullanici)
        {
            var sonFatura = dc.faturas.Where(x => x.MusteriID == custid && x.iptal == false && x.tur == "Fatura").OrderByDescending(x => x.son_odeme_tarihi).FirstOrDefault();
            var paket = dc.abonelik_pakets.FirstOrDefault(x => x.paket_id == paketid);
            var grup = dc.cihaz_grups.FirstOrDefault(x => x.grupid == -1);
            if (paket != null)
            {
                //daha önce hiç faturası yoksa DateTime.Now'a ekleyerek başlanır
                //daha önce fatura varsa ve bu fatura bitiş tarihi bugünden eskiyse yine bugüne eklenir
                //daha önce fatura varsa ve bu fatura bitis tarihi bugünden sonraysa bu tarihe ekleme yapılır

                DateTime son_odeme_tarihi = islem_tarihi.AddDays(gun);

                if (sonFatura != null && sonFatura.son_odeme_tarihi > islem_tarihi)
                {
                    son_odeme_tarihi = sonFatura.son_odeme_tarihi.AddDays(gun);

                }

                decimal kdv = 0;
                decimal oiv = 0;
                decimal otv = 0;
                if (grup.kdv != null)
                {
                    kdv = (decimal)grup.kdv;
                }
                if (grup.oiv != null)
                {
                    oiv = (decimal)grup.oiv;
                }
                if (grup.otv != null)
                {
                    otv = (decimal)grup.otv;
                }

                decimal vergisiz_tutar = (100 * paket_tutari) / (kdv + oiv + otv + 100);
                for (int i = 1; i <= adet; i++)
                {

                    fatura f = new fatura();
                    f.son_odeme_tarihi = son_odeme_tarihi;
                    f.bakiye = paket_tutari;
                    f.Firma = "firma";
                    f.guncelleme_tarihi = son_odeme_tarihi;
                    f.inserted = kullanici;
                    f.iptal = false;
                    f.islem_tarihi = islem_tarihi;
                    f.islemci_user = kullanici;
                    f.OIV = vergisiz_tutar * oiv / 100;
                    f.vergisiz_tutar = vergisiz_tutar;
                    f.KDV = vergisiz_tutar * kdv / 100;
                    f.MusteriID = custid;
                    f.no = "0";
                    f.odenen = 0;

                    f.paket_id = paketid;
                    f.sattis_tarih = islem_tarihi;
                    f.taksit_no = 1;
                    f.tc = "0";
                    f.tur = "Fatura";
                    f.tutar = paket_tutari;
                    dc.faturas.Add(f);
                    if (i != adet)
                    {
                        son_odeme_tarihi = son_odeme_tarihi.AddDays(gun);
                    }


                }


                var musteri = dc.customers.FirstOrDefault(x => x.CustID == custid);
                if (musteri != null)
                {
                    musteri.expire = son_odeme_tarihi;
                    musteri.paket_id = paketid;
                }

                KaydetmeIslemleri.kaydetR(dc);
                return musteri;
            }
            else {
                return null;
            }

        }


    }

}
